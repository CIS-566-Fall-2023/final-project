import {vec2, vec3, vec4} from 'gl-matrix';
import * as DAT from 'dat.gui';
import Square from './geometry/Square';
import {ParticlesGroup} from './Particle';
import ScreenBuffer from './geometry/ScreenBuffer';

import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL, FBO} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';

 
const controls = {
  Particle_Color: [0, 72, 255],
  num_particles: 10000,
  Particle_Size : 0.7,
  Gravity: 30.0,
  Wind: 0.0,
  'Noisy Wind': false,
  Obstacle_Size: 30.0,
  'Show Obstacles': true,
  'Lock Camera': true,
  Default_Gen: default_generation,
  FBM_Gen: fbm_generation,
  fbm_freq: 1.0,
  fbm_amp: 0.5,
};

let time: number = 0.0;
let camera_locked = true;
let show_obstacles = true;

let particles: ParticlesGroup;
let square: Square;           // for each particle
let screenBuf: ScreenBuffer;  // for obstacles color
let screenBufP: ScreenBuffer; // for obstacles area

let obstacle_positions: Array<vec2>;
obstacle_positions = new Array<vec2>();
obstacle_positions.push(vec2.fromValues(0.5, 0.5)); // Default starting obstacle

let generation_type: number = 0.0;
// 0 = DEFAULT GENERATION
// 1 = FBM GENERATION

function default_generation()
{
  generation_type = 0.0;
}

function fbm_generation()
{
  generation_type = 1.0;
}

function loadScene() {
  square = new Square();
  square.create();

  screenBuf = new ScreenBuffer(0, 0, 1, 1);
  screenBuf.create();
  screenBufP = new ScreenBuffer(-0.5, -0.5, 0.5, 0.5);
  screenBufP.create();

  particles = new ParticlesGroup(controls.num_particles);
  particles.create();
  particles.setVBOs();

  square.setNumInstances(1); // Grid of Particles
}

