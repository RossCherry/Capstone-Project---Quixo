using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public GameObject[,] Board;
    private const int ROW_COUNT_X = 5;
    private const int COL_COUNT_Y = 5;
    public GameObject piecePrefab;
    public bool isPlayerOne = true;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllPieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generate Game Board
    private void GenerateAllPieces()
    {
        Board = new GameObject[ROW_COUNT_X, COL_COUNT_Y];
        for (int i = 0; i < ROW_COUNT_X; i++)
        {
            for (int j = 0; j < COL_COUNT_Y; j++)
            {
                Board[i, j] = GenerateSinglePiece(i, j);
            }
        }
    }

    private GameObject GenerateSinglePiece(int row, int col)
    {
        GameObject piece = Instantiate(piecePrefab, new Vector3(row * 2.0f, 0, col * 2.0f), Quaternion.identity);
        piece.GetComponent<GamePiece>().row = row;
        piece.GetComponent<GamePiece>().col = col;
        piece.GetComponent <GamePiece>().board = this;
        piece.GetComponent<GamePiece>().piece = piece;
        piece.tag = "-";
        return piece;
    }
}
