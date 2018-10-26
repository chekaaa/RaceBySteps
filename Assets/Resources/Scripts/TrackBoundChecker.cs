using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBoundChecker : MonoBehaviour
{
    private const string CHECKPOINT_TAG = "Checkpoint";
    private const string BOUND_TAG = "Bound";

    private CarBehaviour m_carBehaviour;


    private Transform m_respawnPosition;

    private void Awake()
    {
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
            transform.rotation = m_respawnPosition.rotation * Quaternion.Euler(0f, 0f, -90f); ;
        }
    }

}
