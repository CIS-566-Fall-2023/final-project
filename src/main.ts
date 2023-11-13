import {vec3, vec4} from 'gl-matrix';
import * as DAT from 'dat.gui';
import Square from './geometry/Square';
import {Particle, ParticlesGroup} from './Particle';

import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';


const controls = {
  Particle_Color: [0, 0, 255],
  Gravity: 30.0,
};

let time: number = 0.0;
let square: Square;
let particles: ParticlesGroup;

function loadScene() {
  square = new Square();
  square.create();

  particles = new ParticlesGroup(1000);
  particles.create();
  particles.setVBOs();

  square.setNumInstances(1); // Grid of Particles
}

function main() {
  const gui = new DAT.GUI();
  gui.addColor(controls, 'Particle_Color').name("Particle Color").onChange(setParticleColor);
  gui.add(controls, 'Gravity', 1.0, 100.0).step(1.0).onChange(setParticleAcceleration);

  const canvas = <HTMLCanvasElement> document.getElementById('canvas');
  const gl = <WebGL2RenderingContext> canvas.getContext('webgl2');
  if (!gl) {
    alert('WebGL 2 not supported!');
  }
  setGL(gl);

  loadScene();

  // Create Camera
  const camera = new Camera(vec3.fromValues(0, 0, -100.0), vec3.fromValues(0, -10, 0));

  // Create Renderer
  const renderer = new OpenGLRenderer(canvas);
  renderer.setClearColor(0.1, 0.1, 0.1, 1);
  
  // GL Settings
  gl.disable(gl.CULL_FACE);
  gl.enable(gl.DEPTH_TEST);
  gl.disable(gl.BLEND);
  gl.clearColor(0.1, 0.1, 0.1, 1);
  gl.blendFunc(gl.ONE, gl.ONE); // Additive blending

  // Create Particle Shader
  const particleShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/particle-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/particle-frag.glsl')),
  ]);

  // Transform Feedback for Particles
  // Transform Feedback is the process for capturing Primitives from the Vertex
  // Processing steps, recording that data in Buffer Objects, which allows one to
  // resubmit data multiple times. Transform Feedback allows shaders to write vertices
  // back to VBOs. We are using them to update the changing variables like position, 
  // velocity, acceleration, and color back to the buffer as they change.  
  let variable_buffer_data = ["v_pos", "v_vel", "v_col", "v_time"];
  const transformFeedbackShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/particle-transfeed-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/particle-transfeed-frag.glsl')),
    ], true, variable_buffer_data
  );

  // SETTER FUNCTIONS
  function setParticleColor() {
    transformFeedbackShader.setParticleColor(vec3.fromValues(
      controls.Particle_Color[0]/255.0,
      controls.Particle_Color[1]/255.0,
      controls.Particle_Color[2]/255.0
    ));
  }

  function setParticleAcceleration() {
    transformFeedbackShader.setParticleAcceleration(vec3.fromValues(0.0, controls.Gravity, 0.0));
  }

  // SET SHADER VALUES
  transformFeedbackShader.setColor(vec4.fromValues(0.0, 1.0, 1.0, 1.0));
  setParticleColor();
  setParticleAcceleration();
  
  // This function will be called every frame
  function tick() {
    // Update Camera and Time
    camera.update();
    time = time + 1.0;
    transformFeedbackShader.setTime(time);
    particleShader.setTime(time);

    // Render objects using Renderers
    gl.viewport(0, 0, window.innerWidth, window.innerHeight);

    renderer.clear();

    // Blend the uncolored part of the square with the background of the image, making a circle
    gl.enable(gl.BLEND); 

    renderer.transformParticles(camera, transformFeedbackShader, [particles]);

    renderer.renderParticles(camera, particleShader, square, [particles]);

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
