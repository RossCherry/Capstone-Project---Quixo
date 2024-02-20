using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int counter = 0;
    public GameObject game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameBoard");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBoard()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (game.GetComponent<GameBoard>().Board[j, i].CompareTag("Player1"))
                {
                    game.GetComponent<GameBoard>().Board[j, i].transform.GetChild(1).gameObject.SetActive(false);
                }
                else if (game.GetComponent<GameBoard>().Board[j, i].CompareTag("Player2"))
                {
                    game.GetComponent<GameBoard>().Board[j, i].transform.GetChild(2).gameObject.SetActive(false);
                }
                game.GetComponent<GameBoard>().Board[j, i].tag = "Blank";
                game.GetComponent<GameBoard>().Board[j, i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-11-body", typeof(Material)) as Material;
            }
        }
    }

    public void BothPlayersCanWin()
    {
        game.GetComponent<GameBoard>().Board[0, 0].tag = "Player1";
        game.GetComponent<GameBoard>().Board[0, 1].tag = "Player1";
        game.GetComponent<GameBoard>().Board[0, 2].tag = "Player1";
        game.GetComponent<GameBoard>().Board[0, 3].tag = "Player1";
        game.GetComponent<GameBoard>().Board[0, 4].tag = "Player2";
        game.GetComponent<GameBoard>().Board[1, 0].tag = "Player2";
        game.GetComponent<GameBoard>().Board[1, 1].tag = "Player2";
        game.GetComponent<GameBoard>().Board[1, 2].tag = "Player2";
        game.GetComponent<GameBoard>().Board[1, 3].tag = "Player2";
        game.GetComponent<GameBoard>().Board[1, 4].tag = "Player1";
    }
}
