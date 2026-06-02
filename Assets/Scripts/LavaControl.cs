using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaController : MonoBehaviour
{
    [Header("Lava Settings")]
    public float baseRiseSpeed = 1.5f;      // Normal slow rise speed
    public float maxRiseSpeed = 10f;        // Max speed when catching up to player

    [Header("Catch-Up Settings")]
    public float catchUpDistance = 10f;     // How far above lava the player must be to trigger catch-up
    public float catchUpSpeed = 8f;         // Speed used when catching up to the player
    public float catchUpAcceleration = 5f;  // How fast lava accelerates toward catch-up speed

    [Header("References")]
    public GameObject player;

    [Header("Kill Settings")]
    public float killDelay = 0.3f;
    public string gameOverScene = "";

    private float riseSpeed;
    private bool playerIsDead = false;

    void Start()
    {
        riseSpeed = baseRiseSpeed;
    }

    void Update()
    {
        RiseLava();
        CheckIfPlayerFellBehind();
    }

    void RiseLava()
    {
        float targetSpeed = baseRiseSpeed;

        if (player != null)
        {
            float distanceAbove = player.transform.position.y - transform.position.y;
            if (distanceAbove > catchUpDistance)
                targetSpeed = catchUpSpeed;
        }

        // Smoothly accelerate toward target speed
        riseSpeed = Mathf.MoveTowards(riseSpeed, targetSpeed, catchUpAcceleration * Time.deltaTime);
        riseSpeed = Mathf.Clamp(riseSpeed, baseRiseSpeed, maxRiseSpeed);

        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    void CheckIfPlayerFellBehind()
    {
        if (player == null || playerIsDead) return;

        if (player.transform.position.y < transform.position.y)
            KillPlayer();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            KillPlayer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            KillPlayer();
    }

    void KillPlayer()
    {
        if (playerIsDead) return;
        playerIsDead = true;

        TryMovement movement = player.GetComponent<TryMovement>();
        if (movement != null) movement.Die();

        if (!string.IsNullOrEmpty(gameOverScene))
            SceneManager.LoadScene(gameOverScene);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}