import {vec2, vec3, vec4} from 'gl-matrix';
import Drawable from '../rendering/gl/Drawable';
import {gl} from '../globals';
import Terrain from './Terrain';
import {QuadTree, Box, Point, Circle} from 'js-quadtree';
import dist from '../noise/Dist';

function canMerge(p1: {x: number, y: number, a: number}, p2: {x: number, y: number, a: number}, margin: number) {
    //if(Math.abs(Math.abs(Math.cos(p1.a)) - Math.abs(Math.cos(p2.a))) > 0.1 || Math.abs(Math.abs(Math.sin(p1.a)) - Math.abs(Math.sin(p2.a))) > 0.1) return false;
    let d =  dist([p1.x, p1.y], [p2.x, p2.y]);
    let p3 = {x: p1.x + d*Math.cos(p1.a), y: p1.y + d*Math.sin(p1.a)};
    return dist([p3.x, p3.y], [p2.x, p2.y]) < margin*d;
}

class Node {
    id: number; //id in the road
    pos: {x: number, y: number}; //position of the node
    branches: boolean; //branching roads, if any at the node
    prev: Node;
    next: Node;
    angle: number;
    rbranch: number; //number of nodes since last branching
    lbranch: number;

    constructor(id: number, pos: {x: number, y: number}, a: number){
        this.pos = pos;
        this.id = id;
        this.angle = a;
        this.next = null;
        this.prev = null;
        this.rbranch = -1; //indicated no branching
        this.lbranch = -1;
        this.branches = false;
    }
}

class Road extends Drawable {
  indices: Uint32Array;
  positions: Float32Array;
  normals: Float32Array;
  colors: Float32Array;

  nodes: any; //array of vec3s corresponding to node positions
  nodecount: number;

  id: any; //id in terrain
  type: any; //denotes the type of road (1: highway, 2: street, 3: road/alley)
  block: {minx: number, miny: number};

  heads: any;   //keep track of the two heads of the road for expansion
                //head format: position, direction
    headcount: number;
    detail: number;

    terrain: Terrain; //reference to terrain

  constructor(spawn: {x: number, y: number}, ddir: number, headcount: number, id: any, type: any, terrain: Terrain) {
    super(); // Call the constructor of the super class. This is required.

    this.id = id; //Set definitions
    this.type = type;
    this.terrain = terrain;
    this.detail = 20 * (4-type); //number of steps between major nodes

    this.headcount = headcount;
    this.nodes = [];
    this.nodecount = 0;
    this.heads = [];

    for(let i = 0; i < headcount; i++) {
        this.addNode(spawn, ddir + Math.PI*i);
        this.heads.push({id: this.nodecount-1, dir: ddir + Math.PI*i});
        this.heads[i].rbranch = Math.random() * this.terrain.street_spacer/3;
        this.heads[i].lbranch = Math.random() * this.terrain.street_spacer/3;
    }
  }

  drawMode(): GLenum {
    return gl.LINES;
  }

