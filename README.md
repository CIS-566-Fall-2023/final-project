# Sci-Fi Corridor Generator
Authors: Tianyi Xiao and Linda Zhu

<details>
  <summary> Milestone 1 </summary>

### Ground Plane
The main highlight of our project is to generate a corridor scene solely based on an input curve, and the user can customize/ edit the curve nodes to update the map dynamically. So our first task was to tackle the grid-ification of a 3D curve and project that into a 2D plane, aka the corridor map (Figure 1). 

![](/img/milestone1/mapGenDiagram.jpg)
*Figure 1. Corridor Map Generation Workflow*

The process is approximately as following:
  - Use a `Transform` to scale down the input curve in Y so it's flatten onto the x-z plane.
  - Create multiple points on the curve using `Resample` to better grid-ify, i.e. snap the points to the cloest grid, the curve later using `Fuse`.
  - Next we can give the curve some width by `Copy`ing a base grid/square `ToPoints` on the curve.
  - After fusing the tiles into one single object, we `Dissolve` the inner edges and do some other group cleanup to get the final 2D plane. This step we use node functionalities from the SideFX Labs plug-in.

Input | Gridification | Tiling | Grouping
---|---|---|---
![](/img/milestone1/inputCurve.png) | ![](/img/milestone1/gridifyCurve.png) | ![](/img/milestone1/copytoptsGround.png) | ![](/img/milestone1/ground.png)


### Corners
Once we have the ground plane, it's convenient to detect the concave (green) and convex (red) corner points. The process is as following:
  - Based on the un-dissolved map and base grid's spacing, we can extrude the sides using `PolyExtrude`.
  - Use a `Labs Measure Curvature` node to measure convex and concave curvature values.
  - Extract the corner points using `Blast` nodes.

Tiling | Extrusion & Curvature | Corner Points
---|---|---
![](/img/milestone1/copytoptsGround.png) | ![](/img/milestone1/measureCurv.png) | ![](/img/milestone1/corners.png)

  
### Digital Assets for Unity
This part is the trickiest due to software compatibility. Figuring out the working versions of Houdini, Houdini Engine for Unity, and Unity is necessary for the corridor map exported as a Houdini Digital Asset (.hda file) to be editable inside Unity. To understand the workflow better here's a summary of the roles of each software:

#### Houdini
Where we procedurally generate the map and calculate points necessary for scene assets placement, e.g. points to place floor tiles, long and short wall panels, ceiling pipes, etc. Similar to the algorithm we learned in LEGO-ifier where we calculate the points to place differenty types of blocks.

#### Houdini Engine for Unity
In Houdini, networks of nodes can be easily wrapped up into HDAs then shared with other artists. With the Houdini Engine, these assets can be loaded into the Unity game editor with procedural controls available to artists.

The results can then be further manipulated in Unity. Anytime a parameter is changed on the asset, the Houdini Engine is called upon to "cook" the network of nodes and publish the results to Unity. This allows for deep integration of HDAs into a Unity game development pipeline. The game content is baked out when the game is published.

In short, only with the Untiy plug-in of Houdini Engine installed will we be able to edit the exposed parameters from the node network and dynamically adjust the output INSIDE Unity. In our project, the exposed parameter is the input curve. The user will be able to view, add, or edit any node on the curve to get a desirable corridor system.

#### Unity
The platform that hosts the complex game (or more precisely, 3D content) development pipeline. Since we leverage the heavy computation of points, aka asset positions, using Houdini, the output of the HDA is just a group of plain geometries, e.g. curves, points and quads. To assemble the actual scene with visually pleasing and stylistic 3D models/assets, we assign prefabs to the HDA output inside Unity. This allows artists to quickly populate places alike using the same assets, and freely change/update what prefabs they want to replace at certain places on the map. 

For milestone 1, we are only using native Unity geometries to test our corridor map HDA. Starting from milestone 2 we will create procedurally modeled assets in Houdini and import them into Unity as prefabs to replace the current walls and floor tiles.

Unity Demo | Output
---|---
![](/img/milestone1/unityDemo.gif) | ![](/img/milestone1/unityHDA.png)


### Walls
We are running ahead of the schedule so we continue on generating different types of walls.
  - Figure out where to place the walls, ceiling tiles and floor tiles (start off with some default prefabs).
</details>

<details>
  <summary> Design Doc </summary>

