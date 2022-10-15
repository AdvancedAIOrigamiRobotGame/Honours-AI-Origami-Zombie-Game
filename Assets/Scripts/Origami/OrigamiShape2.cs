using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class OrigamiShape2 : MonoBehaviour
{
    Mesh mymesh;
    MeshFilter meshFilter;

    [SerializeField] Vector2 planeSize = new Vector2(2, 2);
    [SerializeField] int planeResolution = 3;

    List<Vector3> vertices;
    List<int> triangles;
    // Start is called before the first frame update
    void Awake()
    {
        mymesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mymesh;
    }

    // Update is called once per frame
    void Update()
    {
        planeResolution = Mathf.Clamp(planeResolution, 3, 50);

        GeneratePlane(planeSize, planeResolution);
        LeftToRightSine(Time.timeSinceLevelLoad);
        RippleSine(Time.timeSinceLevelLoad);
        AssignMesh();
    }

    void GeneratePlane(Vector2 size, int resolution)
    {
        // create vertices
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        //create triangles
        triangles = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = (row * resolution) + row + column;

                //first triangle
                triangles.Add(i);
                triangles.Add(i + (resolution) + 1);
                triangles.Add(i + (resolution) + 2);

                //second triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }

    }

    void AssignMesh()
    {
        mymesh.Clear();
        mymesh.vertices = vertices.ToArray();
        mymesh.triangles = triangles.ToArray();
    }

    void LeftToRightSine(float time)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.Sin(time + vertex.x);
            vertices[i] = vertex;
        }
    }

    void RippleSine(float time)
    {
        Vector3 origin = new Vector3(planeSize.x / 2, 0, planeSize.y / 2);
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            float distancefromcenter = (vertex - origin).magnitude;
            vertex.y = Mathf.Sin(time + distancefromcenter);
            vertices[i] = vertex;
        }
    }
}