  //for highways
  creep() {
    let i = 0;
    let new_heads: any = [];

    for(let head of this.heads) {
        let hpos = this.nodes[head.id].pos;
        let hat = this.terrain.getPos(hpos.x, hpos.y)[1];

        let maxSum = -Infinity;
        let bestA = head.dir;

        for(let a = -20; a <= 20; a+= 2.5) {
            let newA = head.dir+a*Math.PI/180;
            let newDir = {x: Math.cos(newA), y: Math.sin(newA)};
            let thissum = 0;
            let newPos;
            for(let ray = 1; ray <= 100; ray+= 4) {
                newPos = {x: hpos.x + newDir.x*ray, y: hpos.y + newDir.y*ray};
                thissum += this.terrain.getPop(newPos.x, newPos.y)*Math.sqrt(ray);
            }

            thissum *= Math.cos(a*Math.PI/180); //discourage turning too much
            thissum *= 1-Math.abs(hat - this.terrain.getPos(newPos.x, newPos.y)[1])*2; //discourage making height differences

            if(thissum > maxSum) {
                maxSum = thissum;
                bestA = newA;
            }
        }

        let npos = {x: hpos.x + 20*Math.cos(bestA), y: hpos.y + 20*Math.sin(bestA)};

        //adds nodes to be draw along terrain
        let lastNode = head.id;
        let div = this.detail;
        let inc = 1/div;
        let w1 = 1 - inc;
        let w2 = 0 + inc;

        let merged = false;
        for(let i = 0; i < div; i++) {
            let newpos = {x: w1*hpos.x + w2*npos.x, y: w1*hpos.y + w2*npos.y};
            w1 -= inc;
            w2 += inc;

            let newNodeInfo = this.addNode(newpos, bestA);
            let newNode = newNodeInfo.node;
            merged = newNodeInfo.merged;
            //branching streets from highways
            if(!merged) {
                for(let p of [[this.nodes[lastNode].rbranch, Math.PI/2], [this.nodes[lastNode].lbranch, -Math.PI/2]]) {
                    if(p[0] > this.terrain.street_spacer) {
                        let bestA = null;
                        let maxPop = this.terrain.creep_spawn * 5;
                        for(let a = -20; a <= 20; a += 5) {
                            let newA = head.dir + p[1] + a*Math.PI/180;
                            let newPop = 0;
                            for(let d = 5; d <= 25; d++) {
                                let newPos = {x: newNode.pos.x + d*Math.cos(newA), y: newNode.pos.y + d*Math.sin(newA)};
                                newPop += this.terrain.getPop(newPos.x, newPos.y);
                            }
                            if(newPop > maxPop) {
                                maxPop = newPop;
                                bestA = newA;
                            }
                        }
                        if(bestA != null) {
                            this.terrain.addStreet({x: newNode.pos.x, y: newNode.pos.y}, bestA);
                            if(p[1] > 0) newNode.rbranch = Math.random()*this.terrain.street_spacer;
                            else newNode.lbranch = Math.random()*this.terrain.street_spacer;
                        }
                    }
                }
            }

            this.linkNode(lastNode, newNode.id);
            lastNode = newNode.id;
            if(merged) break;
        }

        //sets head to new position
        if(!merged) 
            new_heads.push({
                id: lastNode,
                dir: bestA
            });
        i++;
    }

    this.heads = new_heads;

    this.create();
  }

  //for streets
  crawl() {
    let new_heads: any = [];

    for(let head of this.heads) {
        let hpos = this.nodes[head.id].pos;
        let ha = head.dir;

        let bestA = null;
        let npos = null;
        let bestPop = this.terrain.crawl_persist;
        for(let a = -2.5; a <= 2.5; a += 0.5) {
            let newA = ha + a*Math.PI/180;
            let newPop = this.terrain.getPop(hpos.x + 5*Math.cos(newA), hpos.y + 5*Math.sin(newA));
            let otherNodes = this.terrain.nodes.query(new Circle(hpos.x + 5*Math.cos(newA), hpos.y + 5*Math.sin(newA), 3));
            // for(let on of otherNodes) {
            //     if(on.data.type == 2 && on.data.id != this.id){
            //         newPop *= 0.5;
            //         break;
            //     }
            // }
            if(newPop > bestPop) {
                bestPop = newPop;
                bestA = newA;
                npos = {x: hpos.x + 5*Math.cos(newA), y: hpos.y + 5*Math.sin(newA)};
            }
        }
        if(bestA != null) {
            bestA += (Math.random()-0.5)*5*Math.PI/180; //randomness to avoid clumping
            let lastNode = head.id;
            let div = this.detail;
            let inc = 1/div;
            let w1 = 1 - inc;
            let w2 = 0 + inc;

            let merged = false;
            for(let i = 0; i < div; i++) {
                let newpos = {x: w1*hpos.x + w2*npos.x, y: w1*hpos.y + w2*npos.y};
                w1 -= inc;
                w2 += inc;

                let newNodeInfo = this.addNode(newpos, bestA);
                let newNode = newNodeInfo.node;
                merged = newNodeInfo.merged;

                //decay to prevent street from growing too large
                // if(this.nodecount > 100 && Math.random() < street_decay * this.nodecount * 0.01) {
                //     merged = true;
                // }

                if(!merged) {
                    for(let p of [[this.nodes[lastNode].rbranch, Math.PI/2], [this.nodes[lastNode].lbranch, -Math.PI/2]]) {
                        if(p[0] > this.terrain.street_spacer*1.5) {
                            let otherNodes = this.terrain.intersects.query(new Circle(newNode.pos.x, newNode.pos.y, 50));
                            if(otherNodes.length == 0) {
                                let bestA = null;
                                let maxPop = this.terrain.creep_spawn * 5;
                                for(let a = -40; a <= 40; a += 5) {
                                    let newA = head.dir + p[1] + a*Math.PI/180;
                                    let newPop = 0;
                                    for(let d = 5; d <= 25; d++) {
                                        let newPos = {x: newNode.pos.x + d*Math.cos(newA), y: newNode.pos.y + d*Math.sin(newA)};
                                        newPop += this.terrain.getPop(newPos.x, newPos.y);
                                    }
                                    if(newPop > maxPop) {
                                        maxPop = newPop;
                                        bestA = newA;
                                    }
                                }
                                if(bestA != null) {
                                    this.terrain.addStreet({x: newNode.pos.x, y: newNode.pos.y}, bestA);
                                    this.terrain.intersects.insert(new Point(newNode.pos.x,newNode.pos.y, {id: this.id, step: this.nodecount, type: this.type, a: bestA, x: newNode.pos.x, y: newNode.pos.y}));

                                    if(p[1] > 0) newNode.rbranch = Math.random()*this.terrain.street_spacer/3;
                                    else newNode.lbranch = Math.random()*this.terrain.street_spacer/3;
                                }
                            }
                        }
                    }
                }

                this.linkNode(lastNode, newNode.id);
                lastNode = newNode.id;
            }
            if(!merged) new_heads.push({id: lastNode, dir: bestA});
        }
    }

    this.heads = new_heads;

    this.create();
  }

