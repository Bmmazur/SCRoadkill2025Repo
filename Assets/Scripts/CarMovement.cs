using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float neutral = 0f;
    private bool positiveY;
    public bool isBoss = false;
    private void Awake()
    {

        if (transform.position.y > neutral)
        {
            positiveY = true;
        }
        else if (transform.position.y < neutral)
        {
            positiveY = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float topBound = 30.0f;

        float speed = Random.Range(10, 15);
        float bossSpeed = 20;

        if(transform.position.y > topBound || transform.position.y < -topBound)
        {
            Destroy(gameObject);
        }

        if(positiveY)
        {
            if (!isBoss)
            {
                transform.Translate(Vector2.down * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector2.down * Time.deltaTime * bossSpeed);
            }
        }
        else if(!positiveY)
        {
            if (!isBoss)
            {
                transform.Translate(Vector2.up * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(Vector2.up * Time.deltaTime * bossSpeed);
            }
        }
    }
}
