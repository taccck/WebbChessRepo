using UnityEngine;
using UnityEngine.UI;

public class SkinWheel : MonoBehaviour
{
    public static SkinWheel current;
    public SkinDB skinDB;
    public Image[] wheel;
    public Image currenSkin;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (CustomizeMenuSystem.current.white)
        {
            switch (CustomizeMenuSystem.current.currentPiece)
            {
                case Piece.PieceId.pawn:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whitePawn);
                    break;

                case Piece.PieceId.rook:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whiteRook);
                    break;

                case Piece.PieceId.knight:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whiteKnight);
                    break;

                case Piece.PieceId.bishop:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whiteBishop);
                    break;

                case Piece.PieceId.queen:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whiteQueen);
                    break;

                case Piece.PieceId.king:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.whiteKing);
                    break;
            }
        }
        else
        {
            switch (CustomizeMenuSystem.current.currentPiece)
            {
                case Piece.PieceId.pawn:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackPawn);
                    break;

                case Piece.PieceId.rook:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackRook);
                    break;

                case Piece.PieceId.knight:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackKnight);
                    break;

                case Piece.PieceId.bishop:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackBishop);
                    break;

                case Piece.PieceId.queen:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackQueen);
                    break;

                case Piece.PieceId.king:
                    LoadRelevantSkins(CustomizeMenuSystem.current.sf.blackKing);
                    break;
            }
        }
        currenSkin.sprite = wheel[2].sprite;
    }

    void LoadRelevantSkins(int currSkin)
    {
        for (int i = 0; i < 5; i++)
        {
            //get index for image
            int index = ControlSkinIndexRange(currSkin - 2 + i);

            wheel[i].sprite = skinDB.skins[index];
        }
    }

    public int ControlSkinIndexRange(int index)
    {
        int amountOfSkins = skinDB.skins.Length;
        if (index < 0)
        {
            index += amountOfSkins; //add by amount of skins to loop through them
        }
        if (index > amountOfSkins - 1)
        {
            index -= amountOfSkins; //subtract by amount of skins to loop through them
        }

        return index;
    }
}