# TerrainGenerator

This project demonstrates manual mesh construction, multi‑octave noise generation, and flexible terrain shaping, without using Unity’s built‑in terrain tools.

<h2>Project Overview</h2>

This project explores both the fundamentals and advanced techniques of procedural generation by constructing a complete terrain system from scratch.
All geometry, noise, and rendering logic is implemented manually using:

1. Custom vertex grids
2. Triangle indexing
3. Multi‑octave noise (Perlin + fractal noise)
4. Adjustable terrain parameters
5. Editor‑mode visualization

The goal is to showcase strong algorithmic thinking, low‑level geometry manipulation, and technical design skills relevant to game development, graphics programming, and tools engineering.

<h2>Implementation</h2>

The implementation began with building a 2D grid of vertices that formed the foundation of the terrain mesh. 

<img width="691" height="475" alt="Generating_Grid" src="https://github.com/user-attachments/assets/595d7c60-0277-4b37-9ce3-1f33d50da816" />

After establishing the grid, I implemented the triangle indexing system, ensuring that all triangles followed the correct winding order to avoid rendering issues. 

<img width="691" height="432" alt="Step_1" src="https://github.com/user-attachments/assets/7912a96d-d22c-4419-86df-d29769c4ad18" />
<img width="691" height="432" alt="Step_2" src="https://github.com/user-attachments/assets/ac7fdc36-6d6f-4c12-b51f-914a9e14c2a3" />

With the mesh structure in place, I first added single‑octave Perlin noise to generate basic height variation. 

<img width="691" height="452" alt="Single_Octave" src="https://github.com/user-attachments/assets/ef3dbc56-58c4-4dbf-ba5f-32295d28d442" />

Later, I expanded this into a full multi‑octave fractal noise system. During this stage, I spent time debugging frequency and amplitude calculations to correct distortions in the terrain.
Once the noise behaved as expected, I introduced height‑based vertex coloring to make elevation differences visually clear. Finally, I created an editor script that allowed the terrain to be regenerated directly inside the Unity editor, enabling real‑time iteration without entering Play Mode.

<img width="691" height="528" alt="Procedural_Terrain" src="https://github.com/user-attachments/assets/d2152ba3-f471-422e-9806-0a5ad1780228" />

