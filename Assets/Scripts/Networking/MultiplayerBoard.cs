using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class MultiplayerBoard : Board
{
    //PhotonView class performs RPC calls
    private PhotonView photonView;
    
    private void Awake()  
    {
        base.Awake();
        photonView = GetComponent<PhotonView>();
    }


    public override void SelectPieceMoved(Vector2 coords) 
    {   //synchronize method calls using RPCs
        //call an RPC method by calling the RPC method in the PhotonView class
        photonView.RPC(
            nameof(RPC_OnSelectedPieceMoved),   //name of the method
            RpcTarget.AllBuffered,              //which client should perform the method (every other client)
            new object[] {coords}               //array of objects representing RPC_OnSelectedPieceMoved parameters
        );
    }

    public override void SetSelectedPiece(Vector2 coords)
    {
        photonView.RPC(
            nameof(RPC_OnSetSelectedPiece),   //name of the method
            RpcTarget.AllBuffered,            //which client should perform the method (every other client)
            new object[] { coords }           //array of objects representing RPC_OnSelectedPieceMoved parameters
        );
    }



    [PunRPC]
    private void RPC_OnSelectedPieceMoved(Vector2 coords)
    {
        //Same as singleplayer - copy/paste
        //Convert Vector2
        //Call OnSelectedPieceMoved
    }

    [PunRPC]
    private void RPC_OnSetSelectedPiece(Vector2 coords)
    {
        //Same as singleplayer - copy/paste
        //Convert Vector2
        //Call OnSetSelectedPiece
    }

}
