using UnityEngine;
using UnityEngine.Events;

public class CrashController : MonoBehaviour
{
    [Header("Crash Events")]
    [SerializeField] private CrashEvent[] crashEventList;

    private bool positiveY;
    private int[] spawnChoice = new int[2];
    private float[] rotationChoice = new float[3];
    public GameObject crashPrefab;
    [SerializeField] AudioManager audioManager;

    void Start()
    {
        spawnChoice[0] = -15;
        spawnChoice[1] = 15;
        rotationChoice[0] = -0.2f;
        rotationChoice[1] = 0;
        rotationChoice[2] = 0.2f;
    }

    public void SpawnCrash()
    {
        int crashPositionY = Random.Range(0, spawnChoice.Length);
        int crashPositionX = Random.Range(-10, 10);
        int crashIndex = spawnChoice[crashPositionY];
        int rotationRange = Random.Range(0, rotationChoice.Length);
        float rotationIndex = rotationChoice[rotationRange];

        Vector2 spawnPosition = new Vector2(crashPositionX, crashIndex);
        Quaternion spawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, rotationIndex, 1);

        Instantiate(crashPrefab, spawnPosition, spawnRotation);
        audioManager.PlaySFX(audioManager.carCrash);
    }

    // Update is called once per frame
    void Update()
    {
        FireEvent();
    }

    private void FireEvent()
    {
        float totalChance = 0f;
        foreach(CrashEvent crashCar in crashEventList)
        {
            totalChance += crashCar.crashChance;
        }
        float rand = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;
        foreach(CrashEvent crashCar in crashEventList)
        {
            cumulativeChance += crashCar.crashChance;
            if (rand <= cumulativeChance)
            {
                crashCar.crashEvent.Invoke();
                return;
            }
        }
    }

    public void CarCrash()
    {
        Debug.Log("Car has Crashed");
        SpawnCrash();
    }

    public void NothingHappens()
    {
        Debug.Log("Nothing Happened");
        return;
    }
}

[System.Serializable]
public class CrashEvent
{
    public string eventName;
    [Space]
    [Space]
    [Range(0f, 1f)] public float crashChance = 0.5f;
    public UnityEvent crashEvent;
}
