using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PiecePlacer : MonoBehaviour
{
    public ChessSet blackChessSet;
    public ChessSet whiteChessSet;
    public TileBase highlightedTile;
    public TileBase selectedTile;
    public Tilemap selectionsAndHighlights;
    public Tilemap pieceMap;

    Vector3Int[] highlightLocations;
    private void Awake()
    {
        highlightLocations = new Vector3Int[0];
        ChessBoard.chessPieces = new Piece[0];
        ChessBoard.lastPieceIndex = 0;
        ChessBoard.isBlackTurn = false;
        ChessBoard.castlingRooks = new Rook[0];
        ChessBoard.castleLocations = new Vector3Int[0];
        ChessBoard.castlingIndex = 0;
        EnPassant.ClearEPCache();

    }
    void CheckSelectedPiece()
    {

        Vector3Int mousedOverTile = PieceSelector.mousedOverTile;

        if (mousedOverTile.x >= -4 && mousedOverTile.x <= 3 && mousedOverTile.y >= -4 && mousedOverTile.y <= 3)
        {
            if (selectionsAndHighlights.GetTile(mousedOverTile) == highlightedTile)
            {
                if (!ChessBoard.selectedPiece.hasMoved)
                    ChessBoard.selectedPiece.hasMoved = true;
                if (EnPassant.capturableLocation==mousedOverTile)
                {
                    ChessBoard.RemoveChessPieceFromBoard(EnPassant.lastPawnMoved.gridLocation);
                    pieceMap.SetTile(EnPassant.lastPawnMoved.gridLocation, null);
                    pieceMap.SetTileFlags(EnPassant.lastPawnMoved.gridLocation, TileFlags.None);
                }
                if (ChessBoard.IsCastleLocation(mousedOverTile))
                {
                    pieceMap.SetTile(ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation, null);
                    pieceMap.SetTileFlags(ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation, TileFlags.None);

                    if (mousedOverTile.x < ChessBoard.selectedPiece.gridLocation.x)
                        ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation = mousedOverTile + Vector3Int.right;
                    else
                        ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation = mousedOverTile + Vector3Int.left;

                    pieceMap.SetTile(ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation, ChessBoard.castlingRooks[ChessBoard.castlingIndex].image);
                    pieceMap.SetTileFlags(ChessBoard.castlingRooks[ChessBoard.castlingIndex].gridLocation, TileFlags.None);
                    //move castling rook here + 1 space
                }

                EnPassant.ClearEPCache();
                if (ChessBoard.selectedPiece is Pawn &&
                    (ChessBoard.selectedPiece.gridLocation - mousedOverTile).magnitude == 2)
                {
                    Vector3Int locToCapture = ChessBoard.selectedPiece.gridLocation +
                        (mousedOverTile - ChessBoard.selectedPiece.gridLocation) / 2;
                    //set EP location and assign pawn
                    EnPassant.AssignPotentialEnPassant((Pawn)ChessBoard.selectedPiece, locToCapture, ChessBoard.selectedPiece.isBlack);
                }
                if (ChessBoard.IsOccupied(mousedOverTile))
                    ChessBoard.RemoveChessPieceFromBoard(mousedOverTile);

                //the actual placing of a piece starts by removing old images
                pieceMap.SetTile(ChessBoard.selectedPiece.gridLocation, null);
                pieceMap.SetTileFlags(ChessBoard.selectedPiece.gridLocation, TileFlags.None);


                //the actual placing of a piece finishes by assigning new images
                if (ChessBoard.selectedPiece is Pawn && (mousedOverTile.y == -4 || mousedOverTile.y == 3))
                {//A pawn has reached final rank and turned into a queen OR KNIGHT
                    //TODO: put some UI on the board for Queen/knight selection
                    if (ChessBoard.selectedPiece.isBlack)
                    {
                        ChessBoard.selectedPiece.image = blackChessSet.queen.image;
                    }
                    else
                    {
                        ChessBoard.selectedPiece.image = whiteChessSet.queen.image;
                    }
                    pieceMap.SetTile(mousedOverTile, ChessBoard.selectedPiece.image);
                    Queen newQueen = new Queen(ChessBoard.selectedPiece.isBlack);
                    newQueen.gridLocation = mousedOverTile;
                    newQueen.image = ChessBoard.selectedPiece.image;
                    ChessBoard.RemoveChessPieceFromBoard(ChessBoard.selectedPiece);
                    ChessBoard.AddChessPieceToBoard((Queen)newQueen);

                }
                else pieceMap.SetTile(mousedOverTile, ChessBoard.selectedPiece.image);
                ChessBoard.selectedPiece.gridLocation = mousedOverTile;
                PlaySoundOnMove.psom.PlayMoveSound();
                ClearAllHighlights();
                ChessBoard.isBlackTurn = !ChessBoard.isBlackTurn;

            }
            else
            {
                ClearAllHighlights();
                Vector3Int[] locations = new Vector3Int[0];
                for (int i = 0; i < ChessBoard.chessPieces.Length; ++i)
                {
                    if (ChessBoard.chessPieces[i].gridLocation == mousedOverTile //you've clicked a piece registered to the board
                        &&ChessBoard.isBlackTurn == ChessBoard.chessPieces[i].isBlack)//turn color matches piece color
                    {
                        locations = ChessBoard.chessPieces[i].PossibleMoveLocations(mousedOverTile);
                        ChessBoard.selectedPiece = ChessBoard.chessPieces[i];
                    }
                }
                int cachedPieceIndex = -1;
                Vector3Int cachedLocation = ChessBoard.selectedPiece.gridLocation;
                //check if move is legal by moving piece first then using movelegal check then returning to original location
                for (int i = 0; i < locations.Length; ++i)
                {
                    //remove current piece from locations[i] if present;
                    for (int j = 0; j < ChessBoard.chessPieces.Length; ++j)
                    {
                        if (ChessBoard.chessPieces[j].gridLocation == locations[i])
                        {
                            ChessBoard.chessPieces[j].isBlack = !ChessBoard.chessPieces[j].isBlack;
                            cachedPieceIndex = j;
                            break;
                        }
                    }
                    ChessBoard.selectedPiece.gridLocation = locations[i];
                    if (ChessBoard.IsMoveIllegal())
                    {
                        locations = (Vector3Int[])locations.Remove(locations[i]);
                        --i;
                        if (locations.Length < 1)
                            break;
                    }
                    if (cachedPieceIndex != -1)
                    {
                        ChessBoard.chessPieces[cachedPieceIndex].isBlack = !ChessBoard.chessPieces[cachedPieceIndex].isBlack;
                        cachedPieceIndex = -1;
                    }
                }
                ChessBoard.selectedPiece.gridLocation = cachedLocation;
                HighlightTiles(locations);
            }
            ChessBoard.ClearCastleCache();

        }
    }
    private void Update()
    {
        //change if statement to any piece image. Always clear before showing.
        if (Input.GetMouseButtonDown(0))
        {
            CheckSelectedPiece();
        }
    }
    void HighlightTiles(Vector3Int[] locations)
    {
        TileBase[] tiles = new TileBase[locations.Length];
        for (int i = 0; i < tiles.Length; ++i)
        {
            if (locations[i].x<=3 && locations[i].x>=-4 && locations[i].y <=3 && locations[i].y >= -4)
                tiles[i] = highlightedTile;
        }
        selectionsAndHighlights.SetTiles(locations, tiles);
        for (int i = 0; i < tiles.Length; ++i)
        {
            highlightLocations = (Vector3Int[])highlightLocations.Remove(locations[i]);
            highlightLocations = (Vector3Int[])highlightLocations.AddToArray(locations[i]);
        }
    }
    void ClearAllHighlights()
    {
        for (int i=highlightLocations.Length-1;i>=0;--i)
        {
            selectionsAndHighlights.SetTile(highlightLocations[i], null);
            selectionsAndHighlights.SetTileFlags(highlightLocations[i], TileFlags.None);
            highlightLocations= (Vector3Int[]) highlightLocations.Remove(highlightLocations[i]);
        }
    }
    private void Start()
    {
        blackChessSet.FillPiecesArray();
        whiteChessSet.FillPiecesArray();
        for (int i = 0; i < blackChessSet.pieces.Length; ++i)
        {
            if (blackChessSet.pieces[i] != null)
            {
                pieceMap.SetTile(blackChessSet.pieces[i].gridLocation, blackChessSet.pieces[i].image);
                ChessBoard.AddChessPieceToBoard(blackChessSet.pieces[i]);
            }
        }
        for (int i = 0; i < whiteChessSet.pieces.Length; ++i)
        {
            if (whiteChessSet.pieces[i] != null)
            {
                pieceMap.SetTile(whiteChessSet.pieces[i].gridLocation, whiteChessSet.pieces[i].image);
                ChessBoard.AddChessPieceToBoard(whiteChessSet.pieces[i]);
            }
        }
        ChessBoard.AssignKings();
    }
}
