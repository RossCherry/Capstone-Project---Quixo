using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;            

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string TEAM = "team";
    private const int MAX_PLAYERS = 2;
    [SerializeField] private GameInitializer gameInitializer;
    [SerializeField] private UIManager uiManager;
    private MultiplayerGameController gameController;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetDependencies(MultiplayerGameController gameController)
    {
        this.gameController = gameController;
    }

    
    public void Connect ()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable(), MAX_PLAYERS);  //not sure about parameters
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.GameVersion = gameVersion;
        }
    }

    private void Update()
    {
        uiManager.SetConnectionStatusText(PhotonNetwork.NetworkClientState.ToString());         //possibly only being used if this text is to be displayed in the connection screen
    }


    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server. Joining a room.");
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable(), MAX_PLAYERS);

    }

    public override void OnJoinRandomFailed(short code, string reason)
    {
        Debug.Log($"Joining room failed. Reason: {reason}. Creating new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions                                          //first parameter is the name of the room (null is fine)
        {
            MaxPlayers = MAX_PLAYERS
        });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined a room.");
        gameInitializer.CreateMultiplayerBoard();
        SetPlayerTeam();
        gameInitializer.InitializeMultiplayerController();

        //????
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        
        //set the local player in the base controller class with their team
        //call StartNewGame from the base game controller class
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.ActorNumber} joined the room.");
    }

    public override void OnDisconnected(DisconnectCause reason)
    {
        Debug.LogWarningFormat($"OnDisconnected() was called by PUN with reason: {reason}.");
    }

    #endregion



    public void SetPlayerTeam(int team)
    {
        //???
    }


    public bool IsRoomFull()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }

}
