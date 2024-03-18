using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private new PhotonView photonView;
    //private static PhotonView photonView = new PhotonView();
    [SerializeField]
    private LayerMask objects;

    //instead of multiple booleans it now uses a single string to determin the type of game
    string typeOfGame;

    public bool isPlayerOneTurn = true;
    public bool isPlayerOne = true;

    GameObject[] possibleMoves = null;
    private GameObject selectedObject = null;
    bool moveInProgress = false;

    private bool gameOver = false;
    private bool gameOverWindowOpen = false;
    bool didPlayer1Win = false;

    private Tutorial tutorial;
    private bool hasTutorialSetNext = false;
    public static bool isCoroutineRunning = false;
    public static bool isPlayerOneCats = true;
    bool onlyDoOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<AiEasy>() != null)
        {
            typeOfGame = "easy";
        }
        if (gameObject.GetComponent<AiHard>() != null)
        {
            typeOfGame = "hard";
        }
        if (gameObject.GetComponent<NetworkManager>() != null)
        {
            photonView = gameObject.GetComponent<PhotonView>();
            typeOfGame = "network";
        }
        if (GetComponent<Tutorial>() != null)
        {
            tutorial = GetComponent<Tutorial>();
            typeOfGame = "tutorial";

        }
        if (!isPlayerOneCats)
        {
            isPlayerOneTurn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Tutorial Counter", 0) == 1 && !hasTutorialSetNext && typeOfGame == "tutorial")
        {
            tutorial.ResetBoard();
            tutorial.BothPlayersCanWin();
            GameObject[] AiPieces = GameObject.FindGameObjectsWithTag("Player2");
            foreach (var aiPiece in AiPieces)
            {
                aiPiece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-03-body", typeof(Material)) as Material;
                aiPiece.transform.GetChild(2).gameObject.SetActive(true);
            }
            AiPieces = GameObject.FindGameObjectsWithTag("Player1");
            foreach (var aiPiece in AiPieces)
            {
                aiPiece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-01-body", typeof(Material)) as Material;
                aiPiece.transform.GetChild(1).gameObject.SetActive(true);
            }
            hasTutorialSetNext = true;
            PlayerPrefs.DeleteKey("Tutorial Counter");
            PlayerPrefs.Save();
        }
        if (GameActions.GameEnabled)
        {
            if (!moveInProgress && !gameOver && !isCoroutineRunning)
            {
                
                //AI GAME
                if ((typeOfGame == "easy" || typeOfGame == "hard") && !isPlayerOneTurn)
                {
                    moveInProgress = true;
                    StartCoroutine(WaitForAIMove());
                }
                //LOCAL PLAY
                else if (Input.GetMouseButtonDown(0) && typeOfGame != "network")
                {
                    moveInProgress = true;
                    HandleClick();
                }

                //NETWORKING GAME
                //TODO what happens if one quits
                if (typeOfGame == "network")
                {
                    if (onlyDoOnce && !isPlayerOneTurn && !isPlayerOne)
                    {
                        onlyDoOnce = false;
                        UpdateColors();
                    }
                    isPlayerOne = GetComponent<NetworkManager>().getIsPlayerOne();
                    if (Input.GetMouseButtonDown(0) && isPlayerOneTurn && isPlayerOne)
                    {
                        moveInProgress = true;
                        HandleClick();
                    }
                    else if(Input.GetMouseButtonDown(0) && !isPlayerOneTurn && !isPlayerOne)
                    {
                        moveInProgress = true;                        
                        HandleClick();
                        onlyDoOnce = true;
                    }
                }
            }
        }
        //// Deselect Pieces when the GUI is activated
        else if (selectedObject != null)
        {
            UnhighlightPossibleMoves();
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
        if (typeOfGame == "tutorial" && gameOver)
        {
            PlayerPrefs.SetInt("Tutorial Counter", 1);
            PlayerPrefs.Save();
        }
    }

    void HandleClick()
    {
        GamePiece piece;
        bool validMove;

        //If mouse is clicked, fire a raycast at where ever the mouse is pointing and store it in rayHit. It will only hit things in the objects layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, 100, objects))
        {
            ClickOn clickOnScript = rayHit.collider.GetComponent<ClickOn>();

            validMove = clickOnScript.GetComponent<GamePiece>().CheckPickedPiece(isPlayerOneTurn);

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
    void UnhighlightPossibleMoves()
    {
        if (possibleMoves != null)
        {
            DeselectObject();

            // Remove all entries from the possible moves array without setting it to null
            possibleMoves = new GameObject[0];
            HighlightPossibleMoves();
        }

    }
    IEnumerator WaitForValidMove(GameObject piece)
    {
        isCoroutineRunning = true;
        bool validMove = false;
        bool waitingForClick = true;
        ClickOn clickOnScript = null;
        int counter = 0;

        while (waitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, 100, objects))
                {
                    clickOnScript = rayHit.collider.GetComponent<ClickOn>();

                    if (clickOnScript != null && clickOnScript.possibleMove)
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
            for (int i = 0; i < possibleMoves.Length; i++)
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
            isCoroutineRunning = false;
            moveInProgress = false;
        }

    }
    bool ValidateMove(ClickOn clickOnScript)
    {
        bool result = false;

        for (int i = 0; i < possibleMoves.Length; i++)
        {
            if (possibleMoves[i].GetComponent<GamePiece>().piece == clickOnScript.GetComponent<GamePiece>().piece)
            {
                result = true;
            }
        }

        return result;
    }
    public void MovePiece(GameObject piece, GameObject move)
    {
        if (typeOfGame == "network")
        {
            int pieceRow = piece.GetComponent<GamePiece>().row;
            int pieceCol = piece.GetComponent<GamePiece>().col;
            int moveRow = move.GetComponent<GamePiece>().row;
            int moveCol = move.GetComponent<GamePiece>().col; 
            photonView.RPC("RPC_MovePiece", RpcTarget.All, pieceRow, pieceCol, moveRow, moveCol);
        }
        else
        {            
            MakePieceMove(piece, move);
        }
    }

    [PunRPC]
    public void RPC_MovePiece(int pieceRow, int pieceCol, int moveRow, int moveCol)
    {
        GameObject board = GameObject.Find("GameBoard");
        GameObject piece = board.GetComponent<GameBoard>().FindPiece(pieceRow, pieceCol);
        GameObject move = board.GetComponent<GameBoard>().FindPiece(moveRow, moveCol);

        MakePieceMove(piece, move);
    }
    public void MakePieceMove(GameObject piece, GameObject move)
    {
        Debug.Log(piece.ToString());
        if (!isPlayerOneCats)
        {
            if (isPlayerOneTurn)
            {
                piece.GetComponent<GamePiece>().SetPlayer(false);
            }
            else
            {
                piece.GetComponent<GamePiece>().SetPlayer(true);
            }
        }
        else 
        {
            piece.GetComponent<GamePiece>().SetPlayer(isPlayerOneTurn);

        }

        piece.GetComponent<GamePiece>().board.MovePiece(piece.GetComponent<GamePiece>(), move.GetComponent<GamePiece>());

        piece.GetComponent<GamePiece>().board.MovePieces();

        gameOver = piece.GetComponent<GamePiece>().board.CheckWin(isPlayerOneTurn);
        if (gameOver)
        {
            if (!isPlayerOneTurn && piece.GetComponent<GamePiece>().board.didOpponentWin)
            {
                Debug.Log("Player 1 Wins");
                didPlayer1Win = true;
            }
            else if ((isPlayerOneTurn && piece.GetComponent<GamePiece>().board.didOpponentWin))
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

        if (typeOfGame == "easy" || typeOfGame == "hard")
        {
            if (isPlayerOneCats)
            {
                GameObject[] AiPieces = GameObject.FindGameObjectsWithTag("Player2");
                foreach (var aiPiece in AiPieces)
                {
                    aiPiece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-03-body", typeof(Material)) as Material;
                    aiPiece.transform.GetChild(2).gameObject.SetActive(true);
                }
            } 
            else 
            {
                GameObject[] AiPieces = GameObject.FindGameObjectsWithTag("Player1");
                foreach (var aiPiece in AiPieces)
                {
                    aiPiece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-02-body", typeof(Material)) as Material;
                    aiPiece.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
    IEnumerator WaitForAIMove()
    {
        isCoroutineRunning = true;
        KeyValuePair<GamePiece, GamePiece> aiMove;
        if (typeOfGame == "easy")
        {
            aiMove = gameObject.GetComponent<AiEasy>().AITurn();
        }
        else
        {
            aiMove = gameObject.GetComponent<AiHard>().AITurn();
        }
        //Debug.Log("Player 2 Move: (" + aiMove.Key.row + ", " + aiMove.Key.col + ") to (" + aiMove.Value.row + ", " + aiMove.Value.col + ")");
        MovePiece(aiMove.Key.gameObject, aiMove.Value.gameObject);
        isPlayerOneTurn = true;
        yield return null;
    }

    void UpdateColors()
    {
        GameObject[] p1Pieces = GameObject.FindGameObjectsWithTag("Player1");
        foreach (var p1Piece in p1Pieces)
        {
            p1Piece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-02-body", typeof(Material)) as Material;
            p1Piece.transform.GetChild(1).gameObject.SetActive(true);
        }
        GameObject[] p2Pieces = GameObject.FindGameObjectsWithTag("Player2");
        foreach (var p2Piece in p2Pieces)
        {
            p2Piece.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-03-body", typeof(Material)) as Material;
            p2Piece.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    //IEnumerator WaitForNetworkingMove()
    //{
    //    //get Online Players move();
    //    //MovePiece(OnlinePlayer.piece, OnlinePlayer.move);
    //    //isPlayerOneTurn = true;
    //    yield return null;
    //}

}
