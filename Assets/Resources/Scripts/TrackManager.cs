using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager instance;

    public Track[] trackPrefabs;

    public Track currentTrack;

    public int randomIndex;

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

        InstantiateTrack();
    }

    private void InstantiateTrack()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        randomIndex = Random.Range(0, trackPrefabs.Length);
        Debug.Log("track index: " + randomIndex);
        Instantiate(trackPrefabs[randomIndex].track, Vector3.zero, Quaternion.identity);
        Instantiate(trackPrefabs[randomIndex].bounds, Vector3.zero, Quaternion.identity);
        Instantiate(trackPrefabs[randomIndex].spawnPoints, Vector3.zero, Quaternion.identity);

        currentTrack = trackPrefabs[randomIndex];
    }

}

[System.Serializable]
public class Track
{
    public GameObject track, bounds, spawnPoints;
    public Transform cp;
    public int DNFTurns;

    public Track(GameObject _track, GameObject _bounds, GameObject _spawnPoints, Transform _cp, int _DNFTurns)
    {
        track = _track;
        bounds = _bounds;
        spawnPoints = _spawnPoints;
        cp = _cp;
        DNFTurns = _DNFTurns;
    }
}
