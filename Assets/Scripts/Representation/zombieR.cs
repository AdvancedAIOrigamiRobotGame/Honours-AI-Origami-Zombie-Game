using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieR : MonoBehaviour
{

    public string Zombierepresentation;
    // Start is called before the first frame update
    void Start()
    {
        Zombierepresentation = scheme.createRepresentation(1);
    }
}
