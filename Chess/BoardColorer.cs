using UnityEngine;
using UnityEngine.Tilemaps;
public class BoardColorer : MonoBehaviour
{
    public Tile[] tiles;
    public Tilemap chessBoardMap;
    public Tile tile1;
    public Tile tile2;

    public void SelectTile1(int tileSelected)
    {
        tile1 = tiles[tileSelected];
    }
    public void SelectTile2(int tileSelected)
    {
        tile2 = tiles[tileSelected];
    }

    public void RecolorChessBoard()
    {
        bool isColor1 = false;
        for (int i=0;i<64;++i)
        {
            if (isColor1)
                chessBoardMap.SetTile(new Vector3Int(i/8-4,i%8-4, 0), tile1);
            else 
                chessBoardMap.SetTile(new Vector3Int(i/8-4,i%8-4, 0), tile2);
            if (i%8!=7)
                isColor1 = !isColor1;
        }
    }
    private void Start()
    {
        SelectTile1(1);
        SelectTile2(2);
        RecolorChessBoard();
    }
}
