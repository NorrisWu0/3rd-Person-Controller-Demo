using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] bool m_IsDissolving;
    [SerializeField] float m_Buffer;
    [SerializeField] Material m_Material;
    
    public void StartDissolve()
    {
        m_Material.SetFloat("_dissolve", -1);
        m_IsDissolving = true;
        StartCoroutine("Dissolving");
    }

    IEnumerator Dissolving()
    {
        Debug.Log("Dissolving");
        while (m_Material.GetFloat("_dissolve") < 0.5f)
        {
            m_Material.SetFloat("_dissolve", (m_Material.GetFloat("_dissolve") + Time.deltaTime * m_Buffer));
            Debug.Log(m_Material.GetFloat("_dissolve"));
            yield return null;
        }
    }
}
