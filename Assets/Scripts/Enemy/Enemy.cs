using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Basic Setting")]
    [SerializeField] bool m_IsDead;
    [SerializeField] float m_Health;

    [Header("Behavior Setting")]
    [SerializeField] bool m_IsWandering;
    [SerializeField] float m_WanderSpeed;
    [SerializeField] float m_WanderStoppingDistance;
    [Space(5)]
    [SerializeField] bool m_IsChasing;
    [SerializeField] float m_ChaseSpeed;
    [SerializeField] float m_ChaseStoppingDistance;

    [Header("Attack Setting")]
    public Transform m_Target;
    [SerializeField] FieldOfView m_FOV1;
    [SerializeField] FieldOfView m_FOV2;
    [SerializeField] bool m_IsAttacking;
    [SerializeField] float m_AttackRange;


    private Animator m_Animator;
    private NavMeshAgent m_NavAgent;
    private Vector3 m_OriginPos;
    private Vector3 m_RandomPos;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_NavAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("Wander");
    }

    void Update()
    {
        Move();

        #region Ugly code but damn it I don't have enought time to figure out a better way for this!
        if (m_FOV1.target != null)
            m_Target = m_FOV1.target;
        else if (m_FOV2.target != null)
            m_Target = m_FOV2.target;
        else
            m_Target = null;
        #endregion

        if (m_Target == null)
            Debug.Log("No target.");
        else
            Debug.Log(m_Target);

    }

    void OnAnimatorMove()
    {
        m_NavAgent.velocity = m_Animator.deltaPosition / Time.deltaTime;
    }

    #region Wander Function
    IEnumerator Wander()
    {
        m_IsWandering = true;
        m_OriginPos = transform.position;
        m_NavAgent.speed = m_WanderSpeed;
        m_NavAgent.stoppingDistance = m_WanderStoppingDistance;

        while (m_Target == null)
        {
            Debug.Log("In Wander Mode");
            #region Set Patrol Destination
            m_RandomPos = m_OriginPos + (Random.insideUnitSphere * 5);
            m_RandomPos.y = 0;
            m_NavAgent.SetDestination(m_RandomPos);
            #endregion

            #region While on patrol
            while (m_NavAgent.remainingDistance > m_NavAgent.stoppingDistance && m_Target == null)
            {
                yield return null;
            }
            #endregion

            #region While on patrol destination
            float _idleTimer = Random.Range(3f, 5f);
            while (_idleTimer > 0 && m_NavAgent.remainingDistance < m_NavAgent.stoppingDistance && m_Target == null)
            {
                _idleTimer -= Time.deltaTime;
                yield return null;
            }
            #endregion
        }

        m_IsWandering = false;
        StartCoroutine("ChaseTarget");
    }
    #endregion

    #region Chase Target
    IEnumerator ChaseTarget()
    {
        m_IsChasing = true;
        m_NavAgent.speed = m_ChaseSpeed;
        m_NavAgent.stoppingDistance = m_ChaseStoppingDistance;

        while (m_Target != null)
        {
            Debug.Log("In Chase Mode");
            m_NavAgent.SetDestination(m_Target.position);

            yield return null;
        }

        ClearLog();
        Debug.Log("Lost Target!");
        
        while (m_NavAgent.remainingDistance > m_NavAgent.stoppingDistance)
        {
            Debug.Log(m_NavAgent.remainingDistance + "/" + m_NavAgent.stoppingDistance);
            yield return null;
        }

        m_IsChasing = false;
        StartCoroutine("Wander");
    }
    #endregion

    #region Move Function
    void Move()
    {
        float _speed = m_NavAgent.desiredVelocity.magnitude;
        m_Animator.SetFloat("MotionMagnitude", _speed, 0.1f, Time.deltaTime);

        #region Rotate towards target
        Vector3 _desiredMoveDirection = m_NavAgent.destination - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_desiredMoveDirection), 0.1f);
        #endregion

    }
    #endregion

    #region Debug Function
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    private void OnDrawGizmosSelected()
    {
        // Attack Range Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        
        // Random Destination Gizmo
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_RandomPos, .5f);

        // NavAgent Destination Gizmo
        if (m_NavAgent != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_NavAgent.destination, .5f);
        }
    }
    #endregion
}
