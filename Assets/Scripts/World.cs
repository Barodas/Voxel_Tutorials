﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public byte[,,] data;
    public int worldX = 16;
    public int worldY = 16;
    public int WorldZ = 16;

    public GameObject chunk;
    public GameObject[,,] chunks;
    public int chunkSize = 16;

	void Start ()
    {
        // Generate block data
        data = new byte[worldX, worldY, WorldZ];
        for(int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < WorldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1;
                for (int y = 0; y < worldY; y++)
                {
                    if(y <= stone)
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
                    else if(y <= dirt+stone)
                    {
                        data[x, y, z] = 2;
                    }
                }
            }
        }

        // Generate chunks
        chunks = new GameObject[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(WorldZ / chunkSize)];
        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                for (int z = 0; z < chunks.GetLength(2); z++)
                {
                    chunks[x, y, z] = Instantiate(chunk, new Vector3(x * chunkSize, y * chunkSize, z * chunkSize), new Quaternion(0, 0, 0, 0)) as GameObject;
                    Chunk newChunkScript = chunks[x, y, z].GetComponent<Chunk>();

                    newChunkScript.worldGO = gameObject;
                    newChunkScript.chunkSize = chunkSize;
                    newChunkScript.chunkX = x * chunkSize;
                    newChunkScript.chunkY = y * chunkSize;
                    newChunkScript.chunkZ = z * chunkSize;
                }
            }
        }
    }

	void Update ()
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

        if(power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }
        return (int)rValue;
    }
}