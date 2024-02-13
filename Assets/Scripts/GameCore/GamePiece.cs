using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int row;
    public int col;
    public GameBoard board;
    public GameObject piece;
    public bool isBlank = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    public bool CheckPickedPiece(bool isPlayerOne)
    {
        //Is piece an edge piece
        if ((row == 0 || row == 4) || (col == 0 || col == 4))
        {
            //is piece blank
            if (piece.CompareTag("Blank"))
            {
                return true;
            }
            //Is it the correct persons piece
            else if (isPlayerOne && piece.CompareTag("Player1"))
            {
                return true;
            }
            //Is it the correct persons piece
            else if (!isPlayerOne && piece.CompareTag("Player2"))
            {
                return true;
            }

        }
        //It is not a playable piece
        return false;
    }

    public GameObject[] PossibleMoves()
    {
        GameObject[] result;
        bool isCorner = false;

        if(board.isCornerPiece(piece))
        {
            isCorner = true;
            result = new GameObject[2];
        }
        else
        {
            result = new GameObject[3];
        }

        if (isCorner)
        {
            if (row == 0)
            {
                result[0] = board.Board[col, 4];
            }
            else
            {
                result[0] = board.Board[col, 0];
            }

            if(col == 0)
            {
                result[1] = board.Board[4, row];
            }
            else
            {
                result[1] = board.Board[0, row];
            }
        }
        else
        {
            if(row != 0 && row != 4)
            {
                result[0] = board.Board[col, 0];
                result[1] = board.Board[col, 4];
                if(col == 0)
                {
                    result[2] = board.Board[4, row];
                }
                else
                {
                    result[2] = board.Board[0, row];
                }
            }
            else
            {
                result[0] = board.Board[0, row];
                result[1] = board.Board[4, row];
                if (row == 0)
                {
                    result[2] = board.Board[col, 4];
                }
                else
                {
                    result[2] = board.Board[col, 0];
                }
            }
        }

        return result;
    }

    public void SetPlayer(bool isPlayerOneTurn)
    {
        isBlank = false;
        if (isPlayerOneTurn)
        {
            transform.tag = "Player1";
            
        }
        else
        {
            transform.tag = "Player2";
        }
    }

    public void SetNewPosition()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (piece == board.Board[i, j].GetComponent<GamePiece>().piece)
                {
                    row = j;
                    col = i;
                    
                }
            }
        }
    }

    public void MovePiece()
    {
        Vector3 targetPosition = new Vector3(col * 2, 0, row * -2);
        StartCoroutine(MovePieceSmoothly(targetPosition));
    }

    IEnumerator MovePieceSmoothly(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * 1;
            yield return null;
        }

        transform.position = targetPosition; // Ensure the final position is exact
    }

   


}
