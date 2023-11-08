# Islamic Geometric Wood Carving Tool
Aboudi Rai

### Design Doc

#### Introduction
Islam rose in 7th century Arabia as a stark refutation to the idol-worshipping pagans who
 dominated the region economically, politically, and in their sheer number. Having belief in one God
 for whom no manmade idol could represent, the Muslims, upon successful disemination of their
 religion and beliefs across the peninsula, haulted all art depicting living beings. In its stead,
 a new form of art was championed &ndash; one that still captured a beautiful aspect of God's
 creation that may not have been breathing, but was very much alive: geometry.


#### Goal
Through this project, I aim to create a Houdini node that can produced the complex
geometric patterns that are essential to so many pieces of Islamic art. Harnessing the power of a
procedural system, I hope to add a dynamic element onto the baseline patterns by animating the
patterns, either within a single pattern's context, or perhaps even animating between different
patterns.

To maintain the craftsmanship that is so essential to the appreciation of these
decorative wooden arches, carpets, and architectural structures, the tool will be applied to wooden slabs.


#### Inspiration/reference:
- You must have some form of reference material for your final project. Your reference may be a research paper, a blog post, some artwork, a video, another class at Penn, etc.  
- Include in your design doc links to and images of your reference material.

![](img/ref-wooden-arches.png)
[Source](https://www.etsy.com/listing/1467150352/set-of-decorative-arches-islamic-arabic?gpla=1&gao=1&&utm_source=google&utm_medium=cpc&utm_campaign=shopping_us_d-craft_supplies_and_tools-patterns_and_how_to-patterns_and_blueprints&utm_custom1=_k_CjwKCAjwkY2qBhBDEiwAoQXK5e3zWAU40x-ESz_xxUsEbw_pSet9z7k0Me79Zgcc4MTAHoWqSTIgJRoCcxAQAvD_BwE_k_&utm_content=go_2063558056_76452866095_367965825024_pla-322726483858_c__1467150352_12768591&utm_custom2=2063558056&gclid=CjwKCAjwkY2qBhBDEiwAoQXK5e3zWAU40x-ESz_xxUsEbw_pSet9z7k0Me79Zgcc4MTAHoWqSTIgJRoCcxAQAvD_BwE)

![](img/ref-wooden-pieces.png)
[Source](https://www.etsy.com/listing/1365553332/set-vector-arches-dxf-eps-svg-ai-pdf-png?click_key=567e1af9aaf9f8ea4ad89f283786370ebc24bdfd%3A1365553332&click_sum=7c5186a7&external=1&rec_type=ss&ref=pla_similar_listing_top-1&sts=1)

![](img/ref-pattern-0.jpg)
[Source](https://www.alamy.com/seamless-geometric-ornament-based-on-traditional-arabic-art-muslim-mosaicblack-lines-and-white-backgroundgreat-design-for-fabrictextilecoverwrap-image339544071.html)

#### Specification:
- Outline the main features of your project.

The project will be a Houdini node that will have take geometry as its input, and cut from it the
specified geometric pattern. The geometry will be specified by a node parameter that the user will
ahve to set themselves. I will be developing the node with the assumption that simple wooden
slab-like geometry is being input into the node, and so not all geometry will result in the desired output.

The user will have control over:
  - the pattern type
  - the scale of the pattern within the object
  - whether the object is animating and the speed of which
  - pattern fade
      - the user can select two patterns, and the node will cyclicly interpolate back and forth
        between the two different patterns.

#### Techniques:
- What are the main technical/algorithmic tools you’ll be using? Give an overview, citing specific
  papers/articles.

- L-Systems
- Greedy algorithm (similar to one used in lego project):
    to fill up the volume of the input object with the pattern cutouts.
- Noise to generate the paths and shapes of the patterns
  - It will be deterministic, so the patterns maintain their intended look which I will be curating

#### Design:
- How will your program fit together? Make a simple free-body diagram illustrating the pieces.

#### Timeline:
- Create a week-by-week set of milestones for each person in your group. Make sure you explicitly
  outline what each group member's duties will be.

[11/8 &rarr; 11/15] 

[11/15 &rarr; 11/22] 

[11/22 &rarr; 11/27] 

[11/22 &rarr; 12/5] 

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

## Topic Suggestions

### Create a generator in Houdini

### A CLASSIC 4K DEMO
- In the spirit of the demo scene, create an animation that fits into a 4k executable that runs in real-time. Feel free to take inspiration from the many existing demos. Focus on efficiency and elegance in your implementation.
- Example: 
  - [cdak by Quite & orange](https://www.youtube.com/watch?v=RCh3Q08HMfs&list=PLA5E2FF8E143DA58C)

### A RE-IMPLEMENTATION
- Take an academic paper or other pre-existing project and implement it, or a portion of it.
- Examples:
  - [2D Wavefunction Collapse Pokémon Town](https://gurtd.github.io/566-final-project/)
  - [3D Wavefunction Collapse Dungeon Generator](https://github.com/whaoran0718/3dDungeonGeneration)
  - [Reaction Diffusion](https://github.com/charlesliwang/Reaction-Diffusion)
  - [WebGL Erosion](https://github.com/LanLou123/Webgl-Erosion)
  - [Particle Waterfall](https://github.com/chloele33/particle-waterfall)
  - [Voxelized Bread](https://github.com/ChiantiYZY/566-final)

### A FORGERY
Taking inspiration from a particular natural phenomenon or distinctive set of visuals, implement a detailed, procedural recreation of that aesthetic. This includes modeling, texturing and object placement within your scene. Does not need to be real-time. Focus on detail and visual accuracy in your implementation.
- Examples:
  - [The Shrines](https://github.com/byumjin/The-Shrines)
  - [Watercolor Shader](https://github.com/gracelgilbert/watercolor-stylization)
  - [Sunset Beach](https://github.com/HanmingZhang/homework-final)
  - [Sky Whales](https://github.com/WanruZhao/CIS566FinalProject)
  - [Snail](https://www.shadertoy.com/view/ld3Gz2)
  - [Journey](https://www.shadertoy.com/view/ldlcRf)
  - [Big Hero 6 Wormhole](https://2.bp.blogspot.com/-R-6AN2cWjwg/VTyIzIQSQfI/AAAAAAAABLA/GC0yzzz4wHw/s1600/big-hero-6-disneyscreencaps.com-10092.jpg)

### A GAME LEVEL
- Like generations of game makers before us, create a game which generates an navigable environment (eg. a roguelike dungeon, platforms) and some sort of goal or conflict (eg. enemy agents to avoid or items to collect). Aim to create an experience that will challenge players and vary noticeably in different playthroughs, whether that means procedural dungeon generation, careful resource management or an interesting AI model. Focus on designing a system that is capable of generating complex challenges and goals.
- Examples:
  - [Rhythm-based Mario Platformer](https://github.com/sgalban/platformer-gen-2D)
  - [Pokémon Ice Puzzle Generator](https://github.com/jwang5675/Ice-Puzzle-Generator)
  - [Abstract Exploratory Game](https://github.com/MauKMu/procedural-final-project)
  - [Tiny Wings](https://github.com/irovira/TinyWings)
  - Spore
  - Dwarf Fortress
  - Minecraft
  - Rogue

### AN ANIMATED ENVIRONMENT / MUSIC VISUALIZER
- Create an environment full of interactive procedural animation. The goal of this project is to create an environment that feels responsive and alive. Whether or not animations are musically-driven, sound should be an important component. Focus on user interactions, motion design and experimental interfaces.
- Examples:
  - [The Darkside](https://github.com/morganherrmann/thedarkside)
  - [Music Visualizer](https://yuruwang.github.io/MusicVisualizer/)
  - [Abstract Mesh Animation](https://github.com/mgriley/cis566_finalproj)
  - [Panoramical](https://www.youtube.com/watch?v=gBTTMNFXHTk)
  - [Bound](https://www.youtube.com/watch?v=aE37l6RvF-c)

### YOUR OWN PROPOSAL
- You are of course welcome to propose your own topic . Regardless of what you choose, you and your team must research your topic and relevant techniques and come up with a detailed plan of execution. You will meet with some subset of the procedural staff before starting implementation for approval.
