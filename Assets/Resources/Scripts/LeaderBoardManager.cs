using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SocialPlatforms;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;

    public IScore allTimeHighScore;

    public GameObject errorTxt;



    private string m_currentTrackId;

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



    public void GetTrackid()
    {
        switch (TrackManager.instance.randomIndex)
        {
            case 0:
                m_currentTrackId = RBSGPS.leaderboard_track1;
                break;
            case 1:
                m_currentTrackId = RBSGPS.leaderboard_track2;
                break;
            case 2:
                m_currentTrackId = RBSGPS.leaderboard_track3;
                break;
            case 3:
                m_currentTrackId = RBSGPS.leaderboard_track4;
                break;
            case 4:
                m_currentTrackId = RBSGPS.leaderboard_track5;
                break;

        }
    }

    public void GetHighScore()
    {
        PlayGamesPlatform.Instance.LoadScores(
          m_currentTrackId,
          LeaderboardStart.PlayerCentered,
          1,
          LeaderboardCollection.Public,
          LeaderboardTimeSpan.AllTime,
          (data) =>
          {
              LoadUsersAndDisplay(data.PlayerScore, data.Valid);
          });
    }

    internal void LoadUsersAndDisplay(IScore score, bool success)
    {
        if (success)
        {

            allTimeHighScore = score;
        }
        else
        {
            allTimeHighScore = null;
        }

    }

    public void PostTimeToLeaderboard(long _time)
    {


        Social.ReportScore(_time, m_currentTrackId, (bool success) =>
      {
          // handle success or failure
          if (!success)
          {
              if (!Social.localUser.authenticated)
              {
                  Social.localUser.Authenticate((bool success1) =>
                          {
                              //      Debug.Log("@Authenticate");
                              // handle success or failure
                              if (!success1)
                              {
                                  errorTxt.SetActive(true);
                              }
                              else
                              {
                                  Social.ReportScore(_time, m_currentTrackId, (bool success2) =>
                                     {
                                         if (!success2)
                                         {
                                             errorTxt.GetComponent<TMP_Text>().text = "Not high score";
                                             errorTxt.SetActive(true);
                                         }
                                         else
                                         {
                                             {
                                                 errorTxt.SetActive(false);
                                             }
                                         }
                                     });

                              }
                          });
              }
          }
          else
          {
              //   errorTxt.SetActive(true);
              //   errorTxt.GetComponent<TMP_Text>().text = "Score updated : " + _time + "To the track: " + _leaderboardName;

          }


      });

    }

    public void ShowLeaderboard()
    {

        PlayGamesPlatform.Instance.ShowLeaderboardUI(m_currentTrackId);

    }
}
