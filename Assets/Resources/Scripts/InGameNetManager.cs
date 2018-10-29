using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class InGameNetManager : MonoBehaviourPunCallbacks
{

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning(cause.ToString());
        SceneManager.LoadScene(0);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("@OnLeftRoom");
        if (GameManager.instance.isGameEnded)
            return;

        SceneManager.LoadScene(0);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.InRoom && !GameManager.instance.isGameEnded)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // void OnApplicationQuit()
    // {
    //     Debug.Log("Application ending after " + Time.time + " seconds");
    //     if (PhotonNetwork.IsConnected)
    //     {
    //         PhotonNetwork.Disconnect();
    //     }
    // }


}
