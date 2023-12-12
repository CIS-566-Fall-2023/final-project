#### Final Project
I created a WebGL city roadmap generator that procedurally generates a city roadmap from noise-based terrain and population inputs. The user is able to control the following parameters for the simulation:
   - Terrain size: the size of the terrain to generate. For me, >500 usually has a bit of lag when generating the terrain
   - Terrain scaling: scales the terrain height
   - Population scaling: scales the population density
   - Population height factor: how much to scale the population based on the terrain height (more means a larger factor)
   - Generate terrain: regenerates the terrain based on the current terrain parameters
   - Block size: a scaling for how large the city blocks are
   - Street spawn: a parameter for the streets L-system, serves to control the branching. A larger number indicates larger spacing between branching and thus less branching overall
   - Max highways: a number capping the number of highways that can generate initially on the map
   - Play/pause road: plays/pauses the road generation process. The user will be able to see the L-systems generate in real time when playing. Will regenerate based on the current road parameters if it has already finished generating.
   - Color: a viewing option for the user to change what parameter to color the terrain by.
   - Toggle buildings: a viewing option to toggle buildings on the terrain. Buildings won't show up until all the roads have generated, as they are placed in the final stage of the generation process.


#### Video
[https://youtu.be/5yCk9TVha9E](https://youtu.be/5yCk9TVha9E)

#### Final Results
My presentation slide deck (https://docs.google.com/presentation/d/1lm5N7C19Atu_MhVZVObkUcL93ecJ1wlXiOA1gLlM4Wg/edit?usp=sharing) has images from the project and also explanations of my algorithmic implementations. I created a large grid and triangulized it to display the terrain, using a shader to add coloring to the terrain to create the snowy peaks, greener lowlands, and water effects. I also had an option to switch between terrain and population coloring for easier visualization of each parameter. Roads were rendered using a bunch of line segments snapped onto the terrain. I originally wanted to calculate this in the GPU but I was unable to figure out a consistent and clean method of using the noise functions in my calculations as well as in the shader.

I also tried getting the live demo to run under gh-pages, but was unable to figure it out.

#### Post Mortem
The project took a lot longer in the algorithmic parts, as self-intersecting L-systems were very annoying to work with and tweak. Ultimately, this meant that I wasn't able to spend nearly as much time making things look polished visually. However, I was fairy satisfied with the road maps, which looked realistic for the most part.
