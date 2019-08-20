using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAnimationListener : MonoBehaviour
{
    [SerializeField] Collider m_SwordCollider;

    [SerializeField] AudioClip[] m_WalkSFXs;
    [SerializeField] AudioClip[] m_RunSFXs;
    [SerializeField] AudioClip[] m_HitReactionSFXs;

    void ToggleHitbox(float _multiplier)
    {
        if (!m_SwordCollider.enabled)
            m_SwordCollider.GetComponent<SwordOneHand>().damageMultiplier = _multiplier;
        m_SwordCollider.enabled = !m_SwordCollider.enabled;
    }

    void PlayWalkSFX()
    {
        AudioSource.PlayClipAtPoint(GetRandomSFX(m_WalkSFXs), transform.position);
    }

    void PlayRunSFX()
    {
        AudioSource.PlayClipAtPoint(GetRandomSFX(m_RunSFXs), transform.position);
    }

    void PlayHitReactionSFX()
    {
        AudioSource.PlayClipAtPoint(GetRandomSFX(m_HitReactionSFXs), transform.position);
    }

    AudioClip GetRandomSFX(AudioClip[] _clips)
    {
        return _clips[Random.Range(0, _clips.Length)];
    } 


}
