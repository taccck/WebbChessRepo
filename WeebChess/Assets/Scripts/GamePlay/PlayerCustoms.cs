using UnityEngine;

public class PlayerCustoms : MonoBehaviour
{
    public SkinDB skinDB;
    public static PlayerCustoms current;
    public Color white;
    public Color black;
    public Sprite pawnW;
    public Sprite rookW;
    public Sprite knightW;
    public Sprite bishopW;
    public Sprite queenW;
    public Sprite kingW;
    public Sprite pawnB;
    public Sprite rookB;
    public Sprite knightB;
    public Sprite bishopB;
    public Sprite queenB;
    public Sprite kingB;

    private void Awake()
    {
        current = this;

        SaveFile sf = SaveManager.Load();
        white = new Color(sf.whiteColor.Item1, sf.whiteColor.Item2, sf.whiteColor.Item3);
        black = new Color(sf.blackColor.Item1, sf.blackColor.Item2, sf.blackColor.Item3);
        pawnW = skinDB.skins[sf.whitePawn];
        rookW = skinDB.skins[sf.whiteRook];
        knightW = skinDB.skins[sf.whiteKnight];
        bishopW = skinDB.skins[sf.whiteBishop];
        queenW = skinDB.skins[sf.whiteQueen];
        kingW = skinDB.skins[sf.whiteKing];

        pawnB = skinDB.skins[sf.blackPawn];
        rookB = skinDB.skins[sf.blackRook];
        knightB = skinDB.skins[sf.blackKnight];
        bishopB = skinDB.skins[sf.blackBishop];
        queenB = skinDB.skins[sf.blackQueen];
        kingB = skinDB.skins[sf.blackKing];
    }

    public void SetPieceSkins(Piece p)
    {
        if (p.white)
        {
            switch (p.id)
            {
                case Piece.PieceId.pawn:
                    p.skin.sprite = pawnW;
                    break;

                case Piece.PieceId.rook:
                    p.skin.sprite = rookW;
                    break;

                case Piece.PieceId.knight:
                    p.skin.sprite = knightW;
                    break;

                case Piece.PieceId.bishop:
                    p.skin.sprite = bishopW;
                    break;

                case Piece.PieceId.queen:
                    p.skin.sprite = queenW;
                    break;

                case Piece.PieceId.king:
                    p.skin.sprite = kingW;
                    break;
            }
        }
        else
        {
            switch (p.id)
            {
                case Piece.PieceId.pawn:
                    p.skin.sprite = pawnB;
                    break;

                case Piece.PieceId.rook:
                    p.skin.sprite = rookB;
                    break;

                case Piece.PieceId.knight:
                    p.skin.sprite = knightB;
                    break;

                case Piece.PieceId.bishop:
                    p.skin.sprite = bishopB;
                    break;

                case Piece.PieceId.queen:
                    p.skin.sprite = queenB;
                    break;

                case Piece.PieceId.king:
                    p.skin.sprite = kingB;
                    break;
            }
        }
    }
}
