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
  Particle_Color: [ 255, 0, 0 ],
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
  //particles.setVBOs();
}


function main() {
  const gui = new DAT.GUI();
  gui.addColor(controls, 'Particle_Color').name("Main Color");

  const canvas = <HTMLCanvasElement> document.getElementById('canvas');
  const gl = <WebGL2RenderingContext> canvas.getContext('webgl2');
  if (!gl) {
    alert('WebGL 2 not supported!');
  }
  setGL(gl);

  loadScene();

  // Create Camera
  const camera = new Camera(vec3.fromValues(0, 0, 5), vec3.fromValues(0, 0, 0));

  // Create Renderer
  const renderer = new OpenGLRenderer(canvas);
  renderer.setClearColor(0.1, 0.1, 0.1, 1);
  gl.enable(gl.DEPTH_TEST);

  // Create Shaders
  const particleShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/particle-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/particle-frag.glsl')),
  ]);

  const lambert = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/lambert-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/lambert-frag.glsl')),
  ]);
  
  // This function will be called every frame
  function tick() {
    // Update Camera and Time
    camera.update();
    time = time + 1.0;

    // Render objects using Renderers
    gl.viewport(0, 0, window.innerWidth, window.innerHeight);
    renderer.clear();
    renderer.render(camera, lambert, 
      [icosphere],
      vec4.fromValues(controls.Particle_Color[0]/255.0,controls.Particle_Color[1]/255.0,controls.Particle_Color[2]/255.0,1),
    );

    renderer.render(camera, lambert, 
      [cube],
      vec4.fromValues(controls.Particle_Color[0]/255.0,controls.Particle_Color[1]/255.0,controls.Particle_Color[2]/255.0,1),
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
