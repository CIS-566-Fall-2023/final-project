import {mat4, vec3, vec4} from 'gl-matrix';
const Stats = require('stats-js');
import * as DAT from 'dat.gui';
// import Icosphere from './geometry/Icosphere';
// import Square from './geometry/Square';
import Terrain from './geometry/Terrain';
import OpenGLRenderer from './rendering/gl/OpenGLRenderer';
import Camera from './Camera';
import {setGL} from './globals';
import ShaderProgram, {Shader} from './rendering/gl/ShaderProgram';
import Sprawler from './geometry/Sprawler';
import Building from './geometry/Building';
import Cube from './geometry/Cube';

// Define an object with application parameters and button callbacks
// This will be referred to by dat.GUI's functions that add GUI elements.
var terrain = new Terrain(
  {x: 500, y: 500, s:0.5, ps: 0.001, h:5},
  {type: "fbm", min: 0, max: 4, ps: 0.0015, hscale: 1, scale: 1}
);

const controls = {
  'terrain size': 500,
  'terrain scaling': 5,
  'population scaling': 10,
  'pop height factor': 1,
  'generate terrain': tinit,
  'block size': 1,
  'street spawn spacing': 250,
  'max highways': 10,
  color: 0,
  'play/pause road generator': time,
  'toggle buildings': bdraw
};

var cube = new Cube(vec3.fromValues(0, 0, 0), 0.5);

var doTerrainTick = false;
var drawBuildings = false;

function time() {
  doTerrainTick = !doTerrainTick;
  if(terrain.stage == 5) { //reset the roadmap if simulation is done
    terrain.init();
    doTerrainTick = true;
  }
}
function bdraw() {
  drawBuildings = !drawBuildings;
}
function tinit() {
  doTerrainTick = false;
  terrain.init();
  terrain.create();
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
  gui.add(controls, 'terrain size', 50, 1000);
  gui.add(controls, 'terrain scaling', 0, 20).step(1);
  gui.add(controls, 'population scaling', 0, 20).step(1);
  gui.add(controls, 'pop height factor', -2, 2);
  gui.add(controls, 'generate terrain');
  gui.add(controls, 'block size', 0, 2);
  gui.add(controls, 'street spawn spacing', 100, 500);
  gui.add(controls, 'max highways', 1, 20).step(1);
  gui.add(controls, 'play/pause road generator');
  gui.add(controls, 'color', { Terrain: 0, Population: 1, Blank: 2} );
  gui.add(controls, 'toggle buildings');

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

  //terrain.create();
  cube.create();

  let m1 = mat4.create();
  let m2 = mat4.create();
  let m3 = mat4.create();
  mat4.fromScaling(m1, vec3.fromValues(10, 10, 10));
  mat4.fromRotation(m2, Math.PI/4, vec3.fromValues(0, 1, 0));
  // pp1[0] + dx.x * (0.5+i)/2 + dy.x * (0.5+j)/2, minh+thisPop/2, pp1[2] + dx.y * (0.5+i)/2 + dy.y * (0.5+j)/2));
  mat4.fromTranslation(m3, vec3.fromValues(0, 0, 0));
  let m = mat4.create();
  mat4.mul(m, m1, m);
  mat4.mul(m, m2, m);
  mat4.mul(m, m3, m);

  let timetick: number = 0;
  // This function will be called every frame
  function tick() {
    camera.update();
    stats.begin();
    gl.viewport(0, 0, window.innerWidth, window.innerHeight);
    renderer.clear();

      terrain.size.x = controls['terrain size'];
      terrain.size.y = controls['terrain size'];
      terrain.size.h = controls['terrain scaling'];
      terrain.pop.scale = controls['population scaling']/10;
      terrain.pop.hscale = controls['pop height factor'];
      //terrain.generate();
      terrain.street_spacer = controls['street spawn spacing'];
      terrain.block_mult = controls['block size'];
      terrain.max_highways = controls['max highways'];


    if(doTerrainTick) {
      terrain.tick();
      // testSprawl.sprawl();
      // doTerrainTick = false;
    }

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

    if(drawBuildings) {
      gl.enable(gl.DEPTH_TEST);
      renderer.render_super(camera,
        lambert_prog,
        cube,
        terrain.buildings);
      

      // renderer.render_super(camera,
      //   lambert_prog,
      //   cube,
      // testCube);
    }
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
