using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossObject;
    public Transform playerPosition;
    public Timer timer;
    private int[] carChoice = new int[2];

    private void Start()
    {
        carChoice[0] = -25;
        carChoice[1] = 25;
        float bossDelay = Random.Range(5, 10);
        float bossInterval = Random.Range(10, 20);
        InvokeRepeating("SpawnBoss", bossDelay, bossInterval);
    }
    
    public void SpawnBoss()
    {
        int carPosition = Random.Range(0, carChoice.Length);
        int carIndex = carChoice[carPosition];
        Vector2 bossPosition = new Vector2(carIndex, playerPosition.position.y);
        if (timer.timesUp == true)
        {
            Instantiate(bossObject, bossPosition, Quaternion.identity);
        }
    }
}
