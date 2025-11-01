using UnityEngine;

public class CarSpwner : MonoBehaviour
{
    public GameObject[] carObjects;
    private int[] carChoice = new int[2];

    void Start()
    {
        float startDelay = Random.Range(5, 15);
        float spawnInterval = Random.Range(10, 60);
        InvokeRepeating("SpawnCar", startDelay, spawnInterval);
        carChoice[0] = -25;
        carChoice[1] = 25;
    }

    public void SpawnCar()
    {
        int carPosition = Random.Range(0, carChoice.Length);
        int carIndex = carChoice[carPosition];
        int carList = Random.Range(0, carObjects.Length);
        Vector2 spawnPosition = new Vector2(carIndex, transform.position.y);

        Instantiate(carObjects[carList], spawnPosition, Quaternion.identity);
    }
}
