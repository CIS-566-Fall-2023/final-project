# :dvd:AlgebRave:dvd:

## Design Doc


## Introduction
Ever since I played Dance Central by Harmonix Studios(now a part of Ubisoft) in middle school on my Xbox 360/kinect, I was mesmerized by the amalgamation of music and graphics that I got to experience. Fast forward to Fall 2023, when I am taking the Procedural Graphics class at Penn and with every new lecture, thinking back on how could I recreate a fraction of the experience that Dance Central provided me with. And here I am, trying to attempt it for the final project of that class!

## Goals
* **Technical Goal:** I want to create a procedural beat detection system for audio files, and use that to drive custom graphics & visual effects based on live input from Kinect.  
* **Intrinsic Goal:** Having fun! I want both the user of the tool as well as myself during its development to have a blast. I want to create something wherein one could play any song of their liking, and have fun interacting with the splash of graphics appearing on their screen - maybe bust a move or two :dancers:

## Inspiration/References
- Although I am not creating a dance-based pose-matching game, but the inspiration for the environment and the experience is certainly based on Dance Central. [This](https://www.youtube.com/watch?v=kuwB05ASh7E) is a trailer of their sequel, that gives an idea of what the game is about. [This](https://www.gdcvault.com/play/1014487/Break-It-Down-How-Harmonix) is a GDC talk where they _Break It Down_ their game design & development approach, which isn't relevant to this project but definitely is a good inspiration.
- [Wayne Wu](https://www.wuwayne.com/), a graduate from the same program did a very similar [project](https://github.com/wayne-wu/interactive-dance-projection/tree/main) last year. This will be a good reference point for me for the user interactivity & background parts.
- [John Alberse](https://www.johnalberse.com/), a fellow intern I met at Activision in Summer'23 had some experience with Projective Graphics. He shared [this](https://drive.google.com/file/d/1RECgq3cEmV_nBFP9xX_NIgyAxvJbxDen/view) piece of his work with me which I found really inspirational.
- [Fluid Simulation using TouchDesigner](https://www.youtube.com/watch?v=2k6H5Qa_fCE)
- [Making Audio Reactive systems using TouchDesigner](https://www.youtube.com/watch?v=rGoCbVmGtPE)
- [Example of using TouchDesigner + Kinect](https://www.youtube.com/watch?v=tPYTXt1hSx4)

## Specifications
- A **Procedural Beat Detection** system created uisng **TouchDesigner** to identfiy basic parts of music like beats, drums, snares, bass, etc.
- Live input streaming from **Kinect** into TouchDesigner and using it to drive interactive visual elements.
- Incorporate **noise/toolbox functions** with **custom GLSL shaders** to write simple background effects driven off of music.

## Techniques
- **Procedural node-based tool**: Only recently I started gaining some experience with node-based tools like Houdini and Unity Shader Graphs. For this project, I will be using (and also, learning from scratch) **TouchDesigner** because of its ability to provide both great interactivity with Kinect as well as nodes to write custom GLSL shaders.
- **Kinect**: Since I already own a Kinect, even though the old one that shipped with the XBOX 360,, it is nonetheless a powerful device and therefore I'll be using the same for this project for its befitting abilities.
- **Toolbox Functions:** Writing custom shaders for visual effects almost never goes with using Toolbox and Noise functions. Although I haven't finalized each and every single visual aspect of the shaders, I am pretty sure I'll be routinely employing these tools for whatever I would want to achieve.
- **Optical Flow, Particle Simulation, Fluid Simulation, etc:** All such concepts fit really well with the vision for the project, and I will choose a subset of these while researching on the ease of their impolementation that aligns with the project's timeline.

## Design
- How will your program fit together? Make a simple free-body diagram illustrating the pieces.

## Timeline

### Week 1 ( 8 Nov'23 - 15 Nov'23)
* Implement the audio detection system in TouchDesigner that for a given audio file is able to generate signals for musical elements like beats, drums, snares, bass, etc.
* Target generating 4 such audio signals.
* Do a proof-of-concept by driving some basic graphics off a subset/all of these signals.

### Week 2 ( 15 Nov'23 - 22 Nov'23)
* Get started on the user input - hook up Kinect with TouchDesigner.
* Follow basic tutorial(s) to get some easy wins like particle system interactions.
* Implement at least 2 user-interactive features using tools like Optical Flow and Fluid Simulation.
* Hook up audio signals into these features.

### Week 3 ( 22 Nov'23 - 29 Nov'23)
* Work on developing simple yet visually pleasing audio-driven backgrounds.
* Implement 4 different backgrounds driven off of the generated audio signals.
* Combine everything together - audio signals, user input-based Kinect signals, and shader backgrounds.

### Week 4 ( 29 Nov'23 - 6 Dec'23)
* Tackle any delays from the previous milestones.
* Polish, polish, polish!
* Work on documentation.
* Ask fellow students to experiment with the tool and capture some recordings.
* Make a trailer for AlgebRave!

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
