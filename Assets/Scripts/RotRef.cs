using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotRef : MonoBehaviour {

    float rot;
    Vector3 initRot, initRotPlayer;
    [HideInInspector]
    public static int side;
    public Transform player;
    public Transform pont;
    public static bool isRot;
    public PlayerMovement pm;
   

    void Start()
    {
        isRot = false;
        rot = transform.rotation.eulerAngles.y;
        initRot = transform.rotation.eulerAngles;
        initRotPlayer = pm.transform.rotation.eulerAngles;
        
        side = 0;
    }

    float angle;
   
    void Update()
    {
        
        if (Mathf.Abs(side) == 4) side = 0;

        if (Input.GetKeyDown(KeyCode.A))
        {
            rot += 90; side--;//Debug.Log("SIDE: "+side);
            initRotPlayer.y += 90;
            //pm.castRays();
            //if (PlayerMovement.disPlaced) {
            //    isRot = true; Invoke("playerReset", 0.1f);

            //}
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rot -= 90; side++; //Debug.Log("SIDE: " + side);
            initRotPlayer.y -= 90;
            //pm.castRays();
            //if (PlayerMovement.disPlaced) {
            //   isRot = true; Invoke("playerReset", 0.1f);
            //}

        }

        initRot.y = rot;

       // Debug.Log("SIDE: "+side);
        transform.rotation = Quaternion.Euler(initRot);
        
        pont.transform.rotation = transform.rotation;
        //pm.transform.rotation = Quaternion.Euler(initRotPlayer);
         StartCoroutine(PlayerRotateWithDelay());
    }

    IEnumerator PlayerRotateWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        pm.transform.rotation = Quaternion.Euler(initRotPlayer);
    }

    void playerReset()
    {
       // PlayerMovement.disPlaced = false;
    }
       
}
