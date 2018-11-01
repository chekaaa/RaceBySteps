using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class IngameUiManager : MonoBehaviourPun
{

    public static IngameUiManager instance;

    public GameObject playerUIPanel, readyPanel, scorePanel, optionsPanel, waitingEndRacePanel;
    public Transform playerPositionContent;

    //private const string PLAYER_POSITION_PREFAB = "PlayerTxtPrefab";
    public GameObject positionPrefab;
    public Transform arrow, arrowTarget;

    public float maxRot = 90;



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

    private void Update()
    {
        if (GameManager.instance.localCar == null)
            return;

        UpdateArrow();
        UpdatetargetArrow();
    }

    private void UpdateArrow()
    {
        float ang = Mathf.Lerp(-maxRot, maxRot, Mathf.InverseLerp(0f, PlayerController.instance.maxGas,
         GameManager.instance.localCar.speed));
        arrow.eulerAngles = new Vector3(0f, 0f, -ang);
    }

    private void UpdatetargetArrow()
    {
        float ang = Mathf.Lerp(-maxRot, maxRot, Mathf.InverseLerp(0f, PlayerController.instance.maxGas,
         PlayerController.instance.targetSpeed));
        arrowTarget.eulerAngles = new Vector3(0f, 0f, -ang);
    }

    public void SetEndgameUI()
    {
        readyPanel.SetActive(false);
        playerUIPanel.SetActive(false);
        scorePanel.SetActive(true);
        optionsPanel.SetActive(false);
        waitingEndRacePanel.SetActive(false);
    }

    public void SetOptionsUI()
    {
        readyPanel.SetActive(false);
        playerUIPanel.SetActive(false);
        scorePanel.SetActive(false);
        optionsPanel.SetActive(true);
        waitingEndRacePanel.SetActive(false);
    }

    public void SetPlayerUI()
    {
        readyPanel.SetActive(false);
        playerUIPanel.SetActive(true);
        scorePanel.SetActive(false);
        optionsPanel.SetActive(false);
        waitingEndRacePanel.SetActive(false);
    }

    public void SetWaitingEndRace()
    {
        readyPanel.SetActive(false);
        playerUIPanel.SetActive(false);
        scorePanel.SetActive(false);
        optionsPanel.SetActive(false);
        waitingEndRacePanel.SetActive(true);
    }

    public void AddPlayerToLeaderBoard(int _id, int _position, float _raceTime)
    {
        string _username = GameManager.instance.carList[_id].GetComponent<CarInfo>().ownerUsername;
        photonView.RPC("RPCAddPlayerToLeaderboard", RpcTarget.AllBuffered, _id, _position, _raceTime, _username);
    }

    [PunRPC]
    public void RPCAddPlayerToLeaderboard(int _id, int _position, float _raceTime, string _username)
    {

        GameObject go = Instantiate(positionPrefab,
        Vector3.zero, Quaternion.identity);
        go.GetComponent<PositionInfo>().Init(_position, _username, _raceTime);
    }




}
