using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGDTutorial
{
    public class PolygonGenerator : MonoBehaviour
    {
        public List<Vector3> newVertices = new List<Vector3>();
        public List<int> newTriangles = new List<int>();
        public List<Vector2> newUV = new List<Vector2>();

        public List<Vector3> colVertices = new List<Vector3>();
        public List<int> colTriangles = new List<int>();
        public int colCount;
        private MeshCollider _col;

        private Mesh _mesh;
        private float _tUnit = 0.25f;
        private Vector2 _tStone = new Vector2(0, 0);
        private Vector2 _tGrass = new Vector2(0, 1);
        private int squareCount;

        public byte[,] blocks;
        public bool update = false;

        void Start()
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _col = GetComponent<MeshCollider>();

            GenerateTerrain();
            BuildMesh();
            UpdateMesh();
        }

        void Update()
        {
            if (update)
            {
                BuildMesh();
                UpdateMesh();
                update = false;
            }
        }

        private void GenerateTerrain()
        {
            blocks = new byte[96, 128];

            for (int px = 0; px < blocks.GetLength(0); px++)
            {
                int stone = Noise(px, 0, 80, 15, 1);
                stone += Noise(px, 0, 50, 30, 1);
                stone += Noise(px, 0, 10, 10, 1);
                stone += 75;

                print(stone);

                int dirt = Noise(px, 0, 100, 35, 1);
                dirt += Noise(px, 0, 50, 30, 1);
                dirt += 75;

                for (int py = 0; py < blocks.GetLength(1); py++)
                {
                    if (py < stone)
                    {
                        blocks[px, py] = 1;

                        if (Noise(px, py, 12, 16, 1) > 10)
                        {
                            blocks[px, py] = 2;
                        }

                        if (Noise(px, py * 2, 16, 14, 1) > 10)
                        {
                            blocks[px, py] = 0;
                        }
                    }
                    else if (py < dirt)
                    {
                        blocks[px, py] = 2;
                    }
                }
            }
        }

        private int Noise(int x, int y, float scale, float magnitude, float exponent)
        {
            return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * magnitude), exponent));
        }

        private void GenerateSquare(int x, int y, Vector2 texture)
        {
            newVertices.Add(new Vector3(x, y, 0));
            newVertices.Add(new Vector3(x + 1, y, 0));
            newVertices.Add(new Vector3(x + 1, y - 1, 0));
            newVertices.Add(new Vector3(x, y - 1, 0));

            newTriangles.Add(squareCount * 4);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);

            newUV.Add(new Vector2(_tUnit * texture.x, _tUnit * texture.y + _tUnit));
            newUV.Add(new Vector2(_tUnit * texture.x + _tUnit, _tUnit * texture.y + _tUnit));
            newUV.Add(new Vector2(_tUnit * texture.x + _tUnit, _tUnit * texture.y));
            newUV.Add(new Vector2(_tUnit * texture.x, _tUnit * texture.y));

            squareCount++;
        }

        private void GenerateCollider(int x, int y)
        {
            // Top
            if (Block(x, y + 1) == 0)
            {
                colVertices.Add(new Vector3(x, y, 1));
                colVertices.Add(new Vector3(x + 1, y, 1));
                colVertices.Add(new Vector3(x + 1, y, 0));
                colVertices.Add(new Vector3(x, y, 0));

                ColliderTriangles();
                colCount++;
            }



            // Bot
            if (Block(x, y - 1) == 0)
            {
                colVertices.Add(new Vector3(x, y - 1, 0));
                colVertices.Add(new Vector3(x + 1, y - 1, 0));
                colVertices.Add(new Vector3(x + 1, y - 1, 1));
                colVertices.Add(new Vector3(x, y - 1, 1));

                ColliderTriangles();
                colCount++;
            }

            // Left
            if (Block(x - 1, y) == 0)
            {
                colVertices.Add(new Vector3(x, y - 1, 1));
                colVertices.Add(new Vector3(x, y, 1));
                colVertices.Add(new Vector3(x, y, 0));
                colVertices.Add(new Vector3(x, y - 1, 0));

                ColliderTriangles();
                colCount++;
            }

            // Right
            if (Block(x + 1, y) == 0)
            {
                colVertices.Add(new Vector3(x + 1, y, 1));
                colVertices.Add(new Vector3(x + 1, y - 1, 1));
                colVertices.Add(new Vector3(x + 1, y - 1, 0));
                colVertices.Add(new Vector3(x + 1, y, 0));

                ColliderTriangles();
                colCount++;
            }
        }

        private void ColliderTriangles()
        {
            colTriangles.Add(colCount * 4);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 3);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 2);
            colTriangles.Add((colCount * 4) + 3);
        }

        private void BuildMesh()
        {
            for (int px = 0; px < blocks.GetLength(0); px++)
            {
                for (int py = 0; py < blocks.GetLength(1); py++)
                {
                    if (blocks[px, py] != 0) // If block is not air
                    {
                        GenerateCollider(px, py);
                        if (blocks[px, py] == 1)
                        {
                            GenerateSquare(px, py, _tStone);
                        }
                        else if (blocks[px, py] == 2)
                        {
                            GenerateSquare(px, py, _tGrass);
                        }
                    }
                }
            }
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = newVertices.ToArray();
            _mesh.triangles = newTriangles.ToArray();
            _mesh.uv = newUV.ToArray();
            _mesh.RecalculateNormals();

            squareCount = 0;
            newVertices.Clear();
            newTriangles.Clear();
            newUV.Clear();

            Mesh newMesh = new Mesh();
            newMesh.vertices = colVertices.ToArray();
            newMesh.triangles = colTriangles.ToArray();
            _col.sharedMesh = newMesh;

            colVertices.Clear();
            colTriangles.Clear();
            colCount = 0;
        }

        private byte Block(int x, int y)
        {
            if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
            {
                return 1;
            }

            return blocks[x, y];
        }
    }
}