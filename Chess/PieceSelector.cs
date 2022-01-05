using UnityEngine;
using UnityEngine.Tilemaps;

public class PieceSelector : MonoBehaviour
{
    public Tilemap chessPieces;
    public static Vector3Int mousedOverTile;

    public void Update()
    {
        Vector2 tilePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(tilePoint, Vector2.zero);
        mousedOverTile = chessPieces.WorldToCell(tilePoint);
    }
}
