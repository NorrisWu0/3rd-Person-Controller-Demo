using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float m_MouseSensitivity;
    [SerializeField] float m_CameraAngleLimit;
    [SerializeField] Transform m_Target;
    [SerializeField] Vector3 m_Offset;

    float m_Yaw;
    float m_Pitch;

    void Start()
    {
        #region Get stuffs
        //m_Target = Player.instance.transform;
        #endregion

        #region Lock Cursor to center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endregion
    }

    // Last update that get called in the flow
    void LateUpdate()
    {
        ChaseTarget();
        RotateCamera();

    }

    #region Chase Target
    private void ChaseTarget()
    {
        transform.position = m_Target.transform.position;
    }
    #endregion

    #region Rotate Camera
    private void RotateCamera()
    {
        // Get input from mouse
        m_Yaw += Input.GetAxis("Mouse X");
        m_Pitch -= Input.GetAxis("Mouse Y");
        
        //Clamp camera rotation on X
        m_Pitch = Mathf.Clamp(m_Pitch, -m_CameraAngleLimit / m_MouseSensitivity, m_CameraAngleLimit / m_MouseSensitivity);

        // Rotate Camera root
        transform.localEulerAngles = new Vector3(m_Pitch, m_Yaw) * m_MouseSensitivity;
    }
    #endregion
}
