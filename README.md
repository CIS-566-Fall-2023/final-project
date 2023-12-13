# Final Project!

For the final project of this class, we would like to create an isometric bedroom generator!

Presentation: https://docs.google.com/presentation/d/1jxzNpsYsw52H4KbOMdeyZMge3ZEdrqzh09D1cpKF_MM/edit?usp=sharing

## Design Doc

### Introduction
When brainstorming ideas for our final project, we came across isometric rooms and thought they were very visually pleasing and aesthetic. We hope to be able to create a tool that will allow users to easily generate procedural isometric rooms that match the visual quality of the ones we've seen online such that they can be used in movies and video games.

Due to the endless variety of room types, we have a lot of freedom regarding the different combinations of objects we want in our rooms. There is also an endless number of possible ways to place the objects and t play around with the look and feel of each room. We hope that our final tool will be able to incorporate all the aesthetic aspects of the isometric rooms while also maintaining a certain amount of proceduralism. 

### Goal
Our goal is to develop a tool using Houdini that can create different iterations of aesthetically pleasing isometric rooms.

### Inspiration/reference:
Here are some reference photos of the type of room and look we are going for

<img src="https://github.com/kishayan02/final-project/assets/97934823/ab31b5ac-0c26-47aa-9828-4262254117a9" width="350" height="300">

<img src="https://github.com/kishayan02/final-project/assets/97934823/c0c7d4ad-f1db-4e7e-9bf6-10ea60302c1f" width="500" height="300">

### Specification:
- Be able to procedurally generate different types of isometric rooms
- Creating an assortment of procedurally generated assets and different procedural placements of the objects in each room
- Involve user customization by exposing parameters that can be modified

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
<img src="https://github.com/kishayan02/final-project/assets/97934823/0ffb85c6-85f3-49bb-baac-c80b4cf64c02" width="600" height="450">


### Timeline:
Because we are using Houdini and we think it’s best if we work on the project file together, we will be doing our best to meet up in person to complete each milestone.

**Week 1 (11/9 - 11/15)**
- Create a basic working tool that can procedurally place different pieces of furniture that we will substitute with boxes.

**Week 2 (11/16 - 11/27)**
- Procedurally create a variety of assets and ensure that they can be placed into the room properly.

**Week 3 (11/28-12/5)** 
- Polish the scenes by implementing colors, different lighting, and exposing parameters for the user to be able to interact with.
  

## Milestone 1: Implementation part 1 (due 11/15)
https://github.com/kishayan02/final-project/assets/97490525/ea37a7f1-424c-4565-9f7c-5b190d590593

For our first milestone, our goal was to be able to procedurally place assets into a room. This initial implementation uses four boxes as placeholders for the assets, but we have implemented it so that we should be able to easily scale it up to have even more pieces of furniture. We achieved this implementation by utilizing similar logic to the Lego assignment with the basic logic based on iterating through the points of the floor grid and attempting to place an asset. If the asset can fit in the room at that point, we would move to the next asset, otherwise, we would try the next selected point. 

For procedurally generating several different rooms, we randomized the order in which we iterated through the grid points. This allowed us to adjust the seed of the randomization to achieve different rooms.

### Goals for the Next Milestone
As stated in our original timeline, we hope to focus on procedurally generated assets for the next milestone. However, some additional goals have emerged due to the current implementation of our tool. Firstly, we need to adjust the centers and boundaries of the assets to ensure that they are entirely within the boundaries of the room. Next, we would like to think more about restrictions regarding the assets like their proximity to one another, their orientation, and whether certain assets should appear or not. Finally, we also might want to consider the edge case of what the behavior of the tool should be if it is unable to place all the desired assets.


## Milestone 2: Implementation part 2 (due 11/27)
For this milestone, we focused on creating assets that could be modified procedurally. These assets include a bed, bookshelf, desk, chair, dresser, and window (pictures have been moved to the final milestone).

In addition, due to the issues we experienced with our previous implementation, we considered a different way for procedurally placing assets, namely using the SideFX Labs' Lot Subdivision node.

<img src="https://github.com/kishayan02/final-project/assets/97490525/b65ef6dc-6f78-4b27-8a45-adcf4b6baaf4" width="300" height="200">

Our main idea for this revolved around sorting the lots by their area and then placing and fitting the bed into the largest lot, and then the bookshelf, and so on. However, thinking about realistic room layouts and feng shui ("Chinese art of arranging buildings, objects, and space in an environment to achieve harmony and balance"), we realized that this implementation might not be the best for us. 

Due to the highly randomized nature of lot generation, it does not allow us to enforce specific rules that we might want, like having the bed be placed against a wall. Another issue that we ran into was ensuring that we were able to rotate the assets in a reasonable direction when placing them into the room. For example, making sure the dresser faces outwards such the drawers are accessible. Given these issues, for our final submission, we are looking into seeing if there is another method that would give us more control over asset placement while being procedural at the same time.

## Final submission (due 12/5)

