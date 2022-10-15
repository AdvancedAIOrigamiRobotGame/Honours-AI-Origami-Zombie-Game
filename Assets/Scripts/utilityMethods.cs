using Fare;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class Classifier
    {
        public bool isAgent = false; //self
        public bool isZombie = false; //nonself
    }

    class UtilityMethods
    {
        public static string AgentRepresentationPattern = "ORI.{5}CE";
        public static string ZombieRepresentationPattern = ".{10}";


        public static int AffinityHammingDistance(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return -1;
            }
            char[] sToArray = a.ToCharArray();
            char[] tToArray = b.ToCharArray();
            var compare = sToArray.Zip(tToArray, (c1, c2) => new { c1, c2 });
            int d = compare.Count(c => c.c1 != c.c2);
            return d;
        }


        public static Classifier matchingAlgorithm(string representation)
        {
            Classifier classify = new Classifier();
            int d;
            d = UtilityMethods.AffinityHammingDistance(representation, "ORI.....CE");
            if (d <= 5)
            {
                classify.isAgent = true;
                classify.isZombie = false;
            }
            else
            {

                classify.isAgent = false;
                classify.isZombie = true;

            }
            return classify;
        }



        public static string createRepresentation(int agentOrZombie)
        {
            string representation;
            Xeger xeger;
            if (agentOrZombie == 0)
                xeger = new Xeger(AgentRepresentationPattern, new System.Random());
            else
                xeger = new Xeger(ZombieRepresentationPattern, new System.Random());

            representation = xeger.Generate();
            return representation;
        }


        //creates a two dimensional List of random cells with double values based on the given lower and upper bounds.
        public List<string> create_random_cells(int population_size)
        {
            //create random array of strings
            var population = new List<string>();
            //Console.WriteLine("String representations: ");
            for (int r = 0; r < population_size; r++)
            {
                string tempstrRep = createRepresentation(r);
                //Console.WriteLine(tempstrRep);
                population.Add(tempstrRep);
            }

            return population;
        }

        /**
         * Takes in a list of population and then clones it.
         * returning a new list of clones
         */
        public List<(string, int)> clone((string, int) SubjectOfpopulation, double clone_rate)
        {
            var clone_num = (int)(clone_rate / SubjectOfpopulation.Item2);
            var clones = (from x in Enumerable.Range(0, clone_num)
                          select (SubjectOfpopulation.Item1, SubjectOfpopulation.Item2)).ToList();
            return clones;
        }



        public string hypermutateString(string entity)
        {
            char[] chentity = entity.ToCharArray();
            System.Random rand = new System.Random();

            for (int r = 3; r < chentity.Length - 2; r++)
            {
                //create mutated represantation
                char[] chcells;
                if (entity.StartsWith("ORI"))
                    chcells = createRepresentation(0).ToCharArray();
                else
                    chcells = createRepresentation(1).ToCharArray();
                //generate random indexes from [3 - chentity.length-2]
                int index = rand.Next(chentity.Length - 2);
                while(index < 3)
                    index = rand.Next(chentity.Length - 2);
                int c = rand.Next(chentity.Length - 2);
                while (c < 3)
                    c = rand.Next(chentity.Length - 2);
                chentity[index] = chcells[c]; //mutate one random char
            }

            string mutated_entity = "";
            for (int r = 0; r < chentity.Length; r++)
            {
                mutated_entity += chentity[r];
            }

            return mutated_entity;
        }



        /**
         * 
         */
        public List<(string, int)> hypermutate(List<(string, int)> SubjectOfpopulation)
        {
            //mutate population
            List<(string, int)> mutatedPopulation = new List<(string, int)>();
            for (int r = 0; r < SubjectOfpopulation.Count; r++)
                mutatedPopulation[r] = (hypermutateString(SubjectOfpopulation[r].Item1), SubjectOfpopulation[r].Item2);
            return mutatedPopulation;
        }



        /**
         * selection method
         * returns all entities without affinity measures
         */
        public List<string> select(List<(string, int)> pop, List<(string, int)> pop_clones, int pop_size)
        {
            //create array to store both populations
            var population = new List<string>();
            //get all current population
            foreach (var popper in pop)
            {
                population.Add(popper.Item1);
            }
            //get all clones
            foreach (var popi in pop_clones)
            {
                population.Add(popi.Item1);
            }

            //sort and return population
            population.Sort();
            return population;
        }


        public void replace(List<(string, int)> population, List<(string, int)> population_rand, int population_size)
        {
            //create array to store both populations
            var populationreplace = new List<(string, int)>();
            //get all current population
            for (int r = 0; r < population.Count; r++)
            {
                populationreplace.Add(population[r]);
            }

            //get all clones
            for (int r = 0; r < population_rand.Count; r++)
            {
                populationreplace.Add(population_rand[r]);
            }

            //sort and return population
            populationreplace.Sort();
            //replace population
            population = new List<(string, int)>();

            for(int r=0; r < population_size-1; r++)
            {
                population.Add(populationreplace[r]);
            }

            return;
        }
    }
