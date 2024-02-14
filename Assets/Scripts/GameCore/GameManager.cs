using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask objects;

    public bool isPlayerOneTurn = true;
    private bool validMove;
    private bool gameOver = false;
    bool moveInProgress = false;
    bool isAIGame = false;
    bool didPlayer1Win = false;
    bool isNetworkingGame = false;
    GameObject[] possibleMoves;
    private GameObject selectedObject;
    private bool gameOverWindowOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
        possibleMoves = null;
        if (gameObject.GetComponent<AiEasy>() != null)
        {
            isAIGame = true;
        }
        //if (gameObject.GetComponent<Networking>() != null)
        //{
        //    isNetworkingGame = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moveInProgress && !gameOver)
        {
            moveInProgress = true;
            HandleClick();
        }
        if (isAIGame && !isPlayerOneTurn && !moveInProgress && !gameOver)
        {
            moveInProgress = true;
            StartCoroutine(WaitForAIMove());
            
        }
        if (isNetworkingGame && !isPlayerOneTurn && !moveInProgress && !gameOver)
        {
            //run networking protocol
            //if Random.random(Random.Range(0, 1)) == 0
            //    player1 = Firstperson in room
            //pass to last in room !isPlayerOneTurn
        }
        if (gameOver && !gameOverWindowOpen)
        {
            gameOverWindowOpen = true;
            //return a menu screen
            
            if (didPlayer1Win)
            {
                GameActions.ShowGameOver(Outcome.Win, "Player 1");
            }
            else
            {
                GameActions.ShowGameOver(Outcome.Win, "Player 2");
            }
        }

    }

    void HandleClick()
    {
        RaycastHit rayHit;
        GamePiece piece;

        //If mouse is clicked, fire a raycast at where ever the mouse is pointing and store it in rayHit. It will only hit things in the objects layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, 100, objects))
        {
            ClickOn clickOnScript = rayHit.collider.GetComponent<ClickOn>();

            validMove = clickOnScript.gameObject.GetComponent<GamePiece>().CheckPickedPiece(isPlayerOneTurn);

            if (validMove)
            {
                if (selectedObject != null)
                {
                    DeselectObject();
                }

                selectedObject = clickOnScript.gameObject;

                clickOnScript.currentlySelected = true;
                clickOnScript.ClickMe();

                //START OF HIGHLIGHTING POSSIBLE MOVES
                piece = selectedObject.GetComponent<GamePiece>();
                possibleMoves = piece.PossibleMoves();

                

                HighlightPossibleMoves();
                //FINISHED HIGHLIGHTING POSSIBLE MOVES
              
                //START OF MOVE PIECE
                StartCoroutine(WaitForValidMove(clickOnScript.GetComponent<GamePiece>().piece));

                //FINISHING MOVE PIECE
            }
            else
            {
                DeselectObject();
                moveInProgress = false;
            }
            
        }
        else
        {
            
            moveInProgress = false;
        }

        
    }
    void DeselectObject()
    {
        selectedObject.GetComponent<ClickOn>().currentlySelected = false;
        selectedObject.GetComponent<ClickOn>().ClickMe();

        if (possibleMoves != null)
        {
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                possibleMoves[i].GetComponent<ClickOn>().possibleMove = false;
                possibleMoves[i].GetComponent<ClickOn>().ClickMe();
            }
        }
    }
    void HighlightPossibleMoves()
    {
        for (int i = 0; i < possibleMoves.Length; i++)
        {
            possibleMoves[i].GetComponent<ClickOn>().possibleMove = true;
            possibleMoves[i].GetComponent<ClickOn>().ClickMe();
        }
    }
    IEnumerator WaitForValidMove(GameObject piece)
    {
        bool validMove = false;
        bool waitingForClick = true;
        ClickOn clickOnScript = null;
        int counter = 0;

        while (waitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit rayHit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, 100, objects))
                {
                    clickOnScript = rayHit.collider.GetComponent<ClickOn>();

                    if(clickOnScript != null && clickOnScript.possibleMove) 
                    {
                        validMove = ValidateMove(clickOnScript);
                        waitingForClick = !validMove;
                       
                        
                    }
                    else if (counter > 0) 
                    {
                        waitingForClick = false;
                    }
                }
                
            }
            for(int i = 0; i < possibleMoves.Length;i++)
            {
                possibleMoves[i].GetComponent<ClickOn>().possibleMove = true;
                possibleMoves[i].GetComponent<ClickOn>().ClickMe();
            }
            
            counter++;
            yield return null;
        }

        GameObject move = clickOnScript.GetComponent<GamePiece>().piece;
        if (validMove)
        {
            MovePiece(piece, move);
        }
        else
        {
            DeselectObject();
            moveInProgress = false;
        }

    }
    bool ValidateMove(ClickOn clickOnScript)
    {
        bool result = false;

        for(int i = 0; i < possibleMoves.Length; i++)
        {
            if (possibleMoves[i].gameObject.GetComponent<GamePiece>().piece == clickOnScript.gameObject.GetComponent<GamePiece>().piece)
            {
                result = true;
            }
        }

        return result;
    }
    void MovePiece(GameObject piece, GameObject move)
    {
        piece.GetComponent<GamePiece>().SetPlayer(isPlayerOneTurn);

        piece.GetComponent<GamePiece>().board.movePiece(piece.GetComponent<GamePiece>(), move.GetComponent<GamePiece>());

        piece.GetComponent<GamePiece>().board.MovePieces();
        //piece.GetComponent<GamePiece>().SetNewPosition();
        //piece.GetComponent<GamePiece>().MovePiece();

        if (isNetworkingGame)
        {
            //send piece and move
        }

        gameOver = piece.GetComponent<GamePiece>().board.checkWin(isPlayerOneTurn);
        if (gameOver)
        {
            if (!isPlayerOneTurn && piece.gameObject.GetComponent<GamePiece>().board.didOpponentWin)
            {
                Debug.Log("Player 1 Wins");
                didPlayer1Win = true;
            }
            else if((isPlayerOneTurn && piece.gameObject.GetComponent<GamePiece>().board.didOpponentWin))
            {
                Debug.Log("Player 2 Wins");
            }
            else if (isPlayerOneTurn)
            {
                Debug.Log("Player 1 Wins");
                didPlayer1Win = true;
            }
            else if (!isPlayerOneTurn)
            {
                Debug.Log("Player 2 Wins");
            }
        }
        else
        {
            
            moveInProgress = false;
            isPlayerOneTurn = !isPlayerOneTurn;
        }

        DeselectObject();

            GameObject[] AiPieces = GameObject.FindGameObjectsWithTag("Player2");
            foreach (var aiPiece in AiPieces)
            {
                aiPiece.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-03-body", typeof(Material)) as Material;
                aiPiece.transform.GetChild(2).gameObject.SetActive(true);
            }
    }

    IEnumerator WaitForAIMove()
    {
        KeyValuePair<GamePiece, GamePiece> aiMove = gameObject.GetComponent<AiEasy>().AITurn();
        MovePiece(aiMove.Key.gameObject, aiMove.Value.gameObject);
        isPlayerOneTurn = true;
        yield return null;
    }

    IEnumerator WaitForNetworkingMove()
    {
        //get Online Players move();
        //MovePiece(OnlinePlayer.piece, OnlinePlayer.move);
        //isPlayerOneTurn = true;
        yield return null;
    }

}
