using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkMan : MonoBehaviour
{

    #region Public static Fields

    public static NetworkMan instance;

    #endregion

    #region  Public Fields

    public delegate void JoinedRoomHandler();
    public event JoinedRoomHandler JoinedRoom;

    #endregion

    #region Private Serializable Fields


    #endregion


    #region Private Fields


    string gameVersion = "1";

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;


    #endregion


    #region MonoBehaviour CallBacks


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        Connect();
    }


    #endregion


    #region Public Methods



    public void Connect()
    {

        if (PhotonNetwork.IsConnected)
        {

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    #endregion

    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        JoinedRoom();
    }

    #endregion
}
