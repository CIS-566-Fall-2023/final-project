import {vec2, vec3, vec4, mat4, mat3} from 'gl-matrix';
import Drawable from './Drawable';
import Camera from '../../Camera';
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
  unifModel: WebGLUniformLocation;
  unifModelInvTr: WebGLUniformLocation;
  unifViewProj: WebGLUniformLocation;
  unifDimensions: WebGLUniformLocation;
  unifColor: WebGLUniformLocation;

  unifCameraAxes: WebGLUniformLocation;

  unifAcceleration: WebGLUniformLocation;
  unifParticleCol: WebGLUniformLocation;

  unifObstaclePos: WebGLUniformLocation;
  unifObstacleSize: WebGLUniformLocation;

  constructor(shaders: Array<Shader>, isTransformFeedback: boolean = false, variable_buffer_data: string[] = []) {
    this.prog = gl.createProgram();

    for (let shader of shaders) {
      gl.attachShader(this.prog, shader.shader);
    }

    if(isTransformFeedback == true){
      gl.transformFeedbackVaryings(this.prog, variable_buffer_data, gl.SEPARATE_ATTRIBS);
    }

    gl.linkProgram(this.prog);
    if (!gl.getProgramParameter(this.prog, gl.LINK_STATUS)) {
      throw gl.getProgramInfoLog(this.prog);
    }

    this.attrPos = gl.getAttribLocation(this.prog, "vs_Pos");
    this.attrNor = gl.getAttribLocation(this.prog, "vs_Nor");
    this.attrCol = gl.getAttribLocation(this.prog, "vs_Col");
    this.unifTime       = gl.getUniformLocation(this.prog, "u_Time");
    this.unifModel      = gl.getUniformLocation(this.prog, "u_Model");
    this.unifModelInvTr = gl.getUniformLocation(this.prog, "u_ModelInvTr");
    this.unifViewProj   = gl.getUniformLocation(this.prog, "u_ViewProj");
    this.unifDimensions = gl.getUniformLocation(this.prog, "u_Dimensions");
    this.unifColor      = gl.getUniformLocation(this.prog, "u_Color");
    this.unifCameraAxes = gl.getUniformLocation(this.prog, "u_CameraAxes");
  
    this.unifAcceleration = gl.getUniformLocation(this.prog, 'u_Acceleration');
    this.unifParticleCol = gl.getUniformLocation(this.prog, "u_ParticleColor");

    this.unifObstaclePos = gl.getUniformLocation(this.prog, "u_ObstaclePos");
    this.unifObstacleSize = gl.getUniformLocation(this.prog, "u_ObstacleSize");

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

  setDimensions(w: number, h: number)
  {
    this.use();
    if (this.unifDimensions !== -1) {
      gl.uniform2fv(this.unifDimensions, vec2.fromValues(w, h));
    }
  }

  setTime(t: number) {
    this.use();
    if (this.unifTime !== -1) {
      gl.uniform1f(this.unifTime, t);
    }
  }

  setCameraAxes(axes: mat3) {
    this.use();
    if (this.unifCameraAxes !== -1) {
      gl.uniformMatrix3fv(this.unifCameraAxes, false, axes);
    }
  }

  setObstaclePos(pos: vec2, camera: Camera) {
    this.use();
    if (this.unifObstaclePos !== -1) {
      gl.uniform2fv(this.unifObstaclePos, pos);
    }
  }

  setObstacleSize(s: number) {
    this.use();
    if (this.unifObstacleSize !== -1)
    {
      gl.uniform1f(this.unifObstacleSize, s);
    }
  }

  // TRANSFORM FEEDBACK
  setParticleColor(color: vec3) {
    this.use();
    if (this.unifParticleCol !== -1) {
      gl.uniform3fv(this.unifParticleCol, color);
    }
  }

  setParticleAcceleration(a: vec3) {
    this.use();
    if (this.unifAcceleration !== -1) {
      let scale = 5.0;
      gl.uniform3f(this.unifAcceleration, scale * a[0], scale * a[1], scale * a[2]);
    }
  }

  setColor(color: vec4) {
    this.use();
    if (this.unifColor !== -1) {
      gl.uniform4fv(this.unifColor, color);
    }
  }

  // DRAWING

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
  drawParticles(d: Drawable, numParticles: number)
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
    
    // Instead of drawElements, we need to called drawElementsInstanced because we are using
    // instanced rendering, so we can draw multiple of these particles in one draw call.
    
    gl.drawElementsInstanced(d.drawMode(), d.elemCount(), gl.UNSIGNED_INT, 0, numParticles);
    // drawElementsInstanced uses the vertexAttribDivisor for each "in" variable to determine 
    // how to link it to each drawn instance of the bound VBO. For example, the index used to 
    // look in the VBO associated with vs_Pos (attrPos) is advanced by 1 for each thread of the 
    // GPU running the vertex shader since its divisor is 0.
    // On the other hand, the index used to look in the VBO associated with vs_Translate 
    // (attrTranslate) is advanced by 1 only when the next instance of our drawn object 
    // (the square) is processed by the GPU, thus being the same value for the first set of four 
    // vertices, then advancing to a new value for the next four, then the next four, and so on.
  
    if (this.attrPos != -1) gl.disableVertexAttribArray(this.attrPos);
    if (this.attrNor != -1) gl.disableVertexAttribArray(this.attrNor);
    if (this.attrCol != -1) gl.disableVertexAttribArray(this.attrCol);
  }
};

export default ShaderProgram;
