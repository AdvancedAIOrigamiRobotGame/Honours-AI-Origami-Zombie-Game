using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class origamiR : MonoBehaviour
{
    public string Origamirepresentation;

    // Start is called before the first frame update
    void Start()
    {
        Origamirepresentation = scheme.createRepresentation(0);
        NSA.selfSpace.Add(Origamirepresentation);
        NSA.nOrigamis++;
    }

}
