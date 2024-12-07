# WinFormsPhysics Engine

**WinFormsPhysics** is a physics simulation engine built for Windows Forms applications. It allows you to simulate basic mechanics, including gravity, object collisions, and boundary constraints, with a simple and extensible API.

## Features

- **2D Vector Algebra**: Built-in vector operations for positions, velocities, and forces.
- **Gravity Simulation**: Supports both global gravity and object-to-object gravitational interactions.
- **Object Collision Detection**: Handles elastic and inelastic collisions with other objects and screen boundaries.
- **Customizable Physics Parameters**:
  - Adjustable time lapse for simulation speed.
  - Modifiable gravitational constants and vectors.
- **Drag Support**: Interact with objects using drag mechanics.
- **Extensibility**: Designed to integrate seamlessly with custom object forms in Windows Forms.

## Getting Started

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/nekogravitycat/WinFormsPhysics.git
   ```
2. Open the project in Visual Studio.
3. Build and run the solution.

## Project Structure

- **Vector**: Implements basic 2D vector operations for physics calculations.
- **Object**: Represents a physical object in the simulation.
- **PhysicsEngine**: Core engine managing forces, collisions, and object updates.

## Future Improvements

- Add rotational dynamics.
- Introduce friction and air resistance.
- Extend collision handling to handle complex shapes.
