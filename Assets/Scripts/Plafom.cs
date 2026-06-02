using UnityEngine;

public class Plafom : MonoBehaviour
{
    public float jumpForce = 18f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Player must be falling down to trigger a bounce
        if (playerRb.velocity.y <= 0f)
        {
            TryMovement movement = collision.gameObject.GetComponent<TryMovement>();
            if (movement != null) movement.BounceUp(jumpForce);

            if (ScoreMonitor.Instance != null) ScoreMonitor.Instance.AddScore();
        }
    }
}
