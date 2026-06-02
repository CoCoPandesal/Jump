using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int platformCount = 300;

    [Header("Spawn Range")]
    public float minXPosition = -4f;
    public float maxXPosition = 4f;
    public float minYSpacing = 1.2f;
    public float maxYSpacing = 2.2f;

    [Header("Player")]
    public GameObject player;

    [Header("Lava")]
    public GameObject lavaPrefab;          // Assign your lava prefab in Inspector
    public float lavaStartY = -3f;         // Where lava begins (below player)
    public float lavaRiseSpeed = 1.5f;     // Units per second (increase = harder)
    public float lavaAcceleration = 0.05f; // Lava gets faster over time

    private List<GameObject> spawnedPlatforms = new List<GameObject>();
    private float highestPlatformY = 0f;

    void Start()
    {
        SpawnInitialPlatforms();
        PlacePlayerOnFirstPlatform();
        SpawnLava();
    }

    void Update()
    {
        if (player != null && player.transform.position.y + 20f > highestPlatformY)
        {
            SpawnPlatformBatch(20);
        }

        CleanUpOldPlatforms();
    }

    void SpawnInitialPlatforms()
    {
        Vector3 startPos = new Vector3(0f, 0f, 0f);
        GameObject startPlatform = Instantiate(platformPrefab, startPos, Quaternion.identity);
        spawnedPlatforms.Add(startPlatform);
        highestPlatformY = startPos.y;

        SpawnPlatformBatch(platformCount - 1);
    }

    void SpawnPlatformBatch(int count)
    {
        float lastX = 0f;

        for (int i = 0; i < count; i++)
        {
            highestPlatformY += Random.Range(minYSpacing, maxYSpacing);

            float newX;
            int attempts = 0;
            do
            {
                newX = Random.Range(minXPosition, maxXPosition);
                attempts++;
            }
            while (Mathf.Abs(newX - lastX) > 5f && attempts < 10);

            lastX = newX;

            Vector3 spawnPosition = new Vector3(newX, highestPlatformY, 0f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            spawnedPlatforms.Add(platform);
        }
    }

    void PlacePlayerOnFirstPlatform()
    {
        if (player != null)
        {
            player.transform.position = new Vector3(0f, 1.5f, 0f);
        }
    }

    void SpawnLava()
    {
        if (lavaPrefab == null)
        {
            Debug.LogWarning("GameManager: No lava prefab assigned!");
            return;
        }

        // Spawn lava below the starting position
        Vector3 lavaPos = new Vector3(0f, lavaStartY, 0f);
        GameObject lava = Instantiate(lavaPrefab, lavaPos, Quaternion.identity);

        // Pass settings to the LavaController
        LavaController lavaController = lava.GetComponent<LavaController>();
        if (lavaController != null)
        {
            lavaController.baseRiseSpeed = lavaRiseSpeed;
            lavaController.player = player;
        }
        else
        {
            Debug.LogWarning("GameManager: LavaController component not found on lava prefab!");
        }
    }

    void CleanUpOldPlatforms()
    {
        if (player == null) return;

        float despawnThreshold = player.transform.position.y - 30f;

        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] == null)
            {
                spawnedPlatforms.RemoveAt(i);
                continue;
            }

            if (spawnedPlatforms[i].transform.position.y < despawnThreshold)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}