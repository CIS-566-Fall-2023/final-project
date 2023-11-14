# Islands Generation

Members: Xiaoxiao(Crystal) Zou, Keyu Lu, Ruijun(Daniel) Zhong

## Milestone 1

<details>
  <summary> Specifications </summary>

  ## Cloud Simulation (Keyu Lu)
  ### Objective:
  The primary objective of this milestone was to develop a dynamic cloud simulation that realistically mimics the movement, merging, and interaction of clouds in a natural environment.
  
  ### Approach and Technologies Used:
  To achieve this, I employed  Houdini metaball to simulate the dynamic behavior of clouds. This method allowed for the creation of clouds that not only move fluidly but also interact with each other in a natural way, such as merging or bouncing off each other.
  
  ### Fine-Tuning Details:
  **Mountain Noise Integration**: To add a touch of realism, I incorporated mountain noise node. This addition helps in simulating how clouds interact with mountainous terrain, effectively changing their shape and movement patterns.
  **Cloud Noise Enhancement**: To further refine the cloud's appearance, I added Houdini cloud noise. This ensures that each cloud has a unique, lifelike texture, enhancing the overall visual appeal.
  
  ### Demonstration and Insights:
  To showcase the results of this milestone, a demo video is provided below. The video highlights the dynamic cloud simulation in action, showcasing the realistic movement and interactions of the clouds. It offers a glimpse into the intricate details and the level of realism achieved through the combination of metaballs, noise algorithms, and Houdini's advanced capabilities.
  
  [![Cloud Simulation Demo](https://github.com/Cryszzz/final-project/blob/main/566%20Milestone%201%20Cloud.jpeg)](https://vimeo.com/884540553)



## Design Doc

<details>
  <summary> Specifications </summary>
  
  ## Introduction:

  Our project is motivated by the grandeur and ever-changing nature of landscapes, particularly those shaped by the elemental forces of nature such as islands. By procedurally generating islands, we aim to encapsulate the beauty of randomness and the complexity of natural phenomena. 

  ## Goal:

  We intend to achieve a robust procedural island generator system that is dynamic, visually appealing, and varied. Our system will not only generate islands but also simulate accompanying environmental elements like clouds, wave patterns, and ecological aspects like birds. This system could serve as a powerful tool for game development, film, and environmental simulation.

  ## Inspiration/reference: 

  We are inspired by the procedural generation techniques used in game development, such as those seen in "No Man's Sky" and "Minecraft," as well as the rich, complex simulations found in film CGI. We wish that we can create this realistic and visually stunning environment for our audiences. 

  ![](./images/image0.png)
  ![](./images/image1.png)
  ![](./images/image2.png)

  ## Features:
  - Cloud simulation
  - Floating + Animated islands
  - Lighting Effect 
  - Advanced features
      - Port it to Unity for rendering
      - Birds flying around islands
      - Waterfall and lakes on islands

  ## Timeline:

  - Milestone 1 (11/15 7 days): 
      - Main Features working individually on houdini
      - cloud (Keyu)
      - island (Crystal)
      - map (Daniel)
  - Milestone 2 (11/27 12 days):
      - Merge three main features on houdini (Crystal)
      - Lighting effect (Keyu)
      - Birds implmentation in Unity (Daniel)
  - Milestone 3 (12/5 8 days):
      - Polish (Together)
      - Merge everything in Unity for demo (Together)
  ## Techniques:

  We will do our islands generations on Houdini 
  - Map Generation:
    - Wave Function Collapse(Labs WFC Initalize Grid in Houdini)
  - Individual Island Generation:
    - Vines (hair simulation)
    - Water/ Waterfall(fluid particle simulation)
  - Cloud:
    - VBD node 
  - Birds:
    - Flocking system 
    - birds animation

</details>

