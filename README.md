# CIS 5660 Final Project: Procedural Terrarium Generator

As the final project for my Procedural Computer Graphics course at Penn, I will be developing a SideFX Houdini Generator Tool that allows users to create variations of succulents using procedural modeling.

## Design Doc
<details>
<summary>Click Here to Open / Close Design Doc</summary>
  
### Introduction

<img align="right" src="https://debraleebaldwin.com/wp-content/uploads/IMG_1820-1240x827.jpg" width=330 height=auto>

The motivation behind this project is to utilize the power of proceduralism to replicate naturally occurring phenomena from real-life flora! I was especially drawn to the concept of ___phyllotaxis___, the phenomenon in botany where leaves or petals are arranged around a stem in a spiraling pattern. Phyllotactic patterns (Fibonacci sequences, Golden Ratio, etc.) can be observed in a variety of plant species, such as succulents, which result in beautiful and fascinating geometric patterns. 

Succulents come in various shapes, sizes, colors, and arrangements, making them an interesting subject for procedural modeling. While there are endless types of succulents, for this project I will focus on the ___echeveria___, a type of succulent known for its aesthetic, rosette-shaped design.

### Goal
Through this project, I hope to accomplish the following:
1. Develop a procedural modeling tool that generates different variations of succulents, enabling users the creative freedom to create a wide range of succulent species and designs.
2. Continue developing my procedural modeling skills in Houdini, gaining hands-on experience with creating a user-friendly and intuitive tool for 3D artists
3. Create beautiful and realistic final renders of terrariums, populated with succulents (and potentially other greenery like cacti if time permits) generated with my Houdini tool.
  
### Inspiration and Reference Images:

There are endless variations of echeveria succulents which means tons of inspiration to draw from! Below are a few examples of different succulent shapes, colors, and arrangements:

![Frame 3](https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/a134835f-89a2-41ad-83e2-cfb801b750dc)

I also found references to visualize what the final renders could look like. From ceramic pots to glass containers, I plan to draw direct inspiration from these photos when designing my terrariums.

![inspo](https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/b80623c8-1f0a-419a-97cc-71b975b83d39)


### Specification:

The main features of this project include:
1. The procedural succulent generator allowing users to control multiple parameters
   - Parameters such as:
     - number of petals
     - roundness vs. pointiness of the petal tip
     - bend angle of the petals
     - width and length of petals
     - scale of the entire succulent
     - distribution of color
2. Terrarium containers, both glass and ceramic versions modeled and textured in Houdini
3. Polished final renders of terrariums with variations of succulents

#### Extra Credit Features:
1. A procedural cacti generator allowing users to control multiple parameters:
   - Parameters such as:
     - scale of the cacti
     - amount of ridges
     - density of spines
     - length of spines
     - amount of twist
2. Implementing procedural placement, allowing the user to randomly generate arrangements of the succulents

### Techniques:

For this project, I anticipate using Houdini procedural modeling techniques learned throughout the semester and VEX. I have also collected some helpful tutorials, articles, and a paper:
- [Procedural Succulents by Kilian Baur](https://80.lv/articles/006sdf-procedural-art-plant-generation-in-houdini/) - Article breaking down one artist's workflow when creating procedural succulents
- [Fibonacci Flower in Houdini by Junichiro Horikawa](https://www.youtube.com/watch?v=nPWQpQQgWJM) - Youtube Tutorial outlining how to create a Fibonacci pattern in Houdini
- [Procedurally Model Pot & Snake Plant | Houdini Tutorial](https://www.youtube.com/watch?v=BhXcOzpDQ1g) - Youtube Tutorial on how to create a snake plant and a pot. Will refer to this when modeling pots and soil.
- [The use of positional information in the modeling of plants](http://algorithmicbotany.org/papers/sigcourse.2003/2-27-positional.pdf) - Paper describing the mathematics and algorithms behind phyllotaxy in plants.

### Design:
![CIS 5660 Final Project Diagram (1)](https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/47c64ac7-cc7d-4384-846f-02cf8644633a)

### Timeline:

#### Week 1 (11/8 - 11/15):
- Implement a working Houdini tool that can generate basic succulent geometry
  - Expose parameters for users to create custom succulents
  - If time permits, implement color control

#### Week 2 (11/16 - 11/27)
- Continue to refine the succulent generator tool, adding color and tweaking the shape
- Model different versions of the terrarium containers, including round glass containers and ceramic pots
- If time permits, experiment with creating a cacti generator tool

#### Week 3 (11/28 - 12/5)
- Create final renders
  - Generate different variations of succulents and manually place the succulents into the terrariums
  - Add some soil and/or rocks into the terrariums
  - Add lighting and apply materials

</details>

## Milestone 2
<details>
  <summary>Click Here to Open / Close Milestone 2</summary

 <br/>
 
For Milestone 2, I created a working succulent generator that allows the user to adjust the petal's shape and depth using ramps as well as control other parameters such as petal count, petal bend, layers, width, length, and depth. Below are some examples of succulent models created using my generator:

![Screenshot 2023-11-26 190938](https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/fa523889-bdf2-4351-afa3-9c95983bcd9a)

<p float = "left">
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/c01659e6-0ab0-47fe-b862-cb58c2d2c122" width="240" height=235>
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/e49d0e3d-bfe8-4c34-8dfc-1bf72ef97f80" width=240 height="235">
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/c686b4e7-5120-4636-8504-8958ef602768" width="240" height=235>
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/00181d0f-53ab-4924-886d-0bcc63878ba9" width="240" height=235>
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/813013a4-7550-4c01-ac85-226d6ecf87ab" width=240 height="235">
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/9b60e898-9c4a-4e30-b3a7-e45b0c75ea3e" width="240" height=235>
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/337d7072-4b33-4604-b521-86337c80ace5" width="240" height=235>
  <img src = "https://github.com/CIIINDYXUU/Procedural-Terrarium-Generator/assets/88256581/b345f905-c5d8-4d34-86eb-0b2fd5fac4b6" width="240" height=235>
</p>

One major challenge I faced when building the generator was implementing the "golden ratio" algorithm. I was able to generate points following a phyllotactic pattern using VEX I learned from [this tutorial](https://www.youtube.com/watch?v=7xD1JgQTQKY), but I had a lot of difficulty figuring out how to create a petal shape and attach it correctly to each point. With the deadline approaching, I had to pivot and change my strategy -- mainly using the **skin** node to craft the petals and revolving those petals around circles. I'm disappointed I couldn't figure out how to implement phyllotaxis as I originally planned, but I'm overall satisfied with the look of the current version. 

</details>

## Milestone 3
<details>
  <summary>Click Here to Open / Close Milestone 3</summary>
</details>

## Final submission (due 12/5)
<details>
  <summary>Click Here to Open/Close Final Submission</summary>
</details>
