import {vec4, vec3, mat4} from 'gl-matrix';
import Drawable from './Drawable';
import {gl} from '../../globals';

var activeProgram: WebGLProgram = null;

export class Shader {
  shader: WebGLShader;

  constructor(type: number, source: string) {
    this.shader = gl.createShader(type);
    gl.shaderSource(this.shader, source);
    gl.compileShader(this.shader);

    if (!gl.getShaderParameter(this.shader, gl.COMPILE_STATUS)) {
      throw gl.getShaderInfoLog(this.shader);
    }
  }
};

class ShaderProgram {
  prog: WebGLProgram;

  attrPos: number;
  attrNor: number;
  attrCol: number;
  // For this implementation, 'attrCol' is an instanced rendering attribute
  // Instance Rendering means we can render multiple instances in a single draw call
  // and provide each instance with some unique attributes. 
  // Each particle will have a slightly different color, so it needs to be instanced.

  
  unifTime: WebGLUniformLocation;
  unifSpeed: WebGLUniformLocation;
  unifTailSize: WebGLUniformLocation;
  unifMagic: WebGLUniformLocation;
  unifModel: WebGLUniformLocation;
  unifModelInvTr: WebGLUniformLocation;
  unifViewProj: WebGLUniformLocation;
  unifColor: WebGLUniformLocation;
  unifMiddleColor: WebGLUniformLocation;
  unifFrontColor: WebGLUniformLocation;

  constructor(shaders: Array<Shader>) {
    this.prog = gl.createProgram();

    for (let shader of shaders) {
      gl.attachShader(this.prog, shader.shader);
    }
    gl.linkProgram(this.prog);
    if (!gl.getProgramParameter(this.prog, gl.LINK_STATUS)) {
      throw gl.getProgramInfoLog(this.prog);
    }

    this.attrPos = gl.getAttribLocation(this.prog, "vs_Pos");
    this.attrNor = gl.getAttribLocation(this.prog, "vs_Nor");
    this.attrCol = gl.getAttribLocation(this.prog, "vs_Col");
    this.unifTime       = gl.getUniformLocation(this.prog, "u_Time");
    this.unifSpeed      = gl.getUniformLocation(this.prog, "u_Speed");
    this.unifTailSize   = gl.getUniformLocation(this.prog, "u_TailSize");
    this.unifModel      = gl.getUniformLocation(this.prog, "u_Model");
    this.unifModelInvTr = gl.getUniformLocation(this.prog, "u_ModelInvTr");
    this.unifViewProj   = gl.getUniformLocation(this.prog, "u_ViewProj");
    this.unifColor      = gl.getUniformLocation(this.prog, "u_Color");
    this.unifMiddleColor      = gl.getUniformLocation(this.prog, "u_MiddleColor");
    this.unifFrontColor      = gl.getUniformLocation(this.prog, "u_FrontColor");
  }

  use() {
    if (activeProgram !== this.prog) {
      gl.useProgram(this.prog);
      activeProgram = this.prog;
    }
  }

  setModelMatrix(model: mat4) {
    this.use();
    if (this.unifModel !== -1) {
      gl.uniformMatrix4fv(this.unifModel, false, model);
    }

    if (this.unifModelInvTr !== -1) {
      let modelinvtr: mat4 = mat4.create();
      mat4.transpose(modelinvtr, model);
      mat4.invert(modelinvtr, modelinvtr);
      gl.uniformMatrix4fv(this.unifModelInvTr, false, modelinvtr);
    }
  }

  setViewProjMatrix(vp: mat4) {
    this.use();
    if (this.unifViewProj !== -1) {
      gl.uniformMatrix4fv(this.unifViewProj, false, vp);
    }
  }

  setParticleColor(color: vec4) {
    this.use();
    if (this.unifColor !== -1) {
      gl.uniform4fv(this.unifColor, color);
    }
  }

  setTime(time: GLfloat) {
    this.use();
    if (this.unifTime !== -1) {
      gl.uniform1f(this.unifTime, time);
    }
  }

  setSpeed(speed: GLfloat) {
    this.use();
    if(this.unifSpeed !== -1) {
      gl.uniform1f(this.unifSpeed, speed);
    }
  }

  setTailSize(tail: GLfloat) {
    this.use();
    if(this.unifTailSize !== -1) {
      gl.uniform1f(this.unifTailSize, tail);
    }
  }

  draw(d: Drawable) {
    this.use();

    if (this.attrPos != -1 && d.bindPos()) {
      gl.enableVertexAttribArray(this.attrPos);
      gl.vertexAttribPointer(this.attrPos, 4, gl.FLOAT, false, 0, 0);
    }

    if (this.attrNor != -1 && d.bindNor()) {
      gl.enableVertexAttribArray(this.attrNor);
      gl.vertexAttribPointer(this.attrNor, 4, gl.FLOAT, false, 0, 0);
    }

    d.bindIdx();
    gl.drawElements(d.drawMode(), d.elemCount(), gl.UNSIGNED_INT, 0);

    if (this.attrPos != -1) gl.disableVertexAttribArray(this.attrPos);
    if (this.attrNor != -1) gl.disableVertexAttribArray(this.attrNor);
  }

  // This draw function acts similarly to the above draw function
  // It is what actually gets the data from the Drawable and populates
  // the VBOs with the correct data. We have to make one specifically
  // for particles, because of all their extra properties.
  // drawParticles still takes in a Drawable SQUARE because each particle
  // is a tiny instance of square particle texture.
  // Next, drawParticles also takes in the number of particles such 
  // that we can draw every one. 
  drawParticle(d: Drawable, numParticles: number)
  {
    this.use();

    if (this.attrPos != -1 && d.bindPos())
    {
      gl.enableVertexAttribArray(this.attrPos);
      gl.vertexAttribPointer(this.attrPos, 4, gl.FLOAT, false, 0, 0);
      gl.vertexAttribDivisor(this.attrPos, 0); // 0 instances will pass between updates of this attribute
    }

    if (this.attrNor != -1 && d.bindNor())
    {
      gl.enableVertexAttribArray(this.attrNor);
      gl.vertexAttribPointer(this.attrNor, 4, gl.FLOAT, false, 0, 0);
      gl.vertexAttribDivisor(this.attrNor, 0); // 0 instances will pass between updates of this attribute
    }
    
    if (this.attrCol != -1 && d.bindCol())
    {
      gl.enableVertexAttribArray(this.attrCol);
      gl.vertexAttribPointer(this.attrCol, 4, gl.FLOAT, false, 0, 0);
      gl.vertexAttribDivisor(this.attrCol, 1); // 1 instances will pass between updates of this attribute
    }

    d.bindIdx();
    
    // Instead of drawElements, we need to called drawElementsInstanced because
    // we are using instanced rendering, so we can draw multiple of these particles
    // in one draw call
    gl.drawElementsInstanced(d.drawMode(), d.elemCount(), gl.UNSIGNED_INT, 0, numParticles);
  
    if (this.attrPos != -1) gl.disableVertexAttribArray(this.attrPos);
    if (this.attrNor != -1) gl.disableVertexAttribArray(this.attrNor);
    if (this.attrCol != -1) gl.disableVertexAttribArray(this.attrCol);

  }
};

export default ShaderProgram;
