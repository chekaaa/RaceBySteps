using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayServiceManager : MonoBehaviour
{

    public GameObject menuCanvas;

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestServerAuthCode(false)
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

    }

    private void Start()
    {

        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            if (success)
            {
                menuCanvas.SetActive(true);
            }
        });
    }
}
