using System.Collections.Generic;
using System.Linq;
using Engine.Extensions;
using EnhancedEditor;
using UnityEngine;

namespace Genetics
{
    public class Shakespear : MonoBehaviour
    {
        public int generations = 100;
        [UnityEngine.Range(0f, 100f)] public float mutationRate = 1;
        public int populationSize = 100;

        [SerializeField] private List<DNA> population = new();

        private List<int> notEmptyIndex = new List<int>();

        private List<DNA> _matingPool = new();

        [SerializeField] private string targetPhrase = "hello";

        private int _emptyMatingPoolCount = 0;

        [Button(ActivationMode.Always)]
        public void Simulate()
        {
            Setup();
            
            //LogPopulation(population);

            for (var i = 0; i < generations; i++)
            {
                if (population == null)
                {
                    Debug.Log("Aborted. No more fit candidate.");
                    break;
                }
                ProcessGeneration(i);
            }
            
        }
        
        /// <summary>
        /// Create a starting population of N Elements with random DNA
        /// </summary>
        public void Setup()
        {
            notEmptyIndex.Clear();
            _emptyMatingPoolCount = 0;
            _matingPool = new List<DNA>();
            population = new List<DNA>();
            
            int wordSize = targetPhrase.Length;

            for (var i = 0; i < populationSize; i++)
            {
                population.Add(new CatStringDNA(wordSize));
            }
        }

        public void ProcessGeneration(int genIndex)
        {
            Selection(genIndex);
            var nextGeneration = Reproduction();

            //Replaced old population with the new one

            if (nextGeneration == null)
            {
                
                LogPopulation(population);
                Debug.Log("End Of simulation ");
                Debug.Log("Empty Mating Pool " + _emptyMatingPoolCount );
                Debug.Log("Not Empty Indexes ");
                Debug.Log("Pool Indexes : "  + GetIndexStrings());


                string GetIndexStrings()
                {
                    string r = "[ ";

                    foreach (var index in notEmptyIndex)
                    {
                        r += $" {index} ";
                    }

                    r += " ]";

                    return r;
                }
            }
            
            population = nextGeneration;
        }

        public void Selection(int genIndex)
        {
            //Evaluate the fitness score of each element
            foreach (var element in population)
            {
                element.EvaluateFitness(targetPhrase);
            }
            
            UpdateMatingPool(genIndex);
        }

        private void UpdateMatingPool(int genIndex)
        {
            _matingPool.Clear();
            _matingPool = new List<DNA>();

           foreach (var dna in population)
           {
               var occurence = (dna.FitnessScore * 10);

               for (int i = 0; i < occurence; i++)
               {
                   _matingPool.Add(dna);
               }
           }

           if (_matingPool.Count != 0)
           {
               //LogPopulation(_matingPool);
               notEmptyIndex.Add(genIndex);
           }
        }

        public List<DNA> Reproduction()
        {
            var nextGeneration = new List<DNA>();

            if (_matingPool.Count == 0)
            {
                return null;
                _emptyMatingPoolCount++;
                nextGeneration = population;
                return nextGeneration;
            }
            

            for (int i = 0; i < populationSize; i++)
            {
                //Pick two parent according to relative fitness
                DNA parentA = GetFitCandidate();
                DNA parentB = GetFitCandidate();
            
                var child = parentA.CrossOver(parentB);
                child.Mutate();
            
                //Add new child to next gen population
                nextGeneration.Add(child);
            }
            
            return nextGeneration;
        }

        private DNA GetFitCandidate()
        {
            var index = new System.Random().Next(_matingPool.Count);
            
            if (index < 0 || index >= _matingPool.Count)
            {
                Debug.LogError("Criminal Index " + index);
            }
            return _matingPool[index];
        }

        public void LogPopulation(List<DNA> pop)
        {
            int index = 1;
            foreach (var dna in pop)
            {
                dna.LogSelf(index);
                index++;
            }
        }
        

    }
}