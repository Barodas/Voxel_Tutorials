using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASTutorial
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        private Block[,,] _blocks;
        private MeshFilter _filter;
        private MeshCollider _col;

        public static int chunkSize = 16;
        public bool update = true;

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _col = GetComponent<MeshCollider>();

            // Example Chunk Code
            _blocks = new Block[chunkSize, chunkSize, chunkSize];
            for(int x = 0; x < chunkSize; x++)
            {
                for(int y = 0; y < chunkSize; y++)
                {
                    for(int z = 0; z < chunkSize; z++)
                    {
                        _blocks[x, y, z] = new BlockAir();
                    }
                }
            }
            _blocks[3, 5, 2] = new Block();
            UpdateChunk();
            // End Example Chunk Code
        }

        public Block GetBlock(int x, int y, int z)
        {
            return _blocks[x, y, z];
        }

        private void UpdateChunk()
        {
            MeshData meshData = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        meshData = _blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        private void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.vertices.ToArray();
            _filter.mesh.triangles = meshData.triangles.ToArray();
        }
    }
}