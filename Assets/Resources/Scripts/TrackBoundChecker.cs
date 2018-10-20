using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBoundChecker : MonoBehaviour
{
    private const string CHECKPOINT_TAG = "Checkpoint";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == CHECKPOINT_TAG)
        {
            Debug.Log(other.transform.name);
        }
    }

}
