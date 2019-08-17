using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform target;

    private Enemy m_Enemy;

    private void Start()
    {
        m_Enemy = GetComponent<Enemy>();
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float _delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            FindVisiableTargets();
        }
    }

    void FindVisiableTargets()
    {
        target = null;
        Collider[] _targetInView = Physics.OverlapSphere(transform.position, radius, targetMask);

        for (int i = 0; i < _targetInView.Length; i++)
        {
            Transform _target = _targetInView[i].transform;
            Vector3 _dirToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, _dirToTarget) < angle / 2)
            {
                float _dstToTarget = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, _dirToTarget, _dstToTarget, obstacleMask))
                    target = _target;
            }
        }
    }

    public Vector3 ConvertAngle(float _degree, bool _isGlobalAngle)
    {
        if (!_isGlobalAngle)
            _degree += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(_degree * Mathf.Deg2Rad), 0, Mathf.Cos(_degree * Mathf.Deg2Rad));
    }
}
