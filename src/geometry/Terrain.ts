import {mat4, vec2, vec3, vec4} from 'gl-matrix';
import Drawable from '../rendering/gl/Drawable';
import {gl} from '../globals';
import fbm from '../noise/Fbm';
import dist from '../noise/Dist';
import Road from './Road';
import {QuadTree, Box, Point, Circle} from 'js-quadtree';
import Sprawler from './Sprawler';

const sea_level = -7;

class Terrain extends Drawable {
  indices: Uint32Array;
  positions: Float32Array;
  normals: Float32Array;
  populations: Float32Array;

  height: any; //stores the height map
  population: any; //stores the population map

  size: any; //parameters for height map generation
  pop: any; //parameters for population generation

  creep_spawn = 0.3;
  crawl_persist = 0.2;
  street_spacer = 250;

  block_mult = 1;
  max_highways = 10;

  nodes: QuadTree; //nodes
  intersects: QuadTree; //intersection nodes
  stage: number; //generation stage, 0: highways and streets, 1: roads, 2: blocking, 3: adding buildings
  iterations: number; //iterations 
  street_sprawn: number;
  street_snode: number;

  highways: any; //store the highway roads here
  streets: any; //store the streets here
  roads: any; //store the roads here

  buildings: Array<mat4>;

  constructor(size: any, pop: any) {
    super(); // Call the constructor of the super class. This is required.
    
    this.size = {
        x: size.x + 1,
        y: size.y + 1,
        s: size.s, //scales x/y size
        ps: size.ps, //perlin scale for noise detail
        h:size.h //scales height from perlin output
    };

    this.nodes = new QuadTree(new Box(0, 0, this.size.x, this.size.y), {
        removeEmptyNodes: true,  // Specify if the quadtree has to remove subnodes if they are empty (default: false).
        arePointsEqual: (point1, point2) => false, //don't remove repeat points
        maximumDepth: 12,         // Specify the maximum depth of the quadtree. -1 for no limit (default: -1).   
    });

    this.intersects = new QuadTree(new Box(0, 0, this.size.x, this.size.y), {
        removeEmptyNodes: true,  // Specify if the quadtree has to remove subnodes if they are empty (default: false).
        arePointsEqual: (point1, point2) => false, //don't remove repeat points
        maximumDepth: 12,         // Specify the maximum depth of the quadtree. -1 for no limit (default: -1).   
    });

    this.pop = pop;

    this.highways = [];
    this.streets = [];
    this.roads = [];
    
    this.generate();

    this.init();
  }

  //initializes some roads
  init() {
    this.highways = [];
    this.streets = [];
    this.roads = [];
    this.buildings = new Array();
    this.stage = 0;
    this.iterations = 0;
    this.street_sprawn = 0;
    this.street_snode = 0;
    this.nodes.clear();
    this.intersects.clear();

    //find some good starting points for highways
    let sources: any;
    sources = [];
    for(let i = 0; i < this.max_highways; i++) {
        let currmax = 0;
        let maxpos = {x: -1, y: -1};
        for(let x = 0; x < this.size.x; x++) {
            for(let y = 0; y < this.size.y; y++) {
                let canplace = true;
                for(let p of sources) {
                    if(dist([p.x, p.y], [x, y]) < (this.size.x + this.size.y) * 0.15) {
                        canplace = false;
                        break;
                    } 
                }
                if(!canplace) continue;
                let f = this.getPop(x, y);
                if(f > currmax) {
                    currmax = f;
                    maxpos = {x:x, y:y};
                }
            }
        }
        if(currmax > -Infinity) {
            sources.push(maxpos);
        }
        else {
            break;
        }
    }

    for(let i = 0; i < sources.length; i++) {
        let src = sources[i];
        this.highways.push(new Road(src, Math.random()*2*Math.PI, 2, i, 1, this));
        //let next = sources[(i+1)%sources.length];
        //this.highways.push(new Road(src, Math.atan((src.y-next.y)/(src.x-next.x)), 2, i, 1, this));
    }

    //this.create();
  }

