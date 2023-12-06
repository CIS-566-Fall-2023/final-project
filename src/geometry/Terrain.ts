import {vec2, vec3, vec4} from 'gl-matrix';
import Drawable from '../rendering/gl/Drawable';
import {gl} from '../globals';
import fbm from '../noise/Fbm';
import dist from '../noise/Dist';
import Road from './Road';
import {QuadTree, Box, Point, Circle} from 'js-quadtree';

class Terrain extends Drawable {
  indices: Uint32Array;
  positions: Float32Array;
  normals: Float32Array;
  populations: Float32Array;

  height: any; //stores the height map
  population: any; //stores the population map

  size: any; //parameters for height map generation
  pop: any; //parameters for population generation

  nodes: QuadTree; //nodes
  intersects: QuadTree; //intersection nodes
  stage: number; //generation stage, 0: highways and streets, 1: roads, 2: blocking, 3: adding buildings
  iterations: number; //iterations 
  street_sprawn: number;

  highways: any; //store the highway roads here
  streets: any; //store the streets here
  roads: any; //store the roads here

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
        maximumDepth: 8,         // Specify the maximum depth of the quadtree. -1 for no limit (default: -1).   
    });

    this.intersects = new QuadTree(new Box(0, 0, this.size.x, this.size.y), {
        removeEmptyNodes: true,  // Specify if the quadtree has to remove subnodes if they are empty (default: false).
        arePointsEqual: (point1, point2) => false, //don't remove repeat points
        maximumDepth: 8,         // Specify the maximum depth of the quadtree. -1 for no limit (default: -1).   
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
    this.stage = 0;
    this.iterations = 0;
    this.street_sprawn = 0;
    this.nodes.clear();
    this.intersects.clear();

    //find some good starting points for highways
    let sources: any;
    sources = [];
    for(let i = 0; i < 10; i++) {
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
  }

  generate() {
    this.height = [];

    for(let i = 0; i < this.size.x; i++) {
        let temp = [];
        for(let j = 0; j < this.size.y; j++) {
            temp.push(this.size.h*fbm(i*this.size.ps, j*this.size.ps, 8));
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
    else if(this.stage == 1) {
        if(this.street_sprawn >= this.streets.length){
            this.stage++;
        }
        else {
            let roadHead = false;
            for(let road of this.roads) {
                if(road.heads.length > 0) {
                    roadHead = true;
                    road.sprawl();
                }
            }
            if(!roadHead) {
                let street_params = {minx: 10+ Math.random()*4, miny: 10+ Math.random()*4};
                for(let i = street_params.minx; i < this.streets[this.street_sprawn].nodecount; ) {
                    let i2 = Math.floor(i);
                    let thisNode = this.streets[this.street_sprawn].nodes[i2];

                    this.roads.push(new Road(thisNode.pos, thisNode.angle + Math.PI/2, 2, this.roads.length, 3, this));

                    i += street_params.minx + Math.random();

                }
                this.street_sprawn++;
            }
        }
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

    let dh = this.size.h*fbm(x*this.size.ps, y*this.size.ps, 8);

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
                    this.populations[c*3+k] = this.population[p[0]][p[1]];
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
