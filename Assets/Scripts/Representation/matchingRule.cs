using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class matchingRule : MonoBehaviour
{
    public class Classifier
    {
        public bool isAgent = false; //self
        public bool isZombie = false; //nonself
    }

    public static int HammingDistance(string a, string b)
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



    public Classifier matchingAlgorithm1(string a, string b)
    {
        Classifier classify = new Classifier();
        int d;
        d = HammingDistance(a, b);
        if (d == 5 || (a.Contains("ORI") && b.Contains("ORI")) || !a.Contains("ORI") || b.Contains("ORI")) //all origami agents have the same 5 characters in their represantation
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
}
