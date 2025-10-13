using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime;
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
    public void ResetTimer()
    {
        currentTime = 60;
        timesUp = false;
    }
}
