using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASTutorial
{
    public class TerrainGen
    {
        private float stoneBaseHeight = -24;
        private float stoneBaseNoise = 0.05f;
        private float stoneBaseNoiseHeight = 4;
        private float stoneMountainHeight = 48;
        private float stoneMountainFrequency = 0.008f;
        private float stoneMinHeight = -12;
        private float dirtBaseHeight = 1;
        private float dirtNoise = 0.04f;
        private float dirtNoiseHeight = 3;

        public Chunk ChunkGen(Chunk chunk)
        {
            for(int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++)
            {
                for(int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)
                {
                    chunk = ChunkColumnGen(chunk, x, z);
                }
            }
            return chunk;
        }

        public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
            if(stoneHeight < stoneMinHeight)
            {
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);
            }
            stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
            for(int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
            {
                if(y <= stoneHeight)
                {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new Block());
                }
                else if(y <= dirtHeight)
                {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockGrass());
                }
                else
                {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
                }
            }
            return chunk;
        }

        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((SimplexNoise.Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}