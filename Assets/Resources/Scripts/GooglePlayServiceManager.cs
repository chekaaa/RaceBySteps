using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayServiceManager : MonoBehaviour
{

    public GameObject menuCanvas;

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

    }

    private void Start()
    {

        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log("@Authenticate");
            // handle success or failure
            if (success)
            {
                menuCanvas.SetActive(true);
            }
            else
            {

                menuCanvas.SetActive(false);

            }
        });
    }

    public void ShowLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }

}
