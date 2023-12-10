***<h3 style="text-align: center;">We solemnly swear that we are up to no good . . .</h3>***
<p style="text-align: center"><img src="map_logo.jpg"></p>

# Meet the Team
- Nadine Adnane
- Nick Liu
- Insha Lakhani

# Introduction
Fantasy maps are awesome, and awesome procedural graphics people make them! One of the most iconic and beloved fantasy maps of all time is the Marauder's Map from Harry Potter! 

The Marauder's Map was a magical document that revealed all of Hogwarts School of Witchcraft and Wizardry. Not only did it show every classroom, every hallway, and every corner of the castle, but it also showed every inch of the grounds, as well as all the secret passages that were hidden within its walls and the location of every person in the grounds, portrayed by a dot. It was also capable of accurately identifying each person, and was not fooled by animagi, Polyjuice Potions, or Invisibility Cloaks; even the Hogwarts ghosts or Peeves were not exempt.

# Goal
We would like to develop a tool that procedurally generates a 2D map of a Hogwarts-like campus which also shows where everyone inside is wandering around.

# Inspiration/Reference
We were primarily inspired by the Marauder's Map from the Harry Potter series.

While looking for reference materials, we also came across this Marauder's map someone created for a themed Halloween party:
[Interactive Marauder's Map Halloween Project](https://cartland.medium.com/building-a-marauders-map-6552fa378cda)

### Important features we'd like to implement:

Walls made of text & a roughly rectangular but overall non-uniform map shape</br>
<img src="Footprint.png" height="200">

**Blocky rooms** which are generally rectangular </br>
<img src="BlockRoom.png" height="150">

**Circular towers** and curved hallways around them. Rooms in concentric rings.<br>
Time permitting, we may implement changes in the level of detail depending on the magnification of the map (zoom in/out).

<img src="Tower.png" height="200">

<img src="Tower2.png" height="200">

**Slanted rooms** with clipped corners & slanted hallways

<img src="SlantedHallway.png" height="200">

**Multi-rooms** that lead into other rooms, not just into hallways

<img src="MultiRoom.png" height="200">


# Specification

# Techniques
## Building creation
- Shape grammars for overall generation.

## Wanderer creation
- Path-finding algorithm will be used to draw footprints around the map to represent where people are wandering around.
- If possible, footprints will be animated to fade in and eventually disappear as they do in the film.

## Rendering
- WebGL Shaders to make the map visually appealing and to mimic the appearance in the film - aged paper with worn edges and ink splotches, and a hand-drawn feel for the buildings etc.

## Interactivity
- Time permitting, we could add sliders to vary the ratio of block buildings to circular towers etc.
- We also hope to add a prompt at the start where the user must enter "I solemnly swear that I am up to no good" before they can view the map! :)

# Design
We are currently planning to implement our project using WebGL, which will allow us to generate a live demo.

Below is a free-body diagram which illustrates how the components of our project will work together:

<img src="FBD.png" height="200">

# Timeline
<details>
  <summary>Milestone 1:</summary>
<ul>
	<li>Everyone</li>
	- Set up WebGL project & decide on collaboration methods
    <li>Nick & Nadine</li>
    - Research Shape grammars <br>
    - Generate basic Hogwarts Castle room/doorway structure and navigation mesh
    <li>Insha</li>
    - Basic path-finding </br>
    - Footstep animation (trailing footsteps disappear as more appear)
</ul>
</details>

<details>
	<summary>Milestone 2:</summary>
<ul>
    <li>Nick & Nadine</li>
    - Refine shape grammar/make sure the 4 core map features (blocky rooms, circular towers, slanted rooms, and multi-rooms) are working and looking good! </br>
    - Add detail to rooms (stairs, furniture, etc)
    - Apply text to shapes
    - Add interactive toggles for various parameters </br>
    <li>Insha</li>
    - Refine path-finding/address any bugs </br>
    - Work on map assets/shaders/visuals
</ul>
</details>

<details>
	<summary>Milestone 3:</summary>
<ul>
    <li>Nadine</li>
    - Polish UI/visuals
    <li>Nick</li>
    - Refine parameter toggles & address any remaining shape grammar issues
    <li>Insha</li>
    - Add interactive prompt at the start </br>
    <li>Everyone</li>
	- Address any remaining bugs </br>
	- Publish live demo </br>
	- Polish Github README
</ul>
</details>

# References
[Harry Potter Wiki - Marauder's Map](https://harrypotter.fandom.com/wiki/Marauder%27s_Map)