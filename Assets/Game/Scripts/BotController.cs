using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BotController : MonoBehaviour
{
    //Infor Bot
    public int id;
    public float enemyMaxHealth = 100;
    public float currentHealth;
    //public int damageSlash = 2;
    public float attackRadius = 4;
    //
    string currentAnimName;
    public Animator anim;
    [SerializeField] Rigidbody rb;
    //
    public NavMeshAgent agent;
    public float range;
    public Vector3 randomPoint;
    //
    public int score;
    [SerializeField] Transform Target;
    public float chaseRadius = 12;
    private IState currentState;
    [SerializeField] Weapons weaponsRange;
    public IState CurrentState { get => currentState; set => currentState = value; }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        //Debug.Log(currentState);
    }
    public void OnInit()
    {
        ChangeState(new IdleState());
        currentHealth = enemyMaxHealth;
    }
    public void SetRandomTargetFollow()
    {
        ChangeAnim(Constant.ANIM_WALK, true);
        ChangeAnim(Constant.ANIM_ATTACK, false);
        agent.isStopped = false;
        randomPoint = GetRandomPointOnNavMesh(transform.position, range);
        agent.SetDestination(randomPoint);
    }
    Vector3 GetRandomPointOnNavMesh(Vector3 origin, float distance)
    {
        transform.DOLookAt(Target.position, 1f);
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        randomDirection.Normalize();

        Vector3 randomPoint = origin + randomDirection * Random.Range(0f, distance);

        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomPoint, out navMeshHit, distance, NavMesh.AllAreas);

        return navMeshHit.position;
    }
    public void stopMoving()
    {
        agent.isStopped = true;
    }
    public void FollowTarget()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        if (distance < chaseRadius)
        {
            agent.SetDestination(Target.position);
        }
    }
    public bool IsHaveTargetInRange()
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        //Debug.Log(distance);
        if (distance < attackRadius)
        {
            return true;
        }
        return false;
    }
    public void Attack()
    {
        //Debug.Log("attack");
        ChangeAnim(Constant.ANIM_ATTACK, true);
        if (id == 1)
        {
            weaponsRange.Use();
        }
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            //die
            StartCoroutine(OnDeath());
        }
    }
    public IEnumerator OnDeath()
    {
        stopMoving();
        ChangeAnim(Constant.ANIM_DIE, true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        SettingsPanel.Instance.IncreaseScore(score);
    }
    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }
    public void ChangeAnim(string animName,bool isChange)
    {
        anim.SetBool(animName, isChange);
    }
}
