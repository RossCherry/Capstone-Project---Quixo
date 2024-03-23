using System.Collections;
using System.Collections.Generic;
using TMPro;

//using TreeEditor;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int row;
    public int col;
    public GameBoard board;
    public GameObject piece;
    public bool isBlank = true;

    private Vector3 targetPosition1 = Vector3.zero;
    private Vector3 targetPosition2 = Vector3.zero;
    private Vector3 targetPosition3 = Vector3.zero;
    private Vector3 targetPosition4 = Vector3.zero;
    private Vector3 targetPosition5 = Vector3.zero;

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

        if(board.IsCornerPiece(piece))
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

    public void MovePiece(int newRow, int newCol)
    {
        SetTargetPosition(newRow, newCol);

        StartCoroutine(MovePieceSmoothly());
        

    }

    public void MovePiece()
    {
        Vector3 targetPosition = new Vector3(col * 2, .25f, row * -2);
        StartCoroutine(MovePieceSmoothly(targetPosition));
    }

    //public void MovePieceSmoothly(Vector3 targetPosition)
    //{
    //    float speed = 1f;
    //    this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, speed);
    //}

    private void SetTargetPosition(int newRow, int newCol)
    {
        if(isCornerPiece(newRow, newCol) && !isCornerPiece(row, col))
        {
            if(row == 0)
            {
                targetPosition1 = new Vector3(col * 2, .25f, 2);

                if (col < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(10, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (row == 4)
            {
                targetPosition1 = new Vector3(col * 2, .25f, -10);

                if (newCol == 0)
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);

                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, -10);
                    targetPosition3 = new Vector3(10, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (col == 0)
            {
                targetPosition1 = new Vector3(-2, .25f, row * -2);

                if(row < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
                else
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (col == 4)
            {

            }
        }
        else
        {
            if (row == 0)
            {
                targetPosition1 = new Vector3(col * 2, .25f, 2);

                if (col < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(-2, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(10, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (row == 4)
            {
                targetPosition1 = new Vector3(col * 2, .25f, -10);

                if (col < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, 2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, 2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);

                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, -10);
                    targetPosition3 = new Vector3(10, .25f, 2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, 2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
        }
       
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
        GameManager.isCoroutineRunning = false;
    }

    IEnumerator MovePieceSmoothly()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition1, elapsedTime);
            elapsedTime += Time.deltaTime * 1;
            yield return null;
        }
        transform.position = targetPosition1;
        startingPosition = targetPosition1;

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition2, elapsedTime);
            elapsedTime += Time.deltaTime * 1;
            yield return null;
        }
        transform.position = targetPosition2;
        startingPosition = targetPosition2;

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition3, elapsedTime);
            elapsedTime += Time.deltaTime * 1;
            yield return null;
        }
        transform.position = targetPosition3;
        startingPosition = targetPosition3;

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition4, elapsedTime);
            elapsedTime += Time.deltaTime * 1;
            yield return null;
        }
        transform.position = targetPosition4;
        startingPosition = targetPosition4;

        if (targetPosition5 != Vector3.zero)
        {
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                transform.position = Vector3.Lerp(startingPosition, targetPosition5, elapsedTime);
                elapsedTime += Time.deltaTime * 1;
                yield return null;
            }
            transform.position = targetPosition5;
        }

        targetPosition1 = Vector3.zero;
        targetPosition2 = Vector3.zero;
        targetPosition3 = Vector3.zero;
        targetPosition4 = Vector3.zero;
        targetPosition5 = Vector3.zero;

        GameManager.isCoroutineRunning = false;
    }

    bool isCornerPiece(int ro, int col)
    {
        bool result = false;
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
        return result;
    }




}
