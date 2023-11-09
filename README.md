# Final Project - Debby Lin and Will Cai 

## Project planning: Design Doc (due 11/8)
#### Introduction
We are inspired by infinite procedural landscape generation in games and the sense of both exploration and tranquility that they can offer. We want to create this type of experience with something visually appealing that simulates a slow, calming activity like sailing a boat down a peaceful river.

#### Goal
With this project, we intend to create a stylistically rendered interactive infinite boating game using Unity.

#### Inspiration/reference:
We were inspired by the visuals of Dredge and the gameplay of Slow Roads.io 
- Dredge gameplay photos 

<img src='https://github.com/debbylin02/final-project/assets/82790216/1eea2778-ea77-4014-b8a0-14b425fba9fc' width='300'>

<img src='https://github.com/debbylin02/final-project/assets/82790216/a101bb91-84fd-44d4-a061-4c195c0ef255' width='300'>

<img src='https://github.com/debbylin02/final-project/assets/82790216/920aaaef-220e-4c42-9662-efa765c934e3' width='300'>

<img src='https://github.com/debbylin02/final-project/assets/82790216/393040b1-99ff-4100-a3ee-099aba5f7979' width='300'> 

<img src='https://github.com/debbylin02/final-project/assets/82790216/f3d2e59b-d7ed-41d2-8918-370cea5a8fc9' width='300'>

- Slow Roads.io gameplay
  
<img src='https://github.com/debbylin02/final-project/assets/82790216/10715850-73dd-4ab6-8174-48cf37e5886c' width='300'>

<img src='https://github.com/debbylin02/final-project/assets/82790216/a7b014c9-1288-4cca-90a7-44aeec5dfa01' width='300'> 

<img src='https://github.com/debbylin02/final-project/assets/82790216/cd09162e-96f7-43a6-b616-cd64f4e1b889' width='300'> 


#### Specification:
The main features of our project will include: 
- Infinite procedurally generated environment 
- Water effects/interactions with objects 
- Stylized shaders to replicate the look of Dredge 
- Skybox with a day/night cycle as well as clouds and distance fog   
- User controlled boat/moving camera to make the environment interactable 
- Post-processing effects/filters 


#### Techniques:
- Sky/non-terrain environment 
  - Sky box with day-night cycle
    - https://timcoster.com/2020/02/26/unity-shadergraph-procedural-skybox-tutorial-pt-2-day-night-cycle/ 
    - Example reference: https://www.youtube.com/watch?v=L4t2c1_Szdk 
  - Cloud generation 
    - Individual stylized clouds: https://medium.com/@mikeyoung_97230/creating-stylized-clouds-with-shader-graph-and-shuriken-in-unity3d-ec8f12fb5f0a
    - Infinite cloud plane: https://www.youtube.com/watch?v=Y7r5n5TsX_E&ab_channel=RomanPapush 
  - Distance fog 
    - Tutorial: https://www.youtube.com/watch?v=EFt_lLVDeRo&ab_channel=Acerola 
- Procedural environment generation  
  - Chunking to render terrain chunks within a certain distance of the player
    - Tutorial: https://gamedevacademy.org/split-terrain-unity-tutorial/ 
  - Multithreading (if needed) 
  - Noise/toolbox functions to determine height map for terrain 
  - Cliff face generation will be done with noise/vertex deformation shaders 
  - Procedurally placed assets (i.e. trees, buildings, rocks, etc.) using noise functions 
- Shaders
  - Stylized asset and environment shaders
  - Cel shading with specular highlights/lighting techniques applied (Homework 4 as reference)
  - Water shader
    - Foam/ripple effects
    - Viewing objects below surface
    - Example shader: https://ameye.dev/notes/stylized-water-shader/ 
    - Ripples in water can be generated as separate objects that look the same as the “foam” surrounding objects in water, generated behind the boat when it is moving and lifted above the water plane
- Post-processing effects for lighting/filters
  - Use sun to determine flare
    - Reference: https://www.shadertoy.com/view/4sX3Rs 
  - Vignette
    - Reference: https://www.shadertoy.com/view/lsKSWR 

#### Design:

![Design Doc Diagram](https://github.com/debbylin02/final-project/assets/82790216/9fd2b7fe-692f-4845-ad19-79f445cf90c2)

#### Timeline:
- Week 1 (11/8 - 11/15)
  - Procedural terrain:  Will and Debby
    - Chunking + multithreading (if needed) - Debby
    - Height map + cliffs - Will
    - Asset placement (with basic models) - Debby
  - Moving camera - Will
  - Skybox + day-night cycle - Will
- Week 2 (11/15 - 11/22)
  - Cliff shader (vertex deformation) - Debby
  - Water shader (foam + look of objects underwater) - Will
  - Boat & moving interaction (forward, rotate left/right) - Will
- Week 3 (11/22 - 11/27)
  - Distance fog - Debby
  - Clouds - Debby
  - Stylized shaders for assets - Will/Debby 
  - Attach light to boat (it turns on at night) - Will
  - Water ripple effects - Will
  - Sound & music - Will
- Week 4 (11/27 - 12/5)
  - Post-processing - Will
  - Extremely Basic Fishing Mechanic - Will
  - GUI for user controls - Debby
  - Extra sound effects - Debby
  - Final model imports -  Will/Debby


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

