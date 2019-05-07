using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 40;
    public GameObject explosion;
    private AudioSource audio;

    public Health()
    {

    }

    public void Hit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        if(gameObject.tag == "carrier")
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            audio = GetComponent<AudioSource>();
            Vector3 v = transform.GetComponent<Boid>().velocity;
            GameObject e = Instantiate(explosion, transform.position, Quaternion.identity);
            audio.Play(0);
            Destroy(e, 3f);
            transform.GetComponent<TrailRenderer>().enabled = true;
            SteeringBehaviour[] behaviours = GetComponents<SteeringBehaviour>();
            foreach (SteeringBehaviour b in behaviours)
            {
                b.enabled = false;
            }
            transform.GetComponent<StateMachine>().End();
            transform.GetComponent<PilotController>().enabled = false;
            transform.GetComponent<Boid>().enabled = false;
            gameObject.layer = 0;

            Destroy(transform.gameObject, 1f);
        }
    }
}
