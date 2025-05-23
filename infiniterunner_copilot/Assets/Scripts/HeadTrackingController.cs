/// <summary>
/// Controls the horizontal movement of a UI element based on the user's head position detected via AR face tracking.
/// Utilizes ARFoundation's <see cref="ARFaceManager"/> to track faces and maps the first detected face's local X position
/// to the anchored position of a specified <see cref="RectTransform"/> (playerUI) within screen boundaries.
/// Includes debug logging for face detection status and position.
/// </summary>

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadTrackingController : MonoBehaviour
{
    [SerializeField] private RectTransform playerUI; // Reference to the PlayerUI RectTransform
    [SerializeField] private float moveSpeed = 8f; // Movement speed
    private ARFaceManager faceManager;

    private float screenBoundaryLeft;
    private float screenBoundaryRight;

    void Start()
    {
        // Get ARFaceManager from the scene
        faceManager = Object.FindFirstObjectByType<ARFaceManager>();

        // Calculate screen boundaries
        RectTransform canvasRect = playerUI.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        screenBoundaryLeft = -canvasRect.rect.width / 2 + playerUI.rect.width / 2;
        screenBoundaryRight = canvasRect.rect.width / 2 - playerUI.rect.width / 2;
    }

    void DebugFaceTracking()
    {
        // Remove all Debug.Log statements from DebugFaceTracking
    }

    void Update()
    {
        DebugFaceTracking();

        if (faceManager.trackables.count > 0)
        {
            // Get the first detected face
            foreach (ARFace face in faceManager.trackables)
            {
                // Use the face's position to control UI movement
                Vector3 facePosition = face.transform.localPosition;

                // Map face position to horizontal movement
                float horizontalInput = Mathf.Clamp(facePosition.x, -1f, 1f);

                // Move PlayerUI
                Vector2 newPosition = playerUI.anchoredPosition + new Vector2(horizontalInput * moveSpeed * Time.deltaTime, 0);
                newPosition.x = Mathf.Clamp(newPosition.x, screenBoundaryLeft, screenBoundaryRight);
                playerUI.anchoredPosition = newPosition;

                break; // Only process the first face
            }
        }
    }
}