  generate() {
    this.height = [];

    for(let i = 0; i < this.size.x; i++) {
        let temp = [];
        for(let j = 0; j < this.size.y; j++) {
            temp.push(Math.max(sea_level, this.size.h*fbm(i*this.size.ps, j*this.size.ps, 8)));
        }
        this.height.push(temp);
    }

    console.log("terrain generated with heigh factor: " + this.size.h);

    let offset = 5000; //offset the noise for different sample
    this.population = [];
    if(this.pop.type == "fbm") {
        for(let i = 0; i < this.size.x; i++) {
            let temp = [];
            for(let j = 0; j < this.size.y; j++) {
                let nval = Math.pow(1-this.height[i][j]*0.5, this.pop.hscale) * (fbm(i*this.pop.ps + offset, j*this.pop.ps + offset, 4) + 0.25);
                let pval = Math.min(this.pop.max, Math.max(this.pop.min, nval * this.pop.scale));
                temp.push(pval);
            }
            this.population.push(temp);
        }
    }
  }

  tick() {
    if(this.stage == 0) {
        let head = false;
        for(let road of this.highways) {
            road.creep();
            head = head || (road.heads.length > 0);
        }
        for(let road of this.streets) {
            road.crawl();
            head = head || (road.heads.length > 0);
        }
        if(!head || this.iterations > 100) {
            //prune all empty streets
            let new_streets = [];
            let id = 0;
            for(let street of this.streets) {
                if(street.nodecount > street.headcount) {
                    street.id = id;
                    new_streets.push(street);
                    id++;
                }
            }
            this.streets = new_streets;
            //increment stage to sprawl
            this.stage = 1;
        }
    }
    else if(this.stage == 1 || this.stage == 2) {
        if(this.street_sprawn >= this.streets.length){
            this.street_sprawn = 0;
            this.street_snode = 0;
            this.stage++;
        }
        else {
            //sprawls most recent sprawler, if possible
            if(this.roads.length > 0 && this.roads[this.roads.length-1].queue.size > 0) {
                this.roads[this.roads.length-1].sprawl();
            }
            //otherwise attempt to add in a new sprawler
            else {
                while(this.street_sprawn < this.streets.length) {
                    let street_params = {minx: this.block_mult*(6+ Math.random()*2), miny: this.block_mult*(3+ Math.random()*1), varx: this.block_mult*(0.5), vary: this.block_mult*(0.5)};
                    let jane = this.streets[Math.floor(this.street_sprawn)];
                    console.log("street" + (jane.id+1) + "/" + this.streets.length + "node" + (this.street_snode+1) + "/" + jane.nodecount);
                    let thisNode = jane.nodes[this.street_snode];

                    let angle = thisNode.angle + Math.PI/2;
                    if(this.stage%2 == 1) angle -= Math.PI/2;

                    let p = thisNode.pos;
                    let inc = {x: Math.cos(angle), y: Math.sin(angle)};

                    let l = 5;
                    let hitSomething = false;
                    let hitSprawler = false;

                    while(this.inBounds(p.x, p.y) && !hitSomething) {
                        let otherNodes = this.nodes.query(new Circle(p.x + l*inc.x, p.y + l*inc.y, 1));
                        
                        for(let on of otherNodes) {
                            if(on.data.type != 2 || on.data.id != jane.id) {
                                hitSomething = true;
                                break;
                            }
                        }

                        if(this.getPop(p.x + l*inc.x, p.y + l*inc.y) < 0.1) {
                            hitSomething = true;
                        }
                        l+=0.5;
                    }

                    let otherNodes = this.nodes.query(new Circle(p.x + l/2*inc.x, p.y + l/2*inc.y, 5));

                    for(let on of otherNodes) {
                        if(on.data.type == 3){
                            hitSprawler = true;
                            break;
                        }
                    }


                    //increment along the street
                    this.street_snode += 9;

                    if(this.street_snode >= jane.nodecount) {
                        this.street_sprawn++;
                        this.street_snode = 0;
                    }

                    // if(l < 5) console.log("too short");
                    // if(hitSprawler) console.log("hit");
                    if(l > 8 && !hitSprawler) {
                        this.roads.push(new Sprawler({x: p.x + l/2*inc.x, y: p.y + l/2*inc.y}, street_params, angle + (Math.random()-0.5) * Math.PI/8, this.roads.length, this));
                        break;
                    }
                }
            }
        }
    }
    else if(this.stage == 3) {
        let new_roads = [];
        let i = 0;
        for(let road of this.roads) {
            if(road.gridcount >= 10) {
                road.id = i++;
                new_roads.push(road);
            }
        }

        console.log("pruned " + (this.roads.length-new_roads.length) + "bad roads"); 
        this.roads = new_roads;
        this.stage++;
    }
    else if(this.stage == 4){
        for(let road of this.roads) {
            for(let xx in road.grid) {
                for(let yy in road.grid[xx]) {
                    let x = Number(xx);
                    let y = Number(yy);
                    if(road.isInGrid(x+1, y+1) && road.isInGrid(x+1, y) && road.isInGrid(x, y+1) && road.atGrid(x+1, y+1).links.size > 0 && road.atGrid(x+1, y).links.size > 0 && road.atGrid(x, y+1).links.size > 0) {
                        let p1 = road.atGrid(x, y).pos;
                        let p2 = road.atGrid(x+1, y).pos;
                        let p3 = road.atGrid(x, y+1).pos;
                        let p4 = road.atGrid(x+1, y+1).pos;

                        let thisPop = 5*this.getPop((p1.x + p4.x)/2, (p1.y + p4.y)/2) + 2;
                        thisPop = Math.pow(thisPop, 1.3);

                        let pp1 = this.getPos(p1.x, p1.y);
                        let pp2 = this.getPos(p2.x, p2.y);
                        let pp3 = this.getPos(p3.x, p3.y);
                        let pp4 = this.getPos(p4.x, p4.y);

                        let minh = Math.min(pp1[1], pp2[1], pp3[1], pp4[1]); 

                        let dx = {x: pp2[0]-pp1[0], y:pp2[2]-pp1[2]};
                        let dy = {x: pp3[0]-pp1[0], y:pp3[2]-pp1[2]};

                        let mx = dist([dx.x, dx.y], [0, 0]);
                        let my = dist([dy.x, dy.y], [0, 0]);

                        let a = Math.atan2(dy.y, dy.x);
                        for(let i = 0; i <2; i++) {
                            for(let j = 0; j <2; j++) {
                                let m1 = mat4.create();
                                let m2 = mat4.create();
                                let m3 = mat4.create();
                                mat4.fromScaling(m1, vec3.fromValues(mx, thisPop, my));
                                mat4.fromRotation(m2, a, vec3.fromValues(0, 1, 0));
                                // pp1[0] + dx.x * (0.5+i)/2 + dy.x * (0.5+j)/2, minh+thisPop/2, pp1[2] + dx.y * (0.5+i)/2 + dy.y * (0.5+j)/2));
                                mat4.fromTranslation(m3, vec3.fromValues((pp1[0] + pp4[0])/2, minh, (pp1[2] + pp4[2])/2));
                                let m = mat4.create();
                                mat4.mul(m, m1, m);
                                mat4.mul(m, m2, m);
                                mat4.mul(m, m3, m);
                                this.buildings.push(m);
                            }
                        }
                    }
                }
            }
        }
        console.log("Generated " + this.buildings.length + " buildings");
        this.stage++;
    }
    this.iterations++;
  }

