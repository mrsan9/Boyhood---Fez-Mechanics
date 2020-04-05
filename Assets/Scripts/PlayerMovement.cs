using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody rb;
    public float moveSpeed, jumpSpeed;

    [SerializeField]GameObject rayObject;
    [SerializeField] Transform disObj;
    [HideInInspector]
    public static bool isMoving,disPlaced;
  
    private void Start()
    {       
        isMoving = false; 
        rb = GetComponent<Rigidbody>();       
    }

    GameObject currCube = null;

    float horz;
    bool grounded;
    float jump;
    
    private void Update()
    {       
       

        transform.Translate(new Vector3(horz * moveSpeed * Time.deltaTime, jump, 0f));

        isMoving = (horz > 0 || horz < 0);

        castRays();
        keyInputs();
    }        

    void keyInputs()
    {
        horz = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = jumpSpeed; StartCoroutine(fall());
        }
    }
   // bool isPlaced = false;
    RaycastHit hit,hit1;
    RaycastHit[] hits;
    GameObject nearestObj = null;
    //float[] distances;
    int indexLane = 0;

    Vector3 targetp;

    public void castRays()
    {
        if (isMoving)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, LayerMask.GetMask("ground")) ? true : false;
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
            hits = Physics.RaycastAll(rayObject.transform.position, targetp, 1000f);
            
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
                    if(RotRef.side == 1)
                    targetp = rayObject.transform.position+ Vector3.left * 1000;
                    else
                    targetp = rayObject.transform.position + Vector3.right * 1000;
                    //Debug.Log("Direction:" + targetp);
                    //targetp.z = rayObject.transform.position.z;
                    //targetp.y = rayObject.transform.position.y;
                    //targetp.x *= 1000f;
                }
                Debug.DrawLine(rayObject.transform.position, targetp, Color.blue);
                if (Physics.Raycast(rayObject.transform.position, targetp, out hit1, 1000f) && !grounded)
                {
                    Vector3 pos = transform.position;
                    if (RotRef.side % 2 == 0)
                        pos.z = hit1.collider.transform.position.z;
                    else
                    {
                        pos.x = hit1.collider.transform.position.x;
                        
                    }
                    transform.position = pos;

                    currCube = hit1.collider.gameObject;

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

    IEnumerator fall()
    {
        yield return new WaitForSeconds(0.05f);
        jump = 0;
    }

   
    /* private static float WrapAngle(float angle)
     {
         angle %= 360;
         if (angle > 180)
             return angle - 360;

         return angle;
     }

     private static float UnwrapAngle(float angle)
     {
         if (angle >= 0)
             return angle;

         angle = -angle % 360;

         return 360 - angle;
     }*/

}
