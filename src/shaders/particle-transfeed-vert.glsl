#version 300 es

//This is a vertex shader. While it is called a "shader" due to outdated conventions, this file
//is used to apply matrix transformations to the arrays of vertex data passed to it.
//Since this code is run on your GPU, each vertex is transformed simultaneously.
//If it were run on your CPU, each vertex would have to be processed in a FOR loop, one at a time.
//This simultaneous transformation allows your program to run much faster, especially when rendering
//geometry with millions of vertices.

#define POSITION_LOCATION 2
#define VELOCITY_LOCATION 3
#define TIME_LOCATION 5
#define ID_LOCATION 6

uniform mat4 u_Model;       // The matrix that defines the transformation of the
                            // object we're rendering. In this assignment,
                            // this will be the result of traversing your scene graph.

uniform mat4 u_ModelInvTr;  // The inverse transpose of the model matrix.
                            // This allows us to transform the object's normals properly
                            // if the object has been non-uniformly scaled.

uniform mat4 u_ViewProj;    // The matrix that defines the camera's transformation.
                            // We've written a static matrix for you to use for HW2,
                            // but in HW3 you'll have to generate one yourself

uniform mat3 u_CameraAxes;  // A billboard is a textured polygon (usually a quad) used 
                            // for drawing particles, such that elements with low-level 
                            // detail will always we drawn plane-aligned, facing the camera

uniform float u_Time;
uniform vec3  u_Acceleration;
uniform vec3 u_ParticleColor;

uniform sampler2D u_ObstacleBuffer;
uniform vec3 u_ObstaclePos;

uniform float u_GenType;
//uniform float u_FBMFreq;

in vec4 vs_Pos;             // Not used. The array of vertex positions passed to the shader
in vec4 vs_Nor;             // Not used. The array of vertex normals passed to the shader
in vec4 vs_Col;             // Instanced Rendering Attribute. Each particle instance has a 
                            // different color attribute. The array of vertex colors passed to the shader.

out vec4 fs_Pos;
out vec4 fs_Col;            // The color of each vertex. This is implicitly passed to the fragment shader.

// Variable parameters being stored and updated here for transform feedback & instanced attributes
out vec3 v_pos;
out vec3 v_vel;
out vec3 v_col;
out vec2 v_time;

// On the CPU side, when I created a VAO, I described each attribute by saying
// "this data in this buffer will be attribute 0, the data next to it wil be 
// attribute 1, etc." The VAO only stores this information of the location of 
// this buffer's attributes. The vertex data is stored in the VBO. 
// This line vvv gets the attribute located at POSTION, and puts it IN the 
// specified variable. The location specifies the number of the attribute  
layout(location = POSITION_LOCATION) in vec3 current_pos;
layout(location = VELOCITY_LOCATION) in vec3 current_vel;
layout(location = TIME_LOCATION) in vec2 current_time;
layout(location = ID_LOCATION) in float i;


float random2(vec2 p) {
  return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453);
}

float random3(vec3 p) {
  return fract(sin(dot(p, vec3(987.654, 123.456, 531.975))) * 85734.3545);
}

// randomly distributed in a space (this will be the area visible on screen)
vec3 getParticlePos(float spaceSize){
    vec3 position = vec3(
        random2(vec2(i, 1.5 * i)) * spaceSize * 2.0 - spaceSize,
        random2(vec2(i, 2.5 * i)) * spaceSize - spaceSize * 0.5,
        random2(vec2(i, 0.5 * i)) * spaceSize*0.5  - spaceSize*0.25
    );

    return position;
}


float noise3D(vec3 p)
{
        return fract(sin(dot(p, vec3(127.1,269.5, 59.137))) *
                     43758.5453);
}