  addStreet(pos: {x: number, y: number}, angle: number) {
    this.streets.push(new Road(pos, angle, 1, this.streets.length, 2, this));
  }

  drawMode(): GLenum {
    return gl.TRIANGLES;
  }

  getPos(x: number, y:number) {
    // if(x < 0 || x > this.size.x-1 || y < 0 || y > this.size.y-1) {
    //     return vec3.fromValues(0, 0, 0);
    // }

    let newx = (x- this.size.x/2)*this.size.s;
    let newy = (y- this.size.y/2)*this.size.s;
    //height
    // let dWeight = 0;
    // let dSum = 0;
    // let dx = Math.floor(x);
    // let dy = Math.floor(y);
    // let dh = 0;
    // for(let i = 0; i <= 1; i++) {
    //     for(let j = 0; j <= 1; j++) {
    //         let d = dist([x, y], [dx+i, dy+j]);
    //         if(d == 0) {
    //             dSum = this.height[dx+i][dy+j];
    //             dWeight = 1;
    //             i = 2;
    //             j = 2;
    //         }
    //         else {    
    //             dSum += 1/d;
    //             dWeight += this.height[dx+i][dy+j]/d;
    //         }
    //     }
    // }
    // dh = dSum/dWeight;

    let dh = Math.max(sea_level, this.size.h*fbm(x*this.size.ps, y*this.size.ps, 8));

    return vec3.fromValues(newx, dh, newy);
  }

