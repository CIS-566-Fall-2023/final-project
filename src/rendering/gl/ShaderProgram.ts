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
    this.unifMagic      = gl.getUniformLocation(this.prog, "u_Magic");
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

  setGeometryColor(color: vec4) {
    this.use();
    if (this.unifColor !== -1) {
      gl.uniform4fv(this.unifColor, color);
    }
  }

  setColor(main: vec4, middle: vec4, front: vec4) {
    this.setGeometryColor(main);
    this.use();
    if(this.unifMiddleColor != -1) {
      gl.uniform4fv(this.unifMiddleColor, middle);
    }
    if(this.unifFrontColor != -1) {
      gl.uniform4fv(this.unifFrontColor, front);
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

  setMagic(magic: boolean) {
    this.use();
    if(this.unifMagic !== -1) {
      if(magic) {
        gl.uniform1i(this.unifMagic, 1);
      }
      else 
      {
        gl.uniform1i(this.unifMagic, 0);
      }
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
};

export default ShaderProgram;
