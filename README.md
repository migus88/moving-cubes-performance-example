# Moving Cubes Performance Example

[![Live Demo](https://img.shields.io/badge/Live%20Demo-Play%20Now-brightgreen)](https://migus88.itch.io/cubes-performance-example)

This project demonstrates the significant performance impact that different memory access patterns can have on game performance, even when using the same underlying algorithm.

## Overview

This Unity project showcases how the same simple physics simulation (moving and colliding cubes) can perform drastically differently based solely on how we access and organize data in memory. By implementing the same basic algorithm in several different ways, the performance differences become clearly visible.

## Live Demo

Try out the live WebGL demo here: [https://migus88.itch.io/cubes-performance-example](https://migus88.itch.io/cubes-performance-example)

## Implementations

The project includes several different implementations of the same cube movement and collision algorithm:

### 1. Unity Physics (UnityPhysicsCubesManager)
Uses Unity's built-in physics engine with Rigidbodies and Colliders. This approach is decentralized, with each cube handling its own physics via the PhysicsCube component.

### 2. Decentralized OOP (DecentralizedOopCubesManager)
Each cube is represented by a SelfMovingCube component that handles its own movement and collision detection. This represents a traditional object-oriented approach where each object is responsible for its own behavior.

### 3. Centralized OOP (CentralizedOopCubesManager)
Uses a centralized manager that updates all cubes. Each cube is still represented as a separate Cube object, but the movement and collision logic is handled by the manager rather than the cubes themselves.

### 4. Data-Driven Design with Parallel Arrays (DddParallelArraysCubesManager)
Follows a data-oriented approach using separate parallel arrays for different properties (positions, velocities, etc.). This improves cache coherence when processing one property at a time across all cubes.

### 5. Data-Driven Design with Struct Arrays (DddStructsArrayCubesManager)
Uses an array of structs, where each struct contains all data for a single cube. This keeps all data for each cube together in memory, potentially improving cache locality when processing all properties of a single cube at once.

## Why It Matters

This project demonstrates a deliberately inefficient algorithm to make the performance differences more apparent. In real-world applications, the algorithm itself would be optimized, but the memory access patterns demonstrated here remain critically important for performance.

Key takeaways:
- Memory access patterns can have a massive impact on performance
- Cache coherence and data locality are crucial for high-performance code
- Modern CPUs are often bottlenecked by memory access, not computation
- Data-oriented design can significantly outperform traditional OOP approaches for performance-critical systems

## How to Use

1. Open the project in Unity
2. Load the Example scene
3. Run the scene and toggle between different implementations
4. Observe the FPS counter to see the performance differences
5. Experiment with different numbers of cubes to see how each implementation scales

## License

This project is intended for educational purposes to showcase performance optimization techniques.
