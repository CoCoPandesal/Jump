using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;  // How far left and right it travels
    public float speed = 2f;

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        float offset = transform.position.x - startPos.x;
        if (offset >= moveDistance) direction = -1;
        else if (offset <= -moveDistance) direction = 1;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        // Only count as a landing if player hit the top
        Rigidbody2D playerRb = col.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null && playerRb.velocity.y <= 0f)
        {
            col.transform.SetParent(transform);

            TryMovement movement = col.gameObject.GetComponent<TryMovement>();
            if (movement != null) movement.BounceUp(18f);

            if (ScoreMonitor.Instance != null) ScoreMonitor.Instance.AddScore();
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.transform.SetParent(null);
    }
}
