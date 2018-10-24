using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarInfo : MonoBehaviourPun
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


        photonView.RPC("RPCSyncInfo", RpcTarget.AllBuffered, _spriteIndex, _ownerId);


    }

    [PunRPC]
    public void RPCSyncInfo(int _spriteIndex, int _ownerId)
    {
        ownerId = _ownerId;
        m_spriteRenderer.sprite = carSprites[_spriteIndex];
        if (_ownerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            CameraBehaviour.instance.target = this.transform;
        }
        GameManager.instance.carList.Add(_ownerId, this.gameObject);
        m_trajectoryDisplayer.Init();
        m_carBehaviour.Init();
    }
}