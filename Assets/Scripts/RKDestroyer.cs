using UnityEngine;

public class RKDestroyer : MonoBehaviour
{
    public Rigidbody2D rb;
    RKSpawner rkSpawner;

    private void Awake()
    {
        rkSpawner = GameObject.Find("RKSpawner").GetComponent<RKSpawner>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Debug.Log("Destroyed:" + name);
            Destroy(gameObject);
            rkSpawner.LevelClear();
        }
        else if(collision.gameObject.tag == "car")
        {
            Debug.Log("squelch");
        }
    }
}
