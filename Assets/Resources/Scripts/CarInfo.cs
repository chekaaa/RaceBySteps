using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarInfo : MonoBehaviourPun
{
    public int ownerId;
    public string ownerUsername;

    [SerializeField] private CarBehaviour m_carBehaviour;
    [SerializeField] private TrajectoryDisplayer m_trajectoryDisplayer;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public Sprite[] carSprites;




    public void Init(int _ownerId, int _spriteIndex, string _ownerUsername)
    {
        ownerId = _ownerId;
        m_spriteRenderer.sprite = carSprites[_spriteIndex];
        ownerUsername = _ownerUsername;
        Debug.Log(_ownerUsername);

        photonView.RPC("RPCSyncInfo", RpcTarget.AllBuffered, _spriteIndex, _ownerId, ownerUsername);


    }

    [PunRPC]
    public void RPCSyncInfo(int _spriteIndex, int _ownerId, string _ownerUsername)
    {
        ownerId = _ownerId;
        ownerUsername = _ownerUsername;
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