using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Properties")]
    [SerializeField] int seed;

    [SerializeField, Range(10, 100)] private int width;
    [SerializeField, Range(10, 100)] private int height;

    [SerializeField, Range(0.01f, 1.0f)] private float noiseScale;
    [SerializeField, Range(1.0f, 20.0f)] private float heightMultiplier;

    [SerializeField, Range(1, 5)] private int octavesCount;
    [SerializeField] private float lacunarity; // frequency of the octaves
    [SerializeField] private float persistence;

    Color[] terrainColors;
    [SerializeField] Gradient terrainGradient;
    [SerializeField] private float minTerrainHeight;
    [SerializeField] private float maxTerrainHeight;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    [Header("Debugging")]
    [SerializeField] private bool showVertices;
    [SerializeField] private int skipInterval;


    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        GenerateTerrain();
    }

    void Update()
    {
        UpdateMesh();
    }

    private void InitMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void GenerateGrid()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        minTerrainHeight = float.MaxValue;
        maxTerrainHeight = float.MinValue;

        for (int z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = 0;

                for (int i = 0; i < octavesCount; i++)
                {
                    float frequency = Mathf.Pow(lacunarity, i);
                    float amplitude = Mathf.Pow(persistence, i);

                    y += Mathf.PerlinNoise((x + seed) * noiseScale * frequency, (z + seed) * noiseScale * frequency) * amplitude;
                }

                y *= heightMultiplier;

                vertices[z * (width + 1) + x] = new Vector3(x, y, z);

                if (y > maxTerrainHeight) maxTerrainHeight = y;
                if (y < minTerrainHeight) minTerrainHeight = y;

            }
        }
    }

    private IEnumerator GenerateTriangles()
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
        UpdateMaterial();
        UpdateMesh();
    }

    private void GenerateTrianglesImmediate()
    {
        triangles = new int[width * height * 6];
        int tris = 0;

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
            }
        }
        UpdateMaterial();
        UpdateMesh();
    }

    public void GenerateTerrain()
    {
        InitMesh();
        GenerateGrid();

        if (Application.isPlaying)
        {
            StartCoroutine(GenerateTriangles());
        }
        else
        {
            GenerateTrianglesImmediate();
        }

    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.colors = terrainColors;

        mesh.RecalculateNormals();
    }

    private void UpdateMaterial()
    {
        terrainColors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            float normalizedHeight = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
            terrainColors[i] = terrainGradient.Evaluate(normalizedHeight);
        }
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
