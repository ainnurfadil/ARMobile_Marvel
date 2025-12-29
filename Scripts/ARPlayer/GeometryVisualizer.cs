using UnityEngine;
using System.Collections.Generic;

public class GeometryVisualizer : MonoBehaviour
{
    [Header("Visualization Settings")]
    public Material lineMaterial;
    public Color diagonalColor = Color.yellow;
    public Color edgeColor = Color.white;
    public Color vertexColor = Color.red;
    public float lineWidth = 0.02f;
    public float vertexSize = 0.05f;
    
    [Header("References")]
    public GameObject blockObject;
    
    private List<GameObject> diagonalLines = new List<GameObject>();
    private List<GameObject> edgeLines = new List<GameObject>();
    private List<GameObject> vertices = new List<GameObject>();
    private Vector3[] blockVertices;
    
    void Start()
    {
        if (blockObject == null)
            blockObject = gameObject;
        
        CalculateBlockVertices();
    }
    
    void CalculateBlockVertices()
    {
        // Get the bounds of the block
        Renderer rend = blockObject.GetComponent<Renderer>();
        if (rend != null)
        {
            Bounds bounds = rend.bounds;
            Vector3 center = blockObject.transform.position;
            Vector3 size = bounds.size;
            
            // Calculate 8 vertices of the rectangular block (balok)
            // Following the naming convention: A, B, C, D (bottom), E, F, G, H (top)
            blockVertices = new Vector3[8];
            
            // Bottom vertices
            blockVertices[0] = center + new Vector3(-size.x/2, -size.y/2, -size.z/2); // A
            blockVertices[1] = center + new Vector3(size.x/2, -size.y/2, -size.z/2);  // B
            blockVertices[2] = center + new Vector3(size.x/2, -size.y/2, size.z/2);   // C
            blockVertices[3] = center + new Vector3(-size.x/2, -size.y/2, size.z/2);  // D
            
            // Top vertices
            blockVertices[4] = center + new Vector3(-size.x/2, size.y/2, -size.z/2);  // E
            blockVertices[5] = center + new Vector3(size.x/2, size.y/2, -size.z/2);   // F
            blockVertices[6] = center + new Vector3(size.x/2, size.y/2, size.z/2);    // G
            blockVertices[7] = center + new Vector3(-size.x/2, size.y/2, size.z/2);   // H
        }
    }
    
    public void ShowSpaceDiagonals()
    {
        ClearDiagonals();
        
        if (blockVertices == null || blockVertices.Length != 8)
        {
            CalculateBlockVertices();
        }
        
        // Space diagonals (diagonal ruang): 4 diagonals connecting opposite vertices
        // AG, BH, CE, DF
        CreateLine(blockVertices[0], blockVertices[6], diagonalColor, ref diagonalLines); // A to G
        CreateLine(blockVertices[1], blockVertices[7], diagonalColor, ref diagonalLines); // B to H
        CreateLine(blockVertices[2], blockVertices[4], diagonalColor, ref diagonalLines); // C to E
        CreateLine(blockVertices[3], blockVertices[5], diagonalColor, ref diagonalLines); // D to F
        
        // Highlight the vertices
        ShowVertices();
    }
    
    public void ShowFaceDiagonals()
    {
        ClearDiagonals();
        
        if (blockVertices == null || blockVertices.Length != 8)
        {
            CalculateBlockVertices();
        }
        
        // Face diagonals (diagonal sisi/bidang): diagonals on each face
        // Bottom face: AC, BD
        CreateLine(blockVertices[0], blockVertices[2], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[1], blockVertices[3], diagonalColor, ref diagonalLines);
        
        // Top face: EG, FH
        CreateLine(blockVertices[4], blockVertices[6], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[5], blockVertices[7], diagonalColor, ref diagonalLines);
        
        // Front face: AE, BF
        CreateLine(blockVertices[0], blockVertices[4], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[1], blockVertices[5], diagonalColor, ref diagonalLines);
        
        // Back face: CG, DH
        CreateLine(blockVertices[2], blockVertices[6], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[3], blockVertices[7], diagonalColor, ref diagonalLines);
        
        // Left face: AD, EH
        CreateLine(blockVertices[0], blockVertices[3], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[4], blockVertices[7], diagonalColor, ref diagonalLines);
        
        // Right face: BC, FG
        CreateLine(blockVertices[1], blockVertices[2], diagonalColor, ref diagonalLines);
        CreateLine(blockVertices[5], blockVertices[6], diagonalColor, ref diagonalLines);
    }
    
    public void ShowEdges()
    {
        ClearEdges();
        
        if (blockVertices == null || blockVertices.Length != 8)
        {
            CalculateBlockVertices();
        }
        
        // Bottom edges
        CreateLine(blockVertices[0], blockVertices[1], edgeColor, ref edgeLines); // AB
        CreateLine(blockVertices[1], blockVertices[2], edgeColor, ref edgeLines); // BC
        CreateLine(blockVertices[2], blockVertices[3], edgeColor, ref edgeLines); // CD
        CreateLine(blockVertices[3], blockVertices[0], edgeColor, ref edgeLines); // DA
        
        // Top edges
        CreateLine(blockVertices[4], blockVertices[5], edgeColor, ref edgeLines); // EF
        CreateLine(blockVertices[5], blockVertices[6], edgeColor, ref edgeLines); // FG
        CreateLine(blockVertices[6], blockVertices[7], edgeColor, ref edgeLines); // GH
        CreateLine(blockVertices[7], blockVertices[4], edgeColor, ref edgeLines); // HE
        
        // Vertical edges
        CreateLine(blockVertices[0], blockVertices[4], edgeColor, ref edgeLines); // AE
        CreateLine(blockVertices[1], blockVertices[5], edgeColor, ref edgeLines); // BF
        CreateLine(blockVertices[2], blockVertices[6], edgeColor, ref edgeLines); // CG
        CreateLine(blockVertices[3], blockVertices[7], edgeColor, ref edgeLines); // DH
    }
    
    public void ShowVertices()
    {
        ClearVertices();
        
        if (blockVertices == null || blockVertices.Length != 8)
        {
            CalculateBlockVertices();
        }
        
        // Create spheres at each vertex
        for (int i = 0; i < blockVertices.Length; i++)
        {
            GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            vertex.transform.position = blockVertices[i];
            vertex.transform.localScale = Vector3.one * vertexSize;
            vertex.transform.SetParent(transform);
            
            Renderer rend = vertex.GetComponent<Renderer>();
            rend.material.color = vertexColor;
            
            vertices.Add(vertex);
        }
    }
    
    void CreateLine(Vector3 start, Vector3 end, Color color, ref List<GameObject> lineList)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(transform);
        
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.useWorldSpace = true;
        
        lineList.Add(lineObj);
    }
    
    public void ClearDiagonals()
    {
        foreach (GameObject line in diagonalLines)
        {
            if (line != null)
                Destroy(line);
        }
        diagonalLines.Clear();
    }
    
    public void ClearEdges()
    {
        foreach (GameObject line in edgeLines)
        {
            if (line != null)
                Destroy(line);
        }
        edgeLines.Clear();
    }
    
    public void ClearVertices()
    {
        foreach (GameObject vertex in vertices)
        {
            if (vertex != null)
                Destroy(vertex);
        }
        vertices.Clear();
    }
    
    public void ClearAll()
    {
        ClearDiagonals();
        ClearEdges();
        ClearVertices();
    }
    
    void OnDestroy()
    {
        ClearAll();
    }
}