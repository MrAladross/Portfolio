using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileDetector : MonoBehaviour
{
    public Tile waterTile;
    public Tile circleTile;
    public Tile glowTile;
    public Tile wallTile;

    public Tile[] blockingTiles;

    public TileBase previousTile;
    public Vector3Int previousLocation;
    public TileBase selectedTile;
    public Vector3Int selectedLocation;

    public Tilemap foreGroundMap;
    public Tilemap midGroundMap;

    private Vector3Int previous;
    
    public static TileDetector tileDetector;

    private void Awake()
    {
        tileDetector = this;
    }
    public void Update()
    {

        if (GamePlayManager.gpm.isHoldingCard)
        {
            Vector2 tilePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(tilePoint, Vector2.zero);

            Vector3Int currentCell = midGroundMap.WorldToCell(tilePoint);

            GamePlayManager.gpm.CardToGameBoard(currentCell);
            
        }
            //OptimizedRevealMoveSpaces(currentCell, moveDistance);
           

       



    }
    Dictionary<Vector3Int, TileBase> moveTiles = new Dictionary<Vector3Int, TileBase>();
    Dictionary<Vector3Int, TileBase> currentTiles = new Dictionary<Vector3Int, TileBase>();
    Dictionary<Vector3Int, TileBase> upcomingTiles = new Dictionary<Vector3Int, TileBase>();
    public void ClearCurrentTiles()
    {
        for (int x = 0; x < moveTiles.Count; ++x)
        {
            foreGroundMap.SetTile(moveTiles.ElementAt(x).Key, null);
        }
        moveTiles.Clear();
    }
    public void OptimizedRevealMoveSpaces(Vector3Int currentCell, int moveSpaces)
    {
        ClearCurrentTiles();

        currentTiles.Add(currentCell, midGroundMap.GetTile(currentCell));
        moveTiles.Add(currentCell, midGroundMap.GetTile(currentCell));
        for (int i = 0; i < moveSpaces; ++i)
        {
            int x = currentTiles.Count;
            for (int j=0;j<x;++j)
            {
                CheckCurrentTile(currentTiles.ElementAt(j).Key);
            }
            Debug.Log(x + " number of tiles "+i + " spaces away.");
            currentTiles = new Dictionary<Vector3Int, TileBase>(upcomingTiles);
            upcomingTiles.Clear();
        }
        for (int i=0;i<moveTiles.Count;++i)
        {
            //insert logic here for handling different tilebases
            
            foreGroundMap.SetTile(moveTiles.ElementAt(i).Key, glowTile);
            foreGroundMap.SetTileFlags(moveTiles.ElementAt(i).Key, TileFlags.None);
            foreGroundMap.SetColor(moveTiles.ElementAt(i).Key, Color.blue);
        }
        currentTiles.Clear();
    }
    void CheckCurrentTile(Vector3Int currentTile)
    {
        Vector3Int topRightTile = currentTile + Vector3Int.up+ Vector3Int.right;
        Vector3Int rightTile = currentTile + Vector3Int.right;
        Vector3Int bottomRightTile = currentTile + Vector3Int.right + Vector3Int.down;
        Vector3Int bottomLeftTile = currentTile + Vector3Int.down;
        Vector3Int leftTile = currentTile + Vector3Int.left;
        Vector3Int topLeftTile = currentTile + Vector3Int.up;
        if (currentTile.y % 2 == 0)
        {
             topRightTile = currentTile + Vector3Int.up;
             rightTile = currentTile + Vector3Int.right;
             bottomRightTile = currentTile + Vector3Int.down;
             bottomLeftTile = currentTile + Vector3Int.down + Vector3Int.left;
             leftTile = currentTile + Vector3Int.left;
             topLeftTile = currentTile + Vector3Int.up + Vector3Int.left;
        }

        void CheckNextTile(Vector3Int nextTile)
        {
            if (!moveTiles.ContainsKey(nextTile))
            {
                for (int x = 0; x < blockingTiles.Length; ++x)
                {
                    if (midGroundMap.GetTile(nextTile) == blockingTiles[x])
                        return;
                }
                moveTiles.Add(nextTile, midGroundMap.GetTile(nextTile));
                upcomingTiles.Add(nextTile, midGroundMap.GetTile(nextTile));
            }
        }
        
        CheckNextTile(topRightTile);
        CheckNextTile(rightTile);
        CheckNextTile(bottomRightTile);
        CheckNextTile(bottomLeftTile);
        CheckNextTile(leftTile);
        CheckNextTile(topLeftTile);
       
    }
    
    

    public void SwapTile(Vector3Int currentCell)
    {
        selectedTile = midGroundMap.GetTile(currentCell);
        selectedLocation = currentCell;

        foreGroundMap.SetTile(previousLocation, null);
        midGroundMap.SetTile(selectedLocation, previousTile);
        midGroundMap.SetTile(previousLocation, selectedTile);

    }


    public void FlowerTiles(Vector3Int currentCell)
    {   //center tile first
        midGroundMap.SetTile(currentCell, waterTile);

        Vector3Int tileToSet = currentCell;

        if (currentCell.y % 2 == 0 )
        {//even y coords, use top values from data table
            //set 6 tiles to circle tile
            //topright
            tileToSet = currentCell + Vector3Int.up;
            midGroundMap.SetTile(tileToSet, circleTile);
            //right
            tileToSet = currentCell + Vector3Int.right;
            midGroundMap.SetTile(tileToSet, circleTile);
            //bottomright
            tileToSet = currentCell + Vector3Int.down;
            midGroundMap.SetTile(tileToSet, circleTile);
            //bottomleft
            tileToSet = currentCell + Vector3Int.down+Vector3Int.left;
            midGroundMap.SetTile(tileToSet, circleTile);
            //left
            tileToSet = currentCell + Vector3Int.left;
            midGroundMap.SetTile(tileToSet, circleTile);
            //topleft
            tileToSet = currentCell + Vector3Int.left + Vector3Int.up;
            midGroundMap.SetTile(tileToSet, circleTile);

        }
        else
        {
            //odd y coords, use top values from data table
            //set 6 tiles to circle tile
            //topright
            tileToSet = currentCell + Vector3Int.up+Vector3Int.right;
            midGroundMap.SetTile(tileToSet, circleTile);
            //right
            tileToSet = currentCell + Vector3Int.right;
            midGroundMap.SetTile(tileToSet, circleTile);
            //bottomright
            tileToSet = currentCell + Vector3Int.down + Vector3Int.right;
            midGroundMap.SetTile(tileToSet, circleTile);
            //bottomleft
            tileToSet = currentCell + Vector3Int.down;
            midGroundMap.SetTile(tileToSet, circleTile);
            //left
            tileToSet = currentCell + Vector3Int.left;
            midGroundMap.SetTile(tileToSet, circleTile);
            //topleft
            tileToSet = currentCell + Vector3Int.up;
            midGroundMap.SetTile(tileToSet, circleTile);

        }

    }

    public bool CheckTile(TileBase tile)
    {
        if (tile == glowTile)
            return true;
        return false;
    }
}



