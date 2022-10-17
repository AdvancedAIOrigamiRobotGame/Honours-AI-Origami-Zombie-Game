using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClonalSlection : MonoBehaviour
{
    int population_size;
    int selection_size;
    int random_cells_num;   
    int clone_rate;
    int stop_codition;
    // Start is called before the first frame update
    void Start()
    {
        //get current population size from existing population in the game
        population_size = 100;
        selection_size = 10;
        random_cells_num = 20;
        clone_rate = 20;
        stop_codition = 2;
    }

    // Update is called once per frame
    void Update()
    {
            //1. Initialization
            var clonalg = new UtilityMethods();
            //Create initial random population of antibodies P
            List<string> population = clonalg.create_random_cells(population_size);
            var best_affinity_it = new List<(string, int)>();
            //Antigenic presentation


            var stop = 0;
            while (stop != stop_codition) //while not stopping criterion met do
            {
                var affinity_it = new List<(string, int)>();
                foreach (var antigenZombie in population) //for each antigen do
                {
                        foreach (var antibodyOri in population) //for each antibody do
                        {
                            if (UtilityMethods.matchingAlgorithm(antibodyOri).isAgent && UtilityMethods.matchingAlgorithm(antigenZombie).isZombie && antigenZombie != antibodyOri)
                            {
                                affinity_it.Add((antigenZombie, UtilityMethods.AffinityHammingDistance(antigenZombie, antibodyOri)));
                            }
                        }
                }
                //sort best
                affinity_it.Sort();

                //get best top five affinities
                for (int r=0; r < affinity_it.Count; r++) //for each antigen do
                {
                    if(r < affinity_it.Count - 5) { best_affinity_it.Add(affinity_it[r]); } 
                }
                //sort 
                best_affinity_it.Sort();



                //get population select
                var population_select = new List<(string, int)>();
                for (int r=0; r < affinity_it.Count; r++) //for each antigen do
                {
                    if(r < affinity_it.Count - selection_size) { best_affinity_it.Add(affinity_it[r]); } 
                }


                var population_clones = new List<(string, int)>();
                foreach (var p_i in population_select) //for each antigen do
                {
                    var pip_clones = clonalg.clone(p_i, clone_rate);
                    foreach (var pclone in pip_clones)
                    {
                        population_clones.Add(pclone);
                    }
                }


                //hypermutate population
                population_clones = clonalg.hypermutate(population_clones);
                //Population <- Select(Population, Population_clones, Population_size)
                population = clonalg.select(affinity_it, population_clones, population_size);


                //Population_rand <- CreateRandomCells(RandomCells_num)
                var population_rand = clonalg.create_random_cells(random_cells_num);
                var population_rand_affinity = new List<(string, int)>();
                foreach (var antigenZombie in population_rand) //for each antigen do
                {
                    foreach (var antibodyOri in population_rand) //for each antibody do
                    {
                        if (UtilityMethods.matchingAlgorithm(antibodyOri).isAgent && UtilityMethods.matchingAlgorithm(antigenZombie).isZombie && antigenZombie != antibodyOri)
                        {
                            affinity_it.Add((antigenZombie, UtilityMethods.AffinityHammingDistance(antigenZombie, antibodyOri)));
                        }
                    }
                }
                population_rand_affinity.Sort();


                //Replace(Population, Population_rand)
                clonalg.replace(affinity_it, population_rand_affinity, random_cells_num);
                population = (from x in Enumerable.Range(0, affinity_it.Count)
                              select (affinity_it[x].Item1)).ToList();

                stop++;
            }


            //There should be a way to feed back the new population back to the world, in fact it should give the clones back to the world 
            //and place them at random distances from target antibody

            /**
            foreach (var pop_it in best_affinity_it)
            {
                create new zombie based on clone affinity and random distance from origami
            }**/
    }
}
