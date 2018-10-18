using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    private float steps = 0f;
    public float turnDuration = 5f;
    public float delta = 0.02f;
    public bool isMovePhase = false;

    public Button turnButton;
    public Dictionary<int, GameObject> carList = new Dictionary<int, GameObject>();
    [SerializeField] private Transform m_spawnParent;
    private Transform[] m_spawnList;
    private int m_spawnIndex = 0;

    private const string CARPREFAB_NAME = "PlayerPrefab";

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

    private void Start()
    {
        FillSpawnList();
        m_spawnIndex = 0;
        SpawnCars();
    }



    private void Update()
    {
        {
            if (isMovePhase)
            {

                steps += delta;
                if (steps >= turnDuration)
                {
                    isMovePhase = false;
                    turnButton.interactable = true;
                    steps = 0f;
                }
            }
        }
    }

    public void CompleteTurn()
    {
        isMovePhase = true;
        turnButton.interactable = false;
    }

    private void FillSpawnList()
    {
        m_spawnList = new Transform[m_spawnParent.childCount];
        for (int i = 0; i < m_spawnList.Length; i++)
        {
            m_spawnList[i] = m_spawnParent.GetChild(i);
        }
    }





    private void SpawnCars()
    {
        carList.Clear();
        foreach (var _player in PhotonNetwork.PlayerList)
        {
            Transform _spawn = m_spawnList[m_spawnIndex];
            GameObject go = PhotonNetwork.Instantiate(CARPREFAB_NAME, _spawn.position, _spawn.rotation);
            carList.Add(_player.ActorNumber, go);
            go.GetComponent<CarInfo>().ownerId = _player.ActorNumber;
            go.GetComponent<TrajectoryDisplayer>().Init();
            CameraBehaviour.instance.target = go.transform;
        }

    }


}
