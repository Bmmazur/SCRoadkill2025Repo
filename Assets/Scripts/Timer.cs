using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime = 60f;
    public float addTime = -10f;
    public bool countDown;
    public bool hasLimit;
    public bool timesUp = false;
    public float timerLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        if(hasLimit && currentTime <= timerLimit)
        {
            currentTime = timerLimit;
            SetTimer();
            timesUp = true;
            timerText.color = Color.red;
            enabled = false;
        }
        SetTimer();
    }

    public void SetTimer()
    {
        timerText.text = currentTime.ToString("0");
    }
    public void IncreaseTimer()
    {
        addTime += 10f;
    }
    public void ResetTimer()
    {
        enabled = true;
        currentTime = 60 + addTime;
        timesUp = false;
    }
}
