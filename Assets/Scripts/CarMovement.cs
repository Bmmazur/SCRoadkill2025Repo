using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CarMovement : MonoBehaviour
{
    private float neutral = 0f;
    private bool positiveX;
    public bool isBoss = false;

    public Light2D carLight;
    private float clValue = 0f;
    private void Awake()
    {

        if (transform.position.x > neutral)
        {
            positiveX = true;
            //transform.rotation = new Quaternion(0, 0, 180, 1);
        }
        else if (transform.position.x < neutral)
        {
            positiveX = false;
            transform.rotation = new Quaternion(0, 0, 180, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        carLight.intensity = clValue;
        float sideBound = 25.0f;

        float speed = Random.Range(10, 15);
        float bossSpeed = 20;

        if(transform.position.x > sideBound || transform.position.x < -sideBound)
        {
            Destroy(gameObject);
        }

        if(positiveX)
        {
            if (!isBoss)
            {
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * bossSpeed);
            }
        }
        else if(!positiveX)
        {
            if (!isBoss)
            {
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * bossSpeed);
            }
        }
    }

    public void ChangeCarLights()
    {
        if(clValue <= 20)
        {
            clValue += 5f;
        }
        else
        {
            return;
        }
    }
    public void ResetLights()
    {
        clValue = 0;
    }
}
