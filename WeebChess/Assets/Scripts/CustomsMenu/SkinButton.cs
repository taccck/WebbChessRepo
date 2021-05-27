using UnityEngine;
using UnityEngine.EventSystems;

public class SkinButton : MonoBehaviour, IPointerClickHandler
{
    public int value;

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = 0;
        if (CustomizeMenuSystem.current.white)
        {
            switch (CustomizeMenuSystem.current.currentPiece)
            {
                case Piece.PieceId.pawn:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whitePawn + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whitePawn = index;
                    break;

                case Piece.PieceId.rook:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whiteRook + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whiteRook = index;
                    break;

                case Piece.PieceId.knight:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whiteKnight + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whiteKnight = index;
                    break;

                case Piece.PieceId.bishop:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whiteBishop + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whiteBishop = index;
                    break;

                case Piece.PieceId.queen:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whiteQueen + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whiteQueen = index;
                    break;

                case Piece.PieceId.king:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.whiteKing + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.whiteKing = index;
                    break;
            }
        }
        else
        {
            switch (CustomizeMenuSystem.current.currentPiece)
            {
                case Piece.PieceId.pawn:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackPawn + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackPawn = index;
                    break;

                case Piece.PieceId.rook:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackRook + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackRook = index;
                    break;

                case Piece.PieceId.knight:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackKnight + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackKnight = index;
                    break;

                case Piece.PieceId.bishop:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackBishop + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackBishop = index;
                    break;

                case Piece.PieceId.queen:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackQueen + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackQueen = index;
                    break;

                case Piece.PieceId.king:
                    //get index for image
                    index = CustomizeMenuSystem.current.sf.blackKing + value;
                    index = SkinWheel.current.ControlSkinIndexRange(index);
                    CustomizeMenuSystem.current.sf.blackKing = index;
                    break;
            }
        }
        SkinWheel.current.UpdateUI();
    }
}
