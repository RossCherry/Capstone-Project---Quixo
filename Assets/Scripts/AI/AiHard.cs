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


public class AiHard : MonoBehaviour
{


    public GameObject[,] Board;
    public GameObject game;
    public bool didOpponentWin;
    public GameObject[,] tempGame = new GameObject[5, 5];

    static public int movesSinceLastDraw = 0;

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
        List<GamePiece> avaMoves = new List<GamePiece>();
        

        int counter = 0;

        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                if (c == 0 || r == 0 || c == 4 || r == 4)
                {
                    if (gameObject.GetComponent<GameManager>().isPlayerOneTurn)
                    {
                        if (Board[c, r].tag == "Player1" || Board[c, r].tag == "Blank")
                        {
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    else
                    {
                        if (Board[c, r].tag == "Player2" || Board[c, r].tag == "Blank")
                        {
                            avaMoves.Add(Board[c, r].GetComponent<GamePiece>());
                        }
                    }
                    counter++;
                }
            }
        }

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

        piece.SetPlayer(gameObject.GetComponent<GameManager>().isPlayerOneTurn);
        movePiece(piece, move);


        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
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
        
        if (gameObject.GetComponent<GameManager>().isPlayerOneTurn)
        {
            foreach (var xPiece in X)
            {
                switch (xPiece.row)
                {
                    case 0:
                        switch (xPiece.col)
                        {
                            case 0: value += 6; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 6; break;
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
                            case 0: value += 6; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 6; break;
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
                            case 0: value -= 6; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 6; break;
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
                            case 0: value -= 6; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 6; break;
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
                            case 0: value += 6; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 6; break;
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
                            case 0: value += 6; break;
                            case 1: value += 1; break;
                            case 2: value += 4; break;
                            case 3: value += 1; break;
                            case 4: value += 6; break;
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
                            case 0: value -= 6; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 6; break;
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
                            case 0: value -= 6; break;
                            case 1: value -= 1; break;
                            case 2: value -= 4; break;
                            case 3: value -= 1; break;
                            case 4: value -= 6; break;
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
                value -= 100;
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
                value -= 100;
            }
            else
            {
                value += 100;
            }
        }

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
            value += 10000;
        }
        if (didOpponentWin)
        {
            value -= 10000;
        }
        if (checkWin(!gameObject.GetComponent<GameManager>().isPlayerOneTurn))
        {
            value -= 1000000;
        }
        movePiece(move, piece);
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().isBlank = true;
        tempGame[piece.col, piece.row].GetComponent<GamePiece>().transform.tag = tag;
        for (int r = 0; r < tempGame.GetLength(0); r++)
        {
            for (int c = 0; c < tempGame.GetLength(0); c++)
            {
                tempGame[r, c].GetComponent<GamePiece>().row = c;
                tempGame[r, c].GetComponent<GamePiece>().col = r;
            }
        }
        return value;
    }

