using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Properties")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float noiseScale;


    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    [Header("Debugging")]
    [SerializeField] private bool showVertices;
    [SerializeField] private int skipInterval;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTerrain();
       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    private void GenerateGrid(int width, int height)
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f * noiseScale, z * 0.3f * noiseScale) * 2f;
                vertices[z * (width + 1) + x] = new Vector3(x, y, z);
            }
        }
    }

    private IEnumerator GenerateTriangles(int width, int height)
    {
        triangles = new int[width * height * 6];
        int tris = 0;
        int skip = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = z * (width + 1) + x;

                triangles[tris] = i;
                triangles[tris + 1] = i + width + 1;
                triangles[tris + 2] = i + 1;

                triangles[tris + 3] = i + 1;
                triangles[tris + 4] = i + width + 1;
                triangles[tris + 5] = i + width + 2;

                tris += 6;
                skip += 6;
                if (skip >= skipInterval * 6)
                {
                    yield return new WaitForEndOfFrame();
                    skip = 0;
                }
            }
        }
    }

    private void GenerateTerrain()
    {
        GenerateGrid(width, height);
        StartCoroutine(GenerateTriangles(width, height));
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (!showVertices) return;
        if (vertices == null) return;

        Gizmos.color = Color.blue;
        foreach (var vert in vertices)
        {
            Gizmos.DrawSphere(transform.position + vert, 0.1f);
        }
    }
}
