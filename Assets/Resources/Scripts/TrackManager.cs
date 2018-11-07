using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class TrackManager : MonoBehaviourPun
{
    public static TrackManager instance;

    public Track[] trackPrefabs;

    public Track currentTrack;

    public int randomIndex;
    public int sceneLoadedCount = 0;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        photonView.RPC("RPCOnSceneLoaded", RpcTarget.All);
    }

    [PunRPC]
    public void RPCOnSceneLoaded()
    {
        sceneLoadedCount++;

        if (PhotonNetwork.IsMasterClient)
        {
            if (sceneLoadedCount >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                InstantiateTrack();
            }
        }
    }

    private void InstantiateTrack()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        randomIndex = Random.Range(0, trackPrefabs.Length);
        Debug.Log("track index: " + randomIndex);
        // PhotonNetwork.InstantiateSceneObject(trackPrefabs[randomIndex].track.name, Vector3.zero, Quaternion.identity);
        // PhotonNetwork.InstantiateSceneObject(trackPrefabs[randomIndex].bounds.name, Vector3.zero, Quaternion.identity);
        // PhotonNetwork.InstantiateSceneObject(trackPrefabs[randomIndex].spawnPoints.name, Vector3.zero, Quaternion.identity);
        // Instantiate(trackPrefabs[randomIndex].track, Vector3.zero, Quaternion.identity);
        // Instantiate(trackPrefabs[randomIndex].bounds, Vector3.zero, Quaternion.identity);
        // Instantiate(trackPrefabs[randomIndex].spawnPoints, Vector3.zero, Quaternion.identity);
        photonView.RPC("RPCInitTrack", RpcTarget.AllBuffered, randomIndex);


    }



    [PunRPC]
    public void RPCInitTrack(int _randomIndex)
    {
        randomIndex = _randomIndex;
        currentTrack = trackPrefabs[randomIndex];

        Instantiate(currentTrack.track, Vector3.zero, Quaternion.identity);
        Instantiate(currentTrack.bounds, Vector3.zero, Quaternion.identity);
        Instantiate(currentTrack.spawnPoints, Vector3.zero, Quaternion.identity);


        GameManager.instance.Init();
        RaceManager.instance.Init();
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
