using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeDisplayer : MonoBehaviour
{
    public static TimeDisplayer instance;


    public TMP_Text bestTimeTxt, timeTxt;

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

    private void Update()
    {
        UpdateTimeTxt();
    }



    public void Init()
    {
        LeaderBoardManager.instance.GetHighScore();
        bestTimeTxt.text = "BEST TIME: " + LeaderBoardManager.instance.allTimeHighScore.formattedValue;
    }

    public void UpdateTimeTxt()
    {
        CarInfo _info = GameManager.instance.localCar.GetComponent<CarInfo>();
        if (!_info.isFinished)
        {
            string _formatedTime = FormattTimeFromSeconds(RaceManager.instance.lapTime);
            timeTxt.text = _formatedTime;
        }
    }

    private string FormattTimeFromSeconds(float _seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(_seconds);

        string answer = string.Format("{0:D2}m:{1:D2}s:{2:D2}ms",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        return answer;
    }

}