### Introduction
- We are interested in creating a procedural generator of Sci-Fi game levels to assist artists with faster authoring of stylized scenes.
- We want to utilize various procedural graphics knowledge we learned from this class, e.g. shape grammars, and explore integrating popular 3D tools into one content authoring workflow.

### Goals
- Create a corridor system as a game level map that connects interior spaces given an input curve.
- The level assets such as panels, doors and decorations will be created in a procedural way using Houdini and then ported into Unity. 
- Assemble the final sci-fi level scene in Unity.
- Stretch goal: make the scene interactable with the player.

### Inspiration/reference:
- Inspired by many popular Sci-Fi movies and games, such as Cyberpunk 2077, Halo and Blade Runner, we want to implement a Sci-Fi style level.

![](img/cyberpunk2077.webp) | [Cyberpunk 2077 Art Style](https://www.engadget.com/cyberpunk-2077-review-170013962.html)
---|---
![](/img/halo.jpg) | [Halo 4 Environment Art](https://polycount.com/discussion/159954/the-environment-art-of-halo-4)
![](/img/bladeRunner.jpg) | [Blade Runner Environment Art](https://polycount.com/discussion/193588/blade-runner-2049-memory-lab-environment-art-dump)
![](/img/circuit.png) | [Sci-Fi Circuit Board](https://youtu.be/X7T1NMm5fXw?si=8gHXMNfyAoAtDx7M)
![](/img/scifiLevel.png) | [Sci-Fi Scene in Unreal Engine](https://cubebrush.co/blog/the-making-of-a-sci-fi-corridor-ue4-scene-breakdown)

### Specification:
- Generate the basic structures (corridor map, walls, floor and ceilling) of a game level in Houdini. Integrate these assets into Unity scene.
- Create objects (box, chair, etc.) procedurally for level decoration.
- Create textures for scenes.
- Paint/Populate our levels with textures and objects procedurally.
- Implement some render features to for better visual effect, such as SSAO.
- Make the level interactable.

### Techniques:

- Houdini VEX scripting and node networks.
- Procedural modelling using shape grammars and possibly L-systems.

- Will rely heavily on the references below:
  - We found a helpful [tutorial](https://www.sidefx.com/tutorials/sci-fi-level-builder/), which we believe could be good guidance for us.
  - This article talks about procedural modelling of a [sci-fi cylinder tunnel](https://polycount.com/discussion/101306/breakdown-of-scifi-cylinder-tunnel).
  - [Similar sci-fi scene assembled in UE4](https://cubebrush.co/blog/the-making-of-a-sci-fi-corridor-ue4-scene-breakdown)

### Design:
Orange cells are Houdini stages, green cells are Substance Designer/Painter stages and the blue cell is in Unity. We didn't include the stretch goals in the chart, except the procedural modelling of decoration objects, because we want to ensure the completion of the main project.

![](/img/Design%20Doc%20Diagram%20.jpg)

### Timeline:

- Week 1 (milestone 1) [due 11/15]:
  - [x] Build a corridor map given an input curve that connects grids when the curve overlaps (Houdini - Tianyi). 
  - [x] Figure out where the convex and concave corners are on the map to apply appropriate corner geometry (Houdini - Linda).
  - [x] Link Houdini asset output to a Unity scene using the plugin Houdini Engine for Unity (Unity - Linda).

- Week 2:
  - [x] Figure out where to place the walls, ceiling tiles and floor tiles (start off with some default prefabs) (Houdini + Unity - Linda).
  - [-] Start digital assets generation using procedural modelling: doors and wall panels (Houdini - Tianyi).
  - [ ] Populate the scene with realistic lighting and other shading effects (Unity - Tianyi). 

- Week 3 (milestone 2) [due 11/27]:
  - [ ] Collect/ Create more textures (Online + Substance Designer - Tianyi).
  - [ ] Instead of using the same wall/floor/ceiling tiles everywhere, place procedurally generated digital assets with different sizes in the scene (Houdini - Linda).
  - [ ] Create more props and room objects, e.g. toolbox, machine, etc., for the scene (Houdini - Linda). 

- Week 4 (final) [due 12/5]:
  - [ ] Figure out where to procedurally place the props in the scene, e.g. around the corner or at the end of the corridor (Houdini - Linda). 
  - [ ] Decorate the scene by placing props and add other post-processing effects (Unity - Tianyi).
  - [ ] Do more testing and fix bugs (Both).
  - [ ] Create final renders to showcase and complete README (Both).

</details>