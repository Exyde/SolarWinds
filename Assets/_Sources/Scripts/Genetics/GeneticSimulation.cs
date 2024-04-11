using EnhancedEditor;
using UnityEngine;

namespace Genetics
{
    public class GeneticSimulation : MonoBehaviour
    {
        public string targetString = "hello";
        public int populationSize = 1000;
        public int iterations = 1000;
        public float mutationRate = 0.1f; 
        
        [SerializeField] private Population population;

        [Button(ActivationMode.Always)]
        public void Simulate()
        {
            population = new Population(populationSize, targetString, mutationRate);
            population.Init();

            population.Log();

            for (int i = 0; i < iterations; i++)
            {
                population.UpdateFitness();
                population.ToNextGeneration();
            }

            population.Log();
        }
    }
}