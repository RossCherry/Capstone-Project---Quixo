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
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{
    private new PhotonView photonView;
    [SerializeField]
    private LayerMask objects;

    //instead of multiple booleans it now uses a single string to determine the type of game
    string typeOfGame;

    public bool isPlayerOneTurn = true;
    public static bool isPlayerOne = true;
    public static bool teamIsSet = false;

    GameObject[] possibleMoves = null;
    private GameObject selectedObject = null;
    public bool moveInProgress = false;

    private bool gameOver = false;
    private bool gameOverWindowOpen = false;
    bool didPlayer1Win = false;
    private GameObject lastPiecePlayed;

    public static bool isCoroutineRunning = false;
    public static bool isPlayerOneCats = true;
    bool onlyDoOnce = false;

    public static bool opponentDisconnected = false;


    static public int moveCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        moveInProgress = false;
        isCoroutineRunning = false;
        if (gameObject.GetComponent<AiEasy>() != null)
        {
            typeOfGame = "easy";
        }
        else if (gameObject.GetComponent<AiHard>() != null)
        {
            typeOfGame = "hard";
        }
        else if (gameObject.GetComponent<NetworkManager>() != null)
        {
            photonView = gameObject.GetComponent<PhotonView>();
            typeOfGame = "network";
        }
        else if (gameObject.GetComponent<GameActions>() != null)
        {
            typeOfGame = "game";
        }

        if(typeOfGame != "network" && typeOfGame != null)
        {
            // Get the starting player
            if (PlayerPrefs.GetInt("IsPlayerOne", 1) == 1)
            {
                isPlayerOneCats = true;
            }
            else
            {
                isPlayerOneCats = false;
            }
            if (!isPlayerOneCats)
            {
                isPlayerOneTurn = false;
            }
        }
       
        GUI_Manager.ShowUserTeam();
        if (typeOfGame == "game")
        {
            GameActions.GameEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameActions.GameEnabled && SceneManager.GetActiveScene().name != "Main Menu")
        {
            if (lastPiecePlayed != null)
            {
                gameOver = lastPiecePlayed.GetComponent<GamePiece>().board.CheckWin(isPlayerOneTurn);
            }
            if (!moveInProgress && !gameOver && !isCoroutineRunning)
            {               
                //AI GAME
                if ((typeOfGame == "easy" || typeOfGame == "hard") && !isPlayerOneTurn)
                {
                    moveInProgress = true;
                    StartCoroutine(WaitForAIMove());
                    //HandleClick();
                    //Debug.Log(isPlayerOne);
                    //Debug.Log(isPlayerOneTurn);
                }
                //LOCAL PLAY
                else if (Input.GetMouseButtonDown(0) && typeOfGame != "network")
                {
                    moveInProgress = true;
                    HandleClick();
                    //Debug.Log(isPlayerOne);
                    //Debug.Log(isPlayerOneTurn);
                }

                //NETWORKING GAME
                if (typeOfGame == "network")
                {
                    UpdateColors();

                    //isPlayerOne = GetComponent<NetworkManager>().getIsPlayerOne();
                    if (Input.GetMouseButtonDown(0) && isPlayerOneTurn && isPlayerOne)
                    {
                        moveInProgress = true;
                        HandleClick();
                    }
                    else if(Input.GetMouseButtonDown(0) && !isPlayerOneTurn && !isPlayerOne)
                    {
                        moveInProgress = true;                        
                        HandleClick();
                    }
                }
            }
            
        }
        //// Deselect Pieces when the GUI is activated
        else if (selectedObject != null)
        {
            UnhighlightPossibleMoves();
        }
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, 2000, objects))
        {
            if (rayHit.collider.CompareTag("npcs"))
            {
                moveInProgress = false;

                Animator animator;
                animator = rayHit.collider.GetComponent<Animator>();
                bool notIsClicked = !animator.GetBool("isClicked");
                animator.SetBool("isClicked", notIsClicked);
            }
        }
        if (gameOver && !moveInProgress && !isCoroutineRunning)
        {
            //if (!isPlayerOneTurn && lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin)
            //{
            //    Debug.Log("Player 1 Wins");
            //    didPlayer1Win = true;
            //}
            //else if ((isPlayerOneTurn && lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin))
            //{
            //    Debug.Log("Player 2 Wins");
            //}
            //else if (isPlayerOneTurn)
            //{
            //    Debug.Log("Player 1 Wins");
            //    didPlayer1Win = true;
            //}
            //else if (!isPlayerOneTurn)
            //{
            //    Debug.Log("Player 2 Wins");
            //}
            if (gameOver && !gameOverWindowOpen)
            {
                gameOverWindowOpen = true;
                //return a menu screen
                if (typeOfGame != "network")
                {
                    if (lastPiecePlayed.GetComponent<GamePiece>().board.wasWinDiagonal)
                    {
                        if (lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Cats");
                        }
                        else
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Dogs");
                        }
                    }
                    else if (isPlayerOneCats)
                    {
                        if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Cats");
                            //Debug.Log("Game Over" + 1);
                        }
                        else if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && !lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Dogs");
                        }
                        else if (!didPlayer1Win && isPlayerOneTurn && lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin)
                        {
                            GameActions.ShowGameOver(Outcome.Loss, "Cats");
                            //Debug.Log("Game Over" + 2);
                        }
                        else
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Dogs");
                            //Debug.Log("Game Over" + 3);
                        }
                    }
                    else
                    {
                        if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Cats");
                            //Debug.Log("Game Over" + 4);
                        }
                        else if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && !lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Dogs");
                            //Debug.Log("Game Over" + 4);
                        }
                        else if (!didPlayer1Win && isPlayerOneTurn && lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin)
                        {
                            GameActions.ShowGameOver(Outcome.Loss, "Dogs");
                            //Debug.Log("Game Over" + 5);
                        }
                        else
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Cats");
                            //Debug.Log("Game Over" + 6);
                        }
                    }

                    //GameActions.ShowGameOver(GetWinner());
                }
                else
                {
                    if (lastPiecePlayed.GetComponent<GamePiece>().board.wasWinDiagonal)
                    {
                        if (lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Cats");
                        }
                        else
                        {
                            GameActions.ShowGameOver(Outcome.Win, "Dogs");
                        }
                    }
                    else if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                    {
                        GameActions.ShowGameOver(Outcome.Win, "Cats");
                        //Debug.Log(7);
                    }
                    else if (!lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin && !lastPiecePlayed.GetComponent<GamePiece>().board.player1Win)
                    {
                        GameActions.ShowGameOver(Outcome.Win, "Dogs");
                        //Debug.Log(7);
                    }
                    else if (isPlayerOneTurn && lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin)
                    {
                        GameActions.ShowGameOver(Outcome.Loss, "Cats");
                        //Debug.Log(8);
                    }
                    else
                    {
                        GameActions.ShowGameOver(Outcome.Win, "Dogs");
                        //Debug.Log(9);
                    }
                }
                GameActions.DisableRequestDrawButton();
            }
        }

        // Show the current player and turn
        GUI_Manager.ShowCurrentPlayer();
        GUI_Manager.ShowTurn();

        // Show the current player's image
        GameObject GuiManager = GameObject.Find("GUI Manager");
        if (GuiManager != null)
        {
            GUI_Manager guiManager = GuiManager.GetComponent<GUI_Manager>();
            guiManager.SetUserImage();
        }

        if(opponentDisconnected)
        {
            GUI_Manager.ShowOpponentDisconnectedDialog();
        }
    }

    private Outcome GetWinner()
    {
        // Move opponent's piece into win
        if (lastPiecePlayed.GetComponent<GamePiece>().board.didOpponentWin)
        {
            //Debug.Log("Game Over" + 1 + "isPlayerOne: " + isPlayerOne + " isPlayerOneTurn: " + isPlayerOneTurn + "isPLayerOneCats: " + isPlayerOneCats);
            return Outcome.Loss;            
        }       
        // Opponent gets 5 in a row
        else if (isPlayerOne == isPlayerOneTurn == isPlayerOneCats)
        {
            //Debug.Log("Game Over" + 2 + "isPlayerOne: " + isPlayerOne + " isPlayerOneTurn: "+ isPlayerOneTurn + "isPLayerOneCats: " + isPlayerOneCats);
            return Outcome.Loss;
        }  
        // Player gets 5 in a row
        else
        {
            //Debug.Log("Game Over" + 3 + "isPlayerOne: " + isPlayerOne + " isPlayerOneTurn: " + isPlayerOneTurn + "isPLayerOneCats: " + isPlayerOneCats);
            return Outcome.Win;
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
            if (clickOnScript != null)
            {
                if (!isPlayerOneCats && typeOfGame != "network")
                {
                    validMove = clickOnScript.GetComponent<GamePiece>().CheckPickedPiece(!isPlayerOneTurn);
                }
                else
                {
                    validMove = clickOnScript.GetComponent<GamePiece>().CheckPickedPiece(isPlayerOneTurn);
                }

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
        if(clickOnScript != null) 
        {
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

            yield return null;
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
        //Debug.Log(piece.ToString());
        if (typeOfGame != "network")
        {
            if (!isPlayerOneCats)
            {
                if (isPlayerOneTurn)
                {
                    piece.GetComponent<GamePiece>().SetPlayer(isPlayerOne);
                }
                else
                {
                    piece.GetComponent<GamePiece>().SetPlayer(!isPlayerOne);
                }
            }
            else
            {
                if (isPlayerOneTurn)
                {
                    piece.GetComponent<GamePiece>().SetPlayer(isPlayerOne);
                }
                else
                {
                    piece.GetComponent<GamePiece>().SetPlayer(!isPlayerOne);
                }
            }
        }
        else
        {
            if(!isPlayerOneCats)
            {
                if (isPlayerOneTurn)
                {
                    piece.GetComponent<GamePiece>().SetPlayer(!isPlayerOne);
                }
                else
                {
                    piece.GetComponent<GamePiece>().SetPlayer(isPlayerOne);
                }
            }
            else
            {
                if (isPlayerOneTurn)
                {
                    piece.GetComponent<GamePiece>().SetPlayer(isPlayerOne);
                }
                else
                {
                    piece.GetComponent<GamePiece>().SetPlayer(!isPlayerOne);
                }
            }

            UpdateColors();
            isCoroutineRunning = true;
            moveInProgress = true;
        }

        piece.GetComponent<GamePiece>().board.MovePiece(piece.GetComponent<GamePiece>(), move.GetComponent<GamePiece>());
        lastPiecePlayed = piece;
        //piece.GetComponent<GamePiece>().board.MovePieces();

        //gameOver = piece.GetComponent<GamePiece>().board.CheckWin(isPlayerOneTurn);
        //moveInProgress = false;


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

        moveCount++;
        DeselectObject();
    }
    IEnumerator WaitForAIMove()
    {
        DateTime before = DateTime.Now;
        isCoroutineRunning = true;
        Tuple<GamePiece, GamePiece, int> aiMove;

        if (typeOfGame == "easy")
        {
            aiMove = gameObject.GetComponent<AiEasy>().AITurn();
        }
        else
        {
            aiMove = gameObject.GetComponent<AiHard>().AITurn();
        }
        //Debug.Log("Player 2 Move: (" + aiMove.Key.row + ", " + aiMove.Key.col + ") to (" + aiMove.Value.row + ", " + aiMove.Value.col + ")");
        if (aiMove.Item1 != aiMove.Item2)
        {
            AiHard.movesSinceLastDraw++;
            //Debug.Log("Move: (" + aiMove.Item1.row + ", " + aiMove.Item1.col + ") to (" + aiMove.Item2.row + ", " + aiMove.Item2.col + "): " + aiMove.Item3);
            MovePiece(aiMove.Item1.gameObject, aiMove.Item2.gameObject);
        }
        else
        {
            moveInProgress = false;
            yield return null;
        }

        //isPlayerOneTurn = true;
        DateTime after = DateTime.Now;
        //Debug.Log(after.Subtract(before).TotalSeconds);
        //GameObject gameBoard = GameObject.Find("GameBoard");
        //MovePiece(gameBoard.GetComponent<GameBoard>().Board[4,0], gameBoard.GetComponent<GameBoard>().Board[4, 1]);
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

    void HandleNpcClick()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, 2000, objects))
        {
            //moveInProgress = false;
            if (rayHit.collider.CompareTag("npcs"))
            {
                //DeselectObject();
                //UnhighlightPossibleMoves();
                //moveInProgress = false;
                //isCoroutineRunning = false;

                Animator animator;
                animator = rayHit.collider.GetComponent<Animator>();
                bool notIsClicked = !animator.GetBool("isClicked");
                animator.SetBool("isClicked", notIsClicked);
            }
        }        
    }

    public void TeamSelect(bool isCats)
    {
        //Debug.Log("TeamSelect() called");
        if (Navigation.SelectedScene == "Networking Game")
        {
            OnTeamSelected(isCats);
        }
        else
        {
            SetStartingPlayer(isCats);
            Navigation.LoadSelectedScene();
        }
    }

    public void SetStartingPlayer(bool isCats)
    {
        if(typeOfGame != "network")
        {
            PlayerPrefs.SetInt("IsPlayerOne", isCats ? 1 : 0);
        }
        isPlayerOne = isCats;
        isPlayerOneCats = isCats;
        teamIsSet = true;
    }

    //Only called by master client
    public void OnTeamSelected(bool isCats)
    {
        //set my team
        SetStartingPlayer(isCats);
        //send opponent's team
        photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("RPC_TeamSelect", RpcTarget.OthersBuffered, !isCats);
        
        NetworkManager.checkToStartGame();
    }

    //Only called by non-master client
    [PunRPC]
    public void RPC_TeamSelect(bool isCats)
    {
        SetStartingPlayer(isCats);
        isPlayerOneCats = isCats;
        //Debug.Log($"isCats = {isCats}");

        NetworkManager.checkToStartGame();
    }

    public void SetFalseOpponentDisconnected()
    {
        opponentDisconnected = false;
    }
}
