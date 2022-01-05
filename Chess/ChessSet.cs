using UnityEngine;
using System;
using UnityEngine.Tilemaps;
[Serializable]
public struct ChessSet
{
    public Piece[] pieces;
    public Pawn[] pawns;
    public Knight[] knights;
    public Bishop[] bishops;
    public Rook[] rooks;
    public King king;
    public Queen queen;
    public bool isBlack;
    public void FillPiecesArray()
    {
        int counter = 0;
        pieces = new Piece[pawns.Length + knights.Length + bishops.Length + rooks.Length + 2];
        for (int i = 0; i < pawns.Length; ++i)
        {
            pieces[counter] = pawns[i];
            counter++;
        }
        for (int i = 0; i < knights.Length; ++i)
        {
            pieces[counter] = knights[i];
            counter++;
        }
        for (int i = 0; i < bishops.Length; ++i)
        {
            pieces[counter] = bishops[i];
            counter++;
        }
        for (int i = 0; i < rooks.Length; ++i)
        {
            pieces[counter] = rooks[i];
            counter++;
        }
        pieces[counter] = king;
        counter++;
        pieces[counter] = queen;
    }
}