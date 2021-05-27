using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    bool whitesTurn = true; //whos turn
    public bool end { get; private set; }
    public string checkText { get; private set; }

    public Piece activePiece; //the clicked piece
    public List<Piece.Move> moves = new List<Piece.Move>();

    private void Awake()
    {
        current = this;
    }

    public void Click(Slot selectedSlot)
    {
        foreach (Piece.Move m in moves) //reset slots apperance
            m.slot.SwapSlotState();

        Piece.Move currMove = FindSlotInMoves(selectedSlot, moves);
        if (currMove.slot != null) //move a piece
        {
            MakeMove(currMove);
            moves.Clear();
        }
        else //display a clicked pieces moves 
        {
            moves.Clear();
            activePiece = selectedSlot.Piece;
            if (activePiece != null)
            {
                if (activePiece.white == whitesTurn)
                {
                    moves = new List<Piece.Move>(activePiece.moves);
                    foreach (Piece.Move m in moves)
                    {
                        m.slot.SwapSlotState();
                    }
                }
            }
        }
    }

    public void MakeMove(Piece.Move movingTo)
    {
        Slot slot = movingTo.slot;

        if (movingTo.moveType != Piece.MoveType.normal && movingTo.moveType != Piece.MoveType.passive) //special moves
        {
            switch (movingTo.moveType)
            {
                case Piece.MoveType.castling:
                    int sideDir = 1; //what side casting occurs on
                    if (slot.index.x == 0)
                        sideDir = -1;

                    (int, int) newKingSlot = (activePiece.slot.x + (2 * sideDir), activePiece.slot.y); //move king
                    MovePiece(activePiece, Board.current.GetSlotFromIndex(newKingSlot));

                    (int, int) newRookSlot = (activePiece.slot.x - (1 * sideDir), activePiece.slot.y); //move rook
                    MovePiece(slot.Piece, Board.current.GetSlotFromIndex(newRookSlot));
                    break;

                case Piece.MoveType.enpassant:
                    Slot removePawnAt = Board.current.GetSlotFromIndex((slot.index.x, activePiece.slot.y));
                    RemovePiece(removePawnAt);
                    MovePiece(activePiece, slot);
                    break;

                case Piece.MoveType.twoStep:
                    activePiece.GetComponent<Pawn>().enpassantable = true; //can be enpassanted
                    MovePiece(activePiece, slot);
                    break;
            }
        }
        else
        {
            if (slot.Piece != null) //if a piece is on the slot the active piece is moving to, remove it
                RemovePiece(slot);

            MovePiece(activePiece, slot);
        }

        NextTurn();
    }

    void NextTurn()
    {
        Board.current.ClearThreats();

        Board.current.SetGuardedToFalse();

        Board.current.CheckForPromotion();

        Board.current.UpdatePath();

        Board.current.SetEnpassantToFalse();

        whitesTurn = !whitesTurn;

        CheckForCheck();

        Board.current.CheckLegality();

        End();

        Board.current.UpdateCheckMenu();
    }

    List<Board.Threat> whiteInCheck = new List<Board.Threat>();
    List<Board.Threat> blackInCheck = new List<Board.Threat>();
    void CheckForCheck()
    {
        checkText = "";

        if (whitesTurn)
        {
            whiteInCheck = new List<Board.Threat>();
            foreach (Board.Threat t in Board.current.GetBlackThreat())
            {
                if (t.Threatened.id == Piece.PieceId.king)
                {
                    checkText = "White in check";
                    whiteInCheck.Add(t);
                }
            }
        }

        else
        {
            blackInCheck = new List<Board.Threat>();
            foreach (Board.Threat t in Board.current.GetWhiteThreat())
            {
                if (t.Threatened.id == Piece.PieceId.king)
                {
                    checkText = "Black in check";
                    blackInCheck.Add(t);
                }
            }
        }
    }

    void End()
    {
        if (Board.current.WhiteMoveCount == 0 || Board.current.BlackMoveCount == 0)
        {
            end = true;
            checkText = "Stalemate";


            if (whiteInCheck.Count > 0)
                checkText = "Black wins";
            else if(blackInCheck.Count > 0)
                checkText = "White wins";
        }
    }

    public Board.Threat[] IsMyKingInCheck(bool white)
    {
        if (white)
            return whiteInCheck.ToArray();
        else
            return blackInCheck.ToArray();
    }

    Piece.Move FindSlotInMoves(Slot findMe, List<Piece.Move> list)
    {
        foreach (Piece.Move m in list)
        {
            if (m.slot == findMe) return m;
        }

        return new Piece.Move();
    }

    void MovePiece(Piece piece, Slot newSlot)
    {

        Board.current.GetSlotFromIndex(piece.slot).Piece = null;
        piece.slot = newSlot.index;
        newSlot.Piece = piece;
        piece.MovePiece();
    }

    public void TestMovePiece(Piece piece, Slot newSlot, bool goingBack)
    {
        if (goingBack && Board.current.GetSlotFromIndex(piece.slot).Piece != piece) //if moving back and another piece occupied the tested slot
        {
            newSlot.Piece = piece;
        }
        else
        {
            if (newSlot.Piece == null)
            {
                Board.current.GetSlotFromIndex(piece.slot).Piece = null;
                newSlot.Piece = piece;
            }
            else //a piece is already standing there, so the path is already blocked
            {
                Board.current.GetSlotFromIndex(piece.slot).Piece = null;
            }
        }

        piece.slot = newSlot.index;
    }

    public void RemovePiece(Slot removeFrom)
    {
        removeFrom.Piece.Unsub();
        Destroy(removeFrom.Piece.gameObject);
        removeFrom.Piece = null;
    }
}
