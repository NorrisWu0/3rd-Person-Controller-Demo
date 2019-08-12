using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int m_WalkSpeed;
    public int m_RunSpeed;
    public float allowPlayerRotation;

    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;

    private float m_MoveHorizontal;
    private float m_MoveVertical;

    public static Player instance;
    private Vector3 m_MoveVector;
    private Animator m_Animator;
    private Camera m_MainCamera;
    

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_MainCamera = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            GetMovementInput(m_RunSpeed);
        else
            GetMovementInput(m_WalkSpeed);

        if (Input.GetKey(KeyCode.Space))
            m_Animator.SetBool("IsBlocking", true);
        else
            m_Animator.SetBool("IsBlocking", false);

        if (Input.GetMouseButtonDown(0))
            m_Animator.SetInteger("LightAttackCombo", m_Animator.GetInteger("LightAttackCombo") + 1);
        else if (Input.GetMouseButtonDown(1))
            m_Animator.SetInteger("HeavyAttackCombo", m_Animator.GetInteger("HeavyAttackCombo") + 1);
        

    }

    #region Movement Functions
    private void Move()
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

    private void GetMovementInput(int _speedMultiplier)
    {
        // Calculate input vector
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");

        // Imi fumei no Kodo desu
        //m_Animator.SetFloat("VelocityZ", m_MoveVertical);
        //m_Animator.SetFloat("VelocityX", m_MoveHorizontal);

        // Calculate input magniture - this is for controller
        speed = new Vector2(m_MoveHorizontal, m_MoveVertical).SqrMagnitude();

        if (speed > allowPlayerRotation)
            Move();

        m_Animator.SetFloat("InputMagnitute", speed * _speedMultiplier);
    }
    #endregion

    #region Attack
    private void Attack()
    {
        m_Animator.SetInteger("LightAttackCombo", m_Animator.GetInteger("LightAttackCombo") + 1);
    }

    #endregion

}
