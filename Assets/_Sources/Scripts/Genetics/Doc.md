
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


# Genetic Algo

**Genotype** : The DNA, will be passed from gen to gen. Variate, mutate.
**Phenotype** : The expression of the *genotype*. How tall, small, fast, mesh, color, etc...

Example with Color :
- Phenotype : 0, 127, 255
- Genotype : White, Gray, Black

## Darwin Principles
- Heredity : Mechanism that allow to pass trait from parent to children
- Variation : Variation of traits in a population, also I guess it take small variation in heredity + random mutation probability.
- Selection : Survival of the fittest, meaning "able to reproduce". 
  A mechanism that allow only a subset of the parents to reproduce, the most adapted to the environment or problem.

## Steps
- 1 : **Create a population of N Elements, each with randomly generated DNA** (Example of cats with string as dna)
  - Generate a various population (the more you have, the more likely some will be able to fit the goal, or at least have a chance to evolve)
- 2 : **Selection**
  - **Evaluate Fitness**
    - Produce and evaluate a score for a given element. 
    - Example with cat : car, box, hur scores 2, 0, and 1. 
  - **Create a mating pool**
    - Use a probabilistic method like the *wheel of fortune**
    - Normalize each fitness score and express them as percent.
      - Sums all fitness score and divide each by the sums, you'll get a %
      - Now each element get it's own % of reproduction chance / to be added to the pool ?
- 3 : **Reproduction**
  You could pick one and reproduce with itself, but it breaks variety. We'll use a couple approach.
  - **Crossover**
    - That's a way of mixing the genotype from parent.
      - Flip coin and pick 50/50 a letter from parent A or B
      - Or, pick First half in A et other half in B
  - **Mutation**
    - It help ton counter-balance the fact that the population was initially created randomly on a limited subset of genotype expression.
    - It's an option
    - Express as a *rate* for the GA.
      - 5% rate mean that every gene will have 5% chance of mutate. 
        - Here for our cat example, a 1% rate mean each letter has 1% chance to mutate = select a totally new random letter
  
Then the children become the new population, and we repeat back to step 2 for X generation until we solve !