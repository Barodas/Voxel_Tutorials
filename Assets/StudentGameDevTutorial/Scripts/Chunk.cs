using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGDTutorial
{
    public class Chunk : MonoBehaviour
    {
        private List<Vector3> _newVerts = new List<Vector3>();
        private List<int> _newTris = new List<int>();
        private List<Vector2> _newUV = new List<Vector2>();

        private float tUnit = 0.25f;
        private Vector2 tStone = new Vector2(1, 0);
        private Vector2 tGrass = new Vector2(0, 1);
        private Vector2 tGrassTop = new Vector2(1, 1);

        private Mesh _mesh;
        private MeshCollider _col;

        private int _faceCount;

        public GameObject worldGO;
        private World world;

        public int chunkSize = 16;

        public int chunkX;
        public int chunkY;
        public int chunkZ;

        public bool update;

        void Start()
        {
            world = worldGO.GetComponent<World>();

            _mesh = GetComponent<MeshFilter>().mesh;
            _col = GetComponent<MeshCollider>();

            GenerateMesh();
        }

        void LateUpdate()
        {
            if (update)
            {
                GenerateMesh();
                update = false;
            }
        }

        private byte Block(int x, int y, int z)
        {
            return world.Block(x + chunkX, y + chunkY, z + chunkZ);
        }

        public void GenerateMesh()
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        //This code will run for every block in the chunk
                        if (Block(x, y, z) != 0)
                        {
                            //If the block is solid
                            if (Block(x, y + 1, z) == 0)
                            {
                                //Block above is air
                                CubeTop(x, y, z, Block(x, y, z));
                            }

                            if (Block(x, y - 1, z) == 0)
                            {
                                //Block below is air
                                CubeBot(x, y, z, Block(x, y, z));

                            }

                            if (Block(x + 1, y, z) == 0)
                            {
                                //Block east is air
                                CubeEast(x, y, z, Block(x, y, z));

                            }

                            if (Block(x - 1, y, z) == 0)
                            {
                                //Block west is air
                                CubeWest(x, y, z, Block(x, y, z));

                            }

                            if (Block(x, y, z + 1) == 0)
                            {
                                //Block north is air
                                CubeNorth(x, y, z, Block(x, y, z));

                            }

                            if (Block(x, y, z - 1) == 0)
                            {
                                //Block south is air
                                CubeSouth(x, y, z, Block(x, y, z));
                            }
                        }
                    }
                }
            }

            UpdateMesh();
        }


        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _newVerts.ToArray();
            _mesh.uv = _newUV.ToArray();
            _mesh.triangles = _newTris.ToArray();
            _mesh.RecalculateNormals();

            _newVerts.Clear();
            _newUV.Clear();
            _newTris.Clear();

            _col.sharedMesh = null;
            _col.sharedMesh = _mesh;

            _faceCount = 0;
        }

        private void Cube(Vector2 texturePos)
        {
            _newTris.Add(_faceCount * 4); //1
            _newTris.Add(_faceCount * 4 + 1); //2
            _newTris.Add(_faceCount * 4 + 2); //3
            _newTris.Add(_faceCount * 4); //1
            _newTris.Add(_faceCount * 4 + 2); //3
            _newTris.Add(_faceCount * 4 + 3); //4

            _newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
            _newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
            _newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
            _newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

            _faceCount++;
        }

        private void CubeTop(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x, y, z + 1));
            _newVerts.Add(new Vector3(x + 1, y, z + 1));
            _newVerts.Add(new Vector3(x + 1, y, z));
            _newVerts.Add(new Vector3(x, y, z));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrassTop;
            }
            Cube(texturePos);
        }

        private void CubeNorth(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));
            _newVerts.Add(new Vector3(x + 1, y, z + 1));
            _newVerts.Add(new Vector3(x, y, z + 1));
            _newVerts.Add(new Vector3(x, y - 1, z + 1));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrass;
            }
            Cube(texturePos);
        }

        private void CubeEast(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x + 1, y - 1, z));
            _newVerts.Add(new Vector3(x + 1, y, z));
            _newVerts.Add(new Vector3(x + 1, y, z + 1));
            _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrass;
            }
            Cube(texturePos);
        }

        private void CubeSouth(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x, y - 1, z));
            _newVerts.Add(new Vector3(x, y, z));
            _newVerts.Add(new Vector3(x + 1, y, z));
            _newVerts.Add(new Vector3(x + 1, y - 1, z));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrass;
            }
            Cube(texturePos);
        }

        private void CubeWest(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x, y - 1, z + 1));
            _newVerts.Add(new Vector3(x, y, z + 1));
            _newVerts.Add(new Vector3(x, y, z));
            _newVerts.Add(new Vector3(x, y - 1, z));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrass;
            }
            Cube(texturePos);
        }

        private void CubeBot(int x, int y, int z, byte block)
        {
            _newVerts.Add(new Vector3(x, y - 1, z));
            _newVerts.Add(new Vector3(x + 1, y - 1, z));
            _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));
            _newVerts.Add(new Vector3(x, y - 1, z + 1));

            Vector2 texturePos = new Vector2(0, 0);
            if (Block(x, y, z) == 1)
            {
                texturePos = tStone;
            }
            else if (Block(x, y, z) == 2)
            {
                texturePos = tGrass;
            }
            Cube(texturePos);
        }
    }
}