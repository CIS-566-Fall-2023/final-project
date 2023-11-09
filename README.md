# Sci-Fi Level Generator

### Design Doc
#### Introduction
- We are interested in creating a procedural generator of Sci-Fi game levels to assist artists with faster authoring of stylized scenes.
- We want to utilize various procedural graphics knowledge we learned from this class, e.g. shape grammars, and explore integrating popular 3D tools into one content authoring workflow.

#### Goals
- Create a corridor system as a game level map that connects interior spaces given an input curve.
- The level assets such as panels, doors and decorations will be created in a procedural way using Houdini and then ported into Unity. 
- Assemble the final sci-fi level scene in Unity.
- Stretch goal: make the scene interactable with the player.

#### Inspiration/reference:
- Inspired by many popular Sci-Fi movies and games, such as Cyberpunk 2077, Halo and Blade Runner, we want to implement a Sci-Fi style level.

![](img/cyberpunk2077.webp) | [Cyberpunk 2077 Art Style](https://www.engadget.com/cyberpunk-2077-review-170013962.html)
---|---
![](/img/halo.jpg) | [Halo 4 Environment Art](https://polycount.com/discussion/159954/the-environment-art-of-halo-4)
![](/img/bladeRunner.jpg) | [Blade Runner Environment Art](https://polycount.com/discussion/193588/blade-runner-2049-memory-lab-environment-art-dump)
![](/img/circuit.png) | [Sci-Fi Circuit Board](https://youtu.be/X7T1NMm5fXw?si=8gHXMNfyAoAtDx7M)
![](/img/scifiLevel.png) | [Sci-Fi Scene in Unreal Engine](https://cubebrush.co/blog/the-making-of-a-sci-fi-corridor-ue4-scene-breakdown)

#### Specification:
- Generate the basic structures (corridor map, walls, floor and ceilling) of a game level in Houdini. Integrate these assets into Unity scene.
- Create objects (box, chair, etc.) procedurally for level decoration.
- Create textures for scenes.
- Paint/Populate our levels with textures and objects procedurally.
- Implement some render features to for better visual effect, such as SSAO.
- Make the level interactable.

#### Techniques:

- Houdini VEX scripting and node networks.
- Procedural modelling using shape grammars and possibly L-systems.

- Will rely heavily on the references below:
  - We found a helpful [tutorial](https://www.sidefx.com/tutorials/sci-fi-level-builder/), which we believe could be good guidance for us.
  - This article talks about procedural modelling of a [sci-fi cylinder tunnel](https://polycount.com/discussion/101306/breakdown-of-scifi-cylinder-tunnel).
  - [Similar sci-fi scene assembled in UE4](https://cubebrush.co/blog/the-making-of-a-sci-fi-corridor-ue4-scene-breakdown)

#### Design:
Orange cells are Houdini stages, green cells are Substance Designer/Painter stages and the blue cell is in Unity. We didn't include the stretch goals in the chart, except the procedural modelling of decoration objects, because we want to ensure the completion of the main project.

![](/img/Design%20Doc%20Diagram%20.jpg)

#### Timeline:

- Week 1 (milestone 1):
  - Build a corridor map given an input curve that connects grids when the curve overlaps (Houdini - Tianyi). 
  - Figure out where the convex and concave corners are on the map to apply appropriate corner geometry (Houdini - Linda).
  - Link Houdini asset output to a Unity scene using the plugin Houdini Engine for Unity (Unity - Linda).

- Week 2:
  - Figure out where to place the walls, ceiling tiles and floor tiles (start off with some default prefabs) (Houdini + Unity - Linda).
  - Start digital assets generation using procedural modelling: doors and wall panels (Houdini - Tianyi).
  - Populate the scene with realistic lighting and other shading effects (Unity - Tianyi). 

- Week 3 (milestone 2):
  - Collect/ Create more textures (Online + Substance Designer - Tianyi).
  - Instead of using the same wall/floor/ceiling tiles everywhere, place procedurally generated digital assets with different sizes in the scene (Houdini - Linda).
  - Create more props and room objects, e.g. toolbox, machine, etc., for the scene (Houdini - Linda). 

- Week 4 (final):
  - Figure out where to procedurally place the props in the scene, e.g. around the corner or at the end of the corridor (Houdini - Linda). 
  - Decorate the scene by placing props and add other post-processing effects (Unity - Tianyi).
  - Do more testing and fix bugs (Both).
  - Create final renders to showcase and complete README (Both).


# Final Project!

This is it! The culmination of your procedural graphics experience this semester. For your final project, we'd like to give you the time and space to explore a topic of your choosing. You may choose any topic you please, so long as you vet the topic and scope with an instructor or TA. We've provided some suggestions below. The scope of your project should be roughly 1.5 homework assignments). To help structure your time, we're breaking down the project into 4 milestones:

## Project planning: Design Doc (due 11/8)
Before submitting your first milestone, _you must get your project idea and scope approved by Rachel, Adam or a TA._

Submit your Design doc as usual via pull request against this repository.
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
