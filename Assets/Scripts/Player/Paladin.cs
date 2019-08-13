using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Player {

    [Header("Weapon Setting", order = 3)]
    [SerializeField] bool m_IsAttacking;
    [SerializeField] GameObject m_SwordTrailVFX;

    private enum m_AttackType {LightAttack, HeavyAttack};

    private new void Update()
    {
        base.Update();

        #region Blocking
        if (Input.GetKey(KeyCode.Space))
            m_Animator.SetBool("IsBlocking", true);
        else
            m_Animator.SetBool("IsBlocking", false);
        #endregion

        #region Attack
        if (Input.GetMouseButtonDown(0))
            Attack(m_AttackType.LightAttack);
        else if (Input.GetMouseButtonDown(1))
            Attack(m_AttackType.HeavyAttack);
        #endregion

        #region Toggle Sword VFX - TODO
        if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
        {
            m_IsAttacking = true;
            m_SwordTrailVFX.SetActive(m_IsAttacking);
        }
        else
        {
            m_IsAttacking = false;
            m_SwordTrailVFX.SetActive(m_IsAttacking);
        }
        #endregion

    }
    
    void Attack(m_AttackType _attackType)
    {
        switch (_attackType)
        {
            case m_AttackType.LightAttack:
                m_Animator.SetInteger("LightAttackCombo", m_Animator.GetInteger("LightAttackCombo") + 1);
                break;

            case m_AttackType.HeavyAttack:
                m_Animator.SetInteger("HeavyAttackCombo", m_Animator.GetInteger("HeavyAttackCombo") + 1);
                break;

            default:
                Debug.Log("Incorrect Attack Type");
                break;
        }
    }

}
