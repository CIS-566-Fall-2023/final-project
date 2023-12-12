import {vec3, vec4} from 'gl-matrix';
import Drawable from '../rendering/gl/Drawable';
import {gl} from '../globals';

class Building extends Drawable {
  indices: Uint32Array;
  positions: Float32Array;
  normals: Float32Array;

  y0: number;
  y1: number;
  xz: any;
  
  constructor(xz: any, y: number, h: number) {
    super(); // Call the constructor of the super class. This is required.
    this.xz = xz;
    this.y0 = y;
    this.y1 = y+h;
  }

  create() {

    let tris = 12;

    this.indices = new Uint32Array(tris*3);
    this.normals = new Float32Array(tris*3*4);
    this.positions = new Float32Array(tris*3*4);

    let c = 0;
    for(let i = 0; i < 4; i++) {
        let i2 = (i+1)%4;

        let p1 = vec3.fromValues(this.xz[i].x, this.y0, this.xz[i].z);
        let p2 = vec3.fromValues(this.xz[i2].x, this.y0, this.xz[i2].z);
        let p3 = vec3.fromValues(this.xz[i2].x, this.y1, this.xz[i2].z);
        let p4 = vec3.fromValues(this.xz[i].x, this.y1, this.xz[i].z);

        for(let tri of [
            [p1, p2, p3], 
            [p2, p4, p3]
        ]) {
            //calculate normal and insert 4 times

            let n = vec3.create();
            let d1 = vec3.create();
            let d2 = vec3.create();
            let nun = vec3.create();
            vec3.sub(d1, tri[0], tri[1]);
            vec3.sub(d2, tri[0], tri[2]);
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
            for(let p of tri) {
                this.positions[c*12 + 4*k + 0] = p[0];
                this.positions[c*12 + 4*k + 1] = p[1];
                this.positions[c*12 + 4*k + 2] = p[2];
                this.positions[c*12 + 4*k + 3] = 1;
                k++;
            }
            c++;
        }
    }

    for(let y of [this.y0, this.y1]) {
        let p1 = vec3.fromValues(this.xz[0].x, y, this.xz[0].z);
        let p2 = vec3.fromValues(this.xz[1].x, y, this.xz[1].z);
        let p3 = vec3.fromValues(this.xz[2].x, y, this.xz[2].z);
        let p4 = vec3.fromValues(this.xz[3].x, y, this.xz[3].z);

        for(let tri of [
            [p1, p2, p3], 
            [p2, p4, p3]
        ]) {
            //calculate normal and insert 4 times

            let n = vec3.create();
            let d1 = vec3.create();
            let d2 = vec3.create();
            let nun = vec3.create();
            vec3.sub(d1, tri[0], tri[1]);
            vec3.sub(d2, tri[0], tri[2]);
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
            for(let p of tri) {
                this.positions[c*12 + 4*k + 0] = p[0];
                this.positions[c*12 + 4*k + 1] = p[1];
                this.positions[c*12 + 4*k + 2] = p[2];
                this.positions[c*12 + 4*k + 3] = 1;
                k++;
            }

            //insert indices
            this.indices[c*3] = c*3;
            this.indices[c*3+1] = c*3+1;
            this.indices[c*3+2] = c*3+2;
            c++;
        }
    }                        

    this.generateIdx();
    this.generatePos();
    this.generateNor();

    this.count = this.indices.length;
    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.bufIdx);
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, this.indices, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufNor);
    gl.bufferData(gl.ARRAY_BUFFER, this.normals, gl.STATIC_DRAW);

    gl.bindBuffer(gl.ARRAY_BUFFER, this.bufPos);
    gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);
    console.log(this.indices, this.normals, this.positions);
  }
};

export default Building;
