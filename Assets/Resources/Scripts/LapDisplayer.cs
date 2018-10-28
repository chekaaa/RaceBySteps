using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapDisplayer : MonoBehaviour
{
    private TMP_Text m_lapTxt;

    private void Start()
    {
        m_lapTxt = GetComponent<TMP_Text>();
        m_lapTxt.text = "LAPS 0/" + RaceManager.instance.TotalLaps;
    }

    public void UpdateLapTxt(int _laps)
    {
        _laps = Mathf.Clamp(_laps, 0, RaceManager.instance.TotalLaps);

        m_lapTxt.text = "LAPS " + _laps + "/" + RaceManager.instance.TotalLaps;
    }


}
