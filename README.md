# Final Project - Train View Simulator
#### Video Demo
[<img src="https://github.com/xchennnw/final-project/blob/main/img/final.png" width="100%">](https://www.youtube.com/watch?v=h999fLHmnr4 "Video!")

Above image is a link to video :)

Train view is cool for anyone who wants to relax and enjoy the natural terrain winding out of the train window. We would like to provide a train view generator in Unity to simulate the passenger’s view point inside a large-window landscape express. We will provide a terrain generator to procedurally generate an infinitely-extending terrain landscape and a stylized shader to mimic the style of several pieces of 2D concept art. The project will finally be able to deliver an infinite tour aside by procedural stylized terrain view, with biomes and sky that can be fully customized by users in Unity.

#### Inspiration
<img src="https://github.com/xchennnw/final-project/blob/main/img/train3.png" height="300px"/> <img src="https://github.com/xchennnw/final-project/blob/main/img/ref.png" height="300px"/>


#### Screen Shots
<img src="https://github.com/xchennnw/final-project/blob/main/img/shot.png" height="400px"/>

<details>
  <summary> Project planning: Design Doc </summary>

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

</details>

<details>
  <summary> Milestone 1 (due 11/15) </summary>
  
[<img src="https://github.com/xchennnw/final-project/blob/main/img/train_view_milestone1.png" width="60%">](https://www.youtube.com/watch?v=p49DG-7GNyk "Video!")

Above image is a link to video :)
- Stylized shader
  - In this week, we've played around the stylized shader. We used toon shader as a starting point and added paper post processing to produce a style mimicing the reference image.
- Terrain
  - We decided to use hex map for a relatively easy control of different biomes. Now we implemented a endless hex terrain generator that dynamically changes biome as the camera moves.
- Biome
  - We implemented a basic biome config system. Each biome is composed by 3 layers: near, mid, and far (from the train's view). The hex tile mesh and material can be defined for each layer. For instance, in the current video, there is a snow land biome where the snow is the near layer, the lake is the middle layer, and the black stone is the far layer.
</details>

<details>
  <summary> Milestone 2 (due 11/27) </summary>

[<img src="https://github.com/xchennnw/final-project/blob/main/img/milestone2.png" width="60%">](https://www.youtube.com/watch?v=qRk5k1Ub4C8 "Video!")

Above image is a link to video :)
- Skybox
  - In this week, we've added a stylized skybox. In next step, we will make the sky color be defined in biome config, so that the sky color changes gradually according to current biome.
- Trees and Stones
  - We added L-system trees and stones that user can choose to add for certain layers in a biome, and set the number to spawn. To be improved: add an object pool to control them to improve performance.
- Sand ground shader
  - We added a white sand ground shader to mimic the beach scene in one of the reference images.
- In next days, we are gonna add more biomes, and do some performance optimization
</details>

<details>
  <summary> Final submission (due 12/5) </summary>
    
## Final submission (due 12/5)
In the final milestone, we added more biomes and adjust the color such that visual effect of the whole tune to make it more alike to the reference image. Some vfx effect is added, such as camera shaking and star blinking. Considering the background under a game engine, we add some interactivity for user to control the train.

### Techinical breakdown
#### Terrain & biome
- We use a hex map for a relatively easy control of different biomes.
  - Number of rows and columns showing can be set according to train’s field of view.
  - Also defines the length of each biome.
  - Spawn and disable tiles endlessly as the camera moves, using an object pool to manage all the tiles for performance optimization.

- We use a self-designed biome structure to mimic the window view from a train and make it easy to customize.
  - Each biome is composed by 3 layers: near, mid, and far (from the train's view). 
  - The hex tile meshes, materials, and objects(e.g. tree and stones) can be defined for each layer. 
 
- Biome user config: Each biome is defined by a Unity ScriptableObject. We can customize these for near, mid, and far layers
  - A list of tile meshes the layer uses.
  - Number of rows this layer occupies.
  - Material.
  - Whether to blend the layers. If choose to do so, the row of tiles between 2 layers will has both layers’ tiles according a ratio defined here.
  - Whether to spawn trees and other objects, number to spawn, and some parameters that help spawning.
  - Sky Config for this biome.

- Set ciomes for the terrain
  - There is a list that you can put infinite biomes in specific order, and the train will loop through them.
<br>  

#### Sky
- Sky box
  - Three layer sky color, for each biome
  - Blinking stars
  - Moving poly clouds
- Sky color config
  - Help user to define top Color, horizon color, and ground color to make the sky color fits a biome’s style.

#### Others
- Shaders
  - Toon shader + Paper Style Post process
  - Sand ground
  - Stylized Water Surface
    
- Train simulation camera shaking
  - Simulate train on railroad tracks, shaking and clang on joint of railway.

## Post mortem
Overall our project goes pretty well. We made a delicate train view generator in Unity with stylized landscape and skybox (we are visually satisfied in the middle of it). The coordination is smooth and we all did our job well. After this project, we are more familar with the formation of Unity shadergraph and stylization continue from hw4.
</details>
