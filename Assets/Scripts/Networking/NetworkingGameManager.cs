using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class NetworkingGameManager : GameManager
{
    private new PhotonView photonView;

    private void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    public void SendMove(GameObject piece, GameObject move) 
    {
        photonView.RPC(nameof(RPC_ReceiveMove), RpcTarget.AllBuffered, piece, move );
    }

    [PunRPC]
    private void RPC_ReceiveMove(GameObject piece, GameObject move)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().MovePiece(piece, move);
    }
}
