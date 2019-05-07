using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBomb : MonoBehaviour
{
    public GameObject par;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "bomb")
        {
            transform.parent.GetComponent<Health>().Hit(100);
        }
    }
}
