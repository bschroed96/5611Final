using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    private float AIAttackTimer;
    private Vector3 patrolGoal;
    private bool isAttacking;
    private bool patrol;
    public override void Start() {
        base.Start();
        AIAttackTimer = unitStats.attackSpeed;
        patrolGoal = this.transform.position;
        patrolGoal.x += 20;
        isAttacking = false;
        patrol = true;
    }

    public override void Update() {
        base.Update();
        AIAttackTimer += Time.deltaTime;
        checkRadius();
        if (!isAttacking) {
            // Debug.Log("Patrolling");
            Patrol();
        }
    }

    public void checkRadius() {
        isAttacking = false;
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("PlayerUnit"))
        {
            // var distance = (unit.transform.position - hit.transform.position).magnitude;
            var dist = (this.transform.position - unit.transform.position).magnitude;
            if (dist < 30)
            {
                //Debug.Log("Distance is less than 25");
                SetNewTarget(unit.transform);
                this.AIAttack(unit);
                isAttacking = true;
            }
        }
    }

    public void AIAttack(GameObject playerUnit) 
    {
        //Debug.Log(AIAttackTimer);
        if (AIAttackTimer >= unitStats.attackSpeed) {
            RTSGameManager.UnitTakeDamage(this, playerUnit.GetComponent<UnitController>());
            AIAttackTimer = 0;
            Debug.Log("AI IS ATTACKING!");
            var distance = (this.transform.position - playerUnit.transform.position).magnitude;
            //Debug.Log("Distance: " + distance);
            Debug.Log("AI Health: " + this.getHealth());
            //Debug.Log("Player unit health: " + playerUnit.GetComponent<UnitController>().getHealth());
        }
    }

    public void Patrol() {
        if (Vector3.Distance(patrolGoal, this.transform.position) <= 10) {
            if (patrol) {
                patrolGoal.x -= 30;
                patrol = !patrol;
            } else {
                patrolGoal.x += 30;
                patrol = !patrol;
            }
        }
        MoveUnit(patrolGoal);
    }

}