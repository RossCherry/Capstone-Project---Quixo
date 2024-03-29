using Photon.Chat.Demo;
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

    private Vector3 targetRotation1 = Vector3.zero;
    private Vector3 targetRotation2 = Vector3.zero;
    private Vector3 targetRotation3 = Vector3.zero;
    private Vector3 targetRotation4 = Vector3.zero;
    private Vector3 targetRotation5 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }



    public bool CheckPickedPiece(bool isPlayerOne)
    {
        //Is piece an edge piece
        if ((row == 0 || row == 4) || (col == 0 || col == 4))
        {
            if (GameManager.isPlayerOneCats)
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
            else
            {
                //is piece blank
                if (piece.CompareTag("Blank"))
                {
                    return true;
                }
                //Is it the correct persons piece
                else if (!isPlayerOne && piece.CompareTag("Player1"))
                {
                    return true;
                }
                //Is it the correct persons piece
                else if (isPlayerOne && piece.CompareTag("Player2"))
                {
                    return true;
                }
            }
        }
        
        //It is not a playable piece
        return false;
    }

    public GameObject[] PossibleMoves()
    {
        GameObject[] result;
        bool isCorner = false;

        if (board.IsCornerPiece(piece))
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

            if (col == 0)
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
            if (row != 0 && row != 4)
            {
                result[0] = board.Board[col, 0];
                result[1] = board.Board[col, 4];
                if (col == 0)
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
        SetTargetRotation(newRow, newCol);

        StartCoroutine(MovePieceSmoothly());
    }

    public void MoveOtherPiece()
    {
        Vector3 targetPosition = new Vector3(col * 2, .25f, row * -2);
        StartCoroutine(MoveOtherPieceSmoothly(targetPosition));
    }

    private void SetTargetPosition(int newRow, int newCol)
    {
        //if where you are moving to is a corner piece and you are not moving a corner piece
        if (IsCornerPiece(newRow, newCol) && !IsCornerPiece(row, col))
        {
            if (row == 0)
            {
                targetPosition1 = new Vector3(col * 2, .25f, 2);

                if (newCol > 3)
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(10, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
                else
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (row == 4)
            {
                targetPosition1 = new Vector3(col * 2, .25f, -10);

                if (newCol > 3)
                {
                    targetPosition2 = new Vector3(10, .25f, -10);
                    targetPosition3 = new Vector3(10, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);


                }
                else
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);

                }
            }
            else if (col == 0)
            {
                targetPosition1 = new Vector3(-2, .25f, row * -2);

                if (newRow > 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                    Debug.Log(newRow);
                }
                else
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(newCol * 2, .25f, 2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                    Debug.Log(newRow);
                }
            }
            else if (col == 4)
            {
                targetPosition1 = new Vector3(10, .25f, row * -2);

                if (newRow > 3)
                {
                    targetPosition2 = new Vector3(10, .25f, -10);
                    targetPosition3 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(newCol * 2, .25f, 2);
                    targetPosition4 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
        }
        else
        {
            if (row == 0 && newRow == 4)
            {
                targetPosition1 = new Vector3(col * 2, .25f, 2);

                if (col < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(-2, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                    Debug.Log(newRow);
                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(10, .25f, -10);
                    targetPosition4 = new Vector3(newCol * 2, .25f, -10);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                    Debug.Log(newRow);
                }
            }
            else if (row == 4 && newRow == 0)
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
            else if (col == 0)
            {
                targetPosition1 = new Vector3(-2, .25f, row * -2);

                if (row < 3)
                {
                    targetPosition2 = new Vector3(-2, .25f, 2);
                    targetPosition3 = new Vector3(10, .25f, 2);
                    targetPosition4 = new Vector3(10, .25f, newRow * -2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);

                }
                else
                {
                    targetPosition2 = new Vector3(-2, .25f, -10);
                    targetPosition3 = new Vector3(10, .25f, -10);
                    targetPosition4 = new Vector3(10, .25f, newRow * -2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
            else if (col == 4)
            {
                targetPosition1 = new Vector3(10, .25f, row * -2);

                if (row < 3)
                {
                    targetPosition2 = new Vector3(10, .25f, 2);
                    targetPosition3 = new Vector3(-2, .25f, 2);
                    targetPosition4 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);

                }
                else
                {
                    targetPosition2 = new Vector3(10, .25f, -10);
                    targetPosition3 = new Vector3(-2, .25f, -10);
                    targetPosition4 = new Vector3(-2, .25f, newRow * -2);
                    targetPosition5 = new Vector3(newCol * 2, .25f, newRow * -2);
                }
            }
        }
    }
    
    private void SetTargetRotation(int newRow, int newCol)
    {
        if (row == 0 && newRow == 4)
        {
            targetRotation1 = new Vector3(0, 0, 0);

            if (newCol < 3)
            {
                targetRotation2 = new Vector3(0, 270, 0);
                targetRotation3 = new Vector3(0, 180, 0);
                targetRotation4 = new Vector3(0, 90, 0);

            }
            else
            {
                targetRotation2 = new Vector3(0, 90, 0);
                targetRotation3 = new Vector3(0, 180, 0);
                targetRotation4 = new Vector3(0, 270, 0);

            }
        }
        else if (row == 4 && newRow == 0)
        {
            targetRotation1 = new Vector3(0, 180, 0);

            if (newCol < 3)
            {
                targetRotation2 = new Vector3(0, 270, 0);
                targetRotation3 = new Vector3(0, 0, 0);
                targetRotation4 = new Vector3(0, 90, 0);
            }
            else
            {
                targetRotation2 = new Vector3(0, 90, 0);
                targetRotation3 = new Vector3(0, 0, 0);
                targetRotation4 = new Vector3(0, 270, 0);
            }
        }
        else if (col == 0)
        {
            targetRotation1 = new Vector3(0, 270, 0);

            if (newRow < 3)
            {
                targetRotation2 = new Vector3(0, 0, 0);
                targetRotation3 = new Vector3(0, 90, 0);
                targetRotation4 = new Vector3(0, 180, 0);
            }
            else
            {
                targetRotation2 = new Vector3(0, 180, 0);
                targetRotation3 = new Vector3(0, 90, 0);
                targetRotation4 = new Vector3(0, 0, 0);
            }
        }
        else if (col == 4)
        {
            targetRotation1 = new Vector3(0, 90, 0);

            if (newRow < 3)
            {
                targetRotation2 = new Vector3(0, 0, 0);
                targetRotation3 = new Vector3(0, 270, 0);
                targetRotation4 = new Vector3(0, 180, 0);
            }
            else
            {
                targetRotation2 = new Vector3(0, 180, 0);
                targetRotation3 = new Vector3(0, 270, 0);
                targetRotation4 = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (row == 0)
            {
                targetRotation1 = new Vector3(0, 0, 0);

                if (newCol < 3)
                {
                    targetRotation2 = new Vector3(0, 270, 0);
                    targetRotation3 = new Vector3(0, 180, 0);
                    targetRotation4 = new Vector3(0, 90, 0);

                }
                else
                {
                    targetRotation2 = new Vector3(0, 90, 0);
                    targetRotation3 = new Vector3(0, 180, 0);
                    targetRotation4 = new Vector3(0, 270, 0);

                }

            }
            else
            {
                targetRotation1 = new Vector3(0, 180, 0);

                if (newCol < 3)
                {
                    targetRotation2 = new Vector3(0, 270, 0);
                    targetRotation3 = new Vector3(0, 0, 0);
                    targetRotation4 = new Vector3(0, 90, 0);
                }
                else
                {
                    targetRotation2 = new Vector3(0, 90, 0);
                    targetRotation3 = new Vector3(0, 0, 0);
                    targetRotation4 = new Vector3(0, 270, 0);
                }
            }
        }
        targetRotation5 = targetRotation1;
    }
    IEnumerator MoveOtherPieceSmoothly(Vector3 targetPosition)
    {
        float elapsedTime = .25f;
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
        float speed = 15f;
        Vector3 startingPosition = transform.position;

        transform.localEulerAngles = targetRotation1;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition1, elapsedTime);
            elapsedTime += Time.deltaTime * (speed + .5f);
            yield return null;
        }

        transform.localEulerAngles = targetRotation2;
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(targetPosition1, targetPosition2, elapsedTime);
            elapsedTime += Time.deltaTime * (speed + .5f);
            yield return null;
        }

        transform.localEulerAngles = targetRotation3;
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(targetPosition2, targetPosition3, elapsedTime);
            elapsedTime += Time.deltaTime * (speed);
            yield return null;
        }

        transform.localEulerAngles = targetRotation4;
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(targetPosition3, targetPosition4, elapsedTime);
            elapsedTime += Time.deltaTime * (speed + .5f);
            yield return null;
        }
        

        if (targetPosition5 != Vector3.zero)
        {
            transform.localEulerAngles = targetRotation5;
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                transform.position = Vector3.Lerp(targetPosition4, targetPosition5, elapsedTime);
                elapsedTime += Time.deltaTime * (speed + .5f);
                yield return null;
            }
            //transform.position = targetPosition5;
        }

        targetPosition1 = Vector3.zero;
        targetPosition2 = Vector3.zero;
        targetPosition3 = Vector3.zero;
        targetPosition4 = Vector3.zero;
        targetPosition5 = Vector3.zero;

        MoveOtherPieces();
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<GameManager>().isPlayerOneTurn = !camera.GetComponent<GameManager>().isPlayerOneTurn;
    }

    bool IsCornerPiece(int row, int col)
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

    void MoveOtherPieces()
    {
        board.SetPieces();
        board.MoveOtherPieces();
    }


}
