# Design Doc
## Introduction:
For this project, we wanted to achieve a procedural science fiction environment that invoked a massive sense of scale. We were initially motivated by the art of the Manga: BLAME, which showcases city environments that were designed by a hostile AI that gives an awe-inspiring yet eerie feeling.

## Goal:
We intend to create tools that would aid in a procedural construction of this sort of environment. These tools would eventually result in a fully explorable 3D scene constructed within Unreal Engine. This scene would also result in some aesthetic still shots that can be showcased with a specific artistic vision with some manual tweaking.

## Inspiration/Reference:
Environment Reference: Mostly taken from either BLAME or thematically related fan arts.

<img width="232" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/8b525a52-b443-4f0e-9a8d-bb667cf38e76">
<img width="209" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/6c527fc4-57c1-404e-a5e5-f81d57420f76">
<img width="311" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/3aa2b20a-ffb3-4b29-abd0-b431a0174db4">

Material Reference: Mostly taken from games such as Star Wars Jedi: Survivor
<img width="466" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/6b921521-d17f-44c1-a38c-50f388574cd5">
<img width="464" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/dfc7ba25-982c-4244-93f9-29d6bb0c5ef1">
<img width="351" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/a9dea891-dc95-40b2-8d8d-7c41f43695f4">

## Specification:
- Procedural Tool that will create a base layout of the city walls/buildings (Houdini/PCG)
- Procedural Tool that will take in base meshes and add bevels and indents to make it look like a more mechanized Killer AI Generated City (Houdini)
- Procedural Tool that will take in faces from two different buildings and generate a techno bridge mesh in between them
- Procedural Tool that will generate pipes inside hand-created volumes and along the sides of buildings
- Procedural Tool that will generate wire meshes around/across the buildings
- Procedural Concrete/Metallic Material Variations that have customizable parameters which can be procedurally placed on to building meshes
- Grime Decal that can be procedurally placed on windows with Unreal PCG 
- Unreal PCG System that will scatter props and debris around scene
- Post process filter to make it look like manga

## Techniques:
We will be working between 3 main software packages - Houdini, Substance, and Unreal.
Houdini:
Base layout of city buildings will use random point scattering, booleans with varied other scattered shapes, mesh → volume → mesh
Window/bevel/indent tool will use use separate voxel 

## Design:
![FBD](https://github.com/xcupsilon/project-blame/assets/50472308/4cc702f3-330a-4d7d-8e01-b33ff53fd0a8)

## Timeline:
- Milestone 1 – 11/15
General Vertical Slice:
Rain: general art direction, general building shape construction, bridge placement – in Maya/Houdini
Thomas: rain’s output → punch holes for windows, cut some detail into things, place mid-small size details – all in Houdini m
Dineth: Materials: Concrete material, grime decal coming from windows – in Substance Designer/Painter and Unreal
- Milestone 2 - 11/27
Rain: lighting first pass, bringing into Engine, simple post processes, wires
Thomas: Pipes from some volume input, mid-small size detail modeling + placing
Dineth: Metallic, Black wire material, Cracks decal on the wall
- Final - 12/6
Rain: lighting polish, post processes & materials (potentially manga), scattering small props & garbage using PCG 
Wires
Thomas: Adding additional procedural models that would suit the scene
Dineth: Adding Additional Materials that would look good for the scene

## Milestone 1
- Rain:
For my parts of the milestone, I did some simple primitive modeling in blender and experimented with Unreal's PCG system to scatter buildings on the pillars. Unfortunately, I realized that I am unable to export the generated mesh out as fbx to integrate into our houdini pipeline.

I decided then to switch to working in blender using the geometry node. Here are my results of a modifier tool I created that allowed us to scatter meshes on an existing mesh's surface. The tools had 7 exposed parameters allowing me to fine-tune and art direct the result. Here is what I ended up with for the blockout:
<img width="610" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/2234d012-f3b8-41c8-81b8-c9282331d157">
<img width="1179" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/ac89311f-1ca9-4e46-adb2-d53a509811ba">
<img width="281" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/2e9d46fe-1166-4ad2-8249-4469157d7854">

Here is something simple visdev in Blender using Cycles.
![image](https://github.com/xcupsilon/project-blame/assets/50472308/9a497495-f8ea-4a68-b28c-9a597c238777)

I then migrated the assets over to Unreal and started doing more art directing & visdev there:
![HighresScreenshot_2023 11 15-23 22 15](https://github.com/xcupsilon/project-blame/assets/50472308/4212487c-d1a3-44e7-a75e-6390f8b0379d)
Here are the references I used for this lighting scenario:
<img width="561" alt="image" src="https://github.com/xcupsilon/project-blame/assets/50472308/68e50c02-8a1d-4a74-98d3-77bea578b89a">

- Dineth

- Thomas
Submission: Add a new section to your README titled: Milestone #1, which should include
- written description of progress on your project goals. If you haven't hit all your goals, what's giving you trouble?
- Examples of your generators output so far

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
