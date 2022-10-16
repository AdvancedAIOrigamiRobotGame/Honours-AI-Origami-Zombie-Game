using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NSA: MonoBehaviour
{
    
    public static List<Detector> finalDetectors = new List<Detector>();
    public static List<string> selfSpace = new List<string>();
    public static int nOrigamis;

    // Detector
    public class Detector
    {
        public string representation;
        public matchingRule matchingRule;

    }

    //Censoring phase
    public static void GenerateNonSelfDetectors(List<string> self, int nDectecors)
    {
        //Generate initial Detectors
        System.Random rand = new System.Random();
        for (int i = 0; i < nDectecors; i++)
        {
            Detector detector = new Detector();
            detector.matchingRule = new matchingRule();
            bool validDetector = false;
            while (!validDetector)
            {
                int n = rand.Next(2);
                detector.representation = scheme.createRepresentation(n);
                bool isAgent = false;

                //check if any matched self/origami representation
                foreach (string s in self)
                {
                    matchingRule.Classifier isSelf = new matchingRule.Classifier();
                    isSelf = detector.matchingRule.matchingAlgorithm1(s, detector.representation);
                    if (isSelf.isAgent)
                    {
                        isAgent = true;
                        // break;
                    }
                }
                if (!isAgent)
                {
                    finalDetectors.Add(detector);
                    validDetector = true;
                }

            }

        }
    }

    //Monitoring stage
    public static bool detectNonSelf(string s)
    {
        foreach (Detector detector in finalDetectors)
        {
            if (detector != null)
            {
                matchingRule.Classifier isNonSelf = new matchingRule.Classifier();
                isNonSelf = detector.matchingRule.matchingAlgorithm1(s, detector.representation);
                if (isNonSelf.isZombie)
                    return true;
                else
                    return false;
            }
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
       // while (nOrigamis != 5) { continue; }
        GenerateNonSelfDetectors(selfSpace, 10);
        Debug.Log("Origami List");
        int nSelf = 0;

        foreach(string s in selfSpace)
        {
            Debug.Log("S " + nSelf++ + "- "  + s);
        }

        Debug.Log("Detectors");
        int nD = 0;
        foreach(Detector detector in finalDetectors)
        {
            Debug.Log("D  " + nD++ + "- " + detector.representation);
            Console.WriteLine("D  " + nD++ + "- " + detector.representation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