### Procedural Placement of Assets
| *Main Assets* | *Wall Assets* | *Rug* |
|:--:|:--:|:--:|
| ![image](https://github.com/kishayan02/final-project/assets/97490525/4d484af4-a04f-4106-9bdc-73fc28b78228) | ![image](https://github.com/kishayan02/final-project/assets/97490525/c34e0e76-a0af-4613-8aa2-7b4eb701e43c) | ![image](https://github.com/kishayan02/final-project/assets/97490525/51ffe029-c0ae-4ada-9130-027d6c7db791) |

The main logic behind this new implementation uses 2 sets of points: manually chosen placement points, and points that can't be interesected with. For each groups of assets, we have a switch node that contains all the assets that we want to place, ordered by priority. For each asset, we attempt to place it at each of the placement points until we are to place it without it interesecting with any of the points that can't be interesected with. Once it has been placed, we use a Points From Volume node to add its bounding box to the points that can't be intersected with. We repeat this until we have placed all the assets. Between each group of assest (as well as between each asset), we make sure to pass in an updated group of points that can't be interesected with.

Here are the manually chosen placement points for each group of assests:
| *Main Assets* | *Wall Assets* | *Rug* |
|:--:|:--:|:--:|
| ![image](https://github.com/kishayan02/final-project/assets/97490525/8310f138-aaaa-4f99-be1d-2fef10d6b0c0) | ![image](https://github.com/kishayan02/final-project/assets/97490525/4873c145-c6c9-484e-aaee-d8a5fb1ded4a) | ![image](https://github.com/kishayan02/final-project/assets/97490525/5c3ddf8d-af1e-4323-bb2e-826b33fc8b1c) |

and here are the groups of points that cannot be intersected with that each group uses: 
| *Main Assets* | *Wall Assets* | *Rug* |
|:--:|:--:|:--:|
| ![image](https://github.com/kishayan02/final-project/assets/97490525/c417fe56-a7cc-4fa0-80a8-10638e4f2d11) | ![image](https://github.com/kishayan02/final-project/assets/97490525/9a86b680-26ff-4965-a67b-8cafd25e3f03) | ![image](https://github.com/kishayan02/final-project/assets/97490525/abc3c053-af1c-48af-8fc0-a37464544e14) |

Taking a look at how the objects are placed overlapped with the updated points that can't be intersected with for both the main assets and the wall assets, you can see that points outside the bounding boxes of the objects were added.
| *Main Assets* | *Wall Assets* |
|:--:|:--:|
| ![image](https://github.com/kishayan02/final-project/assets/97490525/632348bf-3692-4946-8d7a-c82261d9b218) | ![image](https://github.com/kishayan02/final-project/assets/97490525/a69bc05e-2de8-427d-9eb2-44e7f2811319) |

This is because we have manually adjusted the bounding boxes of the main assets to ensure that more realistic rooms are created. The wall assets, have their bounding boxes increased the same way for each object to give them a bit of border so they are not placed directly next to another object.

One final note, for the rug placement, the switch node contains several rugs of different sizes and it will stop attempting to place a rug once rug has been successfully placed.

### Procedurally Created Assets
While designing the system to procedurally place all the furniture in the rooms, we simultaenously procedurally create each of our assets, as shown below. 

#### Bookshelf
[https://github.com/kishayan02/final-project/assets/97490525/08dcc6fa-6656-488f-8a78-8e73915ec7d7'
](https://github.com/kishayan02/final-project/assets/97490525/08dcc6fa-6656-488f-8a78-8e73915ec7d7
)
#### Bed and Nightstand
<img src="https://github.com/kishayan02/final-project/assets/97934823/ac169bda-41e6-4c3c-87eb-20c9becd1c12" width="400" height="300">

#### Desk Setup (Desk, chair, laptop, lamp, and trash can)
<img src="https://github.com/kishayan02/final-project/assets/97934823/a1eccf84-3562-4745-a898-dc24365d3e60" width="400" height="300">

#### Dresser
<img src="https://github.com/kishayan02/final-project/assets/97934823/977c77ec-0747-46a0-a57c-ff06dd8419f5" width="400" height="300">

#### Floating Shelf
<img src="https://github.com/kishayan02/final-project/assets/97934823/0c4fc2d5-baa5-4f30-bb73-e6647ce5a939" width="400" height="300">

#### Window
<img src="https://github.com/kishayan02/final-project/assets/97934823/f92351ea-62f8-43b6-a4fb-513f21a0b361" width="300" height="300">

### Final Demo and Renders
The following is our demo and some pictures of the final assets

https://github.com/kishayan02/final-project/assets/97490525/54aa8c59-d3bb-4f52-bcb0-7206362db4b3

<img src="https://github.com/kishayan02/final-project/assets/97934823/6dadc8e5-c327-49aa-b8d2-e3a2491a89bf" width="400" height="300">
<img src="https://github.com/kishayan02/final-project/assets/97934823/4d6401b9-b3a2-45e4-8818-bdb3d6a25b36" width="400" height="300">
<img src="https://github.com/kishayan02/final-project/assets/97934823/47ba8e0c-4449-4e56-8252-7f815e64808f" width="400" height="300">
<img src="https://github.com/kishayan02/final-project/assets/97934823/9dcb4da6-04e4-4d61-9011-5658e9f3ce12" width="400" height="300">

## Post Mortem
We feel like our project came together really well at the end even though we had to pivot a lot between milestones in terms of how we procedurally place the furniture. Despite certain setbacks, we are super happy with the final product. We were able to accomplish all our goals, but if we were to change anything, we would exposed more parameters for the users to control. For our current version, we focused more on the random generation of the room in its entirety, rather than the creation of a tool. Thus, this could be a direction we take our project in in the future.
