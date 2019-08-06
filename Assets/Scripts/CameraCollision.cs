using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] float m_Distance;
    [SerializeField] float m_MinDistance;
    [SerializeField] float m_MaxDistance;

    [SerializeField] float m_Smooth;
    [SerializeField] Vector3 m_DollyDirectionAdjusted;
    
    Vector3 m_DollyDirection;

    private void Awake()
    {
        m_DollyDirection = transform.localPosition.normalized;
        m_Distance = transform.localPosition.magnitude;
    }


    void Update()
    {
        Vector3 _desiredCameraPos = transform.parent.TransformPoint(m_DollyDirection * m_MaxDistance);
        RaycastHit _hit;

        if (Physics.Linecast(transform.parent.position, _desiredCameraPos, out _hit))
            m_Distance = Mathf.Clamp((_hit.distance) * 0.9f, m_MinDistance, m_MaxDistance);
        else
            m_Distance = m_MaxDistance;

        transform.localPosition = Vector3.Lerp(transform.localPosition, m_DollyDirection * m_Distance, Time.deltaTime * m_Smooth);

    }
}
