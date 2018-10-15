using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject menuGUI, lobbyGUI;

    public void OnFindGameButton()
    {
        SNetworkManager.instance.Connect();
    }
}

