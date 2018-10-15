using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager instance;

    public delegate void JoinedRoomHandler();
    public event JoinedRoomHandler JoinedRoom;


    private bool m_isConnectedToMaster;
    public bool IsConnectedToMaster
    {
        get { return m_isConnectedToMaster; }
    }


    [SerializeField] private byte m_maxPlayers = 4;
    [SerializeField] private string m_gameVersion = "0.1";

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Connect();
    }

    private void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = m_gameVersion;
            PhotonNetwork.ConnectUsingSettings();

        }

    }



    public void FindRandomMatch()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = m_maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom("", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (JoinedRoom == null)
            return;
        JoinedRoom();
    }



    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        m_isConnectedToMaster = true;

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        m_isConnectedToMaster = false;
    }

}
