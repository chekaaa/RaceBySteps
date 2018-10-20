using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacksBoundTest : MonoBehaviour
{
    public Transform respawnPosition;

    private void Awake()
    {
        respawnPosition = GameObject.Find("SpawnPoint").transform;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");

        if (other.transform.tag == "Bound")
        {
            transform.position = respawnPosition.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("@OnTriggerEnter2D");
    }


}
