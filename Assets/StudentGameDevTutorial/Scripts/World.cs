using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGDTutorial
{
    public class World : MonoBehaviour
    {
        public byte[,,] data;
        public int worldX = 16;
        public int worldY = 16;
        public int WorldZ = 16;

        public GameObject chunk;
        public Chunk[,,] chunks;
        public int chunkSize = 16;

        void Start()
        {
            // Generate block data
            data = new byte[worldX, worldY, WorldZ];
            for (int x = 0; x < worldX; x++)
            {
                for (int z = 0; z < WorldZ; z++)
                {
                    int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                    stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                    int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1;
                    for (int y = 0; y < worldY; y++)
                    {
                        if (y <= stone)
                        {
                            data[x, y, z] = 1;
                            if (PerlinNoise(x, y, z, 12, 16, 1) > 10)
                            {
                                data[x, y, z] = 2;
                            }

                            if (PerlinNoise(x, y, z, 16, 14, 1) > 10)
                            {
                                data[x, y, z] = 0;
                            }
                        }
                        else if (y <= dirt + stone)
                        {
                            data[x, y, z] = 2;
                        }
                    }
                }
            }

            // Generate chunks
            chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(WorldZ / chunkSize)];

        }

        void Update()
        {

        }

        public byte Block(int x, int y, int z)
        {
            if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= WorldZ || z < 0)
            {
                return 0; // Render faces outside array? (0 = yes, 1 = no)
            }

            return data[x, y, z];
        }

        private int PerlinNoise(int x, int y, int z, float scale, float height, float power)
        {
            float rValue;
            rValue = Noise.Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
            rValue *= height;

            if (power != 0)
            {
                rValue = Mathf.Pow(rValue, power);
            }
            return (int)rValue;
        }

        public void GenerateColumn(int x, int z)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
                chunks[x, y, z] = newChunk.GetComponent<Chunk>();

                chunks[x, y, z].worldGO = gameObject;
                chunks[x, y, z].chunkSize = chunkSize;
                chunks[x, y, z].chunkX = x * chunkSize;
                chunks[x, y, z].chunkY = y * chunkSize;
                chunks[x, y, z].chunkZ = z * chunkSize;
            }
        }

        public void UnloadColumn(int x, int z)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                Destroy(chunks[x, y, z].gameObject);
            }
        }
    }
}