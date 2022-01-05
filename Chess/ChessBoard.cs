using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public static class ChessBoard
{
    public static Piece[] chessPieces;
    public static int lastPieceIndex;
    public static Piece selectedPiece;
    public static bool isBlackTurn;
    public static Vector3Int[] castleLocations;
    public static Rook[] castlingRooks;
    public static int castlingIndex;
    public static King blackKing;
    public static King whiteKing;

    public static bool IsCastleLocation(Vector3Int location)
    {
        for (int i = 0; i < castleLocations.Length; ++i)
            if (location == castleLocations[i])
            {
                castlingIndex = i;
                return true;
            }
        return false;
    }
    public static void ClearCastleCache()
    {
        for (int i = 0; i < castleLocations.Length; ++i)
            castleLocations.RemoveFromArrayAtIndex(0);
        for (int i = 0; i < castlingRooks.Length; ++i)
            castlingRooks.RemoveFromArrayAtIndex(0);
    }
    public static void AddChessPieceToBoard(Piece piece)
    {
        chessPieces = (Piece[])chessPieces.AddToArray(piece);
    }
    public static void RemoveChessPieceFromBoard(Piece piece)
    {
        chessPieces = (Piece[])chessPieces.Remove(piece);
    }
    public static void RemoveChessPieceFromBoard(Vector3Int location)
    {
        for (int i = 0; i < chessPieces.Length; ++i)
            if (chessPieces[i].gridLocation == location)
                chessPieces = (Piece[])chessPieces.Remove(chessPieces[i]);
    }

    public static bool IsOccupied(Vector3Int location)
    {
        for (int i = 0; i < chessPieces.Length; ++i)
        {
            if (chessPieces[i].gridLocation == location)
            {
                lastPieceIndex = i;
                return true;
            }
        }
        return false;
    }
    public static bool IsLastPieceBlack()
    {
        if (chessPieces[lastPieceIndex].isBlack)
            return true;
        return false;
    }
    public static void AssignKings()
    {
        for (int i = 0; i < chessPieces.Length; ++i)
            if (chessPieces[i] is King)
            {
                if (chessPieces[i].isBlack)
                    blackKing = (King)chessPieces[i];
                else whiteKing = (King)chessPieces[i];
            }
    }
    public static bool IsMoveIllegal()
    {
        Vector3Int[][] moveLocationsForWholeTeam = new Vector3Int[0][];
        for (int i=0;i<chessPieces.Length;++i)
        {
            if (chessPieces[i].isBlack != isBlackTurn)
            {
                Vector3Int[] moveLocations = chessPieces[i].PossibleMoveLocations(chessPieces[i].gridLocation);
                moveLocationsForWholeTeam = (Vector3Int[][])moveLocationsForWholeTeam.AddToArray(moveLocations);
            }
        }
        for (int i = 0; i < moveLocationsForWholeTeam.Length; ++i)
        {
            for (int j = 0; j < moveLocationsForWholeTeam[i].Length; ++j)
            {
                if (isBlackTurn)
                {
                    if (moveLocationsForWholeTeam[i][j] == blackKing.gridLocation)
                        return true;
                }
                else
                {
                    if (moveLocationsForWholeTeam[i][j] == whiteKing.gridLocation)
                        return true;
                }
            }
            

        }
        return false;
    }
}
