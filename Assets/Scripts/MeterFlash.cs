
using UnityEngine;

public class MeterFlash : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float speed;

    Renderer ren;

    private void Awake()
    {
        ren = GetComponent<Renderer>();
    }
    private void Update()
    {
        ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }
}
