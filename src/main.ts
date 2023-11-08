import {vec3, vec4} from 'gl-matrix';
import * as DAT from 'dat.gui';
import Icosphere from './geometry/Icosphere';
import Square from './geometry/Square';
import Cube from './geometry/Cube';
import {Particle, ParticlesGroup} from './Particle';

import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';

let time = 0.0;

const controls = {
  Speed: 0.4,
  Tail_Size: 4.0,
  Particle_Color: [ 255, 0, 0 ],
  Middle_Color: [ 255, 178.5, 0 ],
  Front_Color: [ 0, 0, 255 ],
};

let icosphere: Icosphere;
let square: Square;
let cube: Cube;
let particles: ParticlesGroup;

function loadScene() {
  icosphere = new Icosphere(vec3.fromValues(0, 0, 0), 1, 5);
  icosphere.create();
  square = new Square(vec3.fromValues(0, 0, 0));
  square.create();
  cube = new Cube(vec3.fromValues(0, 0, 0));
  cube.create();

  particles = new ParticlesGroup(10);
  particles.create();
  particles.setVBOs();
}


function main() {
  const gui = new DAT.GUI();
  gui.add(controls, 'Speed', 0.025, 0.5).step(0.005).name("Meteor Speed");
  gui.add(controls, 'Tail_Size', 2.0, 10.0).step(1).name("Tail Size");
  gui.addColor(controls, 'Particle_Color').name("Main Color");
  gui.addColor(controls, 'Middle_Color').name("Middle Color");
  gui.addColor(controls, 'Front_Color').name("Tip/Front Detail Color");

  const canvas = <HTMLCanvasElement> document.getElementById('canvas');
  const gl = <WebGL2RenderingContext> canvas.getContext('webgl2');
  if (!gl) {
    alert('WebGL 2 not supported!');
  }
  setGL(gl);

  loadScene();

  const camera = new Camera(vec3.fromValues(0, 0, 5), vec3.fromValues(0, 0, 0));

  const renderer = new OpenGLRenderer(canvas);
  renderer.setClearColor(0.1, 0.1, 0.1, 1);
  gl.enable(gl.DEPTH_TEST);

  const lambert = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/lambert-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/lambert-frag.glsl')),
  ]);
  
  // This function will be called every frame
  function tick() {
    camera.update();
    time = time + 1.0;

    gl.viewport(0, 0, window.innerWidth, window.innerHeight);
    renderer.clear();
    renderer.render(camera, lambert, 
      [icosphere],
      time,
      controls.Speed,
      controls.Tail_Size,
      vec4.fromValues(controls.Particle_Color[0]/255.0,controls.Particle_Color[1]/255.0,controls.Particle_Color[2]/255.0,1),
      vec4.fromValues(controls.Middle_Color[0]/255.0,controls.Middle_Color[1]/255.0,controls.Middle_Color[2]/255.0,1),
      vec4.fromValues(controls.Front_Color[0]/255.0,controls.Front_Color[1]/255.0,controls.Front_Color[2]/255.0,1),
    );

    // Tell the browser to call `tick` again whenever it renders a new frame
    requestAnimationFrame(tick);
  }

  window.addEventListener('resize', function() {
    renderer.setSize(window.innerWidth, window.innerHeight);
    camera.setAspectRatio(window.innerWidth / window.innerHeight);
    camera.updateProjectionMatrix();
  }, false);

  renderer.setSize(window.innerWidth, window.innerHeight);
  camera.setAspectRatio(window.innerWidth / window.innerHeight);
  camera.updateProjectionMatrix();

  // Start the render loop
  tick();
}

main();
