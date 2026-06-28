using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class JumperWireRenderer : MonoBehaviour
{
    [System.Serializable]
    public class WireConnection
    {
        public string label;
        public Transform startPoint;
        public Transform endPoint;
        public Color wireColor = Color.red;
        [Range(0.01f, 1.0f)]
        public float droop = 0.15f; // How much the wire hangs down
        public int segments = 15; // Smoothness of the line
        [HideInInspector]
        public LineRenderer lineRenderer;
    }

    public List<WireConnection> connections = new List<WireConnection>();
    public Material wireMaterial;
    public float wireWidth = 0.015f;

    private void Start()
    {
        GenerateWires();
    }

    private void Update()
    {
        // For development convenience: update lines in real-time if they are moving (e.g. in Editor or Play Mode)
        UpdateWires();
    }

    public void GenerateWires()
    {
        // Clear any existing child line renderers
        List<GameObject> childrenToDestroy = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Wire_"))
            {
                childrenToDestroy.Add(child.gameObject);
            }
        }
        foreach (var child in childrenToDestroy)
        {
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        if (wireMaterial == null)
        {
            // Fallback material
            wireMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        }

        foreach (var conn in connections)
        {
            if (conn.startPoint == null || conn.endPoint == null) continue;

            GameObject wireObj = new GameObject("Wire_" + (string.IsNullOrEmpty(conn.label) ? "Conn" : conn.label));
            wireObj.transform.SetParent(transform, false);

            LineRenderer lr = wireObj.AddComponent<LineRenderer>();
            lr.sharedMaterial = wireMaterial;
            lr.startWidth = wireWidth;
            lr.endWidth = wireWidth;
            lr.positionCount = conn.segments;
            lr.startColor = conn.wireColor;
            lr.endColor = conn.wireColor;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;

            conn.lineRenderer = lr;
        }

        UpdateWires();
    }

    public void UpdateWires()
    {
        foreach (var conn in connections)
        {
            if (conn.startPoint == null || conn.endPoint == null || conn.lineRenderer == null) continue;

            Vector3 start = conn.startPoint.position;
            Vector3 end = conn.endPoint.position;
            Vector3[] positions = new Vector3[conn.segments];

            for (int i = 0; i < conn.segments; i++)
            {
                float t = i / (float)(conn.segments - 1);
                
                // Linear interpolation
                Vector3 pos = Vector3.Lerp(start, end, t);

                // Add a gravity-style sag/droop
                // Parabolic sag equation: h = 4 * droop * t * (1 - t)
                // We sag downwards in world space
                float sag = 4f * conn.droop * t * (1f - t);
                pos.y -= sag;

                positions[i] = pos;
            }

            conn.lineRenderer.SetPositions(positions);
        }
    }

    [ContextMenu("Rebuild Wires")]
    private void RebuildMenu()
    {
        GenerateWires();
    }
}