using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static int counter = 0;
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ResetBoard()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                game.GetComponent<GameBoard>().Board[j, i].tag = "Blank";
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
