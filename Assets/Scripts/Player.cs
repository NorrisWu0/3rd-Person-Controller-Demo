using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Basic Setting
    [Header("Basic Setting")]
    [Space(5)]
    [SerializeField] float m_Health;

    public static Player instance;
    Rigidbody m_RB;
    Animator m_Animator;
    #endregion

    #region Movement Setting
    [Header("Movement Setting")]
    [Space(5)]
    [SerializeField] float m_CamRotationFollowSpeed;
    [SerializeField] float m_WalkSpeedModifier;
    [SerializeField] float m_RunSpeedModifier;
    [SerializeField] float m_SprintSpeedModifier;
    [SerializeField] float m_JumpForce;
    [SerializeField] float m_RayDistance;
    [SerializeField] LayerMask m_LayerMask;

    Vector3 m_Movement;
    float m_MoveHorizontal, m_MoveVertical;
    bool m_IsCrouching = false; 

    [SerializeField] Transform m_ReferencePoint;


    #endregion

    #region Weapon Setting
    [Header("Weapon Setting")]
    [Space(5)]
    [SerializeField] bool m_IsBlocking;
    [SerializeField] bool m_IsAttacking;
    [SerializeField] Vector3 m_BondOffset;
    [SerializeField] Transform m_DebugReferencePoint;

    Transform m_ChestBone;

    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        m_ChestBone = m_Animator.GetBoneTransform(HumanBodyBones.Chest);
    }

    // First update that get called in the flow
    private void FixedUpdate()
    {
        #region Switch between Walk, Run and Sprint
        if (Input.GetKey(KeyCode.LeftShift))
            Move(m_SprintSpeedModifier);
        else if (Input.GetKey(KeyCode.LeftControl))
            Move(m_WalkSpeedModifier);
        else
            Move(m_RunSpeedModifier);
        #endregion

        #region Toggle Crouching
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_IsCrouching)
            {
                m_Animator.SetBool("IsCrouching", !m_IsCrouching);
                m_IsCrouching = !m_IsCrouching;
            }
            else
            {
                m_Animator.SetBool("IsCrouching", !m_IsCrouching);
                m_IsCrouching = !m_IsCrouching;
            }
        }
        #endregion

        // TODO
        if (CheckGround() && Input.GetKeyDown(KeyCode.Space))
            Jump();

        UpdateRefSphereTransform();
    }

    // Second update that get called in the flow
    private void Update()
    {
        #region Attack
        if (Input.GetMouseButton(0))
        {
            m_IsAttacking = true;
            m_Animator.SetBool("IsAttacking", m_IsAttacking);
        }
        else
        {
            m_IsAttacking = false;
            m_Animator.SetBool("IsAttacking", m_IsAttacking);
        }
        #endregion

        #region Block
        if (Input.GetMouseButton(1))
        {
            m_IsBlocking = true;
            m_Animator.SetBool("IsBlocking", m_IsBlocking);
        }
        else
        {
            m_IsBlocking = false;
            m_Animator.SetBool("IsBlocking", m_IsBlocking);
        }
        #endregion

    }

    // Last update that get called in the flow
    private void LateUpdate()
    {

    }


    #region Movement Function
    private void Move(float _speedMultiplier)
    {
        m_MoveHorizontal = Input.GetAxis("Horizontal") * _speedMultiplier;
        m_MoveVertical = Input.GetAxis("Vertical") * _speedMultiplier;

        #region Align Player to Camera Rotation
        if (m_RB.velocity.magnitude > 0.1f)
        {
            float _camRotationY = Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, _camRotationY, transform.eulerAngles.z), m_CamRotationFollowSpeed);
        }
        #endregion

        m_ReferencePoint.transform.localPosition = new Vector3(m_MoveHorizontal, 0, m_MoveVertical);
        m_Animator.transform.LookAt(m_ReferencePoint);

        m_Animator.SetFloat("Velocity", m_RB.velocity.magnitude);
        m_Animator.SetFloat("Forward", m_MoveVertical);
    }

    private void Jump()
    {
        m_RB.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
    }

    private bool CheckGround()
    {
        return Physics.Raycast(transform.position, -transform.up, m_RayDistance, m_LayerMask);
    }

    #endregion

    #region Weapon Function
    private void Aiming()
    {

    }
    #endregion

    #region Support Functions
    private void ToggleBoolean(ref bool _bool)
    {
        _bool = !_bool;
    }

    #endregion

    #region Debug Function
    private void UpdateRefSphereTransform()
    {
    }
    #endregion
}
