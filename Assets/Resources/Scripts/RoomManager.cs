using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public TMP_Text playerCountTxt;
   


    private void Start()
    {
        NetManager.instance.JoinedRoom += UpdatePlayerCountDisplay;
        NetManager.instance.JoinedRoom += CheckFullRoom;
        NetManager.instance.PlayerJoinedRoom += UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom += CheckFullRoom;
    }

    private void OnDisable()
    {
        NetManager.instance.JoinedRoom -= CheckFullRoom;
        NetManager.instance.JoinedRoom -= UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom -= UpdatePlayerCountDisplay;
        NetManager.instance.PlayerJoinedRoom -= CheckFullRoom;

    }

    public void UpdatePlayerCountDisplay()
    {
        playerCountTxt.text = string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);

    }

    public void CheckFullRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }
}
