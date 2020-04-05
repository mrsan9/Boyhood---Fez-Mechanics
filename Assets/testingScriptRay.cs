using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingScriptRay : MonoBehaviour {

   [SerializeField] Transform rayObject;

    RaycastHit[] hits;

    // Update is called once per frame
    void Update()
    {
        hits = Physics.RaycastAll(rayObject.transform.position, rayObject.transform.forward,1000f);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.gameObject);
        }

        //if (Physics.Raycast(rayObject.transform.position, rayObject.transform.forward, out hit, 100f))
        //{
        //    Debug.Log(hit.collider.gameObject);
        //}
    }

}
