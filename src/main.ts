import {vec3, vec4} from 'gl-matrix';
const Stats = require('stats-js');
import * as DAT from 'dat.gui';
// import Icosphere from './geometry/Icosphere';
// import Square from './geometry/Square';
import Terrain from './geometry/Terrain';
import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';

// Define an object with application parameters and button callbacks
// This will be referred to by dat.GUI's functions that add GUI elements.

const controls = {
  terrain: 5,
  color: 0,
  'next tick': time
};

let prevTerrain = 5;

var terrain = new Terrain(
  {x: 500, y: 500, s:0.5, ps: 0.001, h:4},
  {type: "fbm", min: 0, max: 4, ps: 0.0015, hscale: 1, scale: 1}
);  

var doTerrainTick = false;
function time() {
  doTerrainTick = !doTerrainTick;
}

function main() {
  // Initial display for framerate
  const stats = Stats();
  stats.setMode(0);
  stats.domElement.style.position = 'absolute';
  stats.domElement.style.left = '0px';
  stats.domElement.style.top = '0px';
  document.body.appendChild(stats.domElement);

  // Add controls to the gui
  const gui = new DAT.GUI();
  gui.add(controls, 'terrain', 0, 20).step(1);
  gui.add(controls, 'color', { Terrain: 0, Population: 1, Blank: 2} );
  gui.add(controls, 'next tick');

  // get canvas and webgl context
  const canvas = <HTMLCanvasElement> document.getElementById('canvas');
  const gl = <WebGL2RenderingContext> canvas.getContext('webgl2', {alpha:true});
  if (!gl) {
    alert('WebGL 2 not supported!');
  }
  // `setGL` is a function imported above which sets the value of `gl` in the `globals.ts` module.
  // Later, we can import `gl` from `globals.ts` to access it
  setGL(gl);

  // Initial call to load scene
  //TODO

  const camera = new Camera(vec3.fromValues(0, 0, 5), vec3.fromValues(0, 0, 0));

  const renderer = new OpenGLRenderer(canvas);
  renderer.setClearColor(0.2, 0.2, 0.2, 1);
  gl.enable(gl.BLEND);
  gl.blendFunc(gl.ONE, gl.ONE_MINUS_SRC_ALPHA);
  gl.enable(gl.DEPTH_TEST);
  //alpha testing

  var lambert_prog = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/lambert-vert.glsl')), 
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/lambert-frag.glsl'))
  ]);

  var flat_prog = new ShaderProgram([
    new Shader(gl.VERTEX_SHADER, require('./shaders/flat-vert.glsl')), 
    new Shader(gl.FRAGMENT_SHADER, require('./shaders/flat-frag.glsl'))
  ]);

  terrain.create();

  let timetick: number = 0;
  // This function will be called every frame
  function tick() {
    camera.update();
    stats.begin();
    gl.viewport(0, 0, window.innerWidth, window.innerHeight);
    renderer.clear();

    if(prevTerrain != controls.terrain) {
      prevTerrain = controls.terrain;
      terrain.size.h = controls.terrain;
      //terrain.generate();
      terrain.init();
      terrain.create();
    }

    if(doTerrainTick) terrain.tick();

    gl.enable(gl.DEPTH_TEST);
    renderer.render(camera, 
      lambert_prog, 
      [terrain], 
      controls.color
    );
    gl.disable(gl.DEPTH_TEST);
    renderer.render_flat(camera, 
      flat_prog, 
      terrain.roads
    );
    renderer.render_flat(camera, 
      flat_prog, 
      terrain.streets
    );
    renderer.render_flat(camera, 
      flat_prog, 
      terrain.highways
    );
    
    stats.end();

    // Tell the browser to call `tick` again whenever it renders a new frame
    requestAnimationFrame(tick);
    timetick++;
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
