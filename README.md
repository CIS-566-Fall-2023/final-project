# CIS 5660 Final Project

[https://youtu.be/oI4D4JgaK6Q](https://youtu.be/oI4D4JgaK6Q)

#### Introduction
We were inspired to create this character by the various beautiful visuals of our favorite video game characters. 
![IMG_5849](https://github.com/RachelDLin/final-project/assets/43388455/1329f7fc-01cc-4560-a306-4cd1ce50a302)

#### Goal
Create a render of a ginkgo-themed character using a mixture of procedurally and manually modeled assets, procedurally-created textures, and procedural effects of ginkgo leaves falling off the dress. 

#### Inspiration/reference:
Design is based on a combination of traditional chinese clothing and fantastical elements. Inspired by Mihoyo’s Honkai Impact 3rd character designs.

(Original design by Jackie Li)

#### Specification:
- Manually modeled in Maya: hair decorations, clips, pins, shoes
- Manually modeled in ZBrush: figure, hair
- Manually modeled in Marvelous Designer: dress
- Procedurally modeled in Houdini: ginkgo leaves on bottom of dress/ sleeves
- Procedurally textured in Substance Designer/Painter: dress fabric textiles
- Cel-shaded in Unreal: hair, skin
- Unreal particle sim: effects of leaves falling off dress

#### Techniques:
For ginkgo branch: 
- Houdini VEX and Node systems
- L-systems

Texturing: 
- Cel-shading/ bucketing
- Vertex painting (lerp) for blending between materials on a mesh

Unreal Engine:
- Shaders (Post processing and surface)
- Particle Simulation

#### Design:
![Ginkgo Girl FBD](https://github.com/RachelDLin/final-project/assets/43388455/60302a12-ee4e-4e20-bca9-d269eab3cab5)

#### Timeline:
11/15: 
- Finish geometry and UV-unwrapping for all hand-modeled assets
  - Figure, hair
  - Dress
  - Props (hair deco, clips, pins, shoes)
- Start spline-based ginkgo branch generator in Houdini

11/27: 
- Complete a working ginkgo branch generator
- Create fabric textiles
- Unreal particle sim for falling ginkgo leaves
- Merge all elements together in UE5 (Preliminary)
- Create cel-shaded materials for skin and hair

12/5: 
- Refine and reimport various elements as needed
- Polish final image
  - Lighting
  - Background
  - Post process shaders

# Milestone 1 Updates

#### Accomplishments
- Modelled the head, body, and hair for our character.
- Finished modelling embellishments for character.
  - Hairpins, Shoes, Dress designs, etc.

 ##### Maya Modelling
 
| ![](Pictures/bodyHeadHairModelRAW.png) |
|:--:|
|Body, hair, and head model.|

| ![](Pictures/shoesRAW.png) |
|:--:|
|Shoes model.|

| ![](Pictures/ginkgoHairPin2RAW.png) | ![](Pictures/ginkgoHairPinRAW.png) | ![](Pictures/extraGinkgoAssetsRAW.png) |
|:--:|:--:|:--:|
|Pin Model 1|Pin Model 2|Pin Model 3|

| ![](Pictures/ginkgoClipsRAW.png) | ![](Pictures/ginkgoOrnamentRAW.png) |
|:--:|:--:|
|Ginkgo Clips|Ginkgo Ornament|

- Started on procedural generator for the branches. Plan to watch tutorials to create a branch generator, get only the endpoints of the branches, and adding leaves on there.
  - That will probably be the more difficult part for me to figure out, since I already have been having trouble trying to figure out how to parametrise my tool.
  - I am also simultaneously trying to figure out how to map a spline onto a mesh, since that will be where the procedural branch generator starts to grow.

##### Houdini Procedural Generator
![](Pictures/milestone1WIPHoudiniNode.png)

# Milestone 2 Updates

#### Accomplishments
- Completed and refining procedural ginkgo generation tool.
  - Including finding a way to integrate tool with the cloth simulation of the dress.
  - Currently finding a way to better organise the ginkgo leaves, which will require much tooling around with random values.
- Completed Substance Designer procedural texture for the dress.
- Completed Marvellous Designer dress simulation.
- Started setting up project in Unreal Engine.

##### Procedural Ginkgo Leaf Tool

###### First iteration

| ![](Pictures/ginkgoTreeProgressShot.png) | ![](Pictures/ginkgoTree.png) |
| :--: | :--: |
|Following a tutorial to create a tree.|Ginkgo tree asset.|

###### Second iteration

| ![](Pictures/ginkgoTool_withNodeTreeUpdate.png) |
| :--: |
|Manually creating a bezier curve at the border of a grid to simulate replicating the tool on a cloth simulation.|

###### Third iteration
| ![](Pictures/ginkgoGirlDress.png) | ![](Pictures/dressWithGinkgoLeaves.png) |
| :--: | :--: |
|Extracted the borders of the dress as a bezier curve to grow branches with leaves.|Final product mockup. Will be adding more to sleeves and playing with values within the next update.|

##### Procedural Substance Designer Texture
| ![ginkgo_light](https://github.com/RachelDLin/final-project/assets/43388455/25aaecc6-6a82-4366-b5a7-b40f7c7a7fb1) | 
| ![Screenshot 2023-11-27 123622](https://github.com/RachelDLin/final-project/assets/43388455/0697be8a-bbcc-4e78-b1bd-4121164dccec) |
| :--: |
|Created procedural ginkgo leaf mask in Substance Designer. Experimented with using this to create a pbr fabric textile material. 

##### Unreal Engine Setup
| ![HighresScreenshot00000](https://github.com/RachelDLin/final-project/assets/43388455/effbc804-5491-4560-8f06-1477c3ec018f) |
| ![HighresScreenshot00001](https://github.com/RachelDLin/final-project/assets/43388455/537af3f3-743e-4112-9905-55c0e05816bc) |
| :--: |
|Imported and placed all meshes. Set up and assigned simple cel-shaded materials for all meshes. Used ginkgo leaf mask to create simple cel-shaded fabric textile material.|

# Milestone 3 Updates

##### Sculpted Hair
![Screenshot 2023-12-05 232537](https://github.com/RachelDLin/final-project/assets/43388455/e691af2e-0855-41a6-a378-29b08d7351ef)
| :--: |
|Sculpted hair in ZBrush.|

##### Refined Procedural Ginkgo Leaf Tool

Refined numbers and rotations for each of the procedural meshes that the tool outputs. Also added some controllers for ease of use.

|![](Pictures/conpleteGinkgoToolSim.png)|
| :--: |
|Complete shot of the procedural ginkgo leaf tool.|

##### Enhanced Toon Shaders

Set up vertex painting for blending between materials. This was done through a simple linear interpolation node in the material graph. Also added specular highlights to the jade material using the Blinn-Phong shading model. 

##### Simple Landscape

Sculpted a simple landscape in Maya. Created grass card opacity mask by baking a grass mesh onto a plane in Substance Designer. Grass color was set in the toon shader in the same way as all other materials. Used Unreal foliage tool to create a new grass foliage type and apply grass to landscape.

##### Tree

Procedurally created tree branches and trunk in Houdini. Hand-painted branch/leaf texture and created opacity mask in Procreate. 
|![](Pictures/ginkgoTree_MAYA.png)|
| :--: |
|Created the branches in Houdini using procedural tree node. Hand drew textures for ginkgo leaf branches and placed them all over tree in the form of image planes.|

##### Falling Ginkgo Leaf Particle Sim

Created particle simulation out of Unreal Engine 5's Niagara Particle System. Ported a custom texture for the particles, attached a ribbon renderer and extra sprite renderer for additional effects. 

|![](Pictures/unrealGinkgoParticleSimPostProcess.png)|![](Pictures/unrealParticleSimGinkgo_TEST.png)|
| :--: | :--: |
|The particle simulation as viewed in the main Unreal viewport.|The particle simulation asset. As one can observe, there are three emitters that control the simulation's main particles and additional effects.|

##### Setup Cameras & Rendered Videos/Shots

Set up several cinematic cameras to get close-up and overall shots. Used Unreal level sequencer to create key-framed camera pans.


