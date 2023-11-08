# Final Project: Waterfall Particle Simulation

### Design Doc

#### Introduction
With the experience that the Procedural Graphics course has given me, my eyes have been opened to the many different ways that proceduralism can contribute to art. I am especially interested in procedural animation. Although a broad category, being able to conduct some animations procedurally can help artists with tedious work, that would be exhaustive to do my hand. For example, rain. Imagine if an artist had to hand animate every raindrop in a storm, or every droplet in a waterfall! In this project, I seek to find a procedural way to tackle this challenge. 

#### Goal
In this project, I hope to use this opportunity to expand my skills in procedural animation and particle simulation. I intend to implement an animated waterfall using particles. Each droplet of the waterfall will be generated procedurally and randomly. I hope to make the generation of the waterfall also customizable by the artist. Then, they will fall into the scene initially uninterrupted. To extend the artist's capabilities, they can draw obstacles in the scene that the particles will collide with as they continue down the waterfall. 

#### Inspiration/reference:
- I hope to implement the particle animation outlined in this paper ![Particle Animation and Rendering Using Data Parallel Computation](https://www.karlsims.com/papers/ParticlesSiggraph90.pdf) by Karl Sims. The final section on falling water will be especially illuminating. Below are some reference images from the paper:
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
- I will be implementing this paper Particle Animation and Rendering Using Data Parallel Computation](https://www.karlsims.com/papers/ParticlesSiggraph90.pdf) by Karl Sims, which essentially outlines a method for particle simulation via the parallel computing of the GPU.
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
Begin implementing your engine! Don't worry too much about polish or parameter tuning -- this week is about getting together the bulk of your generator implemented. By the end of the week, even if your visuals are crude, the majority of your generator's functionality should be done.

Put all your code in your forked repository.

Submission: Add a new section to your README titled: Milestone #1, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what's giving you trouble?
- Examples of your generators output so far
We'll check your repository for updates. No need to create a new pull request.
## Milestone 3: Implementation part 2 (due 11/27)
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

## Topic Suggestions

### Create a generator in Houdini

### A CLASSIC 4K DEMO
- In the spirit of the demo scene, create an animation that fits into a 4k executable that runs in real-time. Feel free to take inspiration from the many existing demos. Focus on efficiency and elegance in your implementation.
- Example: 
  - [cdak by Quite & orange](https://www.youtube.com/watch?v=RCh3Q08HMfs&list=PLA5E2FF8E143DA58C)

### A RE-IMPLEMENTATION
- Take an academic paper or other pre-existing project and implement it, or a portion of it.
- Examples:
  - [2D Wavefunction Collapse Pokémon Town](https://gurtd.github.io/566-final-project/)
  - [3D Wavefunction Collapse Dungeon Generator](https://github.com/whaoran0718/3dDungeonGeneration)
  - [Reaction Diffusion](https://github.com/charlesliwang/Reaction-Diffusion)
  - [WebGL Erosion](https://github.com/LanLou123/Webgl-Erosion)
  - [Particle Waterfall](https://github.com/chloele33/particle-waterfall)
  - [Voxelized Bread](https://github.com/ChiantiYZY/566-final)

### A FORGERY
Taking inspiration from a particular natural phenomenon or distinctive set of visuals, implement a detailed, procedural recreation of that aesthetic. This includes modeling, texturing and object placement within your scene. Does not need to be real-time. Focus on detail and visual accuracy in your implementation.
- Examples:
  - [The Shrines](https://github.com/byumjin/The-Shrines)
  - [Watercolor Shader](https://github.com/gracelgilbert/watercolor-stylization)
  - [Sunset Beach](https://github.com/HanmingZhang/homework-final)
  - [Sky Whales](https://github.com/WanruZhao/CIS566FinalProject)
  - [Snail](https://www.shadertoy.com/view/ld3Gz2)
  - [Journey](https://www.shadertoy.com/view/ldlcRf)
  - [Big Hero 6 Wormhole](https://2.bp.blogspot.com/-R-6AN2cWjwg/VTyIzIQSQfI/AAAAAAAABLA/GC0yzzz4wHw/s1600/big-hero-6-disneyscreencaps.com-10092.jpg)

### A GAME LEVEL
- Like generations of game makers before us, create a game which generates an navigable environment (eg. a roguelike dungeon, platforms) and some sort of goal or conflict (eg. enemy agents to avoid or items to collect). Aim to create an experience that will challenge players and vary noticeably in different playthroughs, whether that means procedural dungeon generation, careful resource management or an interesting AI model. Focus on designing a system that is capable of generating complex challenges and goals.
- Examples:
  - [Rhythm-based Mario Platformer](https://github.com/sgalban/platformer-gen-2D)
  - [Pokémon Ice Puzzle Generator](https://github.com/jwang5675/Ice-Puzzle-Generator)
  - [Abstract Exploratory Game](https://github.com/MauKMu/procedural-final-project)
  - [Tiny Wings](https://github.com/irovira/TinyWings)
  - Spore
  - Dwarf Fortress
  - Minecraft
  - Rogue

### AN ANIMATED ENVIRONMENT / MUSIC VISUALIZER
- Create an environment full of interactive procedural animation. The goal of this project is to create an environment that feels responsive and alive. Whether or not animations are musically-driven, sound should be an important component. Focus on user interactions, motion design and experimental interfaces.
- Examples:
  - [The Darkside](https://github.com/morganherrmann/thedarkside)
  - [Music Visualizer](https://yuruwang.github.io/MusicVisualizer/)
  - [Abstract Mesh Animation](https://github.com/mgriley/cis566_finalproj)
  - [Panoramical](https://www.youtube.com/watch?v=gBTTMNFXHTk)
  - [Bound](https://www.youtube.com/watch?v=aE37l6RvF-c)

### YOUR OWN PROPOSAL
- You are of course welcome to propose your own topic . Regardless of what you choose, you and your team must research your topic and relevant techniques and come up with a detailed plan of execution. You will meet with some subset of the procedural staff before starting implementation for approval.
