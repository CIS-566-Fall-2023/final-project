# Final Project: Townhouse Generator
by Sherry Li

## Design Doc

#### Introduction

When walking around my neighborhood near the west end of campus, I always find myself admiring the facades of the townhouses. Although they all follow a similar structure, each has its own unique charm. 

During the Procedural Jellyfish assignment, I really enjoyed making a tool that lets artists tweak many parameters to achieve a desired result and also instantly generate random variations to quickly populate a scene. I think this sort of procedural tool would be perfect to generate townhouses, which are often needed in large numbers along a street and impractical to model individually.

#### Goal

With this project, I intend to create a Houdini procedural modelling tool that lets artists generate variations of townhouse facades by customizing the parameters of a controller node.

#### Inspiration/reference:

My inspiration comes from the buildings around me. Specifically, I aim to reference the architectural style of flat-roof conjoined rowhouses that are [distinctive to Philadelphia](https://philadelphiaencyclopedia.org/essays/row-houses/).

<p align="center">
<img width="600" alt="image" src="https://github.com/sherryli02/final-project/assets/97941858/588fd1c7-869c-47c5-be21-4cd627137c96">
</p>

I want to have a substantial degree of realism, but not to the point of hyperrealism. Stylistically, I want to match the environments of animated films and games that have stylized characters but much more realistic buildings.

<p align="center">
<img width="600" alt="image" src="https://github.com/sherryli02/final-project/assets/97941858/6e32bf31-babb-4b7f-b2c9-988345c570a5">
</p>

#### Specification:

The user will be able to control a wide variety of parameters that affect building dimensions, roof styles, brick styles, window and shutter styles, door styles, staircase/door height, colors of each component, presence of items like AC units, flags, and lamps, and other details. I aim to strike a balance between specification and noise/proceduralism such that the user has a high level of control, but not so much that it becomes tedious to construct one house. 

Stretch goals include having the user be able to simply scrub through a seed to generate infinite random townhouse variations, and generating houses in a row along a street.

#### Techniques:

I aim to use a wide variety of Houdini nodes. Ideally, I hope to work with multiparms and Python scripting to customize instances of geometry. YouTube tutorials I'm looking at include [(x)](https://www.youtube.com/watch?v=gmYvA05orbs), [(x)](https://www.youtube.com/watch?v=h1JK3HL7Sb8), and [(x)](https://www.youtube.com/watch?v=uIe97023sDk).

#### Design:

<p align="center">
<img width="600" alt="image" src="https://github.com/sherryli02/final-project/assets/97941858/1b9efc7b-181d-483a-9441-aa16d52d0ac3">
</p>

#### Timeline:

Milestone 1 (by 11/15): Create the base geometry of all building elements (house, windows, door, staircase, inner details).

Milestone 2 (by 11/27): Refine geometry (bricks, outer details, color).

Milestone 3 (by 11/5): Implement stretch goals if possible (randomization, trees). Polish GUI controls and reate renders.



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
