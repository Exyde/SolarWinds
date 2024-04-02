
# Entity System

Sort of GOAP thing but not really ? Humm sounds kinda like

## Behaviors Components - The basic bricks

That's core.
[The morphologies of creatures and the neural systems for
controlling their muscle forces are both generated automatically
using genetic algorithms](https://www.karlsims.com/papers/siggraph94.pdf)

In this work, the phenotype embodiment of a virtual creature is a
hierarchy of articulated three-dimensional rigid parts. The genetic
representation of this morphology is a directed graph of nodes and
connections. Each graph contains the developmental instructions
for growing a creature, and provides a way of reusing instructions
to make similar or recursive components within the creature

- Gather / Collect
- Eat
- Drink
- Walk
- Fly
- Swim
- Die
- Birth
- Follow / Hunt
- Flee / Avoid
- Craft ? Custom behaviors ? See Rain World as well
- Less is more I guess, the things will come out of the systems

## External Sensors
- InSun / In Shadow
- Day/Night (Static)
- InRain
- Detect Water
- Detect Weather
- Detect Food Source
- Detect Entities = ScanSystem around on timings
- Detect Soulmate

## Structure 
- Genotype : Directed Graph of nodes and connections
  - Nodes
    - Developmental instructions for growing
    - Recursive component system
    - Can connect to themselves or in cycle : fractal structure
    - Connect to same child multiple times : duplicate appendices
  - Can be recursive
- Phenotype
  - Made from the graph, start at **root node**
  - Synthesize parts from the node information, while tracing the graph


**Node** : Describe a Rigid part
- Dimensions : Physical Shape (box ?)
- Joint-type constraints on the relative motion between this part and it's parent 
  - Degrees of freedom + Movement allowed for each degrees : Is this axis ?
  - Rigid, Revolute, twist, universal, bend-twist, twist-bend, spherical
- Joint-Limits : "The point beyond which restoring spring forces will be exerted for each degree of freedom" ?
- Recursive-Limit : How many time this node should generate a phenotype part when in a recursive cycle
- Set of local **Neurons** : _Later_
- Set of Connections to other nodes
  - Placement of a child parent relative to parent
    - position : only on parent surface
    - orientation
    - scale
    - reflection : used for negative scale and symmetry ?
    - terminal-only : apply this connection only if when recursive limit is reached

**The Brain**
- Input : Sensors Values (Environment Infos)
- Output : Effector Values (Muscle Control)
  - Applied as forces or torques on degrees of freedom