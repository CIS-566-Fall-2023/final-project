import {vec3, vec4} from 'gl-matrix';
import * as DAT from 'dat.gui';
import Icosphere from './geometry/Icosphere';
import Square from './geometry/Square';
import Cube from './geometry/Cube';

import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';

let time = 0.0;
let magic = false;

const controls = {
  Speed: 0.4,
  Tail_Size: 4.0,
  Main_Color: [ 255, 0, 0 ],
  Middle_Color: [ 255, 178.5, 0 ],
  Front_Color: [ 0, 0, 255 ],
  'Magic Meteor': magicMedeor,
  'Restore Defaults': restoreDefaults,
};

let icosphere: Icosphere;
let square: Square;
let cube: Cube;

function loadScene() {
  icosphere = new Icosphere(vec3.fromValues(0, 0, 0), 1, 5);
  icosphere.create();
  square = new Square(vec3.fromValues(0, 0, 0));
  square.create();
  cube = new Cube(vec3.fromValues(0, 0, 0));
  cube.create();
}

function magicMedeor() {
  magic = !magic;
  if(magic){
    controls.Main_Color = [ 255, 0, 255 ];
    controls.Middle_Color = [ 76.5, 0, 255 ];
    controls.Front_Color = [ 0, 255, 0 ];
  } else {
    controls.Main_Color = [ 255, 0, 0 ];
    controls.Middle_Color = [ 255, 178.5, 0 ];
    controls.Front_Color = [ 0, 0, 255 ];
  }
}

function restoreDefaults() {
  controls.Speed = 0.4;
  controls.Tail_Size = 4.0;
  magic = false;
  controls.Main_Color = [ 255, 0, 0 ];
  controls.Middle_Color = [ 255, 178.5, 0 ];
  controls.Front_Color = [ 0, 0, 255 ];
}

function main() {
  const gui = new DAT.GUI();
  gui.add(controls, 'Speed', 0.025, 0.5).step(0.005).name("Meteor Speed");
  gui.add(controls, 'Tail_Size', 2.0, 10.0).step(1).name("Tail Size");
  gui.addColor(controls, 'Main_Color').name("Main Color");
  gui.addColor(controls, 'Middle_Color').name("Middle Color");
  gui.addColor(controls, 'Front_Color').name("Tip/Front Detail Color");
  gui.add(controls, 'Magic Meteor');
  gui.add(controls, 'Restore Defaults');

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
    new Shader(gl.VERTEX_SHADER, require('./shaders/fire-vert.glsl')),
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/fire-frag.glsl')),
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
      magic,
      vec4.fromValues(controls.Main_Color[0]/255.0,controls.Main_Color[1]/255.0,controls.Main_Color[2]/255.0,1),
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
