using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string TEAM = "team";
    private const int MAX_PLAYERS = 2;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //DontDestroyOnLoad(this.gameObject);
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
        PhotonNetwork.JoinRandomRoom();
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

        if(PhotonNetwork.IsMasterClient)
        {
            GUI_Manager.ShowTeamSelectionPanel();
        }
        else
        {
            Debug.Log(gameObject.GetComponent<GameManager>().teamIsSet);
            if (gameObject.GetComponent<GameManager>().teamIsSet == false)
            {
                GUI_Manager.ShowWaitingForTeamSelectionPanel();
            }
            else
            {
                //start game
                Navigation.LoadSelectedScene();
            }
        }


    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log($"Player {player.ActorNumber} joined the room.");
        if(IsRoomFull() )
        {
            if (GUI_Manager.IsWaitingForOpponentPanelActive())
            {
                GUI_Manager.HideWaitingForOpponentPanel();
                //start game
                Navigation.LoadSelectedScene();
            }
        }
    }


    public override void OnDisconnected(DisconnectCause reason)
    {
        Debug.LogWarningFormat($"OnDisconnected() was called by PUN with reason: {reason}.");
        if(reason == DisconnectCause.ClientTimeout)
        {
            GUI_Manager.UserDisconnected();
        }
        //if (SceneManager.GetActiveScene().name != "Main Menu")
        //{
        //    Navigation.MainMenu();
        //}
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.LogWarningFormat("Opponent left the game.");
        GameManager.opponentDisconnected = true;

        LeaveGame();
    }

    #endregion


    public void LeaveGame()
    {
        StartCoroutine(DoLeaveGame());
    }

    IEnumerator DoLeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        Navigation.MainMenu();
    }


    public static void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log($"Player left the room.");
    }

    public static void Disconnect()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");
    }

    public static bool IsRoomFull()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void checkToStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("RPC sent");
            if (!IsRoomFull())
            {
                GUI_Manager.ShowWaitingForOpponentPanel();
            }
            else
            {
                //start the game
                Navigation.LoadSelectedScene();
            }
        }
        else
        {
            if (GUI_Manager.IsWaitingForTeamSelectionPanelActive())
            {
                GUI_Manager.HideWaitingForTeamSelectionPanel();
                //start game
                Navigation.LoadSelectedScene();
            }
        }
    }

    

}