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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckPickedPiece()
    {
        //Is piece an edge piece
        if ((row == 0 || row == 4) || (col == 0 || col == 4))
        {
            //is piece blank
            if (piece.CompareTag("-"))
            {
                return true;
            }
            //Is it the correct persons piece
            else if (board.isPlayerOne && piece.CompareTag("X"))
            {
                return true;
            }
            //Is it the correct persons piece
            else if (!board.isPlayerOne && piece.CompareTag("O"))
            {
                return true;
            }

        }
        //It is not a playable piece
        return false;
    }
}
