using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationListener : MonoBehaviour
{
    [SerializeField] Collider m_Hitbox;

    void ToggleHitbox(int _value)
    {
        m_Hitbox.enabled = !m_Hitbox.enabled;
    }
}
