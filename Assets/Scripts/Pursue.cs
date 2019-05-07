using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
    public Transform target = null;
    public Vector3 targetPos;
    public GameObject guns;
    public LayerMask layer = -1;
    public float interval = 0.1f;
    private float t = 0.0f;
    private Vector3 v;

    public void Start()
    {

    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying && isActiveAndEnabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

    public override Vector3 Calculate()
    {
        if (target.tag == "plane")
        {
            v = target.GetComponent<Boid>().velocity;
        }
        else
        {
            v = Vector3.zero;
        }
        float dist = Vector3.Distance(target.transform.position, transform.position);
        float time = dist / boid.maxSpeed;

        targetPos = target.transform.position + (v * time);
        return boid.SeekForce(targetPos);
    }

    public void Update()
    {
        t += Time.deltaTime * 5;

        if (Vector3.Angle(transform.forward, target.transform.position - transform.position) < 20 && t >= interval)
        {
            t = 0;
            guns.GetComponent<Shooting>().Shoot();
        }
    }
}