float interpNoise3D(float x, float y, float z)
{
    int intX = int(floor(x));
    float fractX = fract(x);
    int intY = int(floor(y));
    float fractY = fract(y);
    int intZ = int(floor(z));
    float fractZ = fract(z);

    float v1 = noise3D(vec3(intX, intY, intZ));
    float v2 = noise3D(vec3(intX + 1, intY, intZ));
    float v3 = noise3D(vec3(intX, intY + 1, intZ));
    float v4 = noise3D(vec3(intX + 1, intY + 1, intZ));
    
    float v5 = noise3D(vec3(intX, intY, intZ+1));
    float v6 = noise3D(vec3(intX + 1, intY, intZ+1));
    float v7 = noise3D(vec3(intX, intY + 1, intZ+1));
    float v8 = noise3D(vec3(intX + 1, intY + 1, intZ+1));

    float i1 = mix(v1, v2, fractX);
    float i2 = mix(v3, v4, fractX);
    float i3 = mix(v5, v6, fractX);
    float i4 = mix(v7, v8, fractX);
    
    float j1 = mix(i1, i2, fractY);
    float j2 = mix(i3, i4, fractY);
    
    return mix(j1, j2, fractZ);
}

float fbm(float x, float y, float z, float amp, float freq)
{
    float total = 0.0;
    float persistence = 0.5f;
    int octaves = 8;

    for(int i = 1; i <= octaves; i++) {
        total += interpNoise3D(x * freq,
                               y * freq,
                               z * freq) * amp;

        freq *= 2.f;
        amp *= persistence;
    }
    return total;
}

vec3 getParticlePos_FBM(float spaceSize, float amp, float freq){
    vec3 position = vec3(
        fbm(i, 1.5 * i, 1.5 * i, amp, freq) * spaceSize * 2.0 - spaceSize,
        fbm(i, 2.5 * i, 2.5 * i, amp, freq) * spaceSize - spaceSize * 0.5,
        fbm(i, 0.5 * i, 0.5 * i, amp, freq) * spaceSize * 0.5  - spaceSize * 0.25
    );

    return position;
}

const float MAX_SPEED = 80.0;

