using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime; 

public class MultiplayerGameController : GameController, IOnEventCallback //makes a class listen to events
{
    protected const byte SET_GAME_STATE_EVENT_CODE = 1;
        
    private QuixoPlayer localPlayer;                                     //QuixoPlayer is a class declared elsewhere
    private NetworkManager networkManager;  

    public void SetNetworkManager(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);                          //register to Photon as a target callback
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public void SetLocalPlayer()
    {
        localPlayer = team == Team.Cats ? catPlayer : dogPlayer;        //modify for our implementation
    }

    private bool IsLocalPlayersTurn()
    {
        return localPlayer == activePlayer;
    }

    private void SetGameState(GameState state)
    {
        object[] content = new object[] { (int)state };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; //set receivers variable to all clients
        PhotonNetwork.RaiseEvent(
            SET_GAME_STATE_EVENT_CODE,                  //the identifying code for this event so other clients can recognize the event
            content,                                    //array of objects to represent event data
            raiseEventOptions,
            SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == SET_GAME_STATE_EVENT_CODE)
        {
            object[] data = (object[])photonEvent.CustomData;
            GameState state = (GameState)data[0];       //change the state (adjust for our implementation)
            this.state = state;
        }
    }

    //only starts the game if the room is full
    public void TryToStartCurrentGame()
    {
        if (networkManager.IsRoomFUll())
        {
            SetGameState(GameState.Play);               //adjust for our implementation
        }
    }

    //check if the active player is the same player as the local player
    public bool CanPerformMove()
    {
        if (!IsGameInProgress() || !IsLocalPlayersTurn()) //second check is specific to multiplayer, the first should be in the game logic
        {
            return false;
        }
        return true;
    }


   

    

    

    

    
}
