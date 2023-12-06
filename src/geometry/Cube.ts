import {vec3, vec4} from 'gl-matrix';
import Drawable from '../rendering/gl/Drawable';
import {gl} from '../globals';

class Cube extends Drawable {
  indices: Uint32Array;
  positions: Float32Array;
  normals: Float32Array;
  center: vec4;
  side: number;

  constructor(center: vec3, side: number) {
    super(); // Call the constructor of the super class. This is required.
    this.center = vec4.fromValues(center[0], center[1], center[2], 1);
    this.side = side;
  }

  create() {

  const X0 = this.center[0]-this.side/2;
  const X1 = this.center[0]+this.side/2;
  const Y0 = this.center[1]-this.side/2;
  const Y1 = this.center[1]+this.side/2;
  const Z0 = this.center[2]-this.side/2;
  const Z1 = this.center[2]+this.side/2;

  this.indices = new Uint32Array([0, 1, 2,
                                  2, 3, 0,

                                  4, 5, 6,
                                  6, 7, 4,

                                  8, 9, 10,
                                  10, 11, 8,

                                  12, 13, 14,
                                  14, 15, 12,

                                  16, 17, 18,
                                  18, 19, 16,

                                  20, 21, 22,
                                  22, 23, 20
                                ]);
  this.normals = new Float32Array([0, 0, 1, 0,
                                   0, 0, 1, 0,
                                   0, 0, 1, 0,
                                   0, 0, 1, 0,
                                  
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                  
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                   1, 0, 0, 0,
                                  
                                   0, 0, 1, 0,
                                   0, 0, 1, 0,
                                   0, 0, 1, 0,
                                   0, 0, 1, 0,
                                  
                                   0, 1, 0, 0,
                                   0, 1, 0, 0,
                                   0, 1, 0, 0,
                                   0, 1, 0, 0,
                                  
                                   0, 1, 0, 0,
                                   0, 1, 0, 0,
                                   0, 1, 0, 0,
                                   0, 1, 0, 0]);
  this.positions = new Float32Array([X1, Y1, Z1, 1,
                                     X1, Y0, Z1, 1,
                                     X0, Y0, Z1, 1,
                                     X0, Y1, Z1, 1,

                                     X1, Y1, Z0, 1,
                                     X1, Y0, Z0, 1,
                                     X1, Y0, Z1, 1,
                                     X1, Y1, Z1, 1,
                                    
                                     X0, Y1, Z1, 1,
                                     X0, Y0, Z1, 1,
                                     X0, Y0, Z0, 1,
                                     X0, Y1, Z0, 1,

                                     X0, Y1, Z0, 1,
                                     X0, Y0, Z0, 1,
                                     X1, Y0, Z0, 1,
                                     X1, Y1, Z0, 1,
                                    
                                     X1, Y1, Z0, 1,
                                     X1, Y1, Z1, 1,
                                     X0, Y1, Z1, 1,
                                     X0, Y1, Z0, 1,
                                    
                                     X1, Y0, Z1, 1,
                                     X1, Y0, Z0, 1,
                                     X0, Y0, Z0, 1,
                                     X0, Y0, Z1, 1,]);
                                     

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

    console.log(`Created cube`);
  }
};

export default Cube;
