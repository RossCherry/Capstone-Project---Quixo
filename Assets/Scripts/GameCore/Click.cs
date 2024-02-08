using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField]
    private LayerMask objects;

    public bool isPlayerOneTurn = true;
    private bool validMove;
    bool moveInProgress = false;

    GameObject[] possibleMoves;
    private GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
        possibleMoves = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moveInProgress)
        {
            moveInProgress = true;
            HandleClick();
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

            validMove = clickOnScript.gameObject.GetComponent<GamePiece>().CheckPickedPiece();

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
                }
            }
            for(int i = 0; i < possibleMoves.Length;i++)
            {
                possibleMoves[i].GetComponent<ClickOn>().possibleMove = true;
                possibleMoves[i].GetComponent<ClickOn>().ClickMe();
            }
            

            yield return null;
        }
        GameObject move = clickOnScript.GetComponent<GamePiece>().piece;
        MovePiece(piece, move);
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


        DeselectObject();
        moveInProgress = false;
        isPlayerOneTurn = !isPlayerOneTurn;
    }
}
