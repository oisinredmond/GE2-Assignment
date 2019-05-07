using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    RaycastHit h;
    public override void Enter()
    {
        owner.GetComponent<Wander>().enabled = true;
    }

    public override void Think()
    {
            Collider[] visColliders = Physics.OverlapSphere(owner.transform.position, Mathf.Infinity, owner.enemyMask);
            foreach (Collider col in visColliders)
            {
                Vector3 target = col.transform.position - owner.transform.position;
                float dist = Vector3.Distance(col.transform.position, owner.transform.position);
                float angle = Vector3.Angle(target, owner.transform.forward);

                if (angle < 60)
                {
                    if (col.tag == "carrier" && dist > 400)
                    {
                        owner.enemy = col.transform.parent;
                        owner.GetComponent<Wander>().enabled = false;
                        owner.ChangeState(new CarrierAttackState());
                    }
                    else if (col.tag == "plane" && !col.transform.parent.GetComponent<StateMachine>().beingChased
                        && dist < 500 && col.transform.parent != owner.enemy)
                    {
                        col.transform.parent.GetComponent<StateMachine>().beingChased = true;
                        owner.enemy = col.transform.parent;
                        owner.GetComponent<Wander>().enabled = false;
                        owner.ChangeState(new AttackState());
                    }
                }
            }
    }

    public override void Exit()
    {
        owner.chaser = null;
        owner.GetComponent<Wander>().enabled = false;
    }
}


public class AttackState : State
{
    private float timer;

    public override void Enter()
    {
        owner.GetComponent<Pursue>().target = owner.enemy;
        owner.GetComponent<Pursue>().enabled = true;
    }

    public override void Think()
    {
        if(owner.enemy.transform.GetComponent<Health>().health <= 0)
        {
            owner.enemy = null;
            owner.ChangeState(new WanderState());
        }
    }

    public override void Exit()
    {
        owner.GetComponent<Pursue>().enabled = false;
    }
}

public class CarrierAttackState : AttackState
{
    bool dropped;
    RaycastHit h;

    public override void Enter()
    {
        dropped = false;
        owner.GetComponent<ObstacleAvoidance>().weight *= 2;
        base.Enter();
    }

    public override void Think()
    {
        Vector3 target = owner.enemy.transform.position - owner.transform.position;
        float dist = Vector3.Distance(owner.enemy.transform.position, owner.transform.position);
        if (Vector3.Angle(target, owner.transform.forward) > 15 && dist < 200)
        {
            owner.ChangeState(new WanderState());
        }

        if (Physics.Raycast(owner.transform.position, -Vector3.up, out h, Mathf.Infinity, owner.enemyMask) 
            && h.collider.tag == "carrier" && !dropped)
        {
            owner.GetComponent<DropBomb>().Drop();
            dropped = true;
        }

        base.Think();
    }

    public override void Exit()
    {
        owner.GetComponent<ObstacleAvoidance>().weight /= 2;
        base.Exit();
    }
}


public class PilotController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new WanderState());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void EnterFlee(Boid t)
    //{
       // GetComponent<StateMachine>().ChangeState(new FleeState(t));
    //}
}