using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationListener : MonoBehaviour
{
    [SerializeField] Collider m_Hitbox;

    [SerializeField] AudioClip[] m_WalkSFXs;
    [SerializeField] AudioClip[] m_RunSFXs;
    [SerializeField] AudioClip[] m_HitReactionSFXs;

    void ToggleHitbox(float _multiplier)
    {
        m_Hitbox.enabled = !m_Hitbox.enabled;
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
