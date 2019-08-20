using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOneHand : MonoBehaviour
{
    public float damageMultiplier;
    [SerializeField] float m_Damage;

    [SerializeField] AudioClip[] m_SwordHitSFXs;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_AudioSource.PlayOneShot(GetRandomSFX(m_SwordHitSFXs));
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Enemy>().TakeDamage(m_Damage * damageMultiplier);
        }
    }

    AudioClip GetRandomSFX(AudioClip[] _clips)
    {
        return _clips[Random.Range(0, _clips.Length)];
    }

}