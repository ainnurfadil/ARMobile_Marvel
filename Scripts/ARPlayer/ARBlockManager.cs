using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class ARBlockManager : MonoBehaviour
{
    [Header("AR References")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    
    [Header("Block Prefabs")]
    public GameObject basicBlockPrefab;
    public GameObject localBlockPrefab;
    
    [Header("UI")]
    public GameObject placementIndicator;
    public GameObject instructionUI;
    
    private GameObject currentBlock;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Pose placementPose;
    private bool placementPoseValid = false;
    private bool blockPlaced = false;
    
    void Start()
    {
        if (raycastManager == null)
            raycastManager = GetComponent<ARRaycastManager>();
    }
    
    void Update()
    {
        if (!blockPlaced)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
            
            if (placementPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceBlock();
            }
        }
        else
        {
            HandleBlockInteraction();
        }
    }
    
    void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            placementPose = hits[0].pose;
            
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            
            placementPoseValid = true;
        }
        else
        {
            placementPoseValid = false;
        }
    }
    
    void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            if (placementPoseValid && !blockPlaced)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
    }
    
    void PlaceBlock()
    {
        // Choose block type based on selection
        GameObject blockPrefab = basicBlockPrefab; // Default
        
        currentBlock = Instantiate(blockPrefab, placementPose.position, placementPose.rotation);
        currentBlock.AddComponent<BlockInteraction>();
        
        blockPlaced = true;
        
        if (instructionUI != null)
            instructionUI.SetActive(false);
        
        // Disable plane detection after placement
        if (planeManager != null)
            planeManager.enabled = false;
    }
    
    void HandleBlockInteraction()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == currentBlock || hit.transform.IsChildOf(currentBlock.transform))
                    {
                        BlockInteraction interaction = currentBlock.GetComponent<BlockInteraction>();
                        if (interaction != null)
                        {
                            interaction.OnTap();
                        }
                    }
                }
            }
        }
        
        // Pinch to scale
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;
            
            float difference = currentMagnitude - prevMagnitude;
            
            ScaleBlock(difference * 0.01f);
        }
        
        // Rotate with two-finger twist
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                
                float prevAngle = Mathf.Atan2(touch1PrevPos.y - touch0PrevPos.y, touch1PrevPos.x - touch0PrevPos.x) * Mathf.Rad2Deg;
                float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                
                float angleDiff = currentAngle - prevAngle;
                RotateBlock(angleDiff);
            }
        }
    }
    
    void ScaleBlock(float increment)
    {
        if (currentBlock != null)
        {
            Vector3 scale = currentBlock.transform.localScale;
            scale += Vector3.one * increment;
            scale = Vector3.Max(scale, Vector3.one * 0.1f);
            scale = Vector3.Min(scale, Vector3.one * 2f);
            currentBlock.transform.localScale = scale;
        }
    }
    
    void RotateBlock(float angle)
    {
        if (currentBlock != null)
        {
            currentBlock.transform.Rotate(Vector3.up, angle, Space.World);
        }
    }
    
    public void ResetBlock()
    {
        if (currentBlock != null)
        {
            Destroy(currentBlock);
            blockPlaced = false;
            
            if (planeManager != null)
                planeManager.enabled = true;
            
            if (instructionUI != null)
                instructionUI.SetActive(true);
        }
    }
    
    public void SwitchBlockType(bool useLocal)
    {
        if (currentBlock != null)
        {
            Vector3 pos = currentBlock.transform.position;
            Quaternion rot = currentBlock.transform.rotation;
            Vector3 scale = currentBlock.transform.localScale;
            
            Destroy(currentBlock);
            
            GameObject prefab = useLocal ? localBlockPrefab : basicBlockPrefab;
            currentBlock = Instantiate(prefab, pos, rot);
            currentBlock.transform.localScale = scale;
            currentBlock.AddComponent<BlockInteraction>();
        }
    }
}