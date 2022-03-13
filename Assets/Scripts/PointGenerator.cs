using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointGenerator : MonoBehaviour
{
    [SerializeField] 
    private GameObject point;
    [SerializeField] 
    private int numberOfPoints;

    public List<GameObject> points = new List<GameObject>();
    private void Start()
    {   
        // prepare space for random points
        var mainCamera = Camera.main!;
        Vector3[] corners = new Vector3[4];
        mainCamera.CalculateFrustumCorners(new Rect(0.05f, 0.05f, 0.90f, 0.90f), mainCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, corners);
        var minx = corners[0].x;
        var miny = corners[0].y;
        var maxx = corners[2].x;
        var maxy = corners[2].y;

        // generate points
        for (int i = 0; i < numberOfPoints; i++)
        {
            var x = Random.Range(minx, maxx);
            var y = Random.Range(miny, maxy);
            points.Add(Instantiate(point, new Vector3(x, y, 0), Quaternion.identity));
        }

        // solve convex hull problem 
        var positions = points.Select(point => (Vector2)point.transform.position).ToArray();
        var indexes = Quickhull.Solve(positions);

        // draw mesh from convex hull points
        Mesh mesh = new Mesh();
        mesh.vertices = points.Select(p => p.transform.position).ToArray();
        var triangles = new int[(indexes.Length - 2) * 3];
        for (int i = 0; i < indexes.Length - 2; i++)
        {
            triangles[i * 3] = indexes[0];
            triangles[i * 3 + 1] = indexes[i + 1];
            triangles[i * 3 + 2] = indexes[i + 2];
        }
        mesh.triangles = triangles;
        var colors = new Color[positions.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            var r = (positions[i].x - minx) / maxx;
            var g = (positions[i].y - miny) / maxy;
            var b = Mathf.Max(r, g) - Mathf.Abs(r - g);
            r -= b / 2;
            g -= g / 2;
            colors[i] = new Color(r, g, b);
        }

        mesh.colors = colors;
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
    }


}
