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


public class AiEasy : MonoBehaviour
{
    

    public GameObject[,] Board;
    public GameObject game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameBoard");
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
                            avaMoves.Add(Board[c,r].GetComponent<GamePiece>());
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
        int value = 0;
        List<GamePiece> X = new List<GamePiece>();
        List<GamePiece> O = new List<GamePiece>();
        //tempGame.FlipBlock(piece);
        //tempGame.MakeMove(move, piece);

        for (int r = 0; r < Board.GetLength(0); r++)
        {
            for (int c = 0; c < Board.GetLength(0); c++)
            {
                if ((Board[c, r].CompareTag("Player1")))
                {
                    X.Add(Board[c, r].GetComponent<GamePiece>());
                }
                else if ((Board[c, r].CompareTag("Player2")))
                {
                    O.Add(Board[c, r].GetComponent<GamePiece>());
                }
            }
        }

        if (gameObject.GetComponent<GameManager>().isPlayerOneTurn)
        {
            var sameRow = X.FindAll(t => t.row == move.row);
            var sameCol = X.FindAll(t => t.col == move.col);
            var diag1 = X.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = X.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count - 1;
            value += sameCol.Count - 1;
            value += diag1.Count;
            value += diag2.Count;
            var enemyRow = O.FindAll(t => t.row == move.row);
            var enemyCol = O.FindAll(t => t.col == move.col);
            value += enemyRow.Count;
            value += enemyCol.Count;
            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }
        }
        else
        {
            var sameRow = O.FindAll(t => t.row == move.row);
            var sameCol = O.FindAll(t => t.col == move.col);
            var diag1 = O.FindAll(t => t.col == t.row && t.col == move.col);
            var diag2 = O.FindAll(t => t.col + t.row == 4 && (t.col == move.col || t.row == move.row));
            value += sameRow.Count - 1;
            value += sameCol.Count - 1;
            value += diag1.Count;
            value += diag2.Count;
            var enemyRow = X.FindAll(t => t.row == move.row);
            var enemyCol = X.FindAll(t => t.col == move.col);
            value += enemyRow.Count;
            value += enemyCol.Count;
            if (sameRow.Count > 3 || sameCol.Count > 3 || diag1.Count > 3 || diag2.Count > 3)
            {
                value += 1000;
            }
        }

        //tempGame.FlipBlock(piece);
        //tempGame.MakeMove(piece, move);

        return value;
    }


    public KeyValuePair<GamePiece, GamePiece> AITurn()
    {
        Board = game.GetComponent<GameBoard>().Board;

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
                    GameObject[] posMoves = moves[i].PossibleMoves();
                    //Console.WriteLine("Moves:");
                    for (int j = 0; j < posMoves.Length; j++)
                    {
                        //Console.Write(posMoves[j].row + ", ");
                        //Console.WriteLine(posMoves[j].col);
                        moveCounter++;
                        int value = 1;
                        GamePiece curMove = posMoves[j].GetComponent<GamePiece>();
                        switch (curMove.row)
                        {
                            case 0:
                                switch (curMove.col)
                                {
                                    case 0: value += 1; break;
                                    case 2: value += 4; break;
                                    case 4: value += 1; break;
                                }
                                break;
                            case 1:
                                switch (curMove.col)
                                {
                                    case 1: value += 1; break;
                                    case 2: value += 5; break;
                                    case 3: value += 1; break;
                                }
                                break;
                            case 2:
                                switch (curMove.col)
                                {
                                    case 0: value += 2; break;
                                    case 1: value += 3; break;
                                    case 2: value += 6; break;
                                    case 3: value += 3; break;
                                    case 4: value += 2; break;
                                }
                                break;
                            case 3:
                                switch (curMove.col)
                                {
                                    case 1: value += 1; break;
                                    case 2: value += 5; break;
                                    case 3: value += 1; break;
                                }
                                break;
                            case 4:
                                switch (curMove.col)
                                {
                                    case 0: value += 1; break;
                                    case 2: value += 4; break;
                                    case 4: value += 1; break;
                                }
                                break;
                        }
                        if (Board[moves[i].col, moves[i].row].tag == "Blank")
                        {
                            value += 10;
                        }
                        value += CheckBoardValue(moves[i], posMoves[j].GetComponent<GamePiece>());
                        values.Add(new Tuple<GamePiece, GamePiece, int>(moves[i], posMoves[j].GetComponent<GamePiece>(), value));
                    }
                }
            }
        }
        //var bestMoves = values.MaxsBy(t => t.Item3);

        List<Tuple<GamePiece, GamePiece, int>> bestMoves = new List<Tuple<GamePiece, GamePiece, int>>();
        int biggestVal = 0;
        foreach (var move in values)
        {
            if (move.Item3 > biggestVal)
            {
                biggestVal = move.Item3;
            }
        }
        bestMoves = values.Where(value => value.Item3 == biggestVal).ToList();

        System.Random rnd = new System.Random();
        var bestMove = bestMoves.ToArray();
        int index = rnd.Next() % (bestMoves.Count());
        //game.FlipBlock(bestMove[index].Item1);
        //game.MakeMove(bestMove[index].Item2, bestMove[index].Item1);
        //Console.WriteLine("# Moves: " + moveCounter);

        return new KeyValuePair<GamePiece, GamePiece>(bestMove[index].Item1, bestMove[index].Item2);
    }



}
