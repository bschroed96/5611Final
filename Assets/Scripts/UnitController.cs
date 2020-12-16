using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navAgent;
    private Transform currentTarget;
    private float attackTimer;
    private float health;
    public float attack;
    private float attackRange;
    public GameObject PlayerUnit;

    public UnitStats unitStats;

    public virtual void Start() {
        attackTimer = unitStats.attackSpeed;
        health = unitStats.health;
        attack = unitStats.attackDamage;
        attackRange = unitStats.attackRange;
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public virtual void Update() {
        attackTimer += Time.deltaTime;
        if (currentTarget != null ) {
            navAgent.destination = currentTarget.position;

            var distance = (transform.position - currentTarget.position).magnitude; // get lenght of vector
            if (distance <= attackRange && currentTarget.gameObject.tag == "EnemyUnit") {
                Attack();
            }
        }
    }

    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        //navAgent.destination = dest;
        navAgent.SetDestination(dest);
    }

    public void SetSelected(bool isSelected) 
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }

    public void SetNewTarget(Transform enemy) {
        currentTarget = enemy;
    }

    public void Attack() {
        if (attackTimer >= unitStats.attackSpeed) {
            RTSGameManager.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackTimer = 0;
        }
    }

    public void TakeDamage(UnitController enemy, float damage)
    {
        StartCoroutine(Flasher(GetComponent<Renderer>().material.color));
        health -= damage;
        // Debug.Log(damage);
        // Debug.Log(health);
        var rand = Random.Range(1,10);
        if (health <= 0 && rand > 7) {
            if (this.gameObject.tag == "EnemyUnit") {
                //Debug.Log("Enemy Unity is being killed");
                Vector3 pos = this.gameObject.transform.position;
                Destroy(this.gameObject);
                GameObject newObj = new GameObject("newUnit");
                Instantiate(PlayerUnit, pos, Quaternion.identity);
            }
        }
        else if (health <= 0){
                Destroy(this.gameObject);
            }
    }

    public void IncreaseDamage(GameObject powerup, float damage)
    {
        Destroy(powerup);
        this.attack += damage;
    }

    IEnumerator Flasher(Color defaultColor)
    {
        var renderer = GetComponent<Renderer>();
        for (int i = 0; i < 2; i++) {
            renderer.material.color = Color.gray;
            yield return new WaitForSeconds(.05f);
            renderer.material.color = defaultColor;
            yield return new WaitForSeconds(.05f);
        }
    }

    public float getHealth() {return health;}
    public void setHealth(float reset) { this.health = reset; }
    public void setNav(UnityEngine.AI.NavMeshAgent Agent) {navAgent = Agent;}
}
