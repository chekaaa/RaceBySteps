using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [SerializeField] private float m_readyTimeLimit = 15f;
    private float m_readyTime = 0f;
    private float m_steps = 0f;
    private float m_planPhaseTimer = 0f;
    public float turnDuration = 5f;
    public float planPhaseDuration = 4f;
    public float delta = 0.02f;
    public int DNFTurns = 10;
    public int DNFTurnsAfterFinish = 4;
    public int actualDNFTurns;


    private List<int> m_idOfTurnEnded = new List<int>();
    private int m_turnsEnded = 0;
    private int m_movePhasesEnded = 0;
    private int m_playersReady = 0;

    private bool m_isStarted = false;
    [HideInInspector] public bool isGameEnded = false;
    [HideInInspector] public bool isWaiting = false;
    [HideInInspector] public bool isMovePhase = false;
    [HideInInspector] public bool isAPlayerFinished = false;


    public GameObject readyPanel, waitingTxt, readyBtn;
    public Button turnButton;
    public TMP_Text turnTimerTxt;
    public GameObject wheel, gasPedal, breakPedal;
    [HideInInspector] public CarBehaviour localCar;

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
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnCars();
        }
        actualDNFTurns = DNFTurns;
        m_spawnIndex = 0;
        turnTimerTxt.text = (int)m_planPhaseTimer + "";
        m_planPhaseTimer = planPhaseDuration;
    }



    private void Update()
    {
        if (!m_isStarted)
        {
            m_readyTime += Time.deltaTime;
            if (m_readyTime >= m_readyTimeLimit)
            {
                SetReady();
            }
            return;
        }

        if (isGameEnded)
        {
            //Endgame
            isMovePhase = false;
            return;
        }




        if (areAllTurnsEnded())
        {
            m_idOfTurnEnded.Clear();
            actualDNFTurns--;


            // if (PhotonNetwork.IsMasterClient)
            // {



            // }
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPCChangeMovePhase", RpcTarget.All);
            }

            //isMovePhase = true;
        }



        if (isMovePhase)
        {
            // if (PhotonNetwork.IsMasterClient)
            // {
            m_steps += delta;
            if (m_steps >= turnDuration)
            {
                photonView.RPC("RPCMovePhaseEnded", RpcTarget.All);
                isMovePhase = false;
                m_steps = 0f;
                if (isAPlayerFinished)
                {
                    if (actualDNFTurns > DNFTurnsAfterFinish)
                    {
                        actualDNFTurns = DNFTurnsAfterFinish;
                    }

                }
                if (actualDNFTurns < 1)
                {
                    FillDNFCars();
                    RaceManager.instance.CallFinishGame();
                    return;
                }
            }
            // }

        }
        else if (!isWaiting)
        {
            m_planPhaseTimer -= Time.deltaTime;
            turnTimerTxt.text = (int)m_planPhaseTimer + "";
            if (m_planPhaseTimer < 1f)
            {
                CompleteTurn();
            }
        }

    }
    private void FillDNFCars()
    {
        foreach (GameObject car in carList.Values)
        {
            CarInfo _info = car.GetComponent<CarInfo>();
            if (!_info.isFinished)
            {
                _info.StopCar();
                RaceManager.instance.AddCarToPositionList(_info.ownerId, -1f);
            }
        }
    }

    [PunRPC]
    public void RPCChangeMovePhase()
    {
        isMovePhase = true;

    }

    [PunRPC]
    public void RPCMovePhaseEnded()
    {
        m_movePhasesEnded++;

        if (areAllMovePhasesEnded())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPCChangeToPlanPhase", RpcTarget.AllViaServer);
            }
            m_movePhasesEnded = 0;

        }


    }

    private bool areAllMovePhasesEnded()
    {
        if (m_movePhasesEnded >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetReady()
    {
        readyBtn.SetActive(false);
        waitingTxt.SetActive(true);
        photonView.RPC("CMDSetReady", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void CMDSetReady()
    {
        m_playersReady++;
        if (areAllPlayersReady())
        {
            photonView.RPC("RPCStartGame", RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    public void RPCStartGame()
    {
        readyPanel.SetActive(false);
        m_isStarted = true;
    }

    private bool areAllPlayersReady()
    {
        if (m_playersReady == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool areAllTurnsEnded()
    {
        if (m_turnsEnded == PhotonNetwork.CurrentRoom.PlayerCount)
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
        photonView.RPC("RPCPlayerTurnInfo",
         RpcTarget.All,
        PhotonNetwork.LocalPlayer.ActorNumber,
         PlayerController.instance.rotAmount,
        PlayerController.instance.targetSpeed);

        wheel.SetActive(false);
        gasPedal.SetActive(false);
        breakPedal.SetActive(false);
        turnTimerTxt.gameObject.SetActive(false);
        // gasSlider.SetActive(false);
        isWaiting = true;
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
            GameObject go = PhotonNetwork.InstantiateSceneObject(CARPREFAB_NAME, _spawn.position, _spawn.rotation);
            //carList.Add(_player.ActorNumber, go);
            go.GetComponent<CarInfo>().Init(_player.ActorNumber, m_spawnIndex,
            GetUniqueUsername(_player.NickName));
            m_spawnIndex++;
        }
    }

    private string GetUniqueUsername(string _username)
    {
        foreach (GameObject go in carList.Values)
        {
            CarInfo _info = go.GetComponent<CarInfo>();
            if (_info.ownerUsername == _username)
            {
                int _newIndex = 1;
                string _newUsername = "";
                for (int i = PhotonNetwork.CurrentRoom.MaxPlayers; i > 0; i--)
                {

                    string _tempusername = _username + " (" + i + ")";
                    foreach (GameObject car in carList.Values)
                    {
                        CarInfo _info2 = car.GetComponent<CarInfo>();
                        if (_info2.ownerUsername == _tempusername)
                        {
                            _newIndex = i + 1;
                            _newUsername = _username + " (" + _newIndex + ")";
                            return _newUsername;
                        }
                    }
                }
                _newUsername = _username + " (1)";
                return _newUsername;
            }

        }
        return _username;
    }

    public void StopMovementOnDisconnectedPLayer(int _ownerId)
    {
        photonView.RPC("RPCPlayerTurnInfo", RpcTarget.All, _ownerId, 0f, 0f);
    }

    [PunRPC]
    public void RPCPlayerTurnInfo(int _actorId, float _rotAmount, float _targetSpeed)
    {
        if (carList == null)
            return;

        carList[_actorId].GetComponent<CarBehaviour>().SetMoveValues(_rotAmount, _targetSpeed);
        // if (PhotonNetwork.IsMasterClient)
        // {
        m_idOfTurnEnded.Add(_actorId);
        m_turnsEnded++;
        // }
    }

    [PunRPC]
    public void RPCChangeToPlanPhase()
    {
        isMovePhase = false;
        isWaiting = false;
        turnButton.interactable = true;
        wheel.SetActive(true);
        gasPedal.SetActive(true);
        breakPedal.SetActive(true);
        turnTimerTxt.gameObject.SetActive(true);
        // gasSlider.SetActive(true);
    }

    public void QuitGame()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadScene(0);

    }




}
