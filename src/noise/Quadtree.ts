//121 dejavu
class QTNode {
    children: any;
    center: {x: number, y: number};
    size: {x: number, y: number};
    data: any;

    constructor(center: {x: number, y: number}, size: {x: number, y: number}) {
        this.children = null;
        this.center = center;
        this.size = size;
        this.data = [];
    }

    addElement(x: number, y: number, element: any, div: number) {
        if(div == 0) {
            this.data.push(element);
            return;
        }

        if(this.children == null) {
            let newsize = {x: this.size.x/2, y: this.size.y/2};
            this.children = [
                [new QTNode({x: this.center.x - this.size.x/2, y: this.center.y - this.size.y/2}, newsize), new QTNode({x: this.center.x - this.size.x/2, y: this.center.y + this.size.y/2}, newsize)],
                [new QTNode({x: this.center.x + this.size.x/2, y: this.center.y - this.size.y/2}, newsize), new QTNode({x: this.center.x + this.size.x/2, y: this.center.y + this.size.y/2}, newsize)]
            ];
        }

        let a = 0;
        if(x > this.center.x) a = 1;
        let b = 0;
        if(y > this.center.y) b = 1;

        this.children[a][b].addElement(x, y, element, div-1);
    }
    getElement(x: number, y: number) {
        if(this.children == null) {
            if(this.data.length == 0) return null;
            else return this.data[0];
        }
        else {
            let a = 0;
            if(x > this.center.x) a = 1;
            let b = 0;
            if(y > this.center.y) b = 1;

            return this.children[a][b].getElement(x, y);
        }
    }
}
class QuadTree {
    root: QTNode;
    size: {x: number, y: number};
    divs: number;
    constructor(size: {x: number, y: number}, divs: number) {
        this.size = size;
        this.divs = divs;
        this.root = new QTNode(
            {x: this.size.x/2, y: this.size.y/2}, 
            {x: this.size.x/2, y: this.size.y/2}
        );
    }
    addElement(x: number, y: number, element: any) {
        this.root.addElement(x, y, element, this.divs);
    }
    getElement(x: number, y: number): any{
        return this.root.getElement(x, y);
    }
}

export default QuadTree;