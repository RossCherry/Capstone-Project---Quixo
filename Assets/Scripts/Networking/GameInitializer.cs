using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.ComponentModel.Design.Serialization;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private MultiplayerBoard multiplayerBoardPrefab;
    [SerializeField] private MultiplayerGameController multiplayerGameControllerPrefab;

    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private UIManager uiManager;
    //....etc

    public void InstantiateMultiplayerBoard()
    {
        if (!networkManager.IsRoomFull())           //only the first player needs to instantiate
        {
            PhotonNetwork.Instantiate(          //three parameters:
                multiplayerBoardPrefab.name     //multiplayer board prefab
                                                //board anchor position
                                                //board anchor rotation
            );
        }
    }

    public void InitializeMultiplayerController()
    {
        Multiplayerboard board = FindObjectOfType<multiplayerBoard>();
        if (board)          //remove?
        {
            multiplayerGameController controller = Instantiate(multiplayerGameControllerPrefab);
            
            //need to be modified for our implementation:
            controller.SetDependencies(uiManager, board);
            controller.InitializeGame();
            controller.SetNetworkManager(networkManager);
            networkManager.SetDependencies(controller);
            board.SetDependencies(controller);
        }
    }


}
