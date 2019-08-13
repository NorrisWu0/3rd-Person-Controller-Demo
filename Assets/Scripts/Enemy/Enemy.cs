using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Basic Setting")]
    public bool m_IsDead;
    public float m_PatrolSpeed;
    public float m_ChaseSpeed;
    public float m_ChaseRange;
    public float m_DetectionRange;
    public float m_AttackRange;
    public Vector3 m_OriginPos;
    public Vector3 m_RandomPos;

    private Animator m_Animator;

    public NavMeshAgent m_NavAgent;


    void Start()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_OriginPos = transform.position;

        StartCoroutine("Idle");
    }

    #region Enemy Function

    #region Idle
    IEnumerator Idle()
    {
        m_NavAgent.speed = m_PatrolSpeed;

        while (m_Distance > m_DetectionRange)
        {
            #region Set Patrol Destination
            m_RandomPos = m_OriginPos + (Random.insideUnitSphere * 5);
            m_RandomPos.y = 0;
            m_NavAgent.stoppingDistance = 0;
            m_NavAgent.SetDestination(m_RandomPos);
            #endregion

            #region While on patrol
            while (m_NavAgent.remainingDistance > 0.1f && m_Distance > m_DetectionRange)
            {
                yield return null;
            }
            #endregion

            #region While on patrol destination
            float _idleTimer = Random.Range(1f, 3f);
            while (_idleTimer > 0 && m_Distance > m_DetectionRange)
            {
                _idleTimer -= Time.deltaTime;
                yield return null;
            }
            #endregion
        }

        StartCoroutine("FollowPlayer");
    }
    #endregion

    #region Follow Player
    IEnumerator FollowPlayer()
    {
        m_NavAgent.speed = m_ChaseSpeed;

        while (!m_IsDead && m_Distance < m_ChaseRange)
        {
            #region In Attack Range
            while (m_Distance < m_AttackRange)
            {
                m_NavAgent.isStopped = true;
                #region Do attack
                #endregion
                yield return new WaitForSeconds(0.1f);
            }
            #endregion

            #region Out Attack Range
            while (m_Distance > m_AttackRange && m_Distance < m_ChaseRange)
            {
                m_NavAgent.isStopped = false;
                m_NavAgent.SetDestination(m_PlayerPos);
                yield return new WaitForSeconds(0.1f);
            }
            #endregion

            yield return null;
        }

        StartCoroutine("Idle");
    }
    #endregion

    #endregion

    #region Smart Variable Function
    float m_Distance
    {
        get { return Vector3.Distance(transform.position, m_PlayerPos); }
        set { m_Distance = value; }
    }

    Vector3 m_PlayerPos
    {
        get { return Vector3.zero; }
        set { m_PlayerPos = value; }
    }
    #endregion

    #region Debug Function
    private void OnDrawGizmosSelected()
    {
        // Attack Range Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);

        // Detection Range Gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_DetectionRange);

        // Chase Range Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_ChaseRange);

        // Random Destination Gizmo
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_RandomPos, .5f);

    }
    #endregion
}
