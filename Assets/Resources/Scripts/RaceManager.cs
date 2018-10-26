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


    private Dictionary<int, int> lapList = new Dictionary<int, int>();
    public List<int> positionList = new List<int>();


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

    public void AddLapToCar(int _playerId)
    {
        lapList[_playerId]++;
        photonView.RPC("RPCUpdateLapDisplay", RpcTarget.All, lapList[_playerId], _playerId);

        if (ischeckeredFlag(_playerId))
        {
            positionList.Add(_playerId);
            int _pos = positionList.IndexOf(_playerId) + 1;
            IngameUiManager.instance.AddPlayerToLeaderBoard(_playerId, _pos);
        }
        if (areAllFinished())
        {
            //EndGame - Display positions
            Debug.Log("Race ended");
            photonView.RPC("RPCFinishGame", RpcTarget.All);
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


