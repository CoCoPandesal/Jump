using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class BreakablePlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [Tooltip("How long (in seconds) before the platform disappears after being touched.")]
    public float breakDelay = 1.0f;

    [Tooltip("Should the platform respawn after a certain time?")]
    public bool doesRespawn = false;

    [Tooltip("How long (in seconds) before the platform reappears (if doesRespawn is true).")]
    public float respawnDelay = 3.0f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D platformCollider;
    private bool isBreaking = false;

    void Start()
    {
        // Grab the components attached to this platform
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || isBreaking) return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null || playerRb.velocity.y > 0f) return;

        // Bounce the player up before the platform breaks
        TryMovement movement = collision.gameObject.GetComponent<TryMovement>();
        if (movement != null) movement.BounceUp(18f);

        if (ScoreMonitor.Instance != null) ScoreMonitor.Instance.AddScore();

        StartCoroutine(BreakSequence());
    }

    private IEnumerator BreakSequence()
    {
        isBreaking = true;

        // TODO: If you have a cracking animation, trigger it here.
        // Example: GetComponent<Animator>().SetTrigger("Crack");

        // Wait for the specified delay while the player uses their momentum (Ambak)
        yield return new WaitForSeconds(breakDelay);

        // Break the platform (disable the visuals and the physical collision)
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        if (doesRespawn)
        {
            // Wait for the respawn timer
            yield return new WaitForSeconds(respawnDelay);

            // Re-enable the platform
            spriteRenderer.enabled = true;
            platformCollider.enabled = true;
            isBreaking = false;
        }
        else
        {
            // Completely destroy the game object to save memory since it won't return
            Destroy(gameObject);
        }
    }
}