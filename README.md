# Character Creator

# Design Doc

## Introduction

This project will serve as a base for a procedural character creation system that could hopefully be integrated into my game,
[Fling to the Finish](https://store.steampowered.com/app/1054430/Fling_to_the_Finish/), in the future. The game uses an art-style where all characters fit within a bounding-sphere, and must be *roughly* round shaped. So far, all the characters in the game have been hand-created by our amazing team. This is a difficult and time consuming process, and also limits our ability to release unlockable content more freely.

## Goal

A tool made in Unity that allows the user to create roughly spherical characters by using smooth (or hard) blended SDFs and a procedural texturing tool for the generated characters. This tool will allow the user to choose from a bunch of predefined metaball primitive shapes and combine them to make a character. The user will be able to procedurally texture the generated character. Both the geometry and texturing stages will allow the user to tweak parameters to customise the look of their character.

## Inspiration

[Spore](https://store.steampowered.com/app/17390/SPORE/) uses a signed distance field (SDF) (more specifically, metaballs) based approach for its extremely robust character creation engine. While this engine is ahead of its time and really technologically complex, I wish to create a tool that could replicate a small chunk of its functionality.

Most of this will be achieved by referencing [IQ's amazing articles](https://iquilezles.org/articles/) on SDFs, raymarching and procedural texturing of SDFs.

## Specifications

**1. Primitive Metaball based character generator**

This system will use a number of predefined metaballs including:

- a base body shape (sphere, rounded cube, pyramid)
- optional accessories, including
  - eyes, mouth and nose
  - wings
  - visual enhancements, such as a torus

The user will be allowed to first select a base body shape, and then add up to 4 accessories, by moving, rotating, and scaling them in 3D space near the main body metaball.

**2. Raymarching in Unity**

Since the system will use metaballs, it needs a raymarching algorithm to render these metaballs. This will be implemented as a HLSL shader in Unity.

**3. Procedural Texturing**

This will allow the user to colour each part of the generated character based on chosen parameters:
  - shading model (lambertian/phong/PBR)
  - colour and glossiness
  - applying procedural textures on to the shape based on a spherical projection (more explained later)

**Stretch goals:**

**1. Apply different stylized effects for texturing**

Instead of simply rendering the mesh, allow the user to generate a stylized character (toon shaded, pixelated, etc.)

**2. Generated character to mesh conversion, with automated UV mapping and texture projection**

This is required for use in a traditional rasterized pipeline, which is what most games, including Fling to the Finish, use. Since this is not the main focus of the project, this will be a stretch goal based on the progress of the project.

## Techniques

- **Unity Engine** will be used for the development of this tool.
- [SDFs / metaballs](https://iquilezles.org/articles/raymarchingdf/) for character body parts generation, adjustment, and rendering.
- For procedurally texturing based on predefined texture maps, the texture maps will be projected on to a sphere similar to [these PBRT approaches](https://www.pbr-book.org/3ed-2018/Monte_Carlo_Integration/2D_Sampling_with_Multidimensional_Transformations), and then projected on to the generated character based on its surface normals. This approach is rather naive, but will serve as a good starting point.

## Design

## Timeline

- **Week 1 | Milestone 1 (11/15)**
  - Setup raymarching in Unity
  - Create basic SDF shapes
  - Setup moving and combining multiple SDF shapes using smooth and hard blending options
- **Week 2 | Milestone 2**
  - Finish moving, rotating and scaling of different SDF shapes
  - Allow multiple types of blending based on a smoothed interpolation between smooth and hard blending
    - Based on preset AnimationCurves (stretch goal: allow user to manipulate these curves)
- **Week 3 | Milestone 3 (11/27)**
  - Create UI for texturing individual primitives
    - Lambertian shading model
    - Phong shading and reflections
  - Texture mapping based on spherical projection
- **Week 4 | Final (12/5)**
  - Improve UI of the tool
  - Improve visual fidelity
  - Final tweaking and adjustments of visual parameters