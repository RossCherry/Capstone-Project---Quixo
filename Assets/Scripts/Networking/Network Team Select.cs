using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class NetworkTeamSelect : MonoBehaviour
{
    private new PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTeamSelectClick(bool isCats) 
    {
        bool myTeam = false;
        bool opponentTeam = true;
        //set my team
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("RPC_TeamSelect", RpcTarget.OthersBuffered, opponentTeam);
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {

        }
    }
}