void main()
{
    float spaceSize = 100.0;
    float distToCenter = length(current_pos);
    float amp = 0.0;
    float freq = 0.0;

    if (current_time.x == 0.0)
    {
        // create a new particle
        if (u_GenType == 1.0) {
            v_pos = getParticlePos_FBM(spaceSize, amp, freq);
            
            // randomize position and velocity
            vec3 temp = current_pos + v_vel;
            v_pos.x = (fbm(temp.x, temp.y, temp.z, amp, freq) - 0.5) * spaceSize * 2.0;
            v_vel = vec3(fbm(i, 0.0, 0.0, amp, freq), fbm(i, i, i, amp, freq), fbm(2.0 * i, 2.0 * i, 2.0*i, amp, freq));
            v_vel = v_vel - vec3(0.5);
        } 
        else {
            v_pos = getParticlePos(spaceSize);
            v_pos.x = (random3(current_pos + v_vel) - 0.5) * spaceSize * 2.0;
            v_vel = vec3(random2(vec2(i, 0.0)) - 0.5, random2(vec2(i, i)) - 0.5, random2(vec2(2.0 * i, 2.0 * i)) - 0.5);
        }
        v_vel = normalize(v_vel);

        // Color based on a smooth blend based on velocity and position
        float e = length(v_vel) / 150.0;
        float a = smoothstep(0.8, 1.0, length(v_pos.xy));
        vec4 new_color = pow(mix(vec4(u_ParticleColor, 1.0), vec4(0,0,0,0), a), vec4(e));
    
        v_col = new_color.rgb;//+ (1.0 / pow((-(v_pos.y / 1.2) + spaceSize * 0.5) * 0.01, 5.0));

        v_time.x = u_Time;
        //v_time.y = 1000.0;
    }
    else 
    {
        float deltaTime = 0.01;

        vec3 new_v = current_vel + deltaTime * u_Acceleration;
    
        // if Particle is out of bounds
        if (current_pos.y < -spaceSize * 0.5 ) {
            if (u_GenType == 1.0)
            {
               new_v.x = 0.1 * MAX_SPEED * (2.0 * fbm(100.0 * current_pos.x, 100.0 * current_pos.y, 100.0 * current_pos.z, amp, freq) - 1.0);
               vec3 temp1 = current_pos + current_vel;
               new_v.y = MAX_SPEED * fbm(temp1.x, temp1.y, temp1.z, amp, freq);
            }
            else 
            {
                new_v.x = 0.1 * MAX_SPEED * (2.0 * random3(100.0 * current_pos) - 1.0);
                new_v.y = MAX_SPEED * random3(current_pos + current_vel);
            }
        }
        vec2 position_next = vec2(-current_pos.x/(spaceSize*2.0) + 0.5, current_pos.y/spaceSize + 0.6);

        // Color based on a smooth blend based on velocity and position
        float e = length(new_v) / 150.0;
        float a = smoothstep(0.8, 1.0, length(current_pos.xy) / spaceSize);
        vec4 new_color = pow(mix(vec4(u_ParticleColor, 1.0), vec4(0,0,0,0), a), vec4(e));
        v_col = new_color.rgb; //+ (1.0 / pow((-(current_pos.y / 1.2) + spaceSize * 0.5) / 10.0, 5.0));
        
        // Check new position against obstacle buffer
        vec4 tex_color_v = texture(u_ObstacleBuffer, position_next);
        vec2 obstacleNor = 2.0 * tex_color_v.rg - 1.0; // remap

        if (length(obstacleNor) > 0.1)
        {
            // new position has hit an obstacle
            if (length(new_v) < 5.0)
            {
                // If new_v is small, update new_v without disminishng velocity too much 
                // (we are faking some physics here so that we never have particles that are too still)
                // Bounce along obstacle's normal with half the speed
                new_v.xy = obstacleNor * 0.5;

                // Update particle color bc bounce
                v_col = vec3(1.0);
            }
            else 
            {
                // If new_v is large enough, bounce should be a reflection of velocity
                new_v = reflect(vec3(-new_v.x, new_v.y, new_v.z), vec3(obstacleNor, 0.0)) * 2.0;
                new_v *= min(1.0, 0.4 * MAX_SPEED / length(new_v));

                // Update particle color bc bounce
                v_col = vec3(1.0);
            }
        }

        new_v *= min(1.0, 1.2 * MAX_SPEED / length(new_v));
        v_vel = new_v;

        // Velocity has been updated, now update position
        vec3 new_p = current_pos - deltaTime * v_vel;

        // Check if colliding with obstacle (if so, push out of obstacle)
        // Sample Obstacle buffer with padded postion
        vec4 tex_color_p = texture(u_ObstacleBuffer, vec2(-v_pos.x / (spaceSize * 2.0) + 0.5, v_pos.y / (spaceSize) + 0.6));
        vec2 push = 2.0 * tex_color_p.rg - 1.0; 
        // if in obstacle, push will have values, else it will just be 0 and the next calculation will have no effect
        new_p.y += 10.0 * deltaTime * push.y;

        // if Particle new postion is out of bounds
        if (current_pos.y < -spaceSize * 0.5 ) {
            // randomize XZ and move back to top of spaceSize
            if (u_GenType == 1.0)
            {
                vec3 temp2 = new_p + v_vel;
                new_p.x = (fbm(temp2.x, temp2.y, temp2.z, amp, freq) -0.5) * spaceSize * 2.0;//16.0;
                new_p.y += spaceSize + 0.5 * fbm(new_p.x, new_p.y, new_p.z, amp, freq) * (spaceSize - spaceSize*0.5);
                new_p.z = fbm(i, 0.5 * i, 0.0, amp, freq) * spaceSize * 0.5 - spaceSize*0.25;
            }
            else {
                new_p.x = (random3(new_p + v_vel) - 0.5) * spaceSize * 2.0;
                new_p.y += spaceSize + 0.5 * random3(new_p) * (spaceSize - spaceSize*0.5);
                new_p.z = random2(vec2(i, 0.5 * i)) * spaceSize * 0.5 - spaceSize*0.25;
            }
        }
        
        // check again for collisions
        new_p.y += 10.0 * deltaTime * obstacleNor.y;
        new_p.x -= 10.0 * deltaTime * obstacleNor.x;
        
        v_pos = new_p;
        v_time = current_time;
    }

}
