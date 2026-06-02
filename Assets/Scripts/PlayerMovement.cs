using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
   // public float jumpForce = 10f;
    public Rigidbody2D rb;

    private float movex;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // LEFT & RIGHT INPUT
        movex = Input.GetAxis("Horizontal") * speed;

    }

    private void FixedUpdate()
    {
        // MOVE using physics
        Vector2 velocity = rb.velocity;
        velocity.x = movex;
        rb.velocity = velocity;
    }

}