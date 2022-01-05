using UnityEngine;

public static class EnPassant 
{
    public static Pawn lastPawnMoved;
    public static Vector3Int capturableLocation;
    public static bool lastColorIsBlack;

    public static void ClearEPCache()
    {
        lastPawnMoved = new Pawn(false);
        capturableLocation = new Vector3Int(-5, -5, -5);
        lastColorIsBlack = false;
    }
    public static void AssignPotentialEnPassant(Pawn pawn, Vector3Int locationToCapture, bool isBlack)
    {
        lastPawnMoved = pawn;
        capturableLocation = locationToCapture;
        lastColorIsBlack = isBlack;
    }
    public static bool CanEPCapture(Vector3Int location, bool isBlack)
    {
        if (capturableLocation == location  && lastColorIsBlack!=isBlack)
        {
            return true;
        }
        return false;
    }
}
