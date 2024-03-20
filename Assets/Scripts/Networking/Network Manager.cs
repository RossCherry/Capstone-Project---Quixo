using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string TEAM = "team";
    private const int MAX_PLAYERS = 2;
    bool isPlayerOne = true;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(this.gameObject);
    }


    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable(), MAX_PLAYERS);  
        }
        else
        {
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server. Joining a room.");
        PhotonNetwork.JoinRandomOrCreateRoom();
        //PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable(), MAX_PLAYERS);

    }

    public override void OnJoinRandomFailed(short code, string reason)
    {
        Debug.Log($"Joining room failed. Reason: {reason}. Creating new room");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = MAX_PLAYERS };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined a room.");
        //gameInitializer.CreateMultiplayerBoard();
        //SetPlayerTeam();
        //gameInitializer.InitializeMultiplayerController();

        //????
        //PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        //set the local player in the base controller class with their team
        //call StartNewGame from the base game controller class

        //set player team
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            isPlayerOne = true;
            
        }
        else
        {
           // GameManager.setPlayer(false);
           isPlayerOne = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log($"Player {player.ActorNumber} joined the room.");
    }

    public override void OnDisconnected(DisconnectCause reason)
    {
        Debug.LogWarningFormat($"OnDisconnected() was called by PUN with reason: {reason}.");
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.LogWarningFormat("Opponent left the game.");
        GameActions.OpponentDisconnected();
    }

    #endregion



    public void SetPlayerTeam(int team)
    {
        //???
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log($"Player left the room.");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");
    }

    public bool IsRoomFull()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public bool getIsPlayerOne()
    { 
        return isPlayerOne;
    }

    

}