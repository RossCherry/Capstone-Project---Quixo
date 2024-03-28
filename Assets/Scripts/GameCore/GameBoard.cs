using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public GameObject[,] Board;
    private const int ROW_COUNT_X = 5;
    private const int COL_COUNT_Y = 5;
    public bool didOpponentWin;
    //public GameObject piecePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GenerateAllPieces();
    }

 

    //Generate Game Board
    private void GenerateAllPieces()
    {
        Board = new GameObject[ROW_COUNT_X, COL_COUNT_Y];
        int index = 0;

        for (int i = 0; i < ROW_COUNT_X; i++)
        {
            for (int j = 0; j < COL_COUNT_Y; j++)
            {
                Transform child = transform.GetChild(index);
                Board[j, i] = child.gameObject;
                index++;
            }
        }
    }

    public bool IsCornerPiece(GameObject piece)
    {
        bool result = false;
        for(int row = 0; row < ROW_COUNT_X; row++)
        {
            for(int col = 0; col < COL_COUNT_Y; col++)
            {
                if (Board[col, row].GetComponent<GamePiece>().piece == piece)
                {
                    if (row == 0 && col == 0)
                    {
                        result = true;
                    }
                    if (row == 0 && col == 4)
                    {
                        result = true;
                    }
                    if (row == 4 && col == 0)
                    {
                        result = true;
                    }
                    if (row == 4 && col == 4)
                    {
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    public void MovePiece(GamePiece piece, GamePiece move)
    {
        //if (gameObject.GetComponent<Click>().isPlayerOneTurn)
        //{
        //    Debug.Log("Player 1");
        //}
        //else
        //{
        //    Debug.Log("Player 2");
        //}
        Debug.Log("Move: (" + piece.row + ", " + piece.col + ") to (" + move.row + ", " + move.col + ")");
        GameObject temp;
        //Shift is row based
        if(piece.row != move.row)
        {
            if(move.row == 0)
            {
                for (int i = piece.row; i > 0; i--)
                {
                    temp = Board[piece.col, i];
                    Board[piece.col, i] = Board[piece.col, i - 1];
                    Board[piece.col, i - 1] = temp;
                }
            }
            else
            {
                for(int i = piece.row; i < 4; i++)
                {
                    temp = Board[piece.col, i];
                    Board[piece.col, i] = Board[piece.col, i + 1];
                    Board[piece.col, i + 1] = temp;
                }
            }
        }
        //Shift is column based
        else
        {

            if (move.col == 0)
            {
                for (int i = piece.col; i > 0; i--)
                {
                    temp = Board[i, piece.row];
                    Board[i, piece.row] = Board[i - 1, piece.row];
                    Board[i - 1, piece.row] = temp;
                }
            }
            else
            {
                for (int i = piece.col; i < 4; i++)
                {
                    temp = Board[i, piece.row];
                    Board[i, piece.row] = Board[i + 1, piece.row];
                    Board[i + 1, piece.row] = temp;
                }
            }
        }

        piece.MovePiece(move.row, move.col);
    }

    public void SetPieces()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<GamePiece>().SetNewPosition();
        }
    }
    public void MoveOtherPieces()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<GamePiece>().MoveOtherPiece();
        }

    }


    void UpdateBoard()
    {
        int index = 0;

        for (int i = 0; i < ROW_COUNT_X; i++)
        {
            for (int j = 0; j < COL_COUNT_Y; j++)
            {
                Transform child = transform.GetChild(index);
                Board[j, i] = child.gameObject;
                index++;
            }
        }


    }

    public Vector3 GetPosition(int row, int col)
    {
        int x = col;
        int y = row;
        return new Vector3(x, 0, y);
    }

    public bool CheckWin(bool isPlayerOnesTurn)
    {
        int player1WinTracker = 0;
        int player2WinTracker = 0;
        int player1RowTracker = 0;
        int player2RowTracker = 0;
        int player1ColTracker = 0;
        int player2ColTracker = 0;

        //Checks diagnals for a win
        for(int i = 0;i < 5; i++)
        {
            if (Board[i, i].CompareTag("Player1"))
            {
                player1RowTracker++;
            }
            else if (Board[i, i].CompareTag("Player2"))
            {
                player2RowTracker++;
            }

            if (Board[(4-i), i].CompareTag("Player1"))
            {
                player1ColTracker++;
            }
            else if (Board[(4-i), i].CompareTag("Player2"))
            {
                player2ColTracker++;
            }           
        }

        if (player1RowTracker == 5 || player1ColTracker == 5)
        {
            if (!isPlayerOnesTurn)
            {
                didOpponentWin = true;
            }
            return true;
        }
        else if (player2RowTracker == 5 || player2ColTracker == 5)
        {
            if (isPlayerOnesTurn)
            {
                didOpponentWin = true;
            }
            return true;
        }

        player1RowTracker = 0;
        player2RowTracker = 0;
        player1ColTracker = 0;
        player2ColTracker = 0;

        //checks rows and columns for a win
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Board[j, i].CompareTag("Player1"))
                {
                    player1RowTracker++;
                }
                else if (Board[j, i].CompareTag("Player2"))
                {
                    player2RowTracker++;
                }

                if (Board[i, j].CompareTag("Player1"))
                {
                    player1ColTracker++;
                }
                else if (Board[i, j].CompareTag("Player2"))
                {
                    player2ColTracker++;
                }
            }
            if (player1RowTracker == 5 || player1ColTracker == 5)
            {
                player1WinTracker++;
            }
            else if (player2RowTracker == 5 || player2ColTracker == 5)
            {
                player2WinTracker++;
            }
            player1RowTracker = 0;
            player2RowTracker = 0;
            player1ColTracker = 0;
            player2ColTracker = 0;
        }

        if (isPlayerOnesTurn && player1WinTracker > 0 && player2WinTracker == 0)
        {
            return true;
        }
        else if (!isPlayerOnesTurn && player1WinTracker == 0 && player2WinTracker > 0)
        {
            return true;
        }
        else if (player1WinTracker > 0 && player2WinTracker > 0)
        {
            didOpponentWin = true;
            return true;
        }
        return false;
    }

    public GameObject FindPiece(int row, int col)
    {
        return Board[col, row];
    }

    //private GameObject GenerateSinglePiece(int row, int col)
    //{
    //    GameObject piece = Instantiate(piecePrefab, new Vector3(row * 2.0f, 0, col * 2.0f), Quaternion.identity);

    //    piece.GetComponent<GamePiece>().row = row;
    //    piece.GetComponent<GamePiece>().col = col;
    //    piece.GetComponent <GamePiece>().board = this;
    //    piece.GetComponent<GamePiece>().piece = piece;

    //    piece.tag = "Blank";
    //    piece.layer = 6;

    //    return piece;
    //}
}
