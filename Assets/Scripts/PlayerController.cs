using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Variables go here
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public InputActionReference sprint;
    public InputActionReference shovel;
    Vector2 moveDirection = Vector2.zero;
    

    public float leftRange = -19.0f;
    public float rightRange = 19.0f;
    public float botRange = -9.0f;
    public float topRange = 9.0f;
    public float startPos = 8.5f;
    public float moveSpeed = 5.0f;
    public float sprintSpeed = 7.0f;    
    public float slowSpeed = 3.0f;
    public bool isSprinting;

    public float stamina, maxStamina;
    public float sprintCost;
    public float chargeRate = 15f;
    public GameObject sprintMeter;
    private Coroutine sprintRecharge;
    
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
    public SpriteRenderer playerRend;

    [SerializeField] private Animator playerAnimtor;
    [SerializeField] AudioManager audioManager;


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

        //This sets the shovel to the direction the player is moving
        if (moveDirection.magnitude > 0.01f)
        {
            isMoving = true;
            playerAnimtor.SetBool("animWalk", true);
            lastMoveDirection = moveDirection;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            rotPoint.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (moveDirection.magnitude == 0)
        {
            playerAnimtor.SetBool("animWalk", false);
            isMoving = false;
        }
        
        if(stamina < 25)
        {
            sprintMeter.SetActive(true);
        }
        else if(stamina > 25)
        {
            sprintMeter.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //Moves the player at varying speeds depending on their current state
        if (isSprinting)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * sprintSpeed, moveDirection.y * sprintSpeed, transform.position.z);
             if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                stamina -= sprintCost * Time.deltaTime;
                playerAnimtor.SetBool("animSprint", true);
                if (stamina <= 0)
                {
                    stamina = 0;
                    playerAnimtor.SetBool("animSprint", false);
                    isSprinting = false;
                }
                RunCoroutine();
            }
            //audioManager.PlaySFX(audioManager.sprint);

        }
        else if (!isSprinting && !shovelFull)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, transform.position.z);
            //audioManager.PlaySFX(audioManager.walk);
        }

        else if(shovelFull)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * slowSpeed, moveDirection.y * slowSpeed, transform.position.z);
            //audioManager.PlaySFX(audioManager.walk);
        }

        //Sets the position of the roadkill when held by the player
        if (shovelFull && rkObject != null)
        {
            rkObject.transform.position = shovelPoint.position;
            rkObject.transform.SetParent(transform);
            //shovelSprite.GetComponent<SpriteRenderer>().enabled = false;
            //rkSprite.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (rkObject == null)
        {
            shovelFull = false;
            playerRend.sortingOrder = 2;
            //playerAnimtor.SetTrigger("animShovel");
            //shovelSprite.GetComponent<SpriteRenderer>().enabled = true;
            //rkSprite.GetComponent<SpriteRenderer>().enabled = false;
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
        else if(!shovelFull && stamina > 0)
        {
            isSprinting = true;
            Debug.Log("UR Sprinting");
        }
        else if(stamina <= 0)
        {
            isSprinting = false;
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
            playerAnimtor.SetBool("animSprint", false);
            Debug.Log("Stop Sprinting");
        }
    }
    private void SprintFailed()
    {
        //Play animation for sprint failing
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
                    playerAnimtor.SetTrigger("animShovel");
                    audioManager.PlaySFX(audioManager.shovelHit);
                    playerRend.sortingOrder = 0;
                    rkObject = hitInfo.collider.gameObject;
                    rkObject.transform.position = shovelPoint.position;
                    rkObject.transform.SetParent(transform);
                }
            }
            else if (hitInfo.collider == null )
            {
                Debug.Log("missed");
                playerAnimtor.SetTrigger("animShovel");
                audioManager.PlaySFX(audioManager.shovelMiss);
            }

        }
        else if (shovelFull)
        {
            //Play animation for when shovel is full but you can't drop the roadkill
            Debug.Log("Shovel is full");
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
        transform.position = new Vector2(0, startPos);
        gameObject.SetActive(true);
    }

    public void RunCoroutine()
    {
        if (sprintRecharge != null) StopCoroutine(sprintRecharge);
        sprintRecharge = StartCoroutine(RechargeSprint());
    }

    private IEnumerator RechargeSprint()
    {
        yield return new WaitForSeconds(1f);

        while(stamina < maxStamina)
        {
            stamina += chargeRate / 10f;
            if (stamina >= maxStamina) stamina = maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }
}