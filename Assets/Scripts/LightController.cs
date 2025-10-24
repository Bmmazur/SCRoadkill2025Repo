using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerLight;
    private float plValue = 0f;
    private float glValue = 1f;
    
    // Update is called once per frame
    void Update()
    {
        globalLight.intensity = glValue;
        playerLight.intensity = plValue;
    }

    public void ChangeLights()
    {
        if(glValue >= 0)
        {
            glValue -= 0.2f;
        }
        else
        {
            return;
        }
        if(plValue <= 2)
        {
            plValue += 0.5f;
        }
        else
        {
            return;
        }
    }

    public void ResetLights()
    {
        plValue = 0f;
        glValue = 1f;
    }
}
