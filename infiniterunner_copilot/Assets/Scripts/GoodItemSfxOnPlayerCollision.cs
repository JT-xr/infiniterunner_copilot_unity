using UnityEngine;

public class GoodItemSfxOnPlayerCollision : MonoBehaviour
{
    public AudioClip sfxClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && sfxClip != null)
        {
            AudioSource.PlayClipAtPoint(sfxClip, Camera.main.transform.position);
        }
    }
}
