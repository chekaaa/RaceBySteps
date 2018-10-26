using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckpointCounter : MonoBehaviour
{
    // private const string STARTCP_TAG = "StartCp";
    private const string CP_TAG = "Checkpoint";

    private int m_lastCpIndex, m_nextCpIndex = 1;
    private CarInfo m_carInfo;



    private void Start()
    {
        m_carInfo = GetComponent<CarInfo>();
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (other.tag == CP_TAG)
        {
            string _nextCpName = RaceManager.instance.cpList[m_nextCpIndex].name;
            //   Debug.Log("Next Index :" + m_nextCpIndex + " | CPName: " + other.transform.name);
            if (_nextCpName == other.transform.name)
            {
                if (m_nextCpIndex == 0)
                {
                    //Lap

                    RaceManager.instance.AddLapToCar(m_carInfo.ownerId);
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
