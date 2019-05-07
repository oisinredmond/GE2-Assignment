using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour
{
    public GameObject bomb;

    public void Drop()
    {
        GameObject b = Instantiate(bomb, transform.position, transform.rotation);
    }
}
