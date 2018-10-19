using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarInfo : MonoBehaviour
{
    public int ownerId;

    [SerializeField] private CarBehaviour m_carBehaviour;
    [SerializeField] private TrajectoryDisplayer m_trajectoryDisplayer;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public Sprite[] carSprites;

    public void Init(int _ownerId, int _spriteIndex)
    {
        ownerId = _ownerId;
        m_spriteRenderer.sprite = carSprites[_spriteIndex];
        m_carBehaviour.Init();
        m_trajectoryDisplayer.Init();

        if (_ownerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            CameraBehaviour.instance.target = this.transform;
        }

    }
}