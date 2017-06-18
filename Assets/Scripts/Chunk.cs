using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private List<Vector3> _newVerts = new List<Vector3>();
    private List<int> _newTris = new List<int>();
    private List<Vector2> _newUV = new List<Vector2>();

    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(1, 0);
    private Vector2 tGrass = new Vector2(0, 1);

    private Mesh _mesh;
    private MeshCollider _col;

    private int _faceCount;

	void Start ()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _col = GetComponent<MeshCollider>();

        CubeTop(0, 0, 0, 0);
        CubeNorth(0, 0, 0, 0);
        CubeEast(0, 0, 0, 0);
        CubeSouth(0, 0, 0, 0);
        CubeWest(0, 0, 0, 0);
        CubeBot(0, 0, 0, 0);
        UpdateMesh();
	}

	void Update ()
    {
		
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

        _faceCount = 0;
    }

    void Cube(Vector2 texturePos)
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

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }

    private void CubeNorth(int x, int y, int z, byte block)
    {
        _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));
        _newVerts.Add(new Vector3(x + 1, y, z + 1));
        _newVerts.Add(new Vector3(x, y, z + 1));
        _newVerts.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }

    private void CubeEast(int x, int y, int z, byte block)
    {
        _newVerts.Add(new Vector3(x + 1, y - 1, z));
        _newVerts.Add(new Vector3(x + 1, y, z));
        _newVerts.Add(new Vector3(x + 1, y, z + 1));
        _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }

    private void CubeSouth(int x, int y, int z, byte block)
    {
        _newVerts.Add(new Vector3(x, y - 1, z));
        _newVerts.Add(new Vector3(x, y, z));
        _newVerts.Add(new Vector3(x + 1, y, z));
        _newVerts.Add(new Vector3(x + 1, y - 1, z));

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }

    private void CubeWest(int x, int y, int z, byte block)
    {
        _newVerts.Add(new Vector3(x, y - 1, z + 1));
        _newVerts.Add(new Vector3(x, y, z + 1));
        _newVerts.Add(new Vector3(x, y, z));
        _newVerts.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }

    private void CubeBot(int x, int y, int z, byte block)
    {
        _newVerts.Add(new Vector3(x, y - 1, z));
        _newVerts.Add(new Vector3(x + 1, y - 1, z));
        _newVerts.Add(new Vector3(x + 1, y - 1, z + 1));
        _newVerts.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos;
        texturePos = tStone;
        Cube(texturePos);
    }
}
