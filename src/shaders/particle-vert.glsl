#version 300 es

//This is a vertex shader. While it is called a "shader" due to outdated conventions, this file
//is used to apply matrix transformations to the arrays of vertex data passed to it.
//Since this code is run on your GPU, each vertex is transformed simultaneously.
//If it were run on your CPU, each vertex would have to be processed in a FOR loop, one at a time.
//This simultaneous transformation allows your program to run much faster, especially when rendering
//geometry with millions of vertices.

#define POSITION_LOCATION 2
#define VELOCITY_LOCATION 3
#define ID_LOCATION 4

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


in vec4 vs_Pos;             // Not used. The array of vertex positions passed to the shader
in vec4 vs_Nor;             // Not used. The array of vertex normals passed to the shader
in vec4 vs_Col;             // The array of vertex colors passed to the shader.

uniform float u_Time;

out vec4 fs_Pos;
out vec4 fs_Col;            // The color of each vertex. This is implicitly passed to the fragment shader.


// On the CPU side, when I created a VAO, I described each attribute by saying
// "this data in this buffer will be attribute 0, the data next to it wil be 
// attribute 1, etc." The VAO only stores this information of the location of 
// this buffer's attributes. The vertex data is stored in the VBO. 
// This line vvv gets the attribute located at POSTION, and puts it IN the 
// specified variable. The location specifies the number of the attribute  
layout(location = POSITION_LOCATION) in vec3 updated_pos;

void main()
{
    fs_Col = vs_Col;                         // Pass the vertex colors to the fragment shader for interpolation
    fs_Pos = vs_Pos;
    vec4 modelposition = u_Model * vs_Pos;   // Temporarily store the transformed vertex positions for use below

    // Put the position of the model (particle) in line with the refernce frame of the camera
    vec3 alignedBillboard = modelposition.x * u_CameraAxes[0] + modelposition.y * u_CameraAxes[1];
    
    // Add the new updated particle position to the aligned billboard
    vec3 particlePos = updated_pos + alignedBillboard;

    gl_Position = u_ViewProj * vec4(alignedBillboard, 1.0);
    // gl_Position is a built-in variable of OpenGL which is
    // used to render the final positions of the geometry's vertices
}
