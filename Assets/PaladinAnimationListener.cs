using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAnimationListener : MonoBehaviour
{
    [SerializeField] Collider m_SwordCollider;

    void ToggleHitbox(float _multiplier)
    {
        if (!m_SwordCollider.enabled)
            m_SwordCollider.GetComponent<SwordOneHand>().damageMultiplier = _multiplier;
        m_SwordCollider.enabled = !m_SwordCollider.enabled;
    }
}
