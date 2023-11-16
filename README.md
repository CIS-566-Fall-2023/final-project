# Final Project!

For the final project of this class, we (Hansen and Kisha) would like to create an isometric room generator!

## Design Doc

### Introduction
When brainstorming ideas for our final project, we came across isometric rooms and thought they were very visually pleasing. We hope to be able to create a tool that will allow users to easily generate procedural isometric rooms that are able to match the visual quality of the ones we've seen online.

Due to the endless variety of room types, we have a lot of freedom in regard to the different combinations of objects we want in our rooms. There is also an endless number of possible ways to place said objects and a lot of freedom to explore and play around with the look and feel of each room. We hope that our final tool will be able to incorporate all the aesthetic aspects of the isometric rooms while also maintaining a certain amount of proceduralism. 

### Goal
Our goal is to develop a tool using Houdini that can create different iterations of aesthetically pleasing isometric rooms.

### Inspiration/reference:
Here are some reference photos of the type of room and look we are going for!

<img src="https://github.com/kishayan02/final-project/assets/97934823/ab31b5ac-0c26-47aa-9828-4262254117a9" width="450" height="400">

<img src="https://github.com/kishayan02/final-project/assets/97934823/c0c7d4ad-f1db-4e7e-9bf6-10ea60302c1f" width="700" height="400">

### Specification:
- Be able to procedurally generate different types of isometric rooms
- Creating an assortment of procedurally generated assets and procedurally placing the objects in each room
- Involve user customization by exposing parameters that can be modified
- Create and render pretty rooms using our tool!

### Techniques:
- Binary or Ternary Space Partition for Asset Placement
  - https://en.wikipedia.org/wiki/Space_partitioning
  - https://en.wikipedia.org/wiki/Binary_space_partitioning
- L-Systems and Shape Grammars for Asset Generations
- Various tutorials on modeling certain furniture
  - https://www.youtube.com/watch?v=1opgcYYQCpE 
- Possibly Wave function collapse for giving the user control of asset placement
  - https://www.artstation.com/blogs/macmccolor/zL8W/using-houdini-and-the-wave-function-collapse-algorithm-to-create-a-city-level-generator-part-1

### Design:
![image](https://github.com/kishayan02/final-project/assets/97934823/0ffb85c6-85f3-49bb-baac-c80b4cf64c02)

### Timeline:
Because we are using Houdini and we think it’s best if we work on the project file together, we will be doing our best to meet up in person to complete each milestone.

**Week 1 (11/9 - 11/15)**
- Create a basic working tool that can procedurally place different pieces of furniture that we will substitute with boxes.

**Week 2 (11/16 - 11/27)**
- Begin to procedurally create a variety of assets and ensure that they are able to be placed into the room properly.

**Week 3 (11/28-12/5)** 
- Polish the scenes by implementing colors, different lighting, and exposing more parameters for the user to be able to interact with.
  

## Milestone 1: Implementation part 1 (due 11/15)
https://github.com/kishayan02/final-project/assets/97490525/ea37a7f1-424c-4565-9f7c-5b190d590593

For our first milestone, our goal was to be able to procedurally place assets into a room. This initial implementation uses four beveled rectangles as placeholders for the assets, but we have implemented it so that we should be able to easily scale it up to have even more pieces of furniture. We achieved this implementation by utilizing similar logic to the lego assignment with the basic logic based on iterating through the points of the floor grid and attempting to place an asset, placing it if it could fit in the room and then moving on to the next asset or moving onto the next point if it could not fit.

For procedurally generating several different rooms, we randomized the order in which we iterated through the grid points. This allowed us to adjust the seed of the randomization to achieve different rooms.

### Goals for the Next Milestone
As stated in our original timeline, we hope to focus on procedurally generated assets for the next milestone. However, some additional goals have emerged due to our current implementation of our tool. Firstly, we need to adjust the centers and boundaries of the assets to ensure that they are entirely within the boundaries of the room. Next, we would like to think more about restrictions regarding the assets like their proximity to one another, their orientation, and whether certain assets should appear or not. Finally, we also might want to consider the edge case of what the behavior of the tool should be if it is unable to place all the desired assets.


## Milestone 3: Implementation part 2 (due 11/27)
TBA

## Final submission (due 12/5)
TBA
