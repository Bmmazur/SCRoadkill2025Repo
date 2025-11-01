using UnityEngine;

public class CrashMovement : MonoBehaviour
{
    public bool positiveY;
    public bool isCrashing = true;
    public RKSpawner rkSpawner;
    public GameObject bloodSprite;
    private void Awake()
    {
        rkSpawner = GameObject.Find("RKSpawner").GetComponent<RKSpawner>();
        if (transform.position.y > 0)
        {
            positiveY = true;
        }
        else if (transform.position.y < 0)
        {
            positiveY = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float crashEnd = 7.0f;

        float speed = Random.Range(10, 40);

        if (transform.position.y < crashEnd && positiveY || transform.position.y > -crashEnd && !positiveY)
        {
            isCrashing = false;
            Vector2 bloodPosition = new Vector2(transform.position.x, transform.position.y);
            Instantiate(bloodSprite, bloodPosition, Quaternion.identity);
        }

        if (positiveY && isCrashing)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
        else if (!positiveY && isCrashing)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrashing)
        {
            if (collision.gameObject.tag == "Border")
            {
                Debug.Log("Destroyed:" + name);
                Destroy(gameObject);
                rkSpawner.LevelClear();
            }
            else if (collision.gameObject.tag == "car")
            {
                Debug.Log("squelch");
            }
        }
    }
}
