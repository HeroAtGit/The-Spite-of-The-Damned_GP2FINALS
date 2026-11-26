using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;
    private float timeTimeUntilObstacleSpawn;

    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeTimeUntilObstacleSpawn += Time.deltaTime;

        if (timeTimeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            Spawn();
            timeTimeUntilObstacleSpawn = 0f;
        }
    }    

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = Vector2.left * obstacleSpeed;

    }
}
