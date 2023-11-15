# Final Project!

## Project planning: Design Doc (due 11/8)
#### Introduction
<img src="https://github.com/xchennnw/final-project/blob/main/img/train1.png" height="200px"/> <img src="https://github.com/xchennnw/final-project/blob/main/img/train2.png" height="200px"/><br/>
Train view is cool for anyone who wants to relax and enjoy the natural terrain winding out of the train window. In the game, we may generate a series of landscapes procedurally in an aesthetic pleasant style. Even better, the game will generate a mix of terrain which may be impossible in reality to create a fantastic viewpoint of the virtual world!

#### Goal
We would like to provide a train view generator in Unity to simulate the passenger’s view point inside a large-window landscape express. We will provide a terrain generator to procedurally generate an infinitely-extending terrain landscape and a stylized shader to mimic the style of several pieces of 2D concept art. The project will finally be able to deliver an infinite tour aside by procedural stylized terrain view. 
<img src="https://github.com/xchennnw/final-project/blob/main/img/train3.png" height="300px"/>

#### Inspiration/reference:
- Endless Terrain generator: We’re going to refer to some endless terrain generator blog and tutorial to generate the most diverse types of terrain.
    - [Procedural Landmass Generation](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)
    - [Infinite Procedural Terrain in Unity](https://www.youtube.com/watch?v=f9uueg_AUZs)
    - [Create INFINITE Realistic world in Unreal Engine](https://www.youtube.com/watch?v=KbVpX60-A1g)
    - [Infinite world generation in Unreal Engine](https://www.youtube.com/watch?v=pX4pNfcEfA0)

- Stylized shader
    - [Reference images](https://www.instagram.com/oseo____o/?igshid=OGQ5ZDc2ODk2ZA%3D%3D)
<img src="https://github.com/xchennnw/final-project/blob/main/img/ref.png" height="400px"/>

#### Specification:
- Infinite procedural terrain generator along train track.
    - Different terrain biome.
    - Object scattering for biomes.
- Procedurally generate some of the objects (trees, grass, etc)
- Procedural skybox
    - Changes color theme with respect to current biome
    - Mountains in the distance 
    - Sun, moon, stars with animation
- Cloud (To be decided: being a part of skybox or using other method)
- Stylized shader of 2D concept arts.


#### Techniques:
- Terrain generation
    - Unity Terrain / Hexagon grid / Coded mesh
    - Noise sampling
    - Dynamically generate & delete 
- Object generation
    - Tree: L-system
    - Grass: compute shader
    - Stones: vertex shader
- Object scattering
    - Noise sampling
    - Object pooling
- Further research
    - Smooth interpolation between biomes
    - Cloud
 

#### Design:
<img src="https://github.com/xchennnw/final-project/blob/main/img/design.png" width="600px"/>

#### Timeline:
|  |  Janet  | Yue| 
|---|---|---|
|Milestone 1 (11.8 - 11.15) | Research on stylized shader; Terrain biome  | Research on real-time procedural terrain generator|
|Milestone 2 (11.15 - 11.27)| Biome generation; Stone and grass generation;|  Skybox amd cloud & Tree and any other object; Object scattering|
|Final (11.27 - 12.5)| Better biome interpolation; Polish stylized shader | Overall controlling and interaction|

---
## Milestone 1: Implementation part 1 (due 11/15)
[<img src="https://github.com/xchennnw/final-project/blob/main/img/train_view_milestone1.png" width="60%">](https://www.youtube.com/watch?v=p49DG-7GNyk "Video!")

Above image is a link to video :)
- Stylized shader
  - In this week, we've played around the stylized shader. We used toon shader as a starting point and added paper post processing to produce a style mimicing the reference image.
- Terrain
  - We decided to use hex map for a relatively easy control of different biomes. Now we implemented a endless hex terrain generator that dynamically changes biome as the camera moves.
- Biome
  - We implemented a basic biome config system. Each biome is composed by 3 layers: near, mid, and far (from the train's view). The hex tile mesh and material can be defined for each layer. For instance, in the current video, there is a snow land biome where the snow is the near layer, the lake is the middle layer, and the black stone is the far layer.
  
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
