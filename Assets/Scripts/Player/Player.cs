using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Basic Setting", order = 1)]
    [SerializeField] bool m_IsDead;
    [SerializeField] float m_Health;
    private float m_TotalHealth;

    [Header("UI Setting")]
    [SerializeField] Slider m_HealthBar;

    [Header("Movement Setting", order = 2)]
    [SerializeField] float m_CurrentSpeed;
    [SerializeField] int m_WalkSpeed;
    [SerializeField] int m_RunSpeed;
    public float allowPlayerRotation;
    private float m_MoveHorizontal;
    private float m_MoveVertical;
    
    #region Todo
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    #endregion


    public static Player instance;
    protected Animator m_Animator;
    protected Camera m_MainCamera;
    

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        m_TotalHealth = m_Health;
        m_Animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;

        UpdateUI();

    }
    
    protected void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            GetMovementInput(m_RunSpeed);
        else
            GetMovementInput(m_WalkSpeed);
    }

    #region Take Damage
    public void TakeDamage(float _damage)
    {

        m_Health -= _damage;
        UpdateUI();
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
        m_Animator.SetBool("IsHit", true);
        yield return null;
        m_Animator.SetBool("IsHit", false);
    }
    #endregion

    #region Update UI
    void UpdateUI()
    {
        #region Update Health Bar
        float _value = (m_Health / m_TotalHealth);
        m_HealthBar.value = _value;
        #endregion
    }
    #endregion

    #region Movement Functions
    protected void Move()
    {
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");

        Vector3 _camForward = m_MainCamera.transform.forward;
        Vector3 _camRight = m_MainCamera.transform.right;

        _camForward.y = 0f;
        _camRight.y = 0f;

        _camForward.Normalize();
        _camRight.Normalize();

        desiredMoveDirection = _camForward * m_MoveVertical + _camRight * m_MoveHorizontal;

        if (!blockRotationPlayer)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    protected void GetMovementInput(int _speedMultiplier)
    {
        // Calculate input vector
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");

        // Imi fumei no Kodo desu
        //m_Animator.SetFloat("VelocityZ", m_MoveVertical);
        //m_Animator.SetFloat("VelocityX", m_MoveHorizontal);

        // Calculate input magniture & smooth running and walking
        m_CurrentSpeed = Mathf.Lerp(m_CurrentSpeed, new Vector2(m_MoveHorizontal, m_MoveVertical).SqrMagnitude() * _speedMultiplier, 0.1f);

        if (m_CurrentSpeed > allowPlayerRotation)
            Move();

        m_Animator.SetFloat("InputMagnitute", m_CurrentSpeed);
    }
    #endregion

    #region Attack
    private void Attack()
    {
        m_Animator.SetInteger("LightAttackCombo", m_Animator.GetInteger("LightAttackCombo") + 1);
    }

    #endregion

}
