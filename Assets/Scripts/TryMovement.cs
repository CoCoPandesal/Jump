using UnityEngine;

public class TryMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Screen Wrap")]
    public float screenHalfWidth = 5f;  // Adjust to match your camera width

    private Rigidbody2D rb;
    private bool isDead = false;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        float input = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);

        // Wrap around screen edges
        Vector3 pos = transform.position;
        if (pos.x > screenHalfWidth) pos.x = -screenHalfWidth;
        else if (pos.x < -screenHalfWidth) pos.x = screenHalfWidth;
        transform.position = pos;

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public void BounceUp(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (animator != null) animator.SetTrigger("Death");
    }
}
