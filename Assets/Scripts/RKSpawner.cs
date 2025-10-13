using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class RKSpawner : MonoBehaviour
{
    public GameObject[] roadkill;
    public float spawnRangeX = 6.0f;
    public float spawnRangeY = 10.0f;
    public GameObject rkPrefab;
    public int spawnQuota = 5;
    public int rkCount = 0;
    public MenuManager menuManager;
    public PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public float currentScore = 0;
    public Timer timer;


    void Start()
    {
        //SpawnLoop();
        SetScore();
    }

    public void SpawnLoop()
    {
        for (int i = spawnQuota; i > 0; i--)
        {
            SpawnRoadkill();
            rkCount++;
        }
        timer.ResetTimer();
    }
    public void SpawnRoadkill()
    {
        int roadkillList = Random.Range(0, roadkill.Length);
        float spawnPointX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPointY = Random.Range(-spawnRangeY, spawnRangeY);

        Vector2 spawnPosition = new Vector2(spawnPointX, spawnPointY);

        rkPrefab = Instantiate(roadkill[roadkillList], spawnPosition, Quaternion.identity);
    }
    public void LevelClear()
    {
        currentScore += 100;
        SetScore();
        if(rkCount > 0)
        {
            rkCount--;
        }
        
        if(rkCount <= 0)
        {
            spawnQuota += 2;
            playerController.DisablePlayer();
            menuManager.OpenNextLevel();
            Debug.Log("Level Cleared");
        }
    }
    public void ResetRoadkill()
    {
        spawnQuota = 5;
        currentScore = 0;
        SetScore();
        timer.ResetTimer();
    }
    public void SetScore()
    {
        scoreText.text = "$" + currentScore;
    }
}