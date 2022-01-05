using UnityEngine;
using System;
using UnityEngine.Tilemaps;
[Serializable]
public abstract class Piece
{
    public Tile image;
    public bool isBlack;
    public Vector3Int gridLocation;
    public bool hasMoved;
    public Piece()
    {
        image = null;
        isBlack = false;
        gridLocation = new Vector3Int(0, 0, 0);
        hasMoved = false;
    }
    public abstract Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation);
}

[Serializable]
public class Pawn : Piece
{
    public Pawn(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        Vector3Int nextPosition;
        void CheckNextPosition(Vector3Int dir)
        {
            if (!ChessBoard.IsOccupied(currentLocation + dir))
            {
                if (!hasMoved)
                {
                    nextPosition = currentLocation + dir * 2;
                    if (!ChessBoard.IsOccupied(nextPosition))
                    {
                        spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                    }
                }
                nextPosition = currentLocation + dir;
                spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
            }
            CheckDiagonals(dir);
        }
        void CheckDiagonals(Vector3Int dir)
        {
            nextPosition = currentLocation + dir + Vector3Int.right;
            if (ChessBoard.IsOccupied(nextPosition))
            {
                if (ChessBoard.IsLastPieceBlack() != isBlack)
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
            }
            else
            {
                if (EnPassant.CanEPCapture(nextPosition,isBlack))
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);

            }
            nextPosition = currentLocation + dir + Vector3Int.left;
            if (ChessBoard.IsOccupied(nextPosition))
            {
                if (ChessBoard.IsLastPieceBlack() != isBlack)
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
            }
            else
            {
                if (EnPassant.CanEPCapture(nextPosition,isBlack))
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);

            }
        }
        if (this.isBlack)
        {
            CheckNextPosition(Vector3Int.up);
        }
        if (!this.isBlack)
        {
            CheckNextPosition(Vector3Int.down);
        }
        return spaces;
    }
}

[Serializable]
public class Knight : Piece
{
    public Knight(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        void CheckNextPosition(Vector3Int nextPosition)
        {
            if (!ChessBoard.IsOccupied(nextPosition))
                spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
            else if (ChessBoard.IsLastPieceBlack() != isBlack)
                spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
        }
        CheckNextPosition(currentLocation + Vector3Int.up * 2 + Vector3Int.right);
        CheckNextPosition(currentLocation + Vector3Int.up * 2 + Vector3Int.left);
        CheckNextPosition(currentLocation + Vector3Int.down * 2 + Vector3Int.right);
        CheckNextPosition(currentLocation + Vector3Int.down * 2 + Vector3Int.left);
        CheckNextPosition(currentLocation + Vector3Int.right * 2 + Vector3Int.up);
        CheckNextPosition(currentLocation + Vector3Int.right * 2 + Vector3Int.down);
        CheckNextPosition(currentLocation + Vector3Int.left * 2 + Vector3Int.up);
        CheckNextPosition(currentLocation + Vector3Int.left * 2 + Vector3Int.down);
        return spaces;
    }
}
[Serializable]
public class Bishop : Piece
{
    public Bishop(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        void CheckNextSpaces(Vector3Int dir1, Vector3Int dir2)
        {
            for (int i = 1; i <= 8; ++i)
            {
                Vector3Int nextPosition = currentLocation + dir1 * i + dir2 * i;
                if (!ChessBoard.IsOccupied(nextPosition))
                {
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                }
                else
                {
                    if (ChessBoard.IsLastPieceBlack() != isBlack)
                    {
                        spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                        break;
                    }
                    else break;
                }
            }
        }
        if (currentLocation.x < 3 && currentLocation.y < 3)
            CheckNextSpaces(Vector3Int.up, Vector3Int.right);
        if (currentLocation.x > -4 && currentLocation.y < 3)
            CheckNextSpaces(Vector3Int.up, Vector3Int.left);
        if (currentLocation.x < 3 && currentLocation.y > -4)
            CheckNextSpaces(Vector3Int.down, Vector3Int.right);
        if (currentLocation.x > -4 && currentLocation.y > -4)
            CheckNextSpaces(Vector3Int.down, Vector3Int.left);
        return spaces;
    }
}
[Serializable]
public class Rook : Piece
{
    public Rook(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        void CheckNextSpaces(Vector3Int dir1)
        {
            for (int i = 1; i <= 8; ++i)
            {
                Vector3Int nextPosition = currentLocation + dir1 * i ;
                if (!ChessBoard.IsOccupied(nextPosition))
                {
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                }
                else
                {
                    if (ChessBoard.IsLastPieceBlack() != isBlack)
                    {
                        spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                        break;
                    }
                    else break;
                }
            }
        }
        if (currentLocation.y < 3)
            CheckNextSpaces(Vector3Int.up);
        if (currentLocation.y >-4)
            CheckNextSpaces(Vector3Int.down);
        if (currentLocation.x < 3)
            CheckNextSpaces(Vector3Int.right);
        if (currentLocation.x>-4)
            CheckNextSpaces(Vector3Int.left);
        
        return spaces;
    }
}
[Serializable]
public class Queen : Piece
{
    public Queen(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        void CheckNextSpaces(Vector3Int dir1, Vector3Int dir2)
        {
            for (int i = 1; i <= 8; ++i)
            {
                Vector3Int nextPosition = currentLocation + dir1 * i + dir2 * i;
                if (!ChessBoard.IsOccupied(nextPosition))
                {
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                }
                else
                {
                    if (ChessBoard.IsLastPieceBlack() != isBlack)
                    {
                        spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                        break;
                    }
                    else break;
                }
            }
        }
        void CheckNextSpaces1D(Vector3Int dir1)
        {
            for (int i = 1; i <= 8; ++i)
            {
                Vector3Int nextPosition = currentLocation + dir1 * i;
                if (!ChessBoard.IsOccupied(nextPosition))
                {
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                }
                else
                {
                    if (ChessBoard.IsLastPieceBlack() != isBlack)
                    {
                        spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                        break;
                    }
                    else break;
                }
            }
        }
        if (currentLocation.y < 3)
            CheckNextSpaces1D(Vector3Int.up);
        if (currentLocation.y > -4)
            CheckNextSpaces1D(Vector3Int.down);
        if (currentLocation.x < 3)
            CheckNextSpaces1D(Vector3Int.right);
        if (currentLocation.x > -4)
            CheckNextSpaces1D(Vector3Int.left);

        if (currentLocation.x < 3 && currentLocation.y < 3)
            CheckNextSpaces(Vector3Int.up, Vector3Int.right);
        if (currentLocation.x > -4 && currentLocation.y < 3)
            CheckNextSpaces(Vector3Int.up, Vector3Int.left);
        if (currentLocation.x < 3 && currentLocation.y > -4)
            CheckNextSpaces(Vector3Int.down, Vector3Int.right);
        if (currentLocation.x > -4 && currentLocation.y > -4)
            CheckNextSpaces(Vector3Int.down, Vector3Int.left);
        return spaces;
    }
}
[Serializable]
public class King : Piece
{
    public King(bool isBlack)
    {
        this.isBlack = isBlack;
    }
    public override Vector3Int[] PossibleMoveLocations(Vector3Int currentLocation)
    {
        Vector3Int[] spaces = new Vector3Int[0];
        void CheckNextPosition(Vector3Int nextPosition)
        {
            if (!ChessBoard.IsOccupied(nextPosition))
                spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
            else if (ChessBoard.IsLastPieceBlack() != isBlack)
                spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
        }
        CheckNextPosition(currentLocation + Vector3Int.left);
        CheckNextPosition(currentLocation + Vector3Int.right);
        CheckNextPosition(currentLocation + Vector3Int.down);
        CheckNextPosition(currentLocation + Vector3Int.up);
        CheckNextPosition(currentLocation + Vector3Int.left + Vector3Int.up);
        CheckNextPosition(currentLocation + Vector3Int.left + Vector3Int.down);
        CheckNextPosition(currentLocation + Vector3Int.right + Vector3Int.up);
        CheckNextPosition(currentLocation + Vector3Int.right + Vector3Int.down);

        for (int i = 0; i < ChessBoard.chessPieces.Length; ++i)
        {
            if (ChessBoard.chessPieces[i].isBlack == isBlack && ChessBoard.chessPieces[i] is Rook)
            {
                TryToCastle((Rook)ChessBoard.chessPieces[i]);
            }

        }
        void TryToCastle(Rook rook)//update this after completing the function to investigate if king is in check
        {
            bool failedToCastle = false;
            if (!rook.hasMoved && !hasMoved)
            {
                Vector3Int nextPosition;
                Vector3Int dir = rook.gridLocation - gridLocation;
                if (dir.x < 0)
                {
                    nextPosition = gridLocation + Vector3Int.left * 2;
                    for (int i = 1; i < dir.magnitude; ++i)
                    {
                        if (ChessBoard.IsOccupied(gridLocation + i * Vector3Int.left))
                            failedToCastle = true;
                    }
                }
                else
                {
                    nextPosition = gridLocation + Vector3Int.right * 2;
                    for (int i = 1; i < dir.magnitude; ++i)
                    {
                        if (ChessBoard.IsOccupied(gridLocation + i * Vector3Int.right))
                            failedToCastle = true;
                    }
                }
                //can castle successfully
                if (failedToCastle == false)
                {
                    spaces = (Vector3Int[])spaces.AddToArray(nextPosition);
                    ChessBoard.castleLocations = (Vector3Int[])ChessBoard.castleLocations.AddToArray(nextPosition);
                    ChessBoard.castlingRooks = (Rook[])ChessBoard.castlingRooks.AddToArray(rook);
              //mark this space to castle with this rook
                }

            }
        }
        return spaces;
    }
}