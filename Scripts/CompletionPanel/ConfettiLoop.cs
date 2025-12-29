using UnityEngine;

public class ConfettiLoop : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How fast the confetti falls")]
    public float fallSpeed = 200f;
    
    [Tooltip("The Y position where confetti resets to the top (The Top of your screen)")]
    public float resetHeightY = 1200f; // Adjust based on your canvas size
    
    [Tooltip("The Y position where confetti disappears (The Bottom of your screen)")]
    public float bottomThresholdY = -1200f; // Adjust based on your canvas size

    private RectTransform[] confettiPieces;

    void Start()
    {
        // Get all child confetti objects attached to this parent
        int childCount = transform.childCount;
        confettiPieces = new RectTransform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            confettiPieces[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        // Move every piece of confetti down
        foreach (RectTransform piece in confettiPieces)
        {
            if (piece == null) continue;

            // Move down
            piece.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);

            // Check if it has gone below the screen
            if (piece.anchoredPosition.y < bottomThresholdY)
            {
                ResetConfetti(piece);
            }
        }
    }

    void ResetConfetti(RectTransform piece)
    {
        // Move it back to the top
        float currentX = piece.anchoredPosition.x;
        piece.anchoredPosition = new Vector2(currentX, resetHeightY);
    }
}