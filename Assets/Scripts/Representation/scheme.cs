using Fare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scheme : MonoBehaviour
{
    public string Origamirepresentation;
    // Start is called before the first frame update

    public static string AgentRepresentationPattern = "ORI.{5}CE";
    public static string ZombieRepresentationPattern = ".{10}";



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
}
