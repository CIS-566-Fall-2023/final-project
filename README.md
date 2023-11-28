# Final Project - Debby Lin and Will Cai
## Milestone 2: Implementation part 2 (due 11/27)
### Biomes: Will
- I was mostly modifying the compute shader to create different biomes, specifically a cliffs and a beach biome. Because I was unable to figure out how to fix my previous Perlin function code, I ended up writing new FBM noise functions for each biome, “island” and “cliffs”.
These are simple 2D noise functions that are based off a common interpolation2D function. For cliffs, the heights are amplified much more, and there are some simple functions that elevate certain ranges of heights to be the same, which creates a flat vertical wall effect.
  - Cliff biome
  
  ![Screenshot 2023-11-27 214600](https://github.com/debbylin02/final-project/assets/82790216/57368777-00ca-4ac9-9803-cdd021740c05)
  
  - Island biome
  
  ![Screenshot 2023-11-27 214607](https://github.com/debbylin02/final-project/assets/82790216/29fc42c2-ac27-4b86-8e4a-a9ef492fc8f1)

  
- For making the 2 biomes exist together, I created new 2D noise/interpolation/FBM functions. Then there’s a generateTerrain function which combines the biomes, and finally heightFromPos which calls generateTerrain.and gets the final height value for each position.
- Currently, this code is not completely working, and it’ll be the first thing we fix for the next milestone

### Three-tone biome shaders: Debby  
- I imported URP and Shadergraph to the project so that we could migrate to shadergraphs instead of the default materials.
- I created a mountain biome shader that first took in two colors: a green color for the grassy top and grey for the rocky areas. I used a height based gradient color shader such that the tops of the mountains were green if their y-value was above some value and grey elsewise.
- I then converted this into a three-tone shader graph inspired by the shader work from homework 4 so that the grass and rock colors could be broken up into highlights, midtones, and shadows.
- I also created a three-tone shader for the sandy biome. 
- To improve upon this shader, I am looking to make the shift from grass to rock more obvious and potentially normal-based instead of height-based.  

### Water shader: Debby
- For this section I combined various tutorials in order to try to replicate the water texture from Dredge. 
  - Main shader tutorial: https://ameye.dev/notes/stylized-water-shader/ 
  - Ripple texture: https://www.youtube.com/watch?v=Vg0L9aCRWPE&ab_channel=Brackeys
- Water depth
  - I first calculated a depth fade value between 1 (shallow water) to 0 (deep water). I first took the Scene Depth (Eye) (i.e. the distance in world space units between the camera and objects below the water’s surface) and subtracted the Screen Position (Raw) (i.e. the distance between the camera and the water surface) so that we can get the water depth (i.e. the distance between the objects and water surface. After that, I divided, saturated, and used a One Minus node to further control and clamp the value. 
  - In order to avoid having the depth value change based on camera position, I modified this water depth measurement by calculating the depth in world space. This would measure the distance between an object and the water’s surface vertically rather than at an angle so that the depth does not look different even when viewed at a different angle.  
- Colors and opacity
  - I took the Depth Fade value (calculated before) in order to Lerp between a shallow and deep water color to get a base color. 
  - I then used a Fresnel Effect node to apply a horizon color to the water and Lerped that with the base color. 
  - After that I added the colors of the objects that are beneath the water using a Scene Color node so that the color of objects underwater can be seen. 
- Refraction
  - In order to add some distortion to the water shader, I added a refraction effect to the UVs before they were passed into the Depth Fade subgraph. This was done by applying some Gradient Noise to the Screen Position.
  This added some "wobbliness"  to mimick movement on the surface of the water.
- Foam/ripples
  - In order to create surface ripples I created “Panning UVs” (i.e. I applied movement to the UVs so that there would be movement in the texture) and before passing them into Voronoi noise to create the main shape of the ripples. I then sharpened the shape of the ripples and applied color to them before adding them to the water color.   
  - I then created intersection foam such that wherever objects or biomes intersect with the water surface there would be a ring of white foam around them. This was done by first defining the ring around objects in the water for the foam to go (i.e. the intersection foam mask) which was based on the Depth Fade value and a distance cutoff value. I then passed Panning UVs again into a gradient noise function to get the shape of the foam. Lastly I multiplied the foam mask with the shape of the foam and the foam color before adding it to the water color. 

### New objects assets/models: Debby 
- I imported tree, boat, and rock assets to replace the old preliminary models and created some three tone shaders for these objects.
  - Rock models: https://sketchfab.com/3d-models/70-stylized-rocks-bf0051544c9c41f998c154c546b09669

<img src="https://github.com/debbylin02/final-project/assets/82790216/a213ca7a-b676-4a4b-b0f3-aa93a18e5f03"  width = "350" height = "250"> 
<img src="https://github.com/debbylin02/final-project/assets/82790216/fe302954-1db9-4d54-9261-a8e1fdcdfa02"  width = "350" height = "250">
<img src="https://github.com/debbylin02/final-project/assets/82790216/99c86ff9-9945-4939-82a8-f3adbbd80dc5"  width = "350" height = "250">
<img src="https://github.com/debbylin02/final-project/assets/82790216/429846e8-56e8-4903-8902-7c22557488b6"  width = "350" height = "250">

  - Tree models: https://sketchfab.com/3d-models/low-poly-tree-pack-ea6e844754da494a9c38501b4fff92ad#download

<img src="https://github.com/debbylin02/final-project/assets/82790216/4a431943-a1c9-4470-9ad2-75eb2a49381b" width = "300" height = "350">
<img src="https://github.com/debbylin02/final-project/assets/82790216/6a5dd7ab-7672-4083-83b6-ec2fa20fc235" width = "300" height = "350">
<img src="https://github.com/debbylin02/final-project/assets/82790216/f7d571aa-167e-4b15-a480-7dbf46bdda02" width = "300" height = "350">

  - Boat model: https://sketchfab.com/3d-models/boat-57b1ca19f1484559b22c4b8ad408559d  
- I have already successfully swapped out the boat model, but I have yet to swap out the generated rock and tree models as I need to edit the positions, sizes, orientations, and collision boxes.

### Night light: Will
- This is a very simple point light that is parented to the boat and activates at nighttime.
<img src="https://github.com/debbylin02/final-project/assets/82790216/ab17814e-be45-4cfd-98c1-022e0cf8c5fd" width = "650" height = "400">

----------------------------------------------------

## Milestone 1: Implementation part 1 (due 11/15)
https://github.com/debbylin02/final-project/assets/82790216/d5bd26bb-46f0-4cd5-ac98-a66d43569a69
### Procedural terrain:  Will and Debby
NOTE: For this section, we based our code off of this tutorial on infinite procedural terrain: https://youtu.be/f9uueg_AUZs?si=022T5p3lBSdTLTk6  
  - #### Chunking - Will and Debby
    -  Chunking was done in TerrainController.cs
    -  Kept a dictionary of terrain tiles mapping Vector2 (coordinate of a tile) to GameObjects (terrain tile objects).
    -  Checked if tiles have changed or no tiles exist yet before enabling new tiles that are within the Radius to Render distance around the center tiles and deactivating old tiles that are out of range. This is so that we can limit the amount of terrain that needs to be generated to just the terrain the player can see.
    -  Created a Level object to parent the water plane. The dimensions of the water plane was based on the Radius to Render and the size of each tile (i.e. the Terrain Size field).

  - #### Infinite Landscape Generation + HLSL Noise - Will 
    - Worked on editing GenerateMesh.cs which is called by the main chunking controller TerrainController.cs, and takes in user parameters to create a mesh for a single tile
    - Originally used Mathf.Perlin to generate simple perlin noise based on world coordinates, but wanted to make custom noise
    - Made custom noise function with GLSL in shadertoy that uses multiple Perlin noise functions to create different “biomes”
      
      <img src="https://github.com/debbylin02/final-project/assets/82790216/d2e58d18-246f-4191-a074-603e4d1a4821" width = "600" height = "350">

    - Transferred this to Unity in C# but realized it was causing too much lag, so ended up using a compute shader instead
    - Because compute shader is computationally cheap but running multiple is expensive, we now only call it once per tile—taking in an array of x-y world locations and returning an array of heights
    - Current issues: the Perlin noise currently implemented in our HLSL compute shader results in a repeating pattern issue, which I’m still trying to figure out (the coordinate inputs are correct, and it’s the same exact hashing functions as used in our Shadertoy), so for now we have a gradient noise function
    
  - #### Asset placement (with basic models) - Debby
    - Created two simple objects, “rocks” (red cubes) and “trees” (green cylinders)
    - Asset placement was done using the Terrain Controller and the PlaceObjects.cs script.
      - First a random number of objects was picked between the minimum and maximum number of objects allowed per tile of the terrain (these were variables kept by the Terrain Controller).
      - Then a random prefab was chosen from the list of “placeable objects” kept by the Terrain Controller (in this case, trees and rock objects) as a variable.
      - Then a random point was picked above the terrain based on the terrain size (i.e. the size of each terrain tile - another field in Terrain Controller) to place the object.
      - Unity’s RaycastHit (which gets info back from a raycast) and BoxCast (which casts a box along a ray and returns detailed information on what it collided with) were used to test if an object could be placed at the randomly picked point (i.e. the point was above water level and a box collider around the placed object would intersect with the terrain). 

### Moving camera/player - Will
  - Implemented simple WASD + arrow key controls to move/turn the player
  - Player sphere stays around the origin to reduce lag
  - Camera was made a child of the player object and follows closely behind
      
### Skybox/procedural sky with day-night cycle - Debby
  - I used the unity asset “Simple Sky” to get a skydome mesh and sky texture: https://assetstore.unity.com/packages/3d/environments/simple-sky-cartoon-assets-42373
  - I added SkyManager.cs to automate the changing of the sky color to showcase the passage of time throughout the day. This script calculated the time of the day by splitting up unity’s “Time.deltaTime” value into 24 hours. Based on the hour, it would offset the texture used by the sky material by some amount to change the sky color. This sky material was then attached to a large skydome mesh. This skydome was then centered around the movement of the player so that the sky could move as the player moved around the space using a PositionOnlyFollow script, which took in the player’s position and set that to the center of the skydome. This was done so that the skydome wouldn’t rotate when the player rotated (i.e. which would occur if the skydome was just set as a child of the player).
  - In order to replicate a day-night cycle, I created LightingManager.cs and a lighting preset. The lighting preset contained 3 gradients: ambient color, directional color, and fog color. These three gradients were sampled based on the time of day in order to apply some colored lighting to replicate changing sunlight as the directional light moved. Like the sky box, the sunlight is based on the time of the day (a value between 24 hours). The rotation of the light is also based on the time of day times 360 degrees so that as time passes the direction of the light changes.
  - Current issues: I still need to edit the star/moon assets so that they rise/appear during night time and disappear during the day time. Additionally, I want to attempt to bring clouds into play for a more visually appealing sky. 
  

----------------------

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

