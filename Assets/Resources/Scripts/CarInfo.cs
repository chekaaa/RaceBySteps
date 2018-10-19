using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarInfo : MonoBehaviour
{
    public int ownerId;

    [SerializeField] private CarBehaviour m_carBehaviour;
    [SerializeField] private TrajectoryDisplayer m_trajectoryDisplayer;

    private void Awake()
    {
        m_carBehaviour.Init();
        m_trajectoryDisplayer.Init();

    }
}