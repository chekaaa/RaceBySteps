﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class IngameUiManager : MonoBehaviourPun
{

    public static IngameUiManager instance;

    public GameObject playerUIPanel, readyPanel, scorePanel, optionsPanel, waitingEndRacePanel;
    public Transform playerPositionContent;

    private const string PLAYER_POSITION_PREFAB = "PlayerTxtPrefab";

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

    public void AddPlayerToLeaderBoard(int _id, int _position)
    {

        string _username = GameManager.instance.carList[_id].GetComponent<CarInfo>().ownerUsername;
        GameObject go = PhotonNetwork.InstantiateSceneObject(PLAYER_POSITION_PREFAB,
        Vector3.zero, Quaternion.identity);
        go.GetComponent<PositionInfo>().Init(_position, _username);
    }

}
