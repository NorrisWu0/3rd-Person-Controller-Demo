using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Basic Setting")]
    [SerializeField] bool m_IsDead;
    [SerializeField] float m_Health;
    [SerializeField] float m_TotalHealth;
    [SerializeField] Canvas m_OverheadUI;
    [SerializeField] Slider m_HealthBar;

    [Header("Behavior Setting")]
    [SerializeField] bool m_IsWandering;
    [SerializeField] float m_WanderSpeed;
    [SerializeField] float m_WanderStoppingDistance;
    [Space(5)]
    [SerializeField] bool m_IsChasing;
    [SerializeField] float m_ChaseSpeed;
    [SerializeField] float m_AttackRange;

    [Header("Attack Setting")]
    public Transform m_Target;
    [SerializeField] FieldOfView m_FOV1;
    [SerializeField] FieldOfView m_FOV2;
    [SerializeField] bool m_IsAttacking;

    [Header("DeathFX Setting")]
    [SerializeField] Dissolve m_Joints;
    [SerializeField] Dissolve m_Surface;
    [SerializeField] float m_DeathTimer;


    private Animator m_Animator;
    private NavMeshAgent m_NavAgent;
    private Vector3 m_OriginPos;
    private Vector3 m_RandomPos;

    void Start()
    {
        m_TotalHealth = m_Health;
        m_Animator = GetComponent<Animator>();
        m_NavAgent = GetComponent<NavMeshAgent>();

        UpdateUIStats();
        StartCoroutine("Wander");
    }

    void Update()
    {
        if (!m_IsDead)
            Move();
        else
            Destroy(this.gameObject, m_DeathTimer);

        #region Rotate OverheadUI towards Camera
        m_OverheadUI.transform.LookAt(Camera.main.transform.position);
        #endregion

        #region Ugly code but damn it I don't have enought time to figure out a better way for this!
        if (m_FOV1.target != null)
            m_Target = m_FOV1.target;
        else if (m_FOV2.target != null)
            m_Target = m_FOV2.target;
        else
            m_Target = null;
        #endregion
    }

    void OnAnimatorMove()
    {
        m_NavAgent.velocity = m_Animator.deltaPosition / Time.deltaTime;
    }

    #region Update UI Stats
    void UpdateUIStats()
    {
        #region Update Health Bar
        float _value = m_Health / m_TotalHealth;
        m_HealthBar.value = _value;
        #endregion
    }
    #endregion

    #region TakeDamage Function
    public void TakeDamage(float _damage)
    {
        m_Health -= _damage;
        UpdateUIStats();
        StartCoroutine("StartHitAnimation");

        if (m_Health <= 0)
        {
            m_IsDead = true;
            m_Animator.SetBool("IsDead", true);
        }
    }
    #endregion

    #region Start Hit Rection Animation
    IEnumerator StartHitAnimation()
    {
        Time.timeScale = 0.2f;
        m_Animator.SetBool("IsHit", true);

        yield return new WaitForSecondsRealtime(0.3f);
        
        Time.timeScale = 1;
        m_Animator.SetBool("IsHit", false);
    }
    #endregion

    #region Wander Coroutine
    IEnumerator Wander()
    {
        Debug.Log("In Wander Mode");
        m_IsWandering = true;
        m_OriginPos = transform.position;
        m_NavAgent.speed = m_WanderSpeed;
        m_NavAgent.stoppingDistance = m_WanderStoppingDistance;

        while (m_Target == null)
        {
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

    #region ChaseTarget Coroutine
    IEnumerator ChaseTarget()
    {
        Debug.Log("In Chase Mode");
        m_IsChasing = true;
        m_NavAgent.speed = m_ChaseSpeed;
        m_NavAgent.stoppingDistance = m_AttackRange;

        while (m_Target != null)
        {
            m_NavAgent.SetDestination(m_Target.position);

            if (Vector3.Distance(transform.position, m_Target.position) < m_AttackRange)
                m_IsAttacking = true;
            else
                m_IsAttacking = false;

            m_Animator.SetBool("IsAttacking", m_IsAttacking);
            yield return null;
        }
        
        while (m_NavAgent.remainingDistance > m_NavAgent.stoppingDistance)
        {
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

    #region Debug Functions
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
