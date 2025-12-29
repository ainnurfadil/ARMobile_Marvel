using UnityEngine;
using System.Collections.Generic;

public class BlockInteraction : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightDuration = 0.5f;
    
    [Header("Animation")]
    public float rotationSpeed = 50f;
    public bool autoRotate = false;
    
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private Renderer[] renderers;
    private bool isHighlighted = false;
    private float highlightTimer = 0f;
    
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        
        foreach (Renderer rend in renderers)
        {
            originalColors[rend] = rend.material.color;
        }
        
        // Add colliders if not present
        if (GetComponent<Collider>() == null)
        {
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }
    }
    
    void Update()
    {
        if (autoRotate)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        
        if (isHighlighted)
        {
            highlightTimer -= Time.deltaTime;
            if (highlightTimer <= 0)
            {
                RemoveHighlight();
            }
        }
    }
    
    public void OnTap()
    {
        Highlight();
        ShowInfo();
    }
    
    void Highlight()
    {
        isHighlighted = true;
        highlightTimer = highlightDuration;
        
        foreach (Renderer rend in renderers)
        {
            rend.material.color = highlightColor;
        }
    }
    
    void RemoveHighlight()
    {
        isHighlighted = false;
        
        foreach (Renderer rend in renderers)
        {
            if (originalColors.ContainsKey(rend))
            {
                rend.material.color = originalColors[rend];
            }
        }
    }
    
    void ShowInfo()
    {
        // Show information panel about the block
        // This could be connected to a UI manager
        Debug.Log($"Block tapped: {gameObject.name}");
    }
    
    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
    
    public void ShowDiagonals()
    {
        // Create visual lines showing diagonals
        // This would be implemented based on geometry requirements
    }
    
    public void ShowVertices()
    {
        // Highlight vertices of the block
    }
    
    public void ShowEdges()
    {
        // Highlight edges of the block
    }
}