using System;
using System.Collections.Generic;
using System.Drawing;
using EnhancedEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Sources.Scripts.Genetics
{
    public class Shakespear : MonoBehaviour
    {
        public int generations = 100;
        [UnityEngine.Range(0f, 100f)] public float mutationRate = 1;
        public int populationSize = 100;

        [SerializeField] private List<string> population = new();

        [SerializeField] private string targetPhrase = "hello";

        [Button(ActivationMode.Always)]
        public void Simulate()
        {
            Setup();

            for (var i = 0; i < generations; i++)
            {
                Evolve();
            }
        }
        
        /// <summary>
        /// Create a starting population of N Elements with random DNA
        /// </summary>
        public void Setup()
        {
            population = new List<string>();
            int wordSize = targetPhrase.Length;

            for (var i = 0; i < populationSize; i++)
            {
                var creature = StringExtension.GetRandomString(wordSize);
                population.Add(creature);
            }
        }

        public void Evolve()
        {
            Selection();
            Reproduction();
            
            //Replaced old population with the new one
        }

        public void Selection()
        {
            //Evaluate the fitness score of each element
            //Build a mating pool
        }

        public void Reproduction()
        {
            //Pick two parent according to relative fitness

            string parentA = "";
            string parentB = "";
            
            string child = CrossOver(parentA, parentB);
            child = Mutate(child);
            
            //Add new child to next gen population
        }

        /// <summary>
        /// Create a child by mixing DNA of parents
        /// </summary>
        /// <param name="A">Parent A</param>
        /// <param name="B">Parent B</param>
        /// <returns></returns>
        public string CrossOver(string A, string B)
        {
            string child = "";

            for (var i = 0; i < targetPhrase.Length; i++)
            {
                //Pick a random gene in parent at 50% chance
                child += (Random.Range(0, 100) < 50) ? A[i] : B[i];
            }

            return child;
        }

        /// <summary>
        ///  Modify the child genes based on mutationRate
        /// </summary>
        /// <param name="child">the string to mutate</param>
        /// <returns></returns>
        public string Mutate(string child)
        {
            var mutation = "";

            foreach (var gene in child)
            {
                if (Random.Range(0, 100) < mutationRate)
                {
                    mutation += StringExtension.GetRandomLetter();
                    continue;
                }

                mutation += gene;
            }

            return mutation;
        }

    }
}