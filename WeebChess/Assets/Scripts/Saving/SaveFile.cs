using System;

[Serializable]
public class SaveFile
{
    public int whitePawn = 0;
    public int whiteRook = 0;
    public int whiteKnight = 0;
    public int whiteBishop = 0;
    public int whiteQueen = 0;
    public int whiteKing = 0;


    public int blackPawn = 0;
    public int blackRook = 0;
    public int blackKnight = 0;
    public int blackBishop = 0;
    public int blackQueen = 0;
    public int blackKing = 0;

    public (float, float, float) whiteColor = (1, 1, 1);
    public (float, float, float) blackColor = (0, 0, 0);
}
