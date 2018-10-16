using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomManager : MonoBehaviour
{
    public TMP_Text playerCountTxt;

    private void Start()
    {
        NetManager.instance.JoinedRoom += UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom += UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom += CheckFullRoom;
    }

    private void OnDisable()
    {
        NetManager.instance.JoinedRoom -= UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom -= UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom -= CheckFullRoom;

    }

    public void UpdatePlayerCountDisplay()
    {
        playerCountTxt.text = string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void CheckFullRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
