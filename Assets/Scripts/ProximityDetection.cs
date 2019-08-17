using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProximityDetection : MonoBehaviour
{
    [Header("Proximity Setting")]
    [SerializeField] float m_PatrolSpeed;
    [SerializeField] float m_ChaseSpeed;
    [SerializeField] float m_ChaseRange;
    [SerializeField] float m_DetectionRange;
    
    private float m_AttackRange;
    private Vector3 m_OriginPos;
    private Vector3 m_RandomPos;
    private Animator m_Animator;
    private NavMeshAgent m_NavAgent;

    // Start is called before the first frame update
    void Start()
    {
        m_OriginPos = transform.position;
        m_Animator = GetComponent<Animator>();
        m_NavAgent = GetComponent<NavMeshAgent>();

        StartCoroutine("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
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

        while (m_Distance < m_ChaseRange)
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

    #region DistanceToPlayer and PlayerPosition auto-property variables
    float m_Distance
    {
        get { return Vector3.Distance(transform.position, m_PlayerPos); }
        set { m_Distance = value; }
    }

    Vector3 m_PlayerPos
    {
        get { return Player.instance.transform.position; }
        set { m_PlayerPos = value; }
    }
    #endregion

    #region Debug Function
    private void OnDrawGizmosSelected()
    {
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
