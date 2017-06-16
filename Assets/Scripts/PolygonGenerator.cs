using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    public List<Vector3> newVertices = new List<Vector3>();
    public List<int> newTriangles = new List<int>();
    public List<Vector2> newUV = new List<Vector2>();

    private Mesh _mesh;
    private float _tUnit = 0.25f;
    private Vector2 _tStone = new Vector2(0, 0);
    private Vector2 _tGrass = new Vector2(0, 1);

	void Start ()
    {
        _mesh = GetComponent<MeshFilter>().mesh;

        Vector3 pos = transform.position;
        newVertices.Add(new Vector3(pos.x, pos.y, pos.z));
        newVertices.Add(new Vector3(pos.x + 1, pos.y, pos.z));
        newVertices.Add(new Vector3(pos.x + 1, pos.y - 1, pos.z));
        newVertices.Add(new Vector3(pos.x, pos.y - 1, pos.z));

        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);

        newUV.Add(new Vector2(_tUnit * _tStone.x, _tUnit * _tStone.y + _tUnit));
        newUV.Add(new Vector2(_tUnit * _tStone.x + _tUnit, _tUnit * _tStone.y + _tUnit));
        newUV.Add(new Vector2(_tUnit * _tStone.x + _tUnit, _tUnit * _tStone.y));
        newUV.Add(new Vector2(_tUnit * _tStone.x, _tUnit * _tStone.y));

        _mesh.Clear();
        _mesh.vertices = newVertices.ToArray();
        _mesh.triangles = newTriangles.ToArray();
        _mesh.uv = newUV.ToArray();
        _mesh.RecalculateNormals();
    }
	
	void Update ()
    {
		
	}
}
