using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    private float m_steps = 0f;
    private float m_planPhaseTimer = 0f;
    public float turnDuration = 5f;
    public float planPhaseDuration = 4f;
    public float delta = 0.02f;

    private int m_turnsEnded = 0;

    public bool isMovePhase = false;


    public Button turnButton;
    public TMP_Text turnTimerTxt;
    public GameObject wheel, gasSlider;

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
        m_planPhaseTimer = planPhaseDuration;
        turnTimerTxt.text = (int)m_planPhaseTimer + "";
        SpawnCars();
    }



    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (areAllTurnsEnded())
            {
                isMovePhase = true;
            }
        }

        if (isMovePhase)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                m_steps += delta;
                if (m_steps >= turnDuration)
                {
                    photonView.RPC("RPCChangeToPlanPhase", RpcTarget.AllViaServer);
                    m_steps = 0f;
                }
            }

        }
        else
        {
            m_planPhaseTimer -= Time.deltaTime;
            turnTimerTxt.text = (int)m_planPhaseTimer + "";
            if (m_planPhaseTimer < 1f)
            {
                CompleteTurn();
            }
        }

    }

    private bool areAllTurnsEnded()
    {
        if (m_turnsEnded == carList.Count)
        {
            m_turnsEnded = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CompleteTurn()
    {
        photonView.RPC("CMDPlayerTurnInfo",
         RpcTarget.MasterClient,
        PhotonNetwork.LocalPlayer.ActorNumber,
         PlayerController.instance.rotAmount,
        PlayerController.instance.targetSpeed);

        wheel.SetActive(false);
        gasSlider.SetActive(false);
        m_planPhaseTimer = planPhaseDuration;
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
            go.GetComponent<CarInfo>().Init(_player.ActorNumber, m_spawnIndex);
            m_spawnIndex++;
        }
    }


    [PunRPC]
    public void CMDPlayerTurnInfo(int _actorId, float _rotAmount, float _targetSpeed)
    {
        if (carList == null)
            return;

        carList[_actorId].GetComponent<CarBehaviour>().SetMoveValues(_rotAmount, _targetSpeed);
        m_turnsEnded++;
    }

    [PunRPC]
    public void RPCChangeToPlanPhase()
    {
        isMovePhase = false;
        turnButton.interactable = true;
        wheel.SetActive(true);
        gasSlider.SetActive(true);
    }


}