  getPop(x: number, y:number) {
    // if(x < 0 || x > this.size.x-1 || y < 0 || y > this.size.y-1) {
    //     return 0;
    // }

    // let dWeight = 0;
    // let dSum = 0;
    // let dx = Math.floor(x);
    // let dy = Math.floor(y);
    // for(let i = 0; i <= 1; i++) {
    //     for(let j = 0; j <= 1; j++) {
    //         let d = dist([x, y], [dx+i, dy+j]);
    //         if(d == 0) return this.population[dx+i][dy+j];
    //         dSum += 1/d;
    //         dWeight += this.population[dx+i][dy+j]/d;
    //     }
    // }
    // return dSum/dWeight;
    if(this.getPos(x, y)[1] == sea_level) return -1;
    if(this.pop.type == "fbm") {
        let offset = 5000;
        let nval = Math.pow(1-this.getPos(x, y)[1]*0.5, this.pop.hscale) * (fbm(x*this.pop.ps + offset, y*this.pop.ps + offset, 4) + 0.25);
        let pval = Math.min(this.pop.max, Math.max(this.pop.min, nval * this.pop.scale));
        return pval;     
    }
  }

  inBounds(x: number, y: number) {
    return x > 0 && x < this.size.x-1 && y > 0 && y < this.size.y-1;
  }

  create() {
    let tris = 2*(this.size.x-1)*(this.size.y-1);

    this.indices = new Uint32Array(tris*3);
    this.normals = new Float32Array(tris*3*4);
    this.positions = new Float32Array(tris*3*4);
    this.populations = new Float32Array(tris*3);

    let c = 0;
    for(let i = 0; i < this.size.x-1; i++) {
        for(let j = 0; j < this.size.y-1; j++) {
            let p1 = this.getPos(i, j);
            let p2 = this.getPos(i+1, j);
            let p3 = this.getPos(i, j+1);
            let p4 = this.getPos(i+1, j+1);

            for(let tri of [
                {pos: [p1, p2, p3], ind: [[i, j], [i+1, j], [i, j+1]]}, 
                {pos: [p2, p4, p3], ind: [[i+1, j], [i+1, j+1], [i, j+1]]}
            ]) {
                //calculate normal and insert 4 times

                let n = vec3.create();
                let d1 = vec3.create();
                let d2 = vec3.create();
                let nun = vec3.create();
                vec3.sub(d1, tri.pos[0], tri.pos[1]);
                vec3.sub(d2, tri.pos[0], tri.pos[2]);
                vec3.cross(nun, d2, d1);
                vec3.normalize(n, nun);

                for (let k = 0; k < 3; k++) {
                    this.normals[c*12 + 4*k + 0] = n[0];
                    this.normals[c*12 + 4*k + 1] = n[1];
                    this.normals[c*12 + 4*k + 2] = n[2];
                    this.normals[c*12 + 4*k + 3] = 0;
                }

                //insert positions
                let k = 0;
                for(let p of tri.pos) {
                    this.positions[c*12 + 4*k + 0] = p[0];
                    this.positions[c*12 + 4*k + 1] = p[1];
                    this.positions[c*12 + 4*k + 2] = p[2];
                    this.positions[c*12 + 4*k + 3] = 1;
                    k++;
                }

                //insert populations
                k = 0;
                for(let p of tri.ind) {
                    this.populations[c*3+k] = this.getPop(p[0], p[1]);
                    k++;
                }
                
                //insert indices
                this.indices[c*3] = c*3;
                this.indices[c*3+1] = c*3+1;
                this.indices[c*3+2] = c*3+2;

                c++;
            }            
        }
    }

         
    this.generateIdx();
    this.generatePos();
    this.generateNor();
    this.generatePop();

    this.count = this.indices.length;
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.bufIdx);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, this.indices, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufNor);
    gl.bufferData(gl.ARRAY_BUFFER, this.normals, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufPos);
    gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufPop);
    gl.bufferData(gl.ARRAY_BUFFER, this.populations, gl.STATIC_DRAW);

    //console.log(`Created cube`);
  }
};

export default Terrain;
