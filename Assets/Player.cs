using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject rayObject;
    [SerializeField] public Transform refForGameover;

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;
    

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;
    public GameObject boxParticles;
    public GameObject playerParticles;

    //Wall movement
    public bool walled, wallMove;
    bool[] wallHits = new bool[4];

    private void Start()
    {
        GameController.THIS.isLevelDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.THIS.isLevelDone)
        {
            bool wasOnGround = onGround;
            onGround = Physics.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
                Physics.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer) ||
                Physics.Raycast(transform.position, Vector2.down, groundLength, groundLayer);

            //if (!wasOnGround && onGround)
            //{
            //    StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
            //}

            if (Input.GetButtonDown("Jump"))
            {
                jumpTimer = Time.time + jumpDelay;
                isJumping = true;
            }

            walled = Physics.Raycast(transform.position, transform.forward, out hit, 4f, LayerMask.GetMask("ladder"));


            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            isMoving = (direction.x != 0);


            castRays();

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (onGround)
                    rb.velocity = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.UpArrow) && walled)
            {
                wallMove = true;
                animator.SetBool("isClimb", true);

                rb.isKinematic = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && wallMove)
            {
                rb.isKinematic = false;
                wallMove = false;
                animator.SetBool("isClimb", false);
            }


            if (transform.position.y <= refForGameover.position.y)
            {
                UIManager.instance.RestartGame();
            }
        }
    }
    void FixedUpdate()
    {
        if (!GameController.THIS.isLevelDone)
        {
            if (!wallMove)
                moveCharacter(direction.x);


            if (wallMove)
            {
                if (!walled) { wallMove = false; animator.SetBool("isClimb", false); rb.isKinematic = false; };


                //if (!wallHits[1]) direction.x = 0;

                if (RotRef.side % 2 == 0)
                {
                    if (RotRef.side == 0) dir = 1;
                    else dir = -1;

                    transform.Translate(direction.x * dir * 0.05f, direction.y * 0.05f, 0);
                }
                else
                {
                    if (RotRef.side == 1) dir = 1;
                    else dir = -1;

                    transform.Translate(0, direction.y * 0.05f, direction.x * dir * 0.05f);
                }


            }


            if (jumpTimer > Time.time && (onGround || walled))
            {
                walled = false; wallMove = false;
                rb.isKinematic = false;
                Jump();
            }

            modifyPhysics();
        }
        else
        { rb.velocity = Vector3.zero; animator.SetBool("isRun", false); }
    }

    int dir = 1;
    void moveCharacter(float horizontal)
    {
        

        if (RotRef.side % 2 == 0)
        {
            if (RotRef.side == 0) dir = 1;
            else dir = -1;
            //SoundManager.instance.PlayClip(EAudioClip.WALK_SFX, 1);
            rb.AddForce(new Vector3(horizontal * dir * moveSpeed,0,0));
        }
        else
        {
            if (RotRef.side == 1) dir = 1;
            else dir = -1;
           // SoundManager.instance.PlayClip(EAudioClip.WALK_SFX, 1);
            rb.AddForce(new Vector3(0, 0, horizontal * dir * moveSpeed));
        }

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
        //if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        //{
        //    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        //}
        

        if (direction.x != 0)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isClimb", false);
        }
        else
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isClimb", false);
            if (onGround)
                rb.velocity = new Vector3(0,0,0) ;
        }
        // animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        // animator.SetFloat("vertical", rb.velocity.y);
    }
    void Jump()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode.Impulse);
        jumpTimer = 0;
        SoundManager.instance.PlayClip(EAudioClip.JUMP_SFX, 1);
        // StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            //if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            //{
            //   // rb.drag = linearDrag;
            //}
            //else
            //{
            //    rb.drag = 0f;
            //}
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            //rb.gravityScale = gravity;
            //rb.drag = linearDrag * 0.15f;
            //if (rb.velocity.y < 0)
            //{
            //    rb.gravityScale = gravity * fallMultiplier;
            //}
            //else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            //{
            //    rb.gravityScale = gravity * (fallMultiplier / 2);
            //}
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        characterHolder.gameObject.GetComponent<SpriteRenderer>().flipX = !facingRight;
        //transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "crystal")
        {
            Instantiate(boxParticles,other.gameObject.transform.position,Quaternion.identity);
            Destroy(other.gameObject);
            SoundManager.instance.PlayClip(EAudioClip.COLLECT_SFX, 1.5f);
            UIManager.redBoxes++;
            if (
            UIManager.instance.LevelDoneCheck()) {
                GameController.THIS.isLevelDone = true;
                Instantiate(playerParticles, transform.position+(Vector3.up * 0.5f), Quaternion.identity);
                LeanTween.move(gameObject, this.transform.position + (Vector3.up * 10f), 1.7f);
                SoundManager.instance.PlayClip(EAudioClip.SUCCESS_SFX, 1);
            }
            //LeanTween.move(other.gameObject,this.transform.position+(Vector3.up*2f),0.5f).setOnComplete(()=> { Destroy(other.gameObject); });
        }
    }
    private void OnDrawGizmos()
    {
        

        //if (RotRef.side % 2 == 0)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position + (Vector3.up * 0.5f), transform.forward);
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(transform.position + (Vector3.right * 0.5f), transform.forward);
        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawLine(transform.position - (Vector3.right * 0.5f), transform.forward);
        //}
        //else
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position + (Vector3.up * 0.5f), transform.forward);
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(transform.position + (Vector3.forward * 0.5f), transform.forward);
        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawLine(transform.position - (Vector3.forward * 0.5f), transform.forward);
        //}

       //  Gizmos.DrawLine(rayObject.transform.position, transform.position+transform.forward);
        //  Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
    RaycastHit hit, hit1;
    RaycastHit[] hits;
    GameObject nearestObj = null;

    int indexLane = 0;
    public static bool isMoving, isJumping;
    Vector3 targetp;
    [HideInInspector]
    public bool grounded;
    public void castRays()
    {

        if (isMoving || isJumping)
        {

            grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, LayerMask.GetMask("ground")) ? true : false;

            //if (grounded) isJumping = false;
            // Debug.Log(RotRef.side % 2);
            if (RotRef.side % 2 == 0)
            {
                targetp = -rayObject.transform.forward;
                targetp.z *= 1000f;
                targetp.y = rayObject.transform.position.y;
                targetp.x = rayObject.transform.position.x;
            }
            else
            {
                if (RotRef.side == 1)
                    targetp = rayObject.transform.position + Vector3.right * 1000;
                else
                    targetp = rayObject.transform.position + Vector3.left * 1000;
                //Debug.Log("Direction:"+targetp);
                //targetp.z = rayObject.transform.position.z;
                //targetp.y = rayObject.transform.position.y;
                //targetp.x *= 1000f;
            }
            hits = Physics.RaycastAll(rayObject.transform.position, targetp, 1000f, LayerMask.GetMask("ground"));

            Debug.DrawLine(rayObject.transform.position, targetp, Color.red);
            if (hits.Length > 0)
            {

                //distances = new float[hits.Length];

                //for (int i = 0; i < hits.Length; i++)
                //{

                //    distances[i] = Vector3.Distance(hits[i].collider.gameObject.transform.position,disObj.position);
                //    //Debug.Log("Distance:"+distances[i]);
                //}
                //float min = distances[0];
                //for (int i = 0; i < distances.Length; i++)
                //{
                //    if (min > distances[i])
                //    { min = distances[i];index = i; }
                //}
                //Debug.Log(index);
                if (!Physics.Raycast(rayObject.transform.position, transform.forward, out hit, 20f, LayerMask.GetMask("Default")))
                {
                    //Debug.LogError("!default");
                    Vector3 pos = transform.position;

                    if (RotRef.side % 2 == 0)
                        pos.z = hits[0].collider.transform.position.z;
                    else
                    {
                        pos.x = hits[0].collider.transform.position.x;

                    }
                    //pos.z = hits[index].collider.transform.position.z;
                    transform.position = pos;
                    //isPlaced = true;
                    hits = null;
                }
                /* if (currCube != null)
                 {
                     if (currCube != hit1.collider.gameObject)
                     {
                         Vector3 pos = transform.position;
                         pos.z = hit1.collider.transform.position.z;
                         transform.position = pos;

                         currCube = hit1.collider.gameObject;
                     }
                 }
                 else
                 {
                     if (hit1.collider != null)
                     {
                         Vector3 pos = transform.position;

                         pos.z = hit1.collider.transform.position.z;
                         transform.position = pos;

                         currCube = hit1.collider.gameObject;
                     }
                 }
                */
            }
            else
            {
                if (RotRef.side % 2 == 0)
                {
                    targetp = rayObject.transform.forward;
                    targetp.z *= 1000f;
                    targetp.y = rayObject.transform.position.y;
                    targetp.x = rayObject.transform.position.x;
                }
                else
                {
                    if (RotRef.side == 1)
                        targetp = rayObject.transform.position + Vector3.left * 1000;
                    else
                        targetp = rayObject.transform.position + Vector3.right * 1000;
                    //Debug.Log("Direction:" + targetp);
                    //targetp.z = rayObject.transform.position.z;
                    //targetp.y = rayObject.transform.position.y;
                    //targetp.x *= 1000f;
                }
                Debug.DrawLine(rayObject.transform.position, targetp, Color.blue);

                if (Physics.Raycast(rayObject.transform.position, targetp, out hit1, 1000f, LayerMask.GetMask("ground")) && !grounded)
                {
                    Vector3 pos = transform.position;

                    if (!Physics.Raycast(rayObject.transform.position, transform.forward, out hit, 20f, LayerMask.GetMask("Default")))
                    {
                       // Debug.LogError("!default");
                        if (RotRef.side % 2 == 0)
                            pos.z = hit1.collider.transform.position.z;
                        else
                        {
                            pos.x = hit1.collider.transform.position.x;

                        }
                        transform.position = pos;

                    }

                    /*if (currCube != null)
                    {
                        if (currCube != hit1.collider.gameObject)
                        {
                            Vector3 pos = transform.position;
                            pos.z = hit1.collider.transform.position.z;
                            transform.position = pos;

                            currCube = hit1.collider.gameObject;
                        }
                    }
                    else
                    {
                        if (hit.collider != null)
                        {
                            Vector3 pos = transform.position;
                            pos.z = hit1.collider.transform.position.z;
                            transform.position = pos;

                            currCube = hit1.collider.gameObject;
                        }
                    }*/
                }
                //else
                //    isPlaced = false;

            }
        }

    }



}