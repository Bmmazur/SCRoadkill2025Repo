using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Variables go here
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public InputActionReference sprint;
    public InputActionReference shovel;
    Vector2 moveDirection = Vector2.zero;
    

    public float leftRange = -9.0f;
    public float rightRange = 9.0f;
    public float botRange = -13.0f;
    public float topRange = 13.0f;
    public float moveSpeed = 5.0f;
    public float sprintSpeed = 7.0f;    
    public float slowSpeed = 3.0f;
    public bool isSprinting;
    //public float sprintMeter = 100;

    public bool shovelFull = false;
    public bool shovelCollide = false;
    public Transform shovelPoint;
    public Transform rotPoint;
    public float shovelRange = 0.5f;
    private int rkIndex;
    private int missIndex;
    public bool isMoving = false;
    private Vector2 lastMoveDirection;
    public GameObject shovelSprite;
    public GameObject rkSprite;
    public GameObject playerRK;
    private GameObject rkObject;
    public MenuManager menuManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rkIndex = LayerMask.NameToLayer("Roadkill");
        missIndex = LayerMask.NameToLayer("Miss");
        //rkDespawn = GetComponent<RKSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        //This stops the player from moving off screen
        if (transform.position.x < leftRange)
            {
                transform.position = new Vector3(leftRange, transform.position.y, transform.position.z);
            }
        if (transform.position.x > rightRange)
            {
                transform.position = new Vector3(rightRange, transform.position.y, transform.position.z);
            }
        if (transform.position.y < botRange)
        {
            transform.position = new Vector3(transform.position.x, botRange, transform.position.z);
        }
        if (transform.position.y > topRange)
        {
            transform.position = new Vector3(transform.position.x, topRange, transform.position.z);
        }

        //This empties the sprint meter when sprinting and fills the sprint meter when not sprinting
        /*if (isSprinting && sprintMeter >= 0)
        {
            sprintMeter--;
        }
        else if (!isSprinting && sprintMeter <= 99)
        {
            sprintMeter++;
        }
        if(sprintMeter <= 0)
        {
            isSprinting = false;
        }*/

        //This sets the shovel to the direction the player is moving
        if (moveDirection.magnitude > 0.01f)
        {
            isMoving = true;
            lastMoveDirection = moveDirection;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            rotPoint.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (moveDirection.magnitude == 0)
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        //Moves the player at varying speeds depending on their current state
        if(!isSprinting && !shovelFull)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, transform.position.z);
        }
        else if(isSprinting)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * sprintSpeed, moveDirection.y * sprintSpeed, transform.position.z);
        }        
        else if(shovelFull)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * slowSpeed, moveDirection.y * slowSpeed, transform.position.z);
        }

        if (shovelFull && rkObject != null)
        {
            rkObject.transform.position = shovelPoint.position;
            rkObject.transform.SetParent(transform);
            shovelSprite.GetComponent<SpriteRenderer>().enabled = false;
            rkSprite.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (rkObject == null)
        {
            shovelFull = false;
            shovelSprite.GetComponent<SpriteRenderer>().enabled = true;
            rkSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void ProcessInputs()
    {
        //Processes movement
        moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();

        //Processes the sprint and shovel actions
        sprint.action.performed += x => SprintPressed();
        sprint.action.canceled += x => SprintRelease();
        shovel.action.performed += x => UseShovel();
    }

    private void SprintPressed()
    {
        if(shovelFull)
        {
            SprintFailed();
        }
        else if(!shovelFull)
        {
            isSprinting = true;
            Debug.Log("UR Sprinting");
        }
    }
    private void SprintRelease()
    {
        if (shovelFull)
        {
            return;
        }
        else if (!shovelFull)
        {
            isSprinting = false;
            Debug.Log("Stop Sprinting");
        }
    }
    private void SprintFailed()
    {
        Debug.Log("Sprint Failed");
    }
    private void UseShovel()
    {
        Debug.Log("shovel used");
        if (!isSprinting && !shovelFull)
        {
            RaycastHit2D hitInfo = Physics2D.CircleCast(shovelPoint.position, shovelRange, rotPoint.position);
            Debug.DrawRay(shovelPoint.position, rotPoint.position * shovelRange, Color.green);
            Debug.Log("Shovel hitbox active");
            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == rkIndex)
            {
                if (!shovelFull && rkObject == null)
                {
                    Debug.Log("picking Up");
                    shovelFull = true;
                    rkObject = hitInfo.collider.gameObject;
                    rkObject.transform.position = shovelPoint.position;
                    rkObject.transform.SetParent(transform);
                }
            }
            else if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == missIndex && !shovelFull)
            {
                Debug.Log("missed");
            }

        }
        else if (shovelFull && transform.position.x < 7 || transform.position.x > -7)
        {
            Debug.Log("Shovel is full");
        }
        else if (shovelFull && transform.position.x > 7 || transform.position.x < -7)
        {
            Debug.Log("Empty your shovel");
            return;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(shovelPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(shovelPoint.position, shovelRange);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            Debug.Log("Hit by car RIP");
            //GetComponent<SpriteRenderer>().color = Color.clear;
            Instantiate(playerRK, transform.position, Quaternion.identity);
            menuManager.OpenGameOver();
            DisablePlayer();
            if (shovelFull)
            {
                rkObject.transform.SetParent(null);
                rkObject = null;
            }
        }
    }
    public void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
    public void RespawnPlayer()
    {
        transform.position = new Vector2(-9, 0);
        gameObject.SetActive(true);
    }
}