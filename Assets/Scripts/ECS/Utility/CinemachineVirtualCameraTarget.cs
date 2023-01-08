using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCameraBase))]
public class CinemachineVirtualCameraTarget : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCameraBase m_VCam;
    public Cinemachine.CinemachineVirtualCameraBase VCam
    {
        get
        {
            if (m_VCam == null) m_VCam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
            return m_VCam;
        }
    }

    Transform m_DummyTransform;
    Vector3 m_CurrentPosition;

    private void Start()
    {
        if (m_DummyTransform == null)
        {
            var newObj = new GameObject("DummyTransform");
            m_DummyTransform = newObj.transform;
            m_DummyTransform.position = m_CurrentPosition;
        }

        VCam.LookAt = m_DummyTransform;
        VCam.Follow = m_DummyTransform;
    }

    private void LateUpdate()
    {
        if (m_DummyTransform != null) m_DummyTransform.position = m_CurrentPosition;
    }

    public void SetPosition(Vector3 pos)
    {
        m_CurrentPosition = pos;
    }
}