  //for roads
  /*
  sprawl() {
    let new_heads = [];
    for(let head of this.heads) {
        let long = 3;
        let lastNode = head.id;

        let merged = false;
        let inc = 1/this.detail;
        for(let i = 1; i <= this.detail; i++) {
            let newNodeInfo = this.addNode({
                x: this.nodes[lastNode].pos.x + i*inc*long*Math.cos(head.dir), 
                y: this.nodes[lastNode].pos.y + i*inc*long*Math.sin(head.dir)}
            , head.dir);
            this.linkNode(lastNode, newNodeInfo.node.id);
            lastNode = newNodeInfo.node.id;
            merged = newNodeInfo.merged;
            if(merged) break;
        }
        if(!merged) {
            new_heads.push({
                id: lastNode,
                dir: head.dir
            });
        }
    }

    this.heads = new_heads;
    
    this.create();
  }*/

  addNode(pos: {x: number, y: number}, a: number) : {node: Node, merged: boolean} {
    //out of bounds cutoff
    if(Math.abs(pos.x - this.terrain.size.x/2) > (this.terrain.size.x)*0.5 || Math.abs(pos.y - this.terrain.size.y/2) > (this.terrain.size.y)*0.5) {
        this.nodes.push(new Node(this.nodecount++, pos, a));
        //add node info to terrain node qt
        this.terrain.nodes.insert(new Point(pos.x, pos.y, {id: this.id, step: this.nodecount, type: this.type, a: a, x: pos.x, y: pos.y}));
        return {node: this.nodes[this.nodecount-1], merged: true};
    }

    let toMerge = false;
    //check for collisions
    if(this.type == 1) {
        let otherNodes = this.terrain.nodes.query(new Circle(pos.x, pos.y, 10));
        if(otherNodes.length > 0) {
            for(let on of otherNodes) {
                if(on.data.type == 1) {
                    if(on.data.id != this.id || (Math.abs(this.nodecount - on.data.step) > 5*this.detail)) { // need to connect this road to a different road due to collision
                        //if(on.data.id != this.id) console.log("merging two different ids");
                        //else if((Math.abs(this.nodecount - on.data.step) > 20*this.detail)) console.log("ouroboros");

                        if(canMerge({x: pos.x, y: pos.y, a: a}, {x: on.data.x, y: on.data.y, a: on.data.a}, 0.2)) {
                            this.nodes.push(new Node(this.nodecount++, {x: on.data.x, y: on.data.y}, a));
                            return {node: this.nodes[this.nodecount-1], merged: true};
                        }
                    }
                }
            }
        }
    }
    else if(this.type == 2) {
        let otherNodes = this.terrain.nodes.query(new Circle(pos.x, pos.y, 5));
        for(let on of otherNodes) {
            if((on.data.type == 1 && this.nodecount > 10 && false) || (on.data.type == 2 && this.nodecount > 100)) {
                if(on.data.id != this.id) { // need to connect this road to a different road due to collision
                    if(canMerge({x: pos.x, y: pos.y, a: a}, {x: on.data.x, y: on.data.y, a: on.data.a}, 0.2)) {
                        this.nodes.push(new Node(this.nodecount++, {x: on.data.x, y: on.data.y}, a));
                        return {node: this.nodes[this.nodecount-1], merged: true};
                    }
                }
            }
        }
    }
    else {
        //existing merge noddes to snap to
        let otherNodes = this.terrain.intersects.query(new Circle(pos.x + 2*Math.cos(a), pos.y + 2*Math.sin(a), 2));
        for(let on of otherNodes) {
            if(on.data.type != 3 || (on.data.id != this.id)) { // need to connect this road to a different road due to collision
                if(true){//canMerge({x: pos.x, y: pos.y, a: a}, {x: on.data.x, y: on.data.y, a: on.data.a}, 0.2)) {
                    this.nodes.push(new Node(this.nodecount++, {x: on.data.x, y: on.data.y}, a));
                    return {node: this.nodes[this.nodecount-1], merged: true};
                }
                else {
                    //toMerge = true;
                }
            }
        }
        //new intersection
        otherNodes = this.terrain.nodes.query(new Circle(pos.x + Math.cos(a), pos.y + Math.sin(a), 1.5));
        for(let on of otherNodes) {
            if(on.data.type != 3 || (on.data.id != this.id)) { // need to connect this road to a different road due to collision
                if(true){//canMerge({x: pos.x, y: pos.y, a: a}, {x: on.data.x, y: on.data.y, a: on.data.a}, 0.2)) {
                    this.nodes.push(new Node(this.nodecount++, {x: on.data.x, y: on.data.y}, a));
                    this.terrain.intersects.insert(new Point(on.data.x,on.data.y, {id: this.id, step: this.nodecount, type: this.type, a: a, x: on.data.x, y: on.data.y}));
                    return {node: this.nodes[this.nodecount-1], merged: true};
                }
                else {
                    //toMerge = true;
                }
            }
        }
        if(this.terrain.getPop(pos.x, pos.y) < 0.2) {
            toMerge = true;
        }
    }

    //add new node to its list
    this.nodes.push(new Node(this.nodecount++, pos, a));
    //add node info to terrain node qt
    this.terrain.nodes.insert(new Point(pos.x, pos.y, {id: this.id, step: this.nodecount, type: this.type, a: a, x: pos.x, y: pos.y}));

    return {node: this.nodes[this.nodecount-1], merged: toMerge};
  }

