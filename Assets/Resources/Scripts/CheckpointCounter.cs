using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCounter : MonoBehaviour
{
    // private const string STARTCP_TAG = "StartCp";
    private const string CP_TAG = "Checkpoint";

    private int m_lastCpIndex, m_nextCpIndex = 1, m_startCpIndex = 0;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == CP_TAG)
        {
            string _nextCpName = RaceManager.instance.cpList[m_nextCpIndex].name;
            Debug.Log("Next Index :" + m_nextCpIndex + " | CPName: " + other.transform.name);
            if (_nextCpName == other.transform.name)
            {
                if (m_nextCpIndex == 0)
                {
                    //Lap
                    m_nextCpIndex++;
                    Debug.Log("Lap");
                }
                else
                {
                    m_nextCpIndex++;
                    if (m_nextCpIndex >= RaceManager.instance.CPCount)
                    {
                        m_nextCpIndex = 0;
                    }
                }
                Debug.Log("Next CP:" + RaceManager.instance.cpList[m_nextCpIndex]);
            }
        }
    }
}
