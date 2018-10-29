using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PositionInfo : MonoBehaviourPun
{

    public TMP_Text infoTxt;

    public void Init(int _position, string _username, float _raceTime)
    {
        string formatedTime = FormattTimeFromSeconds(_raceTime);

        if (_raceTime == -1)
        {
            infoTxt.text = "DNF.- " + _username + "   -   -";
        }
        else
        {
            infoTxt.text = _position + ".- " + _username + "   -   " + formatedTime;
        }
        this.transform.SetParent(IngameUiManager.instance.playerPositionContent);
    }



    private string FormattTimeFromSeconds(float _seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(_seconds);

        string answer = string.Format("{0:D2}m:{1:D2}s:{2:D3}ms",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        return answer;
    }
}
