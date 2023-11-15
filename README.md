# Laugh Out Loud's Handmade Toy Co.
### Est 2023, by Liu, Ouyang, Lu

#### Table of Contents

* [Project Proposal](https://github.com/yuhanliu-tech/final-project/#project-proposal-wooden-toy-generator)

* [Milestone 1: Yuhan Liu, Carousel Tool](https://github.com/yuhanliu-tech/final-project/#procedural-animated-carousel-tool---yuhan-liu)

* [Milestone 1: Claire Lu, Ornamental Tool](https://github.com/yuhanliu-tech/final-project/#procedural-ornament-tool---claire-lu)

# Milestone 1:

## Procedural Animated Carousel Tool - Yuhan Liu

Overview: 

* Goal: I wanted to dive deeper into Houdini's techniques and resources by creating an animated carousel generator, challenging myself to balance parameters that always produce desirable results with flexibility and aesthetic input in user design.

* I currently have a tool that can create a variety of carousel types, with three shown below, and plans to further build on the generator and develop stylistic details.

 <img src="https://github.com/yuhanliu-tech/final-project/blob/main/carousels.png" width="600" />

* Breakdown of Tool: 
    * Options that can be changed include the carousel height, radius, divisons (tent shape, horse count), and panels height.
    * The user can also use ramps to customize the decorative topper and the main column.
    * Toggles include outer columns, as well as a switch between horses that are suspended and horses that are grounded.
    * To add some extra adjustment, I also added manual sliders for horse size and horse height, even though they are already decided by other variables like radius and divisions.
    * Moving forwards, I hope to organize the parameters by carousel part, as well as add details while maintaining desirable outputs. 

 <img src="https://github.com/yuhanliu-tech/final-project/blob/main/tool.png" width="400" />

* I also animated the carousel by varying some parameters with the frame rate $F.

 <img src="https://github.com/yuhanliu-tech/final-project/blob/main/animation.gif" width="200" />

Process: 

<img src="https://github.com/yuhanliu-tech/final-project/blob/main/network.png" width="400" />

* I split the overall carousel into parts and worked on each one in subnetworks: Columns, Roof, Base, Music Box, Horses, and Integration. I'll discuss a few of the key implementation features below: 

* Columns, Base

   * The columns and base form the upper half of the carousel. They are the fundamental building blocks of the carousel, and their size determines how the rest of the pieces fit. Thus, I used two main variables, radius and divison, to track the size of base and the height of the columns. I used many MatchSize nodes in order to fit pieces in the carousel.
 
* Roof

    * The roof is composed of two parts, the panels and the center tent-like structure. I modified a tube for the panels. For the tent, I added a line with two bend nodes, one for a concave bend and one for a convex bend. The user can adjust the shape of the roof. The roof took some troubleshooting, as the tent needed to fit neatly inside the panels. However, I had to fiddle around with the MatchNodes, as my roof components were angled such that selecting min/max still produced intersections. 
 
* Horses

    * Poles: I created the twisted poles using a resampled line and two sweep nodes.
    * Horses: The horses are split into two groups, even and odd numbered, as the two groups are offset from each other and move at different heights. The size of the horses are determined by how many there are, which is in turn determined by the number of divisions. 
    * I took the outline from my carousel base to determine where the horses should be placed, using copy to points and orient along curve nodes to point them in the circular direction.
    * I then had to perform bounding tests so that their poles would not stick out of the mesh during the animation. I ended up using a clip node for this.
    * There are two types of horses that require different placement: grounded horses and suspended horses. 

* Integration

    * I integrated my teammates milestone projects into my own in order to create the assembled carousel.
    * I first imported Diana's sliced wooden horse mesh as an FBX. I decided not to use the HIP file because of input mesh dependencies, but I'll fix this next week.
         * I found that Diana's processed mesh really slowed things down, so I decided to pack them, which meant that in animating and clipping my horses, I was limited to working with the poles as I couldn't edit the mesh.
    * I also imported Claire's HIP file for ornamental designs. I turned her project into a subnetwork to create the ring of designs on the music box. Next week, I'll add the controls for her tool to the interface for mine. I also hope to add some designs on the panels. 

Next Steps: 

* Overall, I think I stuck pretty well to my goal of creating a usable tool. I also tried to maintain a clean wireframe. I noticed that many computation-heavy parts of the carousel slowed things down, so I was conscious about keeping the mesh clean and organized. Below is a screenshot of my wireframe.

<img src="https://github.com/yuhanliu-tech/final-project/blob/main/network.png" width="400" />

* As my next steps, I'm excited to continue adding fine details, organizing the controller for my tool, and further cleaning up my network by organizing subnets. I really want to make a polished-looking tool, and hope to explore Houdini Digital Assets. 

## Procedural Ornament Tool - Claire Lu

Overview:
* Goal: I wanted to make a tool to create spiral-shaped ornaments that would add detail to our base carousel mesh.
* I was inspired by ornaments like the ones on the carousel below. We can see that there are many variations to the spiral ornaments - for example, the shape, orientation, number of branching spirals, and so on.
 <img src="https://github.com/yuhanliu-tech/final-project/assets/102630261/74926315-dea6-44b8-820a-18267f9f4130" width="200" />
 
* To guide my procedural tool-making, I focused on the spiral ornament that can be seen in the center of the carousel:
<img src="https://github.com/yuhanliu-tech/final-project/assets/102630261/ab33f9f6-07d5-4b0e-9f0f-d38369400398" width="200" />

* We can see that the spiral ornament is symmetric about a centerline.
* My basic process was to make one side of the ornament, reflect it, and then place a center piece mesh above the combined left and right spirals. I laid out a structure for a basic spiral ornament:
* Spiral Structure: Ornament Components (for one side)
    * Spiral Spine - the main spiral that acts as the backbone for the branching spirals
    * Spiral Branches - the smaller spirals that grow out from the curve of the spiral spine
    * Centerpiece - the part of the ornament that is positioned in the center of the left and right spirals
* The image below demonstrates this structure and my basic workflow for creating the spiral ornament:
  <img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/cd1fde3d-3ea2-43aa-9b6f-dc27443a7731" width = "800" />    
* There were a few visual elements I wanted the end spiral shapes to have:
    * Adjustable shape for the main spiral spine -- I wanted the user to be able to adjust the contrast of the scale of the two spirals that make up a spiral spine
    * A variety of spirals oriented inwards and outwards from the main spiral spine -- the spiral branches can be oriented differently on the same spiral spine, making the end result more visually interesting
    * Adjustable position of the spiral branches -- I wanted the user to be able to adjust the position of the branches relative to the spiral spine 
    * Stylized, wooden look to the end mesh -- since our carousel should have a wooden look, I wanted the ornament detailing to match this

My Process:
* First, I created a basic spiral shape using the spiral node as the shape for the main spiral spine. I created secondary branching spiral nodes and used copy to points to copy a spiral to each point on the main spiral node. However, because a spiral shape is composed of many points, I first used the resample node to increase the length of each line making up the spiral, decreasing the point count.   
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/d5bad27d-9836-42d6-b579-baad65394115" width = "200" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/bf002fe5-77cc-414b-ab2b-113e3940e5b1" width = "200" />

* Using an attribute wrangle node, I controlled the position of the points used as spiral branching points, allowing the user control over the positioning of the spiral branches. Visually, most spiral branches stem from the middle range of points on the spiral spine, so I excluded any points outside of the middle range. This was done by using the spiral's mesh and scaling it to zero along the ends of the spiral, creating a bounding box for the points.
  
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/c535d09a-b0b0-4710-999d-8bef44849734" width = "200" />

* After adding the spiral branches, I sweeped the entire shape using the Ribbon cross section to create a flat mesh. Then, I reflected the shape to create the entirety of the spiral spine and extruded to give it depth. Afterwards, I added a centerpiece mesh.
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/8b7dd6a4-302b-4936-8a6a-cf997811d938" width = "200" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/62135b67-4434-4a62-9f14-43b68b22decf" width = "200" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/5de8b630-b1c0-4d8e-9a48-afb12377c033" width = "200" />

* To give it a wooden look, I beveled the mesh using a custom wood grain noise texture. This created wood patterned indentations on the mesh and helped give it a more polished feel. I also added an additional scaled down version of the spiral spine to mimic the wooden mesh splitting look of the horses on the carousel for this milestone.  
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/ae716cd5-44ea-4933-9625-710990ec3392" width = "600" />

* At the end, we can get something like this:
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/76434eb8-0f44-49ca-b9a0-56f0fb3fee3a" width = "600" />

* Here are some more spiral shapes we can get:
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/f6230a47-3430-475d-81b9-3d8bc3412664" width = "600" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/07bf6e1d-4473-45b4-b565-166e612acd9b" width = "600" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/d9bf9aab-b5f0-4589-9815-f7e2f30ca8b7" width = "600" />
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/48ea1492-c12f-4f8b-a11f-95d155168f04" width = "600" />

* Using a chain node, we can repeat this pattern and place it along the circumference of our carousel. Below is an example of what this might look like with a basic sphere mesh.
<img src = "https://github.com/yuhanliu-tech/final-project/assets/102630261/33f02ad7-63cd-40dd-a1af-6ce79446682f" width = "600" />

# Project Proposal: Wooden Toy Generator

* We are interested in procedural tool development for artists, as well as algorithmic mesh manipulation. 
* In exploring wooden toys, we have to solve such problems, including slicing mesh to become wooden figures, and creating user–customizable tools that produce stylized objects. 

## Goal

* Create customizable tools to generate wooden toys. 
    * The tools will allow for artistic input, giving the user ability to create various different versions of the toy type to create a cohesive collection 
* Stretch goal: rendered image of a toy workshop, including a setup of each of our tools 

## Inspiration/reference:

* Inspired by Gepetto’s wood toy workshop in Pinocchio, we wanted to create tools to generate charming handmade-style wood toys. 
* We had the following ideas for what types of toys to generate and how we would go about it:

![REF](refpics.png)

## Specification:

* Procedural Toy Meshifier
    * Input: mesh | Output: vertex deformed-mesh
    * Slices the mesh into stacked wooden panels (see mesh cutting image above)
    * Variables:
        * Simplification of mesh 
        * Number of sliced layers
    * Possible Resources: Cardboard slice generator
    * Makes it look hand/wood-carved
    * Variables: 
        * Simplification of mesh 
        * “Bumpiness” of carve

* Procedural toy generator
    * For cuckoo clocks, carousels, automata 
    * Generating the customizable base mesh
    * Generating ornamental patterns / details on the surface of the mesh
    * Animation and movement 

* Procedural Textures – stylized, hand/wood-carved look
    * Wood textures
    * Plastic textures

## Techniques:

* We’ll mainly be using VEX and node networks in Houdini. 
* For procedural textures, we plan to learn the Adobe Substance Suite

## Design:

![FBD](fbd.png)

## Timeline:

* Milestone 1: Untextured but assembled toy with basic embellishments
    * Diana: 
        * Develop tool for slicing / flattening given mesh to be used for wooden figures
    * Claire: 
        * Develop tool for creating embellishments / ornamental designs that can be customized and wrapped onto primitive mesh 
            * Inspiration: Procedural Lamp (https://www.youtube.com/watch?v=hGeoPErJ448&t=1s)
    * Yuhan: 
        * Develop tool for base of toy (carousel or cuckoo clocks?) that is user-customizable
     
* Milestone 2: Adding textures, UI & user control improvements
    * Diana: 
        * Allow users more granularity on mesh transformation (isolate eyes as their own layer, etc?)
    * Claire: 
        * Begin working on procedural textures (wood polished, wood bark, wood painted, plastic) in Substance 
    * Yuhan:
        * Combine three tools from Milestone one to create Houdini user-friendly subnetwork that creates one type of toy 

* Milestone 3: Use of tool for “cute lil rendered image”
    * Diana: 
        * Tweak parameterization of slicing/mesh flattening
    * Claire: 
        * Develop secondary tool for a different type of toy 
    * Yuhan: 
        * Work on rendered image in Unreal or Unity, showcasing toy variations and textures 
