import Road from "./Road";
import dist from "../noise/Dist";
import Terrain from "./Terrain";

//manhattan distance deltas
const deltas = [{x: 0, y: 1}, {x: 0, y: -1}, {x: 1, y: 0}, {x: -1, y: 0}];
function d2v(delta: {x: number, y: number}) : number {
    return 2*(delta.x+1) + (delta.y+1);
}

class Sprawler extends Road {
    grid: any;
    gridcount: number;
    queue: Set<any>;
    blocks: {minx: number, miny: number, varx: number, vary: number}; //parameters for block sizes
    center: {x: number, y: number};
    slant: number;

    constructor(spawn: {x: number, y: number}, gsize: {minx: number, miny: number, varx: number, vary: number}, slant: number, id: any, terrain: Terrain) {
        super(spawn, 0, 0, id, 3, terrain);
        this.center = spawn; //source of the grid
        this.blocks = gsize; //block spawn parameters
        this.slant = slant;  //slant grid anggle
        this.grid = {};
        this.gridcount = 0;
        this.queue = new Set();

        this.addToGrid(0, 0, spawn);
        this.expand(0, 0); 
        for(let delta of deltas) {
            this.queue.add({p: {x: 0, y: 0}, d: delta});
        }
    }
    sprawl(): void {
        let new_queue: Set<any> = new Set();
        for(let head of this.queue.values()) {
            let tInfo = this.atGrid(head.p.x, head.p.y);

            //if we've already linked in this direction then continue
            if(tInfo.links.has(d2v(head.d))) continue;
            let tarPos = {x: head.p.x + head.d.x, y: head.p.y + head.d.y};
            let gInfo = this.atGrid(tarPos.x, tarPos.y);

            let long = dist([gInfo.pos.x, gInfo.pos.y], [tInfo.pos.x, tInfo.pos.y]);
            let a = Math.atan2((gInfo.pos.y-tInfo.pos.y),(gInfo.pos.x-tInfo.pos.x));

            let lastNode = this.addNode(tInfo.pos, a).node.id;

            let merged = false;
            let inc = 1/this.detail;
            for(let i = 1; i <= this.detail; i++) {
                let newNodeInfo = this.addNode({
                    x: this.nodes[lastNode].pos.x + inc*long*Math.cos(a),//(gInfo.pos.x - tInfo.pos.x), 
                    y: this.nodes[lastNode].pos.y + inc*long*Math.sin(a)}//(gInfo.pos.y - tInfo.pos.y)}
                , a);

                this.linkNode(lastNode, newNodeInfo.node.id);
                lastNode = newNodeInfo.node.id;
                merged = newNodeInfo.merged;
                if(merged) break;
            }

            if(!merged) { //reached target position successfully
                tInfo.links.add(d2v(head.d));
                this.setGrid(head.p.x, head.p.y, tInfo);
                gInfo.links.add(d2v({x: -head.d.x, y: -head.d.y}));
                this.setGrid(tarPos.x, tarPos.y, gInfo);

                this.expand(tarPos.x, tarPos.y);
                for(const delta of deltas) {
                    if(!gInfo.links.has(d2v(delta))) {
                        new_queue.add({p: tarPos, d: delta});
                    }
                }
            }
        }
        this.queue = new_queue;
        this.create();
    }
    addToGrid(x: number, y: number, pos: {x: number, y: number}) {
        if(!(x in this.grid)) this.grid[x] = {};
        if(!(y in this.grid[x])) {
            this.grid[x][y] = {links: new Set(), pos: pos};
            this.gridcount++;
        }
    }
    isInGrid(x: number, y: number): boolean {
        if(!(x in this.grid)) return false;
        return (y in this.grid[x]);
    }
    atGrid(x: number, y: number) {
        if(!this.isInGrid(x, y)) return null;
        return this.grid[x][y];
    }
    setGrid(x: number, y: number, e: any) {
        this.grid[x][y] = e;
    }
    expand(x: number, y: number) { //initiates neighboring grid coords
        let thisPos = this.atGrid(x, y).pos;
        for(let delta of deltas) {
            let dx = (this.blocks.minx + Math.random()*this.blocks.varx)*delta.x;
            let dy = (this.blocks.miny + Math.random()*this.blocks.vary)*delta.y;
            let ux = {x: Math.cos(this.slant)*dx, y: Math.sin(this.slant)*dx};
            let uy = {x: Math.cos(this.slant+Math.PI/2)*dy, y: Math.sin(this.slant+Math.PI/2)*dy};
            this.addToGrid(x+delta.x, y+delta.y, {x: thisPos.x + ux.x + uy.x, y: thisPos.y + ux.y + uy.y});
        }
    }
}

export default Sprawler;