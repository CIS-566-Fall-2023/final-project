#### Introduction
I did some work generating villages and remembered reading a cool paper on generating city street and highway networks based on population/terrain, which was too overkill for the time. I thought it would be a cool idea to combine the concepts in this paper with some Townscaper-like user interactivity with the environment.

#### Goal
To create some sort of city generator based on realistic parameters like terrain, population, etc.

#### Inspiration/reference:
As mentioned in the intro, I was inspired by the CityEngine outlined in this paper: https://cgl.ethz.ch/Downloads/Publications/Papers/2001/p_Par01.pdf. Halfway through my project, I also discovered that (of course) Oskar Stålberg has a city generator game (https://www.oskarstalberg.com/game/CityGenerator/), which was a good example of city-like closed L-systems but didn't use any features aside from the existing roads for generation.

#### Specification:
Some features present in the simulation include: terrain and population generation parameters, highway and road step-by-step generation. The user should be able to change parameters to influence terrain and population so as to see differences in city generation based on this differernces.

#### Techniques:
I will be looking at L-system generation of highways and roads. More specifically, I'll be using techniques for closed L-systems for road systems, which has been applied to blood vessel generation.

#### Design:
- How will your program fit together? Make a simple free-body diagram illustrating the pieces.

Submission:
- Push all your code / files to your repository
- Come to class ready to present your finished project
- Update your README with two sections 
  - final results with images and a live demo if possible
  - post mortem: how did your project go overall? Did you accomplish your goals? Did you have to pivot?