function main() {
  const gui = new DAT.GUI();
  gui.addColor(controls, 'Particle_Color').name("Particle Color").onChange(setParticleColor);
  gui.add(controls, 'num_particles', 100, 50000).step(10).name("Number of Particles").onChange(loadScene);
  gui.add(controls, 'Particle_Size', 0.4, 2.0).step(0.1).name("Particle Size").onChange(setParticleSize);
  gui.add(controls, 'Gravity', 1.0, 100.0).step(1.0).onChange(setParticleAcceleration);
  gui.add(controls, 'Wind', -30.0, 30.0).step(1.0).onChange(setParticleAcceleration);
  gui.add(controls, 'Noisy Wind').onChange(setNoisyWind);
  gui.add(controls, 'Obstacle_Size', 5.0, 200.0).step(1.0).name("Obstacle Size").onChange(setObstacleSize);
  gui.add(controls, 'Show Obstacles').onChange(showObstacles);
  gui.add(controls, 'Lock Camera').onChange(lockCamera);
  gui.add(controls, 'Default_Gen').name('Default Particle Generation').onChange(setGenerationType);;
  gui.add(controls, 'FBM_Gen').name('FBM Particle Generation').onChange(setGenerationType);
  gui.add(controls, 'fbm_freq', 0.1, 15.0).step(0.1).name("FBM Frequency").onChange(setFBMFreq);
  gui.add(controls, 'fbm_amp', 0.12, 1.0).step(0.02).name("FBM Amplitude").onChange(setFBMAmp);

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
  gl.disable(gl.DEPTH_TEST);
  gl.disable(gl.BLEND);
  gl.clearColor(0.1, 0.1, 0.1, 1);
  gl.blendFunc(gl.ONE, gl.ONE); // Additive blending

  // Create Particle Shader
  const particleShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/particle-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/particle-frag.glsl')),
  ]);

  const obstacleBufferShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/obstacle-buf-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/obstacle-buf-frag.glsl')),
  ], false, ["sampleCoords"]);

  const addObstacleShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/obstacle-add-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/obstacle-add-frag.glsl')),
  ], false, ["from Center"]);
  
  const obstacleAddToBufferShader = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/obstacle-add-to-buf-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/obstacle-add-to-buf-frag.glsl')),
  ], false, ["sampleCoords"]);


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
      controls.Particle_Color[0] / 255.0,
      controls.Particle_Color[1] / 255.0,
      controls.Particle_Color[2] / 255.0
    ));
  }

  function setParticleSize() {
    particleShader.setParticleSize(controls.Particle_Size);
  }

  function setParticleAcceleration() {
    transformFeedbackShader.setParticleAcceleration(vec3.fromValues(controls.Wind, controls.Gravity, 0.0));
  }

  function setNoisyWind()
  {
    transformFeedbackShader.setNoisyWind(controls["Noisy Wind"]);
  }

  function showObstacles()
  {
    show_obstacles = controls["Show Obstacles"];
    if (show_obstacles)
    {
      obstacleBufferShader.setShowObstacles(1.0);
    }
    else 
    {
      obstacleBufferShader.setShowObstacles(0.0);
    }
  }

  function setObstacleSize()
  {
    addObstacleShader.setObstacleSize(controls.Obstacle_Size);
    obstacleAddToBufferShader.setObstacleSize(controls.Obstacle_Size);
  }

  function lockCamera()
  {
    camera_locked = controls["Lock Camera"];
  }

  function setGenerationType()
  {
    transformFeedbackShader.setGenerationType(generation_type);
  }

  function setFBMFreq()
  {
    transformFeedbackShader.setFBMFreq(controls.fbm_freq);
  }

  function setFBMAmp()
  {
    transformFeedbackShader.setFBMAmp(controls.fbm_amp);
  }

  function setupTexture(width: number, height: number)
  {
    let texelData: any = [];
    let obstacleColor = [127, 127, 0, 0];
    // Add color to every texture cell
    for (let i = 0; i < width * height; ++i)
    {
      texelData.push.apply(texelData, obstacleColor);
    }

    // GL work to set up a texture
    let texture = gl.createTexture();
    gl.bindTexture(gl.TEXTURE_2D, texture); // bind texture to this texture slot
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, width, height, 0, gl.RGBA, gl.UNSIGNED_BYTE, new Uint8Array(texelData));
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
    return texture;
  }

  // SET SHADER VALUES
  transformFeedbackShader.setColor(vec4.fromValues(0.0, 1.0, 1.0, 1.0));
  setParticleColor();
  setParticleSize();
  setParticleAcceleration();
  setObstacleSize();
  showObstacles();
  lockCamera();
  setGenerationType();
  setFBMFreq();
  setFBMAmp();

  // This function will be called every frame
  function tick() {
    // Update Camera
    if (camera_locked)
    {
      camera.reset(vec3.fromValues(0, 0, -100.0), vec3.fromValues(0.0, -10, 0));
    }
    camera.update();

    // Update time
    time = time + 1.0;
    transformFeedbackShader.setTime(time);

    // Render objects using Renderers
    gl.viewport(0, 0, window.innerWidth, window.innerHeight);

    // clear current frame buffer (old obstacle data)
    gl.bindFramebuffer(gl.FRAMEBUFFER, null);
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
    // activate texture slot with obstacle texture
    gl.activeTexture(gl.TEXTURE0);
    gl.bindTexture(gl.TEXTURE_2D, texture);

    renderer.clear();
    
    gl.enable(gl.BLEND); // Blend the uncolored part of the square with the background of the image, making a circle
    gl.disable(gl.DEPTH_TEST) // We do not want to see the particle billboards
    
    renderer.renderParticles(camera, particleShader, square, [particles]);
    renderer.transformParticles(camera, transformFeedbackShader, [particles]);
    
    gl.disable(gl.BLEND); // We do not want the obstacles to blend in 
    gl.enable(gl.DEPTH_TEST)

    renderer.renderObs(camera, obstacleBufferShader, [screenBuf]);
    
    gl.enable(gl.BLEND); // Blend the uncolored part of the square with the background of the image, making a circle

    // Tell the browser to call `tick` again whenever it renders a new frame
    requestAnimationFrame(tick);
  }

  window.addEventListener('resize', function() {
    renderer.setSize(window.innerWidth, window.innerHeight);
    camera.setAspectRatio(window.innerWidth / window.innerHeight);
    camera.updateProjectionMatrix();

    addObstacleShader.setDimensions(window.innerWidth, window.innerHeight);
    obstacleAddToBufferShader.setDimensions(window.innerWidth, window.innerHeight);
    
  }, false);

  renderer.setSize(window.innerWidth, window.innerHeight);
  camera.setAspectRatio(window.innerWidth / window.innerHeight);
  camera.updateProjectionMatrix();


  // INITIALIZE TEXTURE AND FRAME BUFFER FOR OBSTACLES
  gl.viewport(0, 0, window.innerWidth, window.innerHeight);
  gl.bindFramebuffer(gl.FRAMEBUFFER, null);
  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
  var width = gl.drawingBufferWidth;
  var height = gl.drawingBufferHeight;

  var texture = setupTexture(width, height);
  let _FBO = FBO.create(gl, width, height);

  addObstacleShader.setDimensions(width, height);
  obstacleAddToBufferShader.setDimensions(width, height);

  gl.enable(gl.BLEND); // Blends away the null parts of the obstacle textures

  // OBSTACLE-USER INTERACTION CODE 
  function addObstacle(x: number, y: number)
  {
    addObstacleShader.setObstaclePos(vec2.fromValues(x, 1.0 - y));
    gl.useProgram(addObstacleShader.prog);
    _FBO.bind(gl, texture, null);

    renderer.renderObs(camera, addObstacleShader, [screenBufP]);
    gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);
    gl.bindFramebuffer(gl.FRAMEBUFFER, null);
    gl.bindTexture(gl.TEXTURE_2D, null);


    obstacleAddToBufferShader.setObstaclePos(vec2.fromValues(x, 1.0 - y));
    gl.useProgram(obstacleAddToBufferShader.prog);
    _FBO.bind(gl, texture, null);

    gl.activeTexture(gl.TEXTURE1);
    gl.bindTexture(gl.TEXTURE_2D, texture);

    renderer.renderObs(camera, obstacleAddToBufferShader, [screenBuf]);
    gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);
    gl.bindFramebuffer(gl.FRAMEBUFFER, null);
    gl.bindTexture(gl.TEXTURE_2D, null);
  }

  var rightClick = 2;
  var isMouseDragging = false;

  canvas.onmousedown = function(event) // ADD OBSTACLE
  {
    if(event.button === rightClick && camera_locked && show_obstacles)
    {
      if(isMouseDragging)
      {
        addObstacle((event.clientX / window.innerWidth), (event.clientY / window.innerHeight));
      }
    }
    transformFeedbackShader.setObstaclePos(vec2.fromValues(
      (2.0 * event.clientX / window.innerWidth) - 1.0,
      1.0 - (2.0 * event.clientY / window.innerHeight)
    ));

    isMouseDragging = true;
  }

  for (let i = 0; i < obstacle_positions.length; i++)
  {
    addObstacle(obstacle_positions[i][0], obstacle_positions[i][0]);
  }

  canvas.onmouseup = function(event)
  {
    if(event.button === rightClick && camera_locked && show_obstacles)
    {
      obstacle_positions.push(vec2.fromValues(
        (event.clientX / window.innerWidth),
        (event.clientY / window.innerHeight)
      ));
    }
    isMouseDragging = false;
  }

  canvas.onmousemove = function(event)
  {
    if(isMouseDragging && camera_locked && show_obstacles)
    {
      addObstacle((event.clientX / window.innerWidth), (event.clientY / window.innerHeight));
    }
  }

  // Start the render loop
  tick();
}

main();
