using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviour
{
    public float radius = 75f;
    public float distance = 0f;
    public float e = 0f;
    public float n = 0f;
    private float t = 5f;
    public float interval = 5f;

    Vector3 target;
    Vector3 prevTarget;
    Vector3 worldTarget;
    Vector3 currentTarget;
    Vector3 rot;
    Collider boundary;

    public void Start()
    {
        prevTarget = transform.position + transform.forward * distance;
        NextTarget();
    }

    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Vector3 localCP = (transform.forward * distance);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z  = 0;

        Vector3 worldCP = transform.position + (Quaternion.Euler(rot) * localCP);
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(worldCP, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldTarget, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, worldTarget);

    }

    // Update is called once per frame
    public override Vector3 Calculate()
    {
        t += Time.deltaTime;

        if (t >= interval)
        {
            prevTarget = worldTarget;
            t = 0;
            NextTarget();
        }

        rot = transform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 0;

        Vector3 localTarget = target + (transform.forward * distance);
        worldTarget = transform.position + Quaternion.Euler(rot) * localTarget;

        if (worldTarget.y < 25f)
        {
            worldTarget.y = 35f;
        }
        else if(worldTarget.y > 400f)
        {
            worldTarget.y = 370f;
        }

        if(worldTarget.x > 600)
        {
            worldTarget.x = 580f;
        }
        else if (worldTarget.x < -600f)
        {
            worldTarget.x = -580f;
        }

        if(worldTarget.z > 800f)
        {
            worldTarget.z = 780f;
        }
        else if(worldTarget.z < -800f)
        {
            worldTarget.z = -780f;
        }

        currentTarget = Vector3.Lerp(prevTarget, worldTarget, t / interval);
        return boid.SeekForce(currentTarget);
    }

    public void NextTarget()
    {
        n = Random.Range(0f, 180f);
        e = Random.Range(-30f, 30f);
        if (e < 0)
        {
            e += 360f;
        }

        if (n > 145 || n < 45)
        {
            e = 0;
        }

        float angle = n * Mathf.Deg2Rad;
        float elevation = e * Mathf.Deg2Rad;


        target.x = Mathf.Cos(angle) * Mathf.Cos(elevation) * radius;
        target.y = Mathf.Sin(elevation) * radius;
        target.z = Mathf.Cos(elevation) * Mathf.Sin(angle) * radius;
    }
}
