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


___ 



# Git Study
# Part 1 - Introduction
- You can commit on your local DT, and that's great.
- Everything has integrity at the lowest level with SHA-1 Hash.
- Three core states
	- modified : changed filed but not commited
	- staged : marked to go to the next commit
	- commited : safely stored in local database
- Working tree/directory : a single checkout : files pulled out from the compressed database in the git directory
- staged area : a file, generally in the git dir, also know as the index. Stores infos about what will be commited next 
- .git directory : most important part. stores metadata and object database of the project. This is what is copied when you clone

# Part 2 - Git Basics

## Commands Lists (To be gitted)
git config --list --show-origin 		// All of your settings and where are they coming from.
git config --global user.name "Sly"		//Settings the global username. you can override it per project. same with user.email
//You can configure almost anything in the config I guess like pull.rebase "true"
git config --global init.defaultBranch main 			//Change the default branch name from master to main
git config user.name				// read a specific key
git help config 					//open doc offline
git config -h 						//terminal help

## Framework Questions
- Logger as dll ? How ?