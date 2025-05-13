using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image overlayImage;
    public Color flashColor = new Color(1, 0, 0, 0.5f); // semi-transparent red
    public float flashDuration = 1f;

    private Color originalColor;
    private Coroutine flashCoroutine;

    void Awake()
    {
        if (overlayImage != null)
        {
            originalColor = overlayImage.color;
            overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f); // fully transparent
        }
    }

    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        overlayImage.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }
}
