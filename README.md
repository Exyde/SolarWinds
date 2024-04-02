# SolarWinds
 Sunborn. Sunrise. And Sundeath.

## Born of a design doc
- A fight fast paced game just like RL 
- Something like a mix between RL, Mirror's Edge, Samurai Game, a Versus Game, les marchombres, Titanfall
- ça va vite, ça meurt vite, round rapide, et tout est basé sur la maitrise d'un controleur simple et systémique
	Clean code, network
- Mécanique systéique, interplay, granularité, comme Samurai Gun
- Gamefeel fort de RL
- Easy to learn, never mastered

## Main Notes
- Utils is nice. You've a lot of extensions to extract
- Add Settings/Scriptable Settings just like Avalanche
- Implement the framework ? Learn from it.

J'aime beaucoup le flow scriptable settings (datas), class C# pure avec injection de settings, ObservableThing et Controller derrière en fin de courses

- Todo: 
	- Implement the framework : parts by parts, just rework it ?
	- Reimport and repush all asetts : ok
	- Add DoTween : ok
	- Merge with the other project on desktop from train : ok 
	- Git ammend day/night thing for sun stsate events
	- Add Scriptable Settings for data management and easy system
	- Observable : wip
	- Get a better structure like in MC or Aria maybe
	- Impletement the kindof actions system from lucas for entity : talk

## Code Structure Notes - Engine

### Core
- Add a service system, like core. someone who's responsable / entry point for creating stuff like logger and enabling singletons
### Camera
- Nothing fancy here yet. Add Cinemachine wrapper ? Add more generic stuff like wrapper for DoFOv, Shake  and so on => Base generic controller ?
### Cheats
- 
### Datas
- Move SO datas / settings here. A folder full off datas.
### Editor
### Extensions
### GameEvents
### Inputs
- Todo : Add and Wrap the new input system
### Logger
- Todo : Create the scriptable settings asset
### Utils
- Meshes : ok


