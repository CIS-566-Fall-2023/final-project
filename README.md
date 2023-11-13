# Final Project: Waterfall Particle Simulation

### Design Doc

#### Introduction
With the experience that the Procedural Graphics course has given me, my eyes have been opened to the many different ways that proceduralism can contribute to art. I am especially interested in procedural animation. Although a broad category, being able to conduct some animations procedurally can help artists with tedious work, that would be exhaustive to do my hand. For example, rain. Imagine if an artist had to hand animate every raindrop in a storm, or every droplet in a waterfall! In this project, I seek to find a procedural way to tackle this challenge. 

#### Goal
In this project, I hope to use this opportunity to expand my skills in procedural animation and particle simulation. I intend to implement an animated waterfall using particles. Each droplet of the waterfall will be generated procedurally and randomly. I hope to make the generation of the waterfall also customizable by the artist. Then, they will fall into the scene initially uninterrupted. To extend the artist's capabilities, they can draw obstacles in the scene that the particles will collide with as they continue down the waterfall.

#### Inspiration/reference:
- I hope to implement the particle animation outlined in this paper [Particle Animation and Rendering Using Data Parallel Computation](https://www.karlsims.com/papers/ParticlesSiggraph90.pdf) by Karl Sims. The final section on falling water will be especially illuminating. Below are some reference images from the paper:
![image](https://github.com/kyraSclark/final-project/assets/60115638/81f98d21-6a28-4246-aa5e-dad71ca9ac99)
![image](https://github.com/kyraSclark/final-project/assets/60115638/73699d48-dffb-416d-98d2-112a2ec6ed79)
![image](https://github.com/kyraSclark/final-project/assets/60115638/3472580f-5805-4b9b-ade7-171edb9a3a05)

- I am inspired by former student Chloe Le, and [her implementation](https://github.com/chloele33/particle-waterfall). Not only do I intend to implement something similar to her version, but I also hope to extend her implementation to include a more procedural and customizable generation of water initially. Below are some reference images from her implementation:
![image](https://github.com/kyraSclark/final-project/assets/60115638/2d987c78-3554-4b4a-a3cd-7e3659222c9a)
![image](https://github.com/kyraSclark/final-project/assets/60115638/e2e78f57-f1ee-4cb0-85cc-ec590d733526)


#### Specification:
- Upon opening the program, there will be a waterfall of particles falling down the screen.
- Many aspects of this scene will be customizable like particle size, particle color, and physical forces like gravity.
- The user can move the camera to change the perspective of the scene, like zooming out or rotating.
- The user can draw obstacles to the waterfall, which the particles will bounce off of.
- When the particles bounce, their color changes.
- The user has control over the size and visibility of the drawn obstacles.
- There will be an additional parameter to customize the noise at which the particles are generated to the scene, affecting the way the particles enter the scene. 

#### Techniques:
- I will be implementing this paper [Particle Animation and Rendering Using Data Parallel Computation](https://www.karlsims.com/papers/ParticlesSiggraph90.pdf) by Karl Sims, which essentially outlines a method for particle simulation via the parallel computing of the GPU.
- The particles will be generated and controlled using vertex and fragment shaders. 

#### Design:
- The main function has the controls and generates the scene. Based on these specifications, we set the particle size, color, forces, obstacle size etc.
- The main function checks for the user's mouse controls and adds obstacles or changes the camera accordingly.
- The main function holds the particles for the scene via a Particle and Particle Collection class, such that all transformations and renderings can be applied to each particle. 
- Every tick, we render the scene, transform the particles, and render the particles.
- When rendering, there are particle vertex and fragment shaders and obstacles vertex and frag shaders.
- The obstacle vert shader collects the position of the obstacle. Its vertex shader colors the obstacles according to the parameters set in the controls.
- The particle vert shader generates the particles based on the specified noise, and controls the particle's position based on the simulation from its physical forces. Finally, the particle frag shader colors the point.
![image](https://github.com/kyraSclark/final-project/assets/60115638/27c99502-19db-4dc4-9dfa-11a8f71b0313)

#### Timeline:
##### 11/08: Design Doc Due
At this stage, I have completed my initial research of the project and completed the design doc. 

##### 11/15: Milestone 1 Due
Implement the basic waterfall physics with basic controls. My goal is that I can generate random particles on the screen and have them move around (preferably fall down). I will also implement some basic controls, like controlling the color of the particles.  

##### 11/27: Milestone 2 Due
Implement obstacles and collision physics. At this point, all physical simulations should be complete, including gravity acceleration, collisions, etc. It might be rudimentary, but it should work at its core. This checkpoint should mark the completion of the "engine". 

##### 12/04: Final Project Due 
Implement customizable generation control and polish. Now, all the GUI controls should be implemented and it should have a polished look. We should be able to control how and if the obstacles appear, and have control over the particle simulation, color, and generation based on noise. 

## Milestone 1: Implementation part 1 (due 11/15)

[LIVE DEMO](https://kyrasclark.github.io/final-project/)

My original goal for this week was to implement the basic waterfall physics with basic control. The goal was that the final output should have random particles on the screen, perhaps moving, with some basic control over them like particle color. 

This week, I was able to accomplish this goal. First, as I dove into my research, I was really overwhelmed at first by how to implement this. In my head, I pictured some implementations of particle simulation I have seen and built before, but I had not fully comprehended what that would look like when using the parallel computing on the GPU. I was faced with a lot more academic challenges than I expected. Instead of diving into implementation like I expected, I spent much of this week studying GPU programming techniques that were completely new to me. But as a result, I learned about the following techniques which proved usuful in my later implementation:
- **VAOs**: VAOs are Vertex Array Objects. They store attributes about each particle, in assoiation with the particle's buffer. Such that each buffer object, has a list of attributes encapsulating it's buffer's current state. On the CPU side, when I created a VAO, I described each attribute by saying "this data in this buffer will be attribute 0 (position), the data next to it will be attribute 1 (velocity), etc." The VAO only stores this information of the location of this buffer's attributes. On the other hand, the vertex data is stored in the VBO.
- **Instanced Rendering**: Instance Rendering means we can render multiple instances in a single draw call and provide each instance with some unique attributes. Things that are instanced rendered (like the particles) have instanced rendering attributes like color. That means that each particle will have a slightly different color, so it needs to be instanced.
- **Billboard**: A billboard is a textured polygon (usually a quad) used for drawing particles, such that elements with low-level detail will always we drawn plane-aligned, facing the camera.
- **Transform Feedback**: Transform Feedback is the process for capturing Primitives from the Vertex Processing steps, recording that data in Buffer Objects, which allows one to resubmit data multiple times. Transform Feedback allows shaders to write vertices back to VBOs. We are using them to update the changing variables like position, velocity, and color back to the buffer as they change. This is also where instanced rendering comes in, as each of these changing buffers is an instanced attribute. 

After finishing my research from the paper, other tutorials and implementations, and personal research about OpenGL GPU programming techniques that were new to me, I began my implementation work. I started with code from hw1, to make a simple rendered sphere. 

![m1](https://github.com/kyraSclark/final-project/assets/60115638/370b7509-b282-498c-9529-a3e306ade454)

Then, after create my Particle class, I implemented a particle vert and frag shader that relied on the square Drawable. The frag shader colored in a circular shape on square, to create a "particle".

![m1 1](https://github.com/kyraSclark/final-project/assets/60115638/5421f220-bd27-4fb4-8532-931f575b8631)

Then, I adjusted the camera parameters to make the particle the correct size. I also implemented instanced rendering here and implemented customizable coloring.

![m1 21](https://github.com/kyraSclark/final-project/assets/60115638/1154a2cf-4d9e-46a3-954e-503cc4394daf)

Finally, I created the Transform Feedback shaders to realize the many many particles all moving around. To start, these shaders initialize the particles randomly, but they are affected by gravity.  

![m1 3](https://github.com/kyraSclark/final-project/assets/60115638/f9a68485-1cdb-4328-8aec-4af49fe6cda8)

Now, we have completed milestone1, as you can see in this [live demo](https://kyrasclark.github.io/final-project/)!
The Gravity slider still does not work. Next week, I will work on obstacles and collision/bounce physics! 

## Milestone 2: Implementation part 2 (due 11/27)
We're over halfway there! This week should be about fixing bugs and extending the core of your generator. Make sure by the end of this week _your generator works and is feature complete._ Any core engine features that don't make it in this week should be cut! Don't worry if you haven't managed to exactly hit your goals. We're more interested in seeing proof of your development effort than knowing your planned everything perfectly. 

Put all your code in your forked repository.

Submission: Add a new section to your README titled: Milestone #3, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what did you have to cut and why? 
- Detailed output from your generator, images, video, etc.
We'll check your repository for updates. No need to create a new pull request.

Come to class on the due date with a WORKING COPY of your project. We'll be spending time in class critiquing and reviewing your work so far.

## Final submission (due 12/5)
Time to polish! Spen this last week of your project using your generator to produce beautiful output. Add textures, tune parameters, play with colors, play with camera animation. Take the feedback from class critques and use it to take your project to the next level.

Submission:
- Push all your code / files to your repository
- Come to class ready to present your finished project
- Update your README with two sections 
  - final results with images and a live demo if possible
  - post mortem: how did your project go overall? Did you accomplish your goals? Did you have to pivot?
