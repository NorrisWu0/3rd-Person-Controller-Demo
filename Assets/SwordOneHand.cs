using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOneHand : MonoBehaviour
{
    public float damageMultiplier;
    [SerializeField] float m_Damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Enemy>().TakeDamage(m_Damage * damageMultiplier);
        }
    }
}