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
    //public GameObject piecePrefab;
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

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                
                Debug.Log(Board[j, i] + ": " + i + ", " + j);
            }
            Debug.Log("");
        }
    }

    public bool isCornerPiece(GameObject piece)
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

    public void movePiece(GamePiece piece, GamePiece move)
    {
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
        //Destroy(temp);
        //UpdateBoard();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                
                Debug.Log(Board[j, i] + ": " + i + ", " + j);
            }
            Debug.Log("");
        }
    }

    public void MovePieces()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<GamePiece>().SetNewPosition();
        }

        foreach(Transform child in transform)
        {

            child.GetComponent<GamePiece>().MovePiece();
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
