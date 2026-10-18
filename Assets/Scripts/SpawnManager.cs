using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject groundObstaclePrefab;
    [SerializeField] private GameObject flyingObstaclePrefab;
    [SerializeField] private GameObject scoreItemPrefab;

    [SerializeField] private float spawnX = 11f;

    [SerializeField] private float minSpawnDelay = 1.5f;
    [SerializeField] private float maxSpawnDelay = 2.8f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minSpawnDelay, maxSpawnDelay)
            );

            if (GameManager.Instance.IsGameOver)
                continue;

            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject()
    {
        int random = Random.Range(0, 3);

        GameObject prefab;
        float y;

        if (random == 0)
        {
            prefab = groundObstaclePrefab;
            y = -2.4f;
        }
        else if (random == 1)
        {
            prefab = flyingObstaclePrefab;
            y = -0.3f;
        }
        else
        {
            prefab = scoreItemPrefab;
            y = Random.Range(-0.5f, 1.5f);
        }

        Instantiate(
            prefab,
            new Vector3(spawnX, y, 0),
            Quaternion.identity
        );
    }
}