﻿using System.Collections;
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
    public Player pm;
    public Material sky;

    void Start()
    {
        isRot = false;
        rot = transform.rotation.eulerAngles.y;
        initRot = transform.rotation.eulerAngles;
        initRotPlayer = pm.transform.rotation.eulerAngles;
        
        side = 0;
    }

    float angle;

    float time;
    public float skyRotSpeed=1f;
    void Update()
    {

        time += Time.deltaTime;
        if (time >= 360) time = 0f;

        sky.SetFloat("_Rotation",time*skyRotSpeed);

        if (Mathf.Abs(side) == 4) side = 0;

        if (pm.onGround) {
           
        if (Input.GetKeyDown(KeyCode.A))
        {
            rot += 90; side--;//Debug.Log("SIDE: "+side);
            if (side == -2) side = 2;
            if (side == -3) side = 1;
            if (side == -1) side = 3;
                Player.isJumping = false;

                initRotPlayer.y += 90;
            StartCoroutine(PlayerRotateWithDelay());
            //pm.castRays();
            //if (PlayerMovement.disPlaced) {
            //    isRot = true; Invoke("playerReset", 0.1f);

            //}
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rot -= 90; side++; //Debug.Log("SIDE: " + side);

                Player.isJumping = false;
                initRotPlayer.y -= 90;
            StartCoroutine(PlayerRotateWithDelay());
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
        }
    }

    IEnumerator PlayerRotateWithDelay()
    {
        yield return new WaitForSeconds(0.15f);
        pm.transform.rotation = Quaternion.Euler(initRotPlayer);
    }

    void playerReset()
    {
       // PlayerMovement.disPlaced = false;
    }
       
}
