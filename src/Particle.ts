import {vec3} from 'gl-matrix';
import {gl} from './globals';

class Particle 
{
    p: vec3;
    v: vec3;
    a: vec3;

    constructor(position: vec3, velocity: vec3, acceleration: vec3)
    {
        this.p = position;
        this.v = velocity;
        this.a = acceleration;
    }

    update(deltaTime: number)
    {
        // update position
        let deltaPos = vec3.create();
        vec3.scale(deltaPos, this.v, deltaTime);
        vec3.add(this.p, deltaPos, this.p);

        // update velocity
        let deltaVel = vec3.create();
        vec3.scale(deltaVel, this.a, deltaTime);
        vec3.add(this.v, deltaVel, this.v);
    }
}

const POSITION_LOCATION = 2;
const VELOCITY_LOCATION = 3;
const ID_LOCATION = 4;

const NUM_LOCATIONS = 5;

class ParticlesGroup 
{
    numParticles: number;
    positions: Float32Array;
    velocities: Float32Array;

    particleIDs: Float32Array;

    particleVBOs: WebGLBuffer[][];
    particleVAOs: WebGLVertexArrayObject[]; // Store attributes about each particle, in assoiation with the particle's buffer

    constructor(numParticles: number)
    {
        this.numParticles = numParticles;
        this.positions = new Float32Array(this.numParticles * 3);
        this.velocities = new Float32Array(this.numParticles * 3);

        this.particleIDs  = new Float32Array(this.numParticles);

        // The VAO gives us room to bind these attributes to the VBO
        // [particle, attribute]
        this.particleVAOs = [gl.createVertexArray(), gl.createVertexArray()];
    }

    create() 
    {
        // set defaults for all particles in ParticlesGroup
        for (let i = 0; i < this.numParticles; i++)
        {
            this.particleIDs[i] = i;

            this.positions[i*3] = 0.0;
            this.positions[i*3 + 1] = 0.0;
            this.positions[i*3 + 2] = 0.0;

            this.velocities[i*3] = 0.0;
            this.velocities[i*3 + 1] = 0.0;
            this.velocities[i*3 + 2] = 0.0;
        }
    }

    setVBOs()
    {
        this.particleVBOs = new Array(this.particleVAOs.length);

        for (let i = 0; i < this.particleVAOs.length; ++i)
        {
            // each VBO is an array with a number of attributes
            this.particleVBOs[i] = new Array(NUM_LOCATIONS);

            gl.bindVertexArray(this.particleVAOs[i]);

            this.particleVBOs[i][POSITION_LOCATION] = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, this.particleVBOs[i][POSITION_LOCATION]);
            gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);
            gl.vertexAttribPointer(POSITION_LOCATION, 3, gl.FLOAT, false, 0, 0);
            gl.enableVertexAttribArray(POSITION_LOCATION);
            
            this.particleVBOs[i][VELOCITY_LOCATION] = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, this.particleVBOs[i][VELOCITY_LOCATION]);
            gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);
            gl.vertexAttribPointer(VELOCITY_LOCATION, 3, gl.FLOAT, false, 0, 0);
            gl.enableVertexAttribArray(VELOCITY_LOCATION);
            
            this.particleVBOs[i][ID_LOCATION] = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, this.particleVBOs[i][ID_LOCATION]);
            gl.bufferData(gl.ARRAY_BUFFER, this.positions, gl.STATIC_DRAW);
            gl.vertexAttribPointer(ID_LOCATION, 1, gl.FLOAT, false, 0, 0);
            gl.enableVertexAttribArray(ID_LOCATION);

            gl.bindBuffer(gl.ARRAY_BUFFER, null);

            gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 0, this.particleVBOs[i][POSITION_LOCATION]);
            gl.bindBufferBase(gl.TRANSFORM_FEEDBACK_BUFFER, 1, this.particleVBOs[i][VELOCITY_LOCATION]);
        }
    }
}
export {Particle, ParticlesGroup};
