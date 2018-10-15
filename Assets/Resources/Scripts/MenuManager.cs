using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuManager : MonoBehaviour
{

    public GameObject menuGUI, lobbyGUI;

    private void Start()
    {
        NetManager.instance.JoinedRoom += ChangeToLobby;
    }

    private void OnDisable()
    {
        NetManager.instance.JoinedRoom -= ChangeToLobby;
    }

    public void OnFindGameButton()
    {
        NetManager.instance.FindRandomMatch();
    }

    public void ChangeToLobby()
    {
        menuGUI.SetActive(false);
        lobbyGUI.SetActive(true);
    }
}