  linkNode(prev: number, next: number) {
    this.nodes[prev].next = next;
    this.nodes[next].prev = prev;
    if(this.nodes[next].rbranch == -1) this.nodes[next].rbranch = this.nodes[prev].rbranch+1;
    if(this.nodes[next].lbranch == -1) this.nodes[next].lbranch = this.nodes[prev].lbranch+1;
  }

  create() {
    //first count the number of segments to draw
    let todraw = 0;
    for(let node of this.nodes) {
        if(node.next){
            let next = this.nodes[node.next];
            if(this.terrain.inBounds(node.pos.x, node.pos.y) && this.terrain.inBounds(node.pos.x, node.pos.y)) todraw++;
        } 
    }

  this.indices = new Uint32Array((todraw)*2);
  this.positions = new Float32Array(this.nodes.length*4);
  this.colors = new Float32Array(this.nodes.length*4);

  let c: any = [];
   if(this.type == 1) {
    c = [1, 0, 0, 1];
   }
   else if(this.type == 2) {
    c = [0, 0, 1, 1];
   }
   else{
    c = [0.5, 0, 0.5, 1];
   }
         
  for(let i = 0; i < this.nodes.length; i++) {
    let p = this.terrain.getPos(this.nodes[i].pos.x, this.nodes[i].pos.y);
    this.positions[4*i] = p[0];
    this.positions[4*i+1] = p[1];//+0.05;
    this.positions[4*i+2] = p[2];
    this.positions[4*i+3] = 1;

    this.colors[4*i] = c[0];
    this.colors[4*i+1] = c[1];
    this.colors[4*i+2] = c[2];
    this.colors[4*i+3] = c[3];
  }

  let i = 0;
  for(let node of this.nodes) {
    if(node.next){
        //let next = this.nodes[node.next];
        if(this.terrain.inBounds(node.pos.x, node.pos.y) && this.terrain.inBounds(node.pos.x, node.pos.y)){
            this.indices[2*i] = node.id;
            this.indices[2*i+1] = node.next;
            i++;
        }
    } 
  }

    this.generateIdx();
    this.generatePos();
    this.generateCol();

    this.count = this.indices.length;
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.bufIdx);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, this.indices, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufCol);
    gl.bufferData(gl.ARRAY_BUFFER, this.colors, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufPos);
    gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);

    //console.log(`Created cube`);
  }
};

export default Road;
