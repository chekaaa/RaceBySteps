using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TrackBoundChecker : MonoBehaviourPun
{
    private const string CHECKPOINT_TAG = "Checkpoint";
    private const string BOUND_TAG = "Bound";

    private CarBehaviour m_carBehaviour;
    private CarInfo m_carInfo;


    private Transform m_respawnPosition;

    private void Awake()
    {
        m_carInfo = GetComponent<CarInfo>();
        m_respawnPosition = GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        m_carBehaviour = GetComponent<CarBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == CHECKPOINT_TAG)
        {
            m_respawnPosition = other.transform;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.transform.tag == BOUND_TAG)
        {
            m_carBehaviour.StopCar();
            transform.position = m_respawnPosition.position;
            transform.rotation = m_respawnPosition.rotation * Quaternion.Euler(0f, 0f, -90f);
            if (m_carInfo.ownerId == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                IngameUiManager.instance.arrow.eulerAngles = new Vector3(0f, 0f, 90f);
                IngameUiManager.instance.arrowTarget.eulerAngles = new Vector3(0f, 0f, 90f);
            }
        }

    }

}
