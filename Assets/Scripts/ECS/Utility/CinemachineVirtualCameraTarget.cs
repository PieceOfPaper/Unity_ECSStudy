using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

    private void Start() 
    {
        VCam.LookAt = null;
        VCam.Follow = null;
    }

    private void LateUpdate()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = entityManager.CreateEntityQuery(typeof(MovableComponent));
        if (query.IsEmpty == false)
        {
            var entity = query.GetSingletonEntity();
            var localTransform = entityManager.GetComponentData<LocalTransform>(entity);

            if (m_DummyTransform == null)
            {
                var newObj = new GameObject("DummyTransform");
                m_DummyTransform = newObj.transform;
                VCam.LookAt = m_DummyTransform;
                VCam.Follow = m_DummyTransform;
            }
            m_DummyTransform.position = localTransform.Position;
        }
    }
}
