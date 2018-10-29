using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RaceManager : MonoBehaviourPun
{
    public static RaceManager instance;

    public LapDisplayer lapDisplayer;

    public Transform cpParent;
    [HideInInspector] public List<Transform> cpList = new List<Transform>();
    [HideInInspector] public int CPCount;

    public int TotalLaps = 3;

    private float m_lapTime = 0f;

    private Dictionary<int, int> lapList = new Dictionary<int, int>();
    public List<PlacingInfo> positionList = new List<PlacingInfo>();


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
        if (!PhotonNetwork.IsMasterClient)
            return;

        FillCpList();
        FillLapList();
    }

    private void Update()
    {
        if (GameManager.instance.isMovePhase)
        {
            m_lapTime += Time.deltaTime;
        }
    }

    public void AddLapToCar(int _playerId)
    {
        lapList[_playerId]++;
        photonView.RPC("RPCUpdateLapDisplay", RpcTarget.All, lapList[_playerId], _playerId);

        if (ischeckeredFlag(_playerId))
        {

            AddCarToPositionList(_playerId, m_lapTime);
        }
        if (areAllFinished())
        {
            //EndGame - Display positions
            Debug.Log("Race ended");
            CallFinishGame();
        }

    }
    public void CallFinishGame()
    {
        photonView.RPC("RPCFinishGame", RpcTarget.All);
    }

    public void AddCarToPositionList(int _ownerId, float _raceTime)
    {
        positionList.Add(new PlacingInfo(_ownerId, _raceTime));
        if (positionList.IndexOf(new PlacingInfo(_ownerId, _raceTime)) == 0)
        {
            //First on finish. Start countdown for DNF
            GameManager.instance.isAPlayerFinished = true;
        }
        // int _pos = positionList.IndexOf(new PlacingInfo(_ownerId, _raceTime)) + 1;
        int _pos = positionList.FindIndex(x => x.ownerId == _ownerId) + 1;
        //Debug.Log("Position: " + _pos);
        IngameUiManager.instance.AddPlayerToLeaderBoard(_ownerId, _pos, _raceTime);
        photonView.RPC("RPCWaitingForEndRace", RpcTarget.All, _ownerId);
        GameManager.instance.carList[_ownerId].GetComponent<CarInfo>().StopCar();
    }

    [PunRPC]
    public void RPCWaitingForEndRace(int _id)
    {
        if (_id == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            IngameUiManager.instance.SetWaitingEndRace();
        }


    }




    [PunRPC]
    public void RPCFinishGame()
    {
        GameManager.instance.isGameEnded = true;
        IngameUiManager.instance.SetEndgameUI();

    }

    [PunRPC]
    public void RPCUpdateLapDisplay(int _laps, int _playerId)
    {
        if (_playerId == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            lapDisplayer.UpdateLapTxt(_laps);
        }
    }



    private bool areAllFinished()
    {
        if (positionList.Count >= GameManager.instance.carList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ischeckeredFlag(int _playerId)
    {
        if (lapList[_playerId] >= TotalLaps)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FillLapList()
    {
        foreach (int i in GameManager.instance.carList.Keys)
        {
            lapList.Add(i, 0);
        }
    }

    private void FillCpList()
    {
        //Checkpoints must be in order in the editor
        foreach (Transform t in cpParent)
        {
            cpList.Add(t);
        }
        CPCount = cpList.Count;
    }
}

public class PlacingInfo
{
    public int ownerId;
    public float raceTime;

    public PlacingInfo(int _ownerId, float _racetime)
    {
        this.ownerId = _ownerId;
        this.raceTime = _racetime;
    }


}