    public Tuple<GamePiece, GamePiece, int> AITurn()
    {
        Board = game.GetComponent<GameBoard>().Board;


        const int MAX_MOVES = 27000;
        int moveCounter = 0;
        List<Tuple<GamePiece, GamePiece, int>> values = new List<Tuple<GamePiece, GamePiece, int>>();
        GamePiece[] moves = AvailablePieces();
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].row >= 0 || moves[i].col >= 0)
            {
                if (moves[i].CheckPickedPiece((gameObject.GetComponent<GameManager>().isPlayerOneTurn)))
                {
                    GameObject[] posMoves = moves[i].PossibleMoves();
                    for (int j = 0; j < posMoves.Length; j++)
                    {
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
        List<Tuple<GamePiece, GamePiece, int>> NewBestValues = new List<Tuple<GamePiece, GamePiece, int>>();
        List<Tuple<GamePiece, GamePiece, int>> LastBestValues = new List<Tuple<GamePiece, GamePiece, int>>();


        Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");


        foreach (var move in values)
        {
            if (moveCounter > MAX_MOVES) break;

            Board = game.GetComponent<GameBoard>().Board;


            KeyValuePair<int, int> tempPiece = new KeyValuePair<int, int>(move.Item1.row, move.Item1.col);
            KeyValuePair<int, int> tempMove = new KeyValuePair<int, int>(move.Item2.row, move.Item2.col);
            List<GamePiece> tg = new List<GamePiece>();
            
            string tag = move.Item1.tag;
            
            move.Item1.SetPlayer(gameObject.GetComponent<GameManager>().isPlayerOneTurn);
            movePiece(move.Item1, move.Item2);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Board[i, j] = tempGame[i, j];
                    tg.Add(tempGame[i, j].GetComponent<GamePiece>());
                }
            }

            if (checkWin(gameObject.GetComponent<GameManager>().isPlayerOneTurn) && !didOpponentWin)
            {
                NewBestValues.Add(new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2, 100000));
                Debug.Log("Winning move before analyzing opponent's move");
                //int mr1 = move.Item2.row;
                move.Item2.row = tempMove.Key;
                //int mc1 = move.Item2.col;
                move.Item2.col = tempMove.Value;
                //int pr1 = move.Item1.row;
                move.Item1.row = tempPiece.Key;
                //int pc1 = move.Item1.col;
                move.Item1.col = tempPiece.Value;


                movePiece(move.Item2, move.Item1);
                tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().isBlank = true;
                tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().transform.tag = tag;
                for (int r = 0; r < tempGame.GetLength(0); r++)
                {
                    for (int c = 0; c < tempGame.GetLength(0); c++)
                    {
                        tempGame[r, c].GetComponent<GamePiece>().row = c;
                        tempGame[r, c].GetComponent<GamePiece>().col = r;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Board[i, j] = tempGame[i, j];
                    }
                }
                return new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2, move.Item3);

            }
            gameObject.GetComponent<GameManager>().isPlayerOneTurn = !gameObject.GetComponent<GameManager>().isPlayerOneTurn;
            

            for (int r = 0; r < tempGame.GetLength(0); r++)
            {
                for (int c = 0; c < tempGame.GetLength(0); c++)
                {
                    tempGame[r, c].GetComponent<GamePiece>().row = c;
                    tempGame[r, c].GetComponent<GamePiece>().col = r;
                }
            }
            
            

            List<Tuple<GamePiece, GamePiece, int>> eValues = new List<Tuple<GamePiece, GamePiece, int>>();
            GamePiece[] eMoves = AvailablePieces();
            for (int i = 0; i < eMoves.Length; i++)
            {
                if (eMoves[i].row >= 0 || eMoves[i].col >= 0)
                {
                    if (eMoves[i].CheckPickedPiece((gameObject.GetComponent<GameManager>().isPlayerOneTurn)))
                    {
                        GameObject[] posMoves = PossibleMoves(eMoves[i].gameObject);
                        for (int j = 0; j < posMoves.Length; j++)
                        {
                            moveCounter++;
                            int value = 1;
                            GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                            if (Board[eMoves[i].col, eMoves[i].row].tag == "Blank")
                            {
                                value += 100;
                            }
                            value += CheckBoardValue(eMoves[i], posMoves[j].GetComponent<GamePiece>());
                            eValues.Add(new Tuple<GamePiece, GamePiece, int>(eMoves[i], posMoves[j].GetComponent<GamePiece>(), value));
                        }
                    }
                }
            }
            eValues.Sort(delegate (Tuple<GamePiece, GamePiece, int> x, Tuple<GamePiece, GamePiece, int> y)
             {
                 return y.Item3.CompareTo(x.Item3);
             });
            int newValue = move.Item3 - eValues.Max(t => t.Item3);



            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            foreach (var move3 in eValues)
            {

                if (moveCounter > MAX_MOVES) break;

                Board = game.GetComponent<GameBoard>().Board;


                KeyValuePair<int, int> tempPiece2 = new KeyValuePair<int, int>(move3.Item1.row, move3.Item1.col);
                KeyValuePair<int, int> tempMove2 = new KeyValuePair<int, int>(move3.Item2.row, move3.Item2.col);
                List<GamePiece> tg2 = new List<GamePiece>();

                string tag2 = move3.Item1.tag;

                move3.Item1.SetPlayer(gameObject.GetComponent<GameManager>().isPlayerOneTurn);
                movePiece(move3.Item1, move3.Item2);

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Board[i, j] = tempGame[i, j];
                        tg2.Add(tempGame[i, j].GetComponent<GamePiece>());
                    }
                }

                if (checkWin(gameObject.GetComponent<GameManager>().isPlayerOneTurn) && didOpponentWin)
                {
                    LastBestValues.Add(new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2, -10000000));
                    Debug.Log("Opponent will win");
                    //int mr1 = move.Item2.row;
                    move3.Item2.row = tempMove2.Key;
                    //int mc1 = move.Item2.col;
                    move3.Item2.col = tempMove2.Value;
                    //int pr1 = move.Item1.row;
                    move3.Item1.row = tempPiece2.Key;
                    //int pc1 = move.Item1.col;
                    move3.Item1.col = tempPiece2.Value;


                    movePiece(move3.Item2, move3.Item1);
                    tempGame[move3.Item1.col, move3.Item1.row].GetComponent<GamePiece>().isBlank = true;
                    tempGame[move3.Item1.col, move3.Item1.row].GetComponent<GamePiece>().transform.tag = tag2;
                    for (int r = 0; r < tempGame.GetLength(0); r++)
                    {
                        for (int c = 0; c < tempGame.GetLength(0); c++)
                        {
                            tempGame[r, c].GetComponent<GamePiece>().row = c;
                            tempGame[r, c].GetComponent<GamePiece>().col = r;
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Board[i, j] = tempGame[i, j];
                        }
                    }
                    break;

                }
                gameObject.GetComponent<GameManager>().isPlayerOneTurn = !gameObject.GetComponent<GameManager>().isPlayerOneTurn;


                for (int r = 0; r < tempGame.GetLength(0); r++)
                {
                    for (int c = 0; c < tempGame.GetLength(0); c++)
                    {
                        tempGame[r, c].GetComponent<GamePiece>().row = c;
                        tempGame[r, c].GetComponent<GamePiece>().col = r;
                    }
                }



                List<Tuple<GamePiece, GamePiece, int>> newValues = new List<Tuple<GamePiece, GamePiece, int>>();
                GamePiece[] newMoves = AvailablePieces();
                for (int i = 0; i < newMoves.Length; i++)
                {
                    if (newMoves[i].row >= 0 || newMoves[i].col >= 0)
                    {
                        if (newMoves[i].CheckPickedPiece((gameObject.GetComponent<GameManager>().isPlayerOneTurn)))
                        {
                            GameObject[] posMoves = PossibleMoves(newMoves[i].gameObject);
                            for (int j = 0; j < posMoves.Length; j++)
                            {
                                moveCounter++;
                                int value = 1;
                                GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                                if (Board[newMoves[i].col, newMoves[i].row].tag == "Blank")
                                {
                                    value += 100;
                                }
                                value += CheckBoardValue(newMoves[i], posMoves[j].GetComponent<GamePiece>());
                                newValues.Add(new Tuple<GamePiece, GamePiece, int>(newMoves[i], posMoves[j].GetComponent<GamePiece>(), value));
                            }
                        }
                    }
                }
                newValues.Sort(delegate (Tuple<GamePiece, GamePiece, int> x, Tuple<GamePiece, GamePiece, int> y)
                {
                    return y.Item3.CompareTo(x.Item3);
                });
                int newValue2 = move.Item3 - eValues.Max(t => t.Item3) + newValues[0].Item3;


                gameObject.GetComponent<GameManager>().isPlayerOneTurn = !gameObject.GetComponent<GameManager>().isPlayerOneTurn;


                //int mr = move.Item2.row;
                move3.Item2.row = tempMove2.Key;
                //int mc = move.Item2.col;
                move3.Item2.col = tempMove2.Value;
                //int pr = move.Item1.row;
                move3.Item1.row = tempPiece2.Key;
                //int pc = move.Item1.col;
                move3.Item1.col = tempPiece2.Value;

                //Debug.Log("AiMove: (" + move.Item1.row + ", " + move.Item1.col + ") - (" + move.Item2.row + ", " + move.Item2.col + "): " + move.Item3);
                //Debug.Log("BestResponse(" + move3.Item1.row + ", " + move3.Item1.col + ") - (" + move3.Item2.row + ", " + move3.Item2.col + "): " + move3.Item3);
                //Debug.Log("BestAiResponse: (" + newValues[0].Item1.row + ", " + newValues[0].Item1.col + ") - (" + newValues[0].Item2.row + ", " + newValues[0].Item2.col + "): " + newValues[0].Item3);
                //Debug.Log("Total Value of the Moves: " + newValue2);


                LastBestValues.Add(new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2, newValue));


                movePiece(move3.Item2, move3.Item1);
                tempGame[move3.Item1.col, move3.Item1.row].GetComponent<GamePiece>().isBlank = true;
                tempGame[move3.Item1.col, move3.Item1.row].GetComponent<GamePiece>().transform.tag = tag2;
                for (int r = 0; r < tempGame.GetLength(0); r++)
                {
                    for (int c = 0; c < tempGame.GetLength(0); c++)
                    {
                        tempGame[r, c].GetComponent<GamePiece>().row = c;
                        tempGame[r, c].GetComponent<GamePiece>().col = r;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Board[i, j] = tempGame[i, j];
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //int mr = move.Item2.row;
            move.Item2.row = tempMove.Key;
            //int mc = move.Item2.col;
            move.Item2.col = tempMove.Value;
            //int pr = move.Item1.row;
            move.Item1.row = tempPiece.Key;
            //int pc = move.Item1.col;
            move.Item1.col = tempPiece.Value;

            //Debug.Log("AiMove: (" + move.Item1.row + ", " + move.Item1.col + ") - (" + move.Item2.row + ", " + move.Item2.col + "): " + move.Item3 + " - " + eValues[0].Item3 + " = " + newValue);
            //Debug.Log("BestResponse(" + eValues[0].Item1.row + ", " + eValues[0].Item1.col + ") - (" + eValues[0].Item2.row + ", " + eValues[0].Item2.col + ")");

            NewBestValues.Add(new Tuple<GamePiece, GamePiece, int>(move.Item1, move.Item2, newValue));



            gameObject.GetComponent<GameManager>().isPlayerOneTurn = !gameObject.GetComponent<GameManager>().isPlayerOneTurn;

            movePiece(move.Item2, move.Item1);
            tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().isBlank = true;
            tempGame[move.Item1.col, move.Item1.row].GetComponent<GamePiece>().transform.tag = tag;
            for (int r = 0; r < tempGame.GetLength(0); r++)
            {
                for (int c = 0; c < tempGame.GetLength(0); c++)
                {
                    tempGame[r, c].GetComponent<GamePiece>().row = c;
                    tempGame[r, c].GetComponent<GamePiece>().col = r;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Board[i, j] = tempGame[i, j];
                }
            }


        }

        LastBestValues.Sort(delegate (Tuple<GamePiece, GamePiece, int> x, Tuple<GamePiece, GamePiece, int> y)
        {
            return y.Item3.CompareTo(x.Item3);
        });
        
        System.Random rnd = new System.Random();
        Debug.Log(moveCounter);
        Debug.Log("Move value: " + LastBestValues[0].Item3);
        var result = LastBestValues[0];
        if (GameManager.moveCount > 20 && result.Item3 < 0 && movesSinceLastDraw >= 2)
        {
            movesSinceLastDraw = 0;
            GameActions.OpponentRequestedDraw();
            return new Tuple<GamePiece, GamePiece, int>(Board[0, 0].GetComponent<GamePiece>(), Board[0, 0].GetComponent<GamePiece>(), 0);
        }
        else if (GameManager.moveCount > 100 && movesSinceLastDraw >= 2)
        {
            movesSinceLastDraw = 0;
            GameActions.OpponentRequestedDraw();
            return new Tuple<GamePiece, GamePiece, int>(Board[0, 0].GetComponent<GamePiece>(), Board[0, 0].GetComponent<GamePiece>(), 0);
        }
        else
        {
            return new Tuple<GamePiece, GamePiece, int>(LastBestValues[0].Item1, LastBestValues[0].Item2, LastBestValues[0].Item3);
        }
    }



}
