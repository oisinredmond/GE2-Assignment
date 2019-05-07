using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag != "plane")
        {
            audio.Play(0);
            Debug.Log("Hit");
            GameObject e = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(e, 3f);
            Destroy(transform.parent.gameObject);
        }
    }
}
