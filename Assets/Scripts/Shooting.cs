using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float bulletSpeed = 1500f;
    public Rigidbody bullet;
    private int i = 0;
    private int l;
    private List<GameObject> guns = new List<GameObject>();
    private AudioSource audio;
    private Rigidbody rb;
    private string tag;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        guns.Add(this.transform.GetChild(0).gameObject);
        guns.Add(this.transform.GetChild(1).gameObject);
        if (gameObject.layer == LayerMask.NameToLayer("US"))
        {
            l = LayerMask.NameToLayer("JP");
        }
        else if(gameObject.layer == LayerMask.NameToLayer("JP"))
        {
            l = LayerMask.NameToLayer("US");
        }
    }

    

    public void Shoot()
    {
        RaycastHit hit;
        Rigidbody b = (Rigidbody)Instantiate(bullet, guns[i].transform.position, guns[i].transform.rotation);
        b.velocity = (guns[i].transform.forward * bulletSpeed) + rb.velocity;
        audio.Play(0);
        if (i == 1)
        {
            i = 0;
        }
        else
        {
            i++;
        }
        if(Physics.Raycast(guns[i].transform.position, guns[i].transform.forward, out hit, Mathf.Infinity, 1 << l))
        {
            hit.collider.transform.parent.GetComponent<Health>().Hit(4);
        }
    }
}
