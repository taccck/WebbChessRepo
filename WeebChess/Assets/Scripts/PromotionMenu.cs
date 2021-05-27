using UnityEngine;
using UnityEngine.UI;

public class PromotionMenu : MonoBehaviour
{
    public static PromotionMenu current;
    private void Start()
    {
        current = this;
    }

    Pawn pawn;
    public GameObject menu;
    public Image[] backs;
    public void Open(Pawn _pawn)
    {
        menu.SetActive(true);
        pawn = _pawn;
        foreach (Image i in backs)
        {
            if (pawn.white)
            {
                i.color = PlayerCustoms.current.white;
            }
            else
            {
                i.color = PlayerCustoms.current.black;
            }
        }
    }

    public void AddRook()
    {
        GameManager.current.RemovePiece(Board.current.GetSlotFromIndex(pawn.slot)); //remove pawn
        Board.current.AddPiece(pawn.slot.x, pawn.slot.y, Board.current.rook, pawn.white); //add new piece
        menu.SetActive(false); //hide menu
    }

    public void AddKnight()
    {
        GameManager.current.RemovePiece(Board.current.GetSlotFromIndex(pawn.slot));
        Board.current.AddPiece(pawn.slot.x, pawn.slot.y, Board.current.knight, pawn.white);
        menu.SetActive(false);
    }

    public void AddBishop()
    {
        GameManager.current.RemovePiece(Board.current.GetSlotFromIndex(pawn.slot));
        Board.current.AddPiece(pawn.slot.x, pawn.slot.y, Board.current.bishop, pawn.white);
        menu.SetActive(false);
    }

    public void AddQueen()
    {
        GameManager.current.RemovePiece(Board.current.GetSlotFromIndex(pawn.slot));
        Board.current.AddPiece(pawn.slot.x, pawn.slot.y, Board.current.queen, pawn.white);
        menu.SetActive(false);
    }
}
