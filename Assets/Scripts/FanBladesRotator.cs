using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanBladesRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 600f; // Rotation speed in degrees per second

    private Transform bladesChild;

    void Start()
    {
        SplitFanMesh();
    }

    void Update()
    {
        // Only rotate the detached blades child
        if (bladesChild != null)
        {
            bladesChild.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void SplitFanMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mf == null || mf.sharedMesh == null) return;

        Mesh originalMesh = mf.sharedMesh;
        Vector3[] vertices = originalMesh.vertices;
        int[] triangles = originalMesh.triangles;
        Vector3[] normals = originalMesh.normals;
        Vector2[] uvs = originalMesh.uv;

        // 1. DSU to find connected geometric islands (discrete sub-meshes)
        int vertexCount = vertices.Length;
        int[] parent = new int[vertexCount];
        for (int i = 0; i < vertexCount; i++) parent[i] = i;

        int Find(int i)
        {
            int r = i;
            while (parent[r] != r) r = parent[r];
            int curr = i;
            while (parent[curr] != r)
            {
                int next = parent[curr];
                parent[curr] = r;
                curr = next;
            }
            return r;
        }

        void Union(int i, int j)
        {
            int rootI = Find(i);
            int rootJ = Find(j);
            if (rootI != rootJ) parent[rootI] = rootJ;
        }

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Union(triangles[i], triangles[i + 1]);
            Union(triangles[i + 1], triangles[i + 2]);
        }

        // Group vertices by their island root
        Dictionary<int, List<int>> groups = new Dictionary<int, List<int>>();
        for (int i = 0; i < vertexCount; i++)
        {
            int root = Find(i);
            if (!groups.ContainsKey(root)) groups[root] = new List<int>();
            groups[root].Add(i);
        }

        // Classify rotating vs static vertices
        HashSet<int> rotatingVertices = new HashSet<int>();
        float threshold2D = 0.033f; // Threshold of 3.3 cm radius from central Z-axis

        foreach (var pair in groups)
        {
            List<int> groupVerts = pair.Value;
            if (groupVerts.Count < 10) continue;

            float maxDist2D = 0f;
            foreach (int vIdx in groupVerts)
            {
                Vector3 v = vertices[vIdx];
                float dist2D = Mathf.Sqrt(v.x * v.x + v.y * v.y);
                if (dist2D > maxDist2D) maxDist2D = dist2D;
            }

            // Islands with max vertex distance < 3.3cm are the center axle and blades
            if (maxDist2D < threshold2D)
            {
                foreach (int vIdx in groupVerts) rotatingVertices.Add(vIdx);
            }
        }

        // 2. Separate triangles into static and rotating lists
        List<int> staticTriangles = new List<int>();
        List<int> rotatingTriangles = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int v0 = triangles[i];
            int v1 = triangles[i + 1];
            int v2 = triangles[i + 2];

            if (rotatingVertices.Contains(v0) || rotatingVertices.Contains(v1) || rotatingVertices.Contains(v2))
            {
                rotatingTriangles.Add(v0);
                rotatingTriangles.Add(v1);
                rotatingTriangles.Add(v2);
            }
            else
            {
                staticTriangles.Add(v0);
                staticTriangles.Add(v1);
                staticTriangles.Add(v2);
            }
        }

        // 3. Create compact meshes
        Mesh staticMesh = CreateCompactMesh(originalMesh, staticTriangles);
        Mesh bladesMesh = CreateCompactMesh(originalMesh, rotatingTriangles);

        // Assign the static frame mesh to this object
        mf.sharedMesh = staticMesh;

        // 4. Instantiate a child object for the rotating blades
        GameObject bladesObj = new GameObject("Blades_Instance");
        bladesObj.transform.SetParent(transform, false);
        bladesObj.transform.localPosition = Vector3.zero;
        bladesObj.transform.localRotation = Quaternion.identity;
        bladesObj.transform.localScale = Vector3.one;

        MeshFilter childMf = bladesObj.AddComponent<MeshFilter>();
        childMf.sharedMesh = bladesMesh;

        MeshRenderer childMr = bladesObj.AddComponent<MeshRenderer>();
        childMr.sharedMaterial = mr.sharedMaterial;

        bladesChild = bladesObj.transform;
    }

    private Mesh CreateCompactMesh(Mesh src, List<int> oldTriangles)
    {
        Vector3[] srcVerts = src.vertices;
        Vector3[] srcNormals = src.normals;
        Vector2[] srcUVs = src.uv;

        Dictionary<int, int> oldToNewIndex = new Dictionary<int, int>();
        List<Vector3> newVertices = new List<Vector3>();
        List<Vector3> newNormals = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        List<int> newTriangles = new List<int>();

        foreach (int oldIndex in oldTriangles)
        {
            if (!oldToNewIndex.TryGetValue(oldIndex, out int newIndex))
            {
                newIndex = newVertices.Count;
                oldToNewIndex[oldIndex] = newIndex;
                newVertices.Add(srcVerts[oldIndex]);
                if (srcNormals != null && srcNormals.Length > oldIndex) newNormals.Add(srcNormals[oldIndex]);
                if (srcUVs != null && srcUVs.Length > oldIndex) newUVs.Add(srcUVs[oldIndex]);
            }
            newTriangles.Add(newIndex);
        }

        Mesh mesh = new Mesh();
        mesh.name = src.name + "_Split";
        mesh.vertices = newVertices.ToArray();
        if (newNormals.Count > 0) mesh.normals = newNormals.ToArray();
        if (newUVs.Count > 0) mesh.uv = newUVs.ToArray();
        mesh.triangles = newTriangles.ToArray();
        return mesh;
    }
}
