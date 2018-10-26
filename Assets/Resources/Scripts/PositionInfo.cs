using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PositionInfo : MonoBehaviourPun
{

    public TMP_Text infoTxt;

    void Awake()
    {

    }

    public void Init(int _position, string _username)
    {
        photonView.RPC("RPCInit", RpcTarget.All, _position, _username);
    }

    [PunRPC]
    public void RPCInit(int _position, string _username)
    {
        infoTxt.text = _position + ".- " + _username;
        this.transform.parent = IngameUiManager.instance.playerPositionContent;
    }
}
