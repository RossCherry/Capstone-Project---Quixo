using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


public class AiEasy : MonoBehaviour
{


    public GameObject[,] Board;
    public GameObject game;
    public bool didOpponentWin;
    public GameObject[,] tempGame = new GameObject[5, 5];



    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameBoard");
    }

    public bool IsCornerPiece(GameObject piece)
    {
        bool result = false;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (tempGame[col, row].GetComponent<GamePiece>().piece == piece)
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


    public GameObject[] PossibleMoves(GameObject piece)
    {
        GameObject[] result;
        bool isCorner = false;

        if (IsCornerPiece(piece))
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
            if (piece.GetComponent<GamePiece>().row == 0)
            {
                result[0] = tempGame[piece.GetComponent<GamePiece>().col, 4];
            }
            else
            {
                result[0] = tempGame[piece.GetComponent<GamePiece>().col, 0];
            }

            if (piece.GetComponent<GamePiece>().col == 0)
            {
                result[1] = tempGame[4, piece.GetComponent<GamePiece>().row];
            }
            else
            {
                result[1] = tempGame[0, piece.GetComponent<GamePiece>().row];
            }
        }
        else
        {
            if (piece.GetComponent<GamePiece>().row != 0 && piece.GetComponent<GamePiece>().row != 4)
            {
                result[0] = tempGame[piece.GetComponent<GamePiece>().col, 0];
                result[1] = tempGame[piece.GetComponent<GamePiece>().col, 4];
                if (piece.GetComponent<GamePiece>().col == 0)
                {
                    result[2] = tempGame[4, piece.GetComponent<GamePiece>().row];
                }
                else
                {
                    result[2] = tempGame[0, piece.GetComponent<GamePiece>().row];
                }
            }
            else
            {
                result[0] = tempGame[0, piece.GetComponent<GamePiece>().row];
                result[1] = tempGame[4, piece.GetComponent<GamePiece>().row];
                if (piece.GetComponent<GamePiece>().row == 0)
                {
                    result[2] = tempGame[piece.GetComponent<GamePiece>().col, 4];
                }
                else
                {
                    result[2] = tempGame[piece.GetComponent<GamePiece>().col, 0];
                }
            }
        }

        return result;
    }


    public void movePiece(GamePiece piece, GamePiece move)
    {
        GameObject temp;
        //Shift is row based
        if (piece.row != move.row)
        {
            if (move.row < piece.row)
            {
                for (int i = piece.row; i > move.row; i--)
                {
                    temp = tempGame[piece.col, i];
                    tempGame[piece.col, i] = tempGame[piece.col, i - 1];
                    tempGame[piece.col, i - 1] = temp;
                }
            }
            else
            {
                for (int i = piece.row; i < move.row; i++)
                {
                    temp = tempGame[piece.col, i];
                    tempGame[piece.col, i] = tempGame[piece.col, i + 1];
                    tempGame[piece.col, i + 1] = temp;
                }
            }
        }
        //Shift is column based
        else
        {

            if (move.col < piece.col)
            {
                for (int i = piece.col; i > move.col; i--)
                {
                    temp = tempGame[i, piece.row];
                    tempGame[i, piece.row] = tempGame[i - 1, piece.row];
                    tempGame[i - 1, piece.row] = temp;
                }
            }
            else
            {
                for (int i = piece.col; i < move.col; i++)
                {
                    temp = tempGame[i, piece.row];
                    tempGame[i, piece.row] = tempGame[i + 1, piece.row];
                    tempGame[i + 1, piece.row] = temp;
                }
            }
        }
    }

    public bool checkWin(bool isPlayerOnesTurn)
    {
        didOpponentWin = false;
        int player1WinTracker = 0;
        int player2WinTracker = 0;
        int player1RowTracker = 0;
        int player2RowTracker = 0;
        int player1ColTracker = 0;
        int player2ColTracker = 0;

        //Checks diagnals for a win
        for (int i = 0; i < 5; i++)
        {
            if (tempGame[i, i].CompareTag("Player1"))
            {
                player1RowTracker++;
            }
            else if (tempGame[i, i].CompareTag("Player2"))
            {
                player2RowTracker++;
            }

            if (tempGame[(4 - i), i].CompareTag("Player1"))
            {
                player1ColTracker++;
            }
            else if (tempGame[(4 - i), i].CompareTag("Player2"))
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
                if (tempGame[j, i].CompareTag("Player1"))
                {
                    player1RowTracker++;
                }
                else if (tempGame[j, i].CompareTag("Player2"))
                {
                    player2RowTracker++;
                }

                if (tempGame[i, j].CompareTag("Player1"))
                {
                    player1ColTracker++;
                }
                else if (tempGame[i, j].CompareTag("Player2"))
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

    GamePiece[] AvailablePieces()
    {
        //I Initialize the Array size to 16 because that's the max amount of moves we can ever have
        //Im still figuiring out how to make the size of the array change depending on the available moves, and not be just a predetermined size
        List<GamePiece> avaMoves = new List<GamePiece>();
        //Coordinate[] avaMoves = new Coordinate[16];
        //GamePiece temp = new GamePiece();
        //temp.row = 0;
        //temp.col = 0;

        int counter = 0;

        //System.Collections.IEnumerator myEnumerator = Board.GetEnumerator();
        //int rows = 0;
        //int cols = Board.GetLength(Board.Rank - 1);
        //int row = 0;
        //int col = -1;
        //Iterate through the entire board array obtaining the coords of each piece
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                if (c == 0 || r == 0 || c == 4 || r == 4)
                {
                    if(gameObject.GetComponent<GameManager>().isPlayerOneTurn)
                {
                        if (Board[c, r].tag == "Player1" || Board[c, r].tag == "Blank")
                        {
                            //Console.WriteLine("X(" + row + "," + col + ")");
                            //temp.row = r;
                            //temp.col = c;
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    else
                    {
                        if (Board[c, r].tag == "Player2" || Board[c, r].tag == "Blank")
                        {
                            //Console.WriteLine("O(" + row + "," + col + ")");
                            //temp.row = r;
                            //temp.col = c;
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    //avaMoves.Add(temp);
                    counter++;
                }
            }
        }

        //while (myEnumerator.MoveNext())
        //{
        //    if (rows < cols)
        //    {
        //        rows++;
        //        col++;
        //    }
        //    else
        //    {
        //        rows = 1;
        //        col = 0;
        //        row++;
        //    }

        //    //Check if the piece is an outer piece
        //    if (col == 0 || row == 0 || col == cols - 1 || row == cols - 1)
        //    {
        //        //Check which pieces are of the player or blank
        //        if (gameObject.GetComponent<Click>().isPlayerOneTurn)
        //        {
        //            if (Board[row, col].tag == "Player1" || Board[row, col].tag == "Blank")
        //            {
        //                //Console.WriteLine("X(" + row + "," + col + ")");
        //                temp.row = row;
        //                temp.col = col;
        //            }
        //        }
        //        else
        //        {
        //            if (Board[row, col].tag == "Player2" || Board[row, col].tag == "Blank")
        //            {
        //                //Console.WriteLine("O(" + row + "," + col + ")");
        //                temp.row = row;
        //                temp.col = col;
        //            }
        //        }
        //        //insert the coord into the resulting array
        //        avaMoves.Add(temp);
        //        counter++;
        //    }
        //}
        //Here I'm initializing the rest of the coords missing in the array to (-1,-1)
        //Ideally I would like to just resize the array to have the same size as the amount of available moves
        //for (int i = counter; i < 16; i++)
        //{
        //    temp.row = -1;
        //    temp.col = -1;
        //    avaMoves[i] = temp;
        //}
        return avaMoves.ToArray();
    }

    int CheckBoardValue(GamePiece piece, GamePiece move)
    {

        string tag = piece.tag;
        int value = 0;
        KeyValuePair<int, int> tempPiece = new KeyValuePair<int, int>(piece.row, piece.col);
        KeyValuePair<int, int> tempMove = new KeyValuePair<int, int>(move.row, move.col);
        List<GamePiece> tg = new List<GamePiece>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                tempGame[i, j] = Board[i, j];
                tg.Add(tempGame[i, j].GetComponent<GamePiece>());
            }
        }
        List<GamePiece> X = new List<GamePiece>();
        List<GamePiece> O = new List<GamePiece>();

        //Debug.Log("-------------------------------------------------------------------");
        //for (int r = 0; r < tempGame.GetLength(0); r++)
        //{
        //    for (int c = 0; c < tempGame.GetLength(0); c++)
        //    {
        //        Debug.Log(r + ", " + c + " (" + tempGame[r, c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
        //    }
        //}
        //Debug.Log("-------------------------------------------------------------------");
        piece.SetPlayer(gameObject.GetComponent<GameManager>().isPlayerOneTurn);
        movePiece(piece, move);
        //Debug.Log("Moving Piece: 1(" + piece.col + "," + piece.row + ")");
        //Debug.Log("Here: 1(" + move.col + "," + move.row + ")");
        //Debug.Log("-------------------------------------------------------------------");
        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log(tempGame[i, 0].GetComponent<GamePiece>().tag + " " + tempGame[i, 1].GetComponent<GamePiece>().tag + " " + tempGame[i, 2].GetComponent<GamePiece>().tag + " " + tempGame[i, 3].GetComponent<GamePiece>().tag + " " + tempGame[i, 4].GetComponent<GamePiece>().tag);
        //}
        //Debug.Log("-------------------------------------------------------------------");


        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
                //Debug.Log(r + ", " + c + " (" + tempGame[r,c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
                if ((tempGame[c, r].CompareTag("Player1")))
                {
                    X.Add(tempGame[c, r].GetComponent<GamePiece>());
                }
                else if ((tempGame[c, r].CompareTag("Player2")))
                {
                    O.Add(tempGame[c, r].GetComponent<GamePiece>());
                }
            }
        }
        //foreach (var item in tempGame)
        //{
        //    Debug.Log("(" + item.GetComponent<GamePiece>().col + ", " + item.GetComponent<GamePiece>().row + "): " + item.GetComponent<GamePiece>().tag);
        //}
        //X = tg.FindAll(t => t.GetComponent<GamePiece>().tag == "Player1");
        //O = tg.FindAll(t => t.GetComponent<GamePiece>().tag == "Player2");

        //Debug.Log("X");
        //foreach (var cube in X)
        //{
        //    Debug.Log("X Piece: (" + cube.col + "," + cube.row + ")");
        //}
        //Debug.Log("O");
        //foreach (var cube in O)
        //{
        //    Debug.Log("O Piece: (" + cube.col + "," + cube.row + ")");
        //}
        //Debug.Log("-------------------------------------------------------------------");

        if (gameObject.GetComponent<GameManager>().isPlayerOneTurn)
        {
            foreach (var xPiece in X)
            {
                switch (xPiece.row)
                {
                    case 0:
                        switch (xPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 2; break;
                        }
                        break;
                    case 1:
                        switch (xPiece.col)
                        {
                            case 0: value += 1; break;
                            case 1: value += 4; break;
                            case 2: value += 6; break;
                            case 3: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 2:
                        switch (xPiece.col)
                        {
                            case 0: value += 4; break;
                            case 1: value += 5; break;
                            case 2: value += 7; break;
                            case 3: value += 5; break;
                            case 4: value += 4; break;
                        }
                        break;
                    case 3:
                        switch (xPiece.col)
                        {
                            case 0: value += 1; break;
                            case 1: value += 4; break;
                            case 2: value += 6; break;
                            case 3: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 4:
                        switch (xPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 2; break;
                        }
                        break;
                }

            }
            foreach (var oPiece in O)
            {
                switch (oPiece.row)
                {

                    case 0:
                        switch (oPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 2; break;
                        }
                        break;
                    case 1:
                        switch (oPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 1: value -= 4; break;
                            case 2: value -= 6; break;
                            case 3: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 2:
                        switch (oPiece.col)
                        {
                            case 0: value -= 4; break;
                            case 1: value -= 5; break;
                            case 2: value -= 7; break;
                            case 3: value -= 5; break;
                            case 4: value -= 4; break;
                        }
                        break;
                    case 3:
                        switch (oPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 1: value -= 4; break;
                            case 2: value -= 6; break;
                            case 3: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 4:
                        switch (oPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 2; break;
                        }
                        break;
                }
            }
        }
        else
        {
            foreach (var oPiece in O)
            {
                switch (oPiece.row)
                {
                    case 0:
                        switch (oPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 2; break;
                        }
                        break;
                    case 1:
                        switch (oPiece.col)
                        {
                            case 0: value += 1; break;
                            case 1: value += 4; break;
                            case 2: value += 6; break;
                            case 3: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 2:
                        switch (oPiece.col)
                        {
                            case 0: value += 4; break;
                            case 1: value += 5; break;
                            case 2: value += 7; break;
                            case 3: value += 5; break;
                            case 4: value += 4; break;
                        }
                        break;
                    case 3:
                        switch (oPiece.col)
                        {
                            case 0: value += 1; break;
                            case 1: value += 4; break;
                            case 2: value += 6; break;
                            case 3: value += 4; break;
                            case 4: value += 1; break;
                        }
                        break;
                    case 4:
                        switch (oPiece.col)
                        {
                            case 0: value += 2; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 2; break;
                        }
                        break;
                }

            }
            foreach (var xPiece in X)
            {
                switch (xPiece.row)
                {

                    case 0:
                        switch (xPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 2; break;
                        }
                        break;
                    case 1:
                        switch (xPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 1: value -= 4; break;
                            case 2: value -= 6; break;
                            case 3: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 2:
                        switch (xPiece.col)
                        {
                            case 0: value -= 4; break;
                            case 1: value -= 5; break;
                            case 2: value -= 7; break;
                            case 3: value -= 5; break;
                            case 4: value -= 4; break;
                        }
                        break;
                    case 3:
                        switch (xPiece.col)
                        {
                            case 0: value -= 1; break;
                            case 1: value -= 4; break;
                            case 2: value -= 6; break;
                            case 3: value -= 4; break;
                            case 4: value -= 1; break;
                        }
                        break;
                    case 4:
                        switch (xPiece.col)
                        {
                            case 0: value -= 2; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 2; break;
                        }
                        break;
                }
            }
        }


        if (gameObject.GetComponent<GameManager>().isPlayerOneTurn)
        {
            var sameRow = X.FindAll(t => t.row == move.row);
            var sameCol = X.FindAll(t => t.col == move.col);
            var diag1 = X.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = X.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count;
            value += sameCol.Count;
            value += diag1.Count;
            value += diag2.Count;

            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }

            //var enemyRow = O.FindAll(t => t.row == move.row);
            //var enemyCol = O.FindAll(t => t.col == move.col);
            //value += enemyRow.Count;
            //value += enemyCol.Count;

            var eRow0 = O.FindAll(t => t.row == 0);
            var eRow1 = O.FindAll(t => t.row == 1);
            var eRow2 = O.FindAll(t => t.row == 2);
            var eRow3 = O.FindAll(t => t.row == 3);
            var eRow4 = O.FindAll(t => t.row == 4);

            var eCol0 = O.FindAll(t => t.col == 0);
            var eCol1 = O.FindAll(t => t.col == 1);
            var eCol2 = O.FindAll(t => t.col == 2);
            var eCol3 = O.FindAll(t => t.col == 3);
            var eCol4 = O.FindAll(t => t.col == 4);

            var ediag1 = O.FindAll(t => t.col == t.row && t.col == move.col);
            var ediag2 = O.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            if (ediag1.Count >= 4 || ediag2.Count >= 4 || eRow0.Count >= 4 || eRow1.Count >= 4 || eRow2.Count >= 4 || eRow3.Count >= 4 || eRow4.Count >= 4 || eCol0.Count >= 4 || eCol1.Count >= 4 || eCol2.Count >= 4 || eCol3.Count >= 4 || eCol4.Count >= 4)
            {
                value -= 1000;
            }
            else
            {
                value += 100;
            }

        }
        else
        {
            var sameRow = O.FindAll(t => t.row == move.row);
            var sameCol = O.FindAll(t => t.col == move.col);
            var diag1 = O.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = O.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count;
            value += sameCol.Count;
            value += diag1.Count;
            value += diag2.Count;

            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }

            //var enemyRow = X.FindAll(t => t.row == move.row);
            //var enemyCol = X.FindAll(t => t.col == move.col);
            //value += enemyRow.Count;
            //value += enemyCol.Count;
            var eRow0 = X.FindAll(t => t.row == 0);
            var eRow1 = X.FindAll(t => t.row == 1);
            var eRow2 = X.FindAll(t => t.row == 2);
            var eRow3 = X.FindAll(t => t.row == 3);
            var eRow4 = X.FindAll(t => t.row == 4);

            var eCol0 = X.FindAll(t => t.col == 0);
            var eCol1 = X.FindAll(t => t.col == 1);
            var eCol2 = X.FindAll(t => t.col == 2);
            var eCol3 = X.FindAll(t => t.col == 3);
            var eCol4 = X.FindAll(t => t.col == 4);

            var ediag1 = X.FindAll(t => t.col == t.row && t.col == move.col);
            var ediag2 = X.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            if (ediag1.Count >= 4 || ediag2.Count >= 4 || eRow0.Count >= 4 || eRow1.Count >= 4 || eRow2.Count >= 4 || eRow3.Count >= 4 || eRow4.Count >= 4 || eCol0.Count >= 4 || eCol1.Count >= 4 || eCol2.Count >= 4 || eCol3.Count >= 4 || eCol4.Count >= 4)
            {
                //Debug.Log("Loosing Move: (" + move.col + "," + move.row + ") to (" + piece.col + ", " + piece.row + ")");
                //Debug.Log("-------------------------------------------------------------------");
                value -= 1000;
            }
            else
            {
                value += 100;
            }
        }

        //if (checkWin(gameObject.GetComponent<Click>().isPlayerOneTurn) && !didOpponentWin)
        //{
        //    value += 100000;
        //}

        //tempGame.FlipBlock(piece);
        //tempGame.MakeMove(piece, move);
        //Debug.Log("-------------------------------------------------------------------");
        int mr = move.row;
        move.row = tempMove.Key;
        int mc = move.col;
        move.col = tempMove.Value;
        int pr = piece.row;
        piece.row = tempPiece.Key;
        int pc = piece.col;
        piece.col = tempPiece.Value;
        if (checkWin(gameObject.GetComponent<GameManager>().isPlayerOneTurn) && !didOpponentWin)
        {
            Debug.Log("Winning Move: (" + move.col + "," + move.row + ") to (" + piece.col + ", " + piece.row + ")");
            Debug.Log("-------------------------------------------------------------------");
            value = 1000000;
        }
        if (didOpponentWin)
        {
            value -= 1000000;
        }
        if (checkWin(!gameObject.GetComponent<GameManager>().isPlayerOneTurn))
        {
            value -= 1000000;
        }
        movePiece(move, piece);
        //Debug.Log("Moving Piece: 2(" + move.col + "," + move.row + ")");
        //Debug.Log("Here: 2(" + piece.col + "," + piece.row + ")");
        //Debug.Log("-------------------------------------------------------------------");

        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
        //}
        //Debug.Log("-------------------------------------------------------------------");
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().isBlank = true;
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().transform.tag = tag;
        //Debug.Log("-------------------------------------------------------------------");

        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log(tempGame[i, 0].tag + " " + tempGame[i, 1].tag + " " + tempGame[i, 2].tag + " " + tempGame[i, 3].tag + " " + tempGame[i, 4].tag);
        //}
        //Debug.Log("-------------------------------------------------------------------");

        //Debug.Log("-------------------------------------------------------------------");
        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
                //Debug.Log(r + ", " + c + " (" + tempGame[r, c].GetComponent<GamePiece>().col + ", " + tempGame[r, c].GetComponent<GamePiece>().row + "): " + tempGame[r, c].tag);
            }
        }
        //Debug.Log("-------------------------------------------------------------------");





        //move.row = mr;
        //move.col = mc;
        //piece.row = pr;
        //piece.col = pc;
        return value;
    }


    public Tuple<GamePiece, GamePiece, int> AITurn()
    {
        System.Random rnd = new System.Random();


        bool temp = gameObject.GetComponent<GameManager>().isPlayerOneTurn;
        gameObject.GetComponent<GameManager>().isPlayerOneTurn = !GameManager.isPlayerOneCats;

        if (GameManager.moveCount > 20 && rnd.Next() % 3 == 0 && AiHard.movesSinceLastDraw >= 2)
        {
            AiHard.movesSinceLastDraw = 0;
            GameActions.OpponentRequestedDraw();
            gameObject.GetComponent<GameManager>().isPlayerOneTurn = temp;
            return new Tuple<GamePiece, GamePiece, int>(Board[0, 0].GetComponent<GamePiece>(), Board[0, 0].GetComponent<GamePiece>(), 0);

        }
        else
        {


            Board = game.GetComponent<GameBoard>().Board;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    tempGame[i, j] = Board[i, j];
                }
            }
            int moveCounter = 0;
            List<Tuple<GamePiece, GamePiece, int>> values = new List<Tuple<GamePiece, GamePiece, int>>();
            GamePiece[] moves = AvailablePieces();
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i].row >= 0 || moves[i].col >= 0)
                {
                    if (moves[i].CheckPickedPiece((gameObject.GetComponent<GameManager>().isPlayerOneTurn)))
                    {
                        //Console.WriteLine("Piece:");
                        //Console.Write(moves[i].row + ", ");
                        //Console.WriteLine(moves[i].col);
                        GameObject[] posMoves = PossibleMoves(moves[i].GameObject());
                        //Console.WriteLine("Moves:");
                        for (int j = 0; j < posMoves.Length; j++)
                        {
                            //Console.Write(posMoves[j].row + ", ");
                            //Console.WriteLine(posMoves[j].col);
                            moveCounter++;
                            int value = 1;
                            GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                            if (Board[moves[i].col, moves[i].row].tag == "Blank")
                            {
                                value += 100;
                            }
                            value += CheckBoardValue(moves[i], posMoves[j].GetComponent<GamePiece>());
                            values.Add(new Tuple<GamePiece, GamePiece, int>(moves[i], posMoves[j].GetComponent<GamePiece>(), value));
                        }
                    }
                }
            }
            //var bestMoves = values.MaxsBy(t => t.Item3);

            List<Tuple<GamePiece, GamePiece, int>> bestMoves = new List<Tuple<GamePiece, GamePiece, int>>();
            values.Sort(delegate (Tuple<GamePiece, GamePiece, int> x, Tuple<GamePiece, GamePiece, int> y)
            {
                //if (x.Item3 == y.Item3) return 0;
                //else if (x.Item3 > y.Item3) return -1;
                //else if (y.Item3 > x.Item3) return 1;
                //else 
                return y.Item3.CompareTo(x.Item3);
            });
            int biggestVal = values[0].Item3;
            foreach (var move in values)
            {
                if (move.Item3 > biggestVal)
                {
                    biggestVal = move.Item3;
                }
            }
            bestMoves = values.Where(value => value.Item3 == biggestVal).ToList();
            Debug.Log("-------------------------------------------------------------------");
            foreach (var move in values)
            {
                Debug.Log("Move: (" + move.Item1.col + "," + move.Item1.row + ") to (" + move.Item2.col + ", " + move.Item2.row + ")");
                Debug.Log("Value: " + move.Item3);
            }
            Debug.Log("-------------------------------------------------------------------");
            var bestMove = bestMoves.ToArray();
            int index = rnd.Next() % (3);
            //game.FlipBlock(bestMove[index].Item1);
            //game.MakeMove(bestMove[index].Item2, bestMove[index].Item1);
            //Console.WriteLine("# Moves: " + moveCounter);
            if (GameManager.moveCount < 10)
            {
                Debug.Log("Random!");
                index = rnd.Next() % 5;
            }
            gameObject.GetComponent<GameManager>().isPlayerOneTurn = temp;

            return new Tuple<GamePiece, GamePiece, int>(values[index].Item1, values[index].Item2, values[index].Item3);
        }

    }

}
