using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RaceManager : MonoBehaviourPun
{
    public static RaceManager instance;

    public LapDisplayer lapDisplayer;

    public Transform cpParent;
    [HideInInspector] public List<Transform> cpList = new List<Transform>();
    [HideInInspector] public int CPCount;

    public int TotalLaps = 3;

    public float lapTime = 0f;

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
        this.enabled = false;
    }

    public void Init()
    {

        cpParent = TrackManager.instance.currentTrack.cp;
        FillCpList();
        FillLapList();
        this.enabled = true;
    }


    private void Update()
    {
        if (GameManager.instance.isMovePhase)
        {
            lapTime += GameManager.instance.delta;

        }
    }



    public void AddLapToCar(int _playerId)
    {
        lapList[_playerId]++;
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCUpdateLapDisplay", RpcTarget.All, lapList[_playerId], _playerId);
        }

        if (ischeckeredFlag(_playerId))
        {

            AddCarToPositionList(_playerId, lapTime);
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

        GameManager.instance.isGameEnded = true;
        IngameUiManager.instance.SetEndgameUI();
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     photonView.RPC("RPCFinishGame", RpcTarget.All);
        // }
    }



    public void AddCarToPositionList(int _ownerId, float _raceTime)
    {
        positionList.Add(new PlacingInfo(_ownerId, _raceTime));
        if (positionList.FindIndex(x => x.ownerId == _ownerId) == 0)
        {
            //First on finish. Start countdown for DNF
            GameManager.instance.isAPlayerFinished = true;
        }
        // int _pos = positionList.IndexOf(new PlacingInfo(_ownerId, _raceTime)) + 1;
        int _pos = positionList.FindIndex(x => x.ownerId == _ownerId) + 1;
        //Debug.Log("Position: " + _pos);
        if (_ownerId == PhotonNetwork.LocalPlayer.ActorNumber && _raceTime >= 0)
        {
            long _miliseconds = Convert.ToInt64(_raceTime * 1000);
            LeaderBoardManager.instance.PostTimeToLeaderboard(_miliseconds);
        }
        IngameUiManager.instance.AddPlayerToLeaderBoard(_ownerId, _pos, _raceTime);
        if (!areAllFinished())
        {
            // photonView.RPC("RPCWaitingForEndRace", RpcTarget.All, _ownerId);
            RPCWaitingForEndRace(_ownerId);
        }
        GameManager.instance.carList[_ownerId].GetComponent<CarInfo>().StopCar();
    }

    // [PunRPC]
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

        StartCoroutine(StopCarAfterMovingPhase());

    }

    IEnumerator StopCarAfterMovingPhase()
    {
        yield return new WaitUntil(() => !GameManager.instance.isMovePhase);

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
        foreach (int i in PhotonNetwork.CurrentRoom.Players.Keys)
        {
            Debug.Log("ACtorID :" + i);
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


