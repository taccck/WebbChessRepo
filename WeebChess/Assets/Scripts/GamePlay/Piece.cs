using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceId { pawn, rook, knight, bishop, queen, king }
    public PieceId id;
    public SpriteRenderer hat;
    public SpriteRenderer skin;
    public bool white;
    public (int x, int y) slot;
    public Move[] moves;
    public bool unmoved;
    public bool guarded; //set to false from event system

    public List<(Piece, List<Slot>)> threateners = new List<(Piece, List<Slot>)>();
    public List<(Piece, Slot)> protecters = new List<(Piece, Slot)>();

    protected virtual void Start()
    {
        if (white)
            hat.color = PlayerCustoms.current.white;
        else
            hat.color = PlayerCustoms.current.black;

        PlayerCustoms.current.SetPieceSkins(this);

        MovePiece();
        unmoved = true;

        Board.current.OnSetGuardedToFalse += SetGuardedToFalse;
        Board.current.OnUpdatePath += UpdatePath;
        Board.current.OnCheckLegality += CheckLegallity;
        UpdatePath();
    }

    public virtual void Unsub()
    {
        Board.current.OnSetGuardedToFalse -= SetGuardedToFalse;
        Board.current.OnUpdatePath -= UpdatePath;
        Board.current.OnCheckLegality -= CheckLegallity;
    }

    void UpdatePath()
    {
        moves = GetWalkableMoves(GetPath());
    }

    protected virtual SlotRespons[] GetPath()
    {
        //show where it can move

        //code below is an example on how to use it, should be overriden

        List<SlotRespons> slots = new List<SlotRespons>();

        slots.Add(CheckSlot(0, 1, MoveType.normal, null));

        return slots.ToArray();
    }

    protected Move[] GetWalkableMoves(SlotRespons[] slotRespons)
    {
        List<Move> newMoves = new List<Move>();

        foreach (SlotRespons currSlotRespons in slotRespons)
        {
            if (currSlotRespons.moveType == MoveType.normal && currSlotRespons.slotState == SlotState.friendlyPiece) //move protects another piece
            {
                Board.current.GetSlotFromIndex(currSlotRespons.index).Piece.guarded = true;
            }

            if (currSlotRespons.moveType == MoveType.normal && currSlotRespons.slotState != SlotState.offBoard) //add potential threat
                Board.current.AddAttackSlot(currSlotRespons.index, white);

            if (currSlotRespons.walkable)
            {
                Move move = new Move();
                move.Create(currSlotRespons.moveType, currSlotRespons.index);
                newMoves.Add(move);

                if (currSlotRespons.slotState == SlotState.enemyPiece)
                {
                    currSlotRespons.path.Add(slot);
                    Slot[] newPath = Board.current.GetSlotFromIndex(currSlotRespons.path.ToArray());
                    Board.current.AddThreat(newPath, this, Board.current.GetSlotFromIndex(currSlotRespons.index).Piece, white);
                }
            }
        }

        return newMoves.ToArray();
    }

    public bool DoIThreatningTheEnemyKing()
    {
        foreach (SlotRespons slotRespons in GetPath())
        {
            if (IsKingOnSlot(slotRespons)) return true;
        }

        return false;
    }

    protected bool IsKingOnSlot(SlotRespons slotRespons)
    {
        if (slotRespons.slotState == SlotState.enemyPiece)
        {
            Piece currPiece = Board.current.GetSlotFromIndex(slotRespons.index).Piece;
            if (currPiece.id == PieceId.king) return true;
        }
        return false;
    }

    public virtual void MovePiece()
    {
        transform.position = Board.current.GetSlotFromIndex(slot).transform.position;
        unmoved = false;
    }

    protected SlotRespons CheckSlotAdvanced(int xDisplace, int yDisplace, MoveType moveType, List<(int x, int y)> path)
    {
        SlotRespons slotRespons = new SlotRespons();

        slotRespons.index.x = slot.x + xDisplace;
        slotRespons.index.y = slot.y + yDisplace;
        slotRespons.moveType = moveType;
        if (path == null)
            slotRespons.path = new List<(int x, int y)>();
        else
            slotRespons.path = path;

        if (slotRespons.index.x < 8 && slotRespons.index.x >= 0 && slotRespons.index.y < 8 && slotRespons.index.y >= 0) //is the slot index is on the board
        {
            Piece piece = Board.current.slots[slotRespons.index.x, slotRespons.index.y].Piece;
            if (piece != null) // if slot has a piece on it
            {
                if (piece.white != white) //if piece on slot is not the same color as this piece
                    slotRespons.slotState = SlotState.enemyPiece;
                else
                    slotRespons.slotState = SlotState.friendlyPiece;
            }
            else
                slotRespons.slotState = SlotState.empty;
        }
        else
            slotRespons.slotState = SlotState.offBoard;

        return slotRespons;
    }

    protected SlotRespons CheckSlot(int xDisplace, int yDisplace, MoveType moveType, List<(int x, int y)> path)
    {
        SlotRespons slotRespons = CheckSlotAdvanced(xDisplace, yDisplace, moveType, path);

        if (slotRespons.slotState == SlotState.empty || slotRespons.slotState == SlotState.enemyPiece)
            slotRespons.walkable = true;
        else
            slotRespons.walkable = false;

        return slotRespons;
    }

    protected SlotRespons CheckContinousSlot(int xDisplace, int yDisplace, MoveType moveType, List<(int x, int y)> path)
    {
        SlotRespons slotRespons = CheckSlotAdvanced(xDisplace, yDisplace, moveType, path);

        if (slotRespons.slotState == SlotState.empty || slotRespons.slotState == SlotState.enemyPiece)
            slotRespons.walkable = true;
        else
            slotRespons.walkable = false;

        return slotRespons;
    }

    public virtual void CheckLegallity() //sets moves to only the legal ones //returns an int or how manu legal moves can be made
    {
        //is the king in check
        Board.Threat[] threatsToKing = GameManager.current.IsMyKingInCheck(white);

        List<Move> legalMoves = new List<Move>();

        if (threatsToKing.Length == 0) //all moves are legal
        {
            legalMoves = new List<Move>(moves);
        }

        else if (threatsToKing.Length == 1) //can only interupt if there is one threat
        {
            foreach (Slot s in threatsToKing[0].Interuptable)
            {
                foreach (Move m in moves)
                {
                    if (m.slot.index == s.index)
                        legalMoves.Add(m);
                }
            }
        }

        else if (threatsToKing.Length > 1)
        {
            //have to move king if there are two threats so no moves are legal
        }

        //will the king be in check if I make this move
        Board.Threat[] threatsToPiece = Board.current.IsPieceThreatened(this);
        if (threatsToPiece.Length > 0)
        {
            Slot goBack = Board.current.GetSlotFromIndex(slot);
            foreach (Board.Threat threat in threatsToPiece) //loop thorough all pieces threatning this piece
            {
                foreach (Move move in legalMoves.ToArray()) //loop through all moves this piece can make to see if any of the threats will pass on to the king if the move is made
                {
                    GameManager.current.TestMovePiece(this, move.slot, false);

                    if (threat.Threatening.DoIThreatningTheEnemyKing())
                        legalMoves.Remove(move);

                    GameManager.current.TestMovePiece(this, goBack, true);
                }
            }
        }

        moves = legalMoves.ToArray();
        Board.current.AddMoveCount(moves.Length, white);
    }

    public void SetGuardedToFalse()
    {
        guarded = false;
    }

    public enum SlotState { offBoard, friendlyPiece, empty, enemyPiece, thretened }
    public struct SlotRespons
    {
        public List<(int x, int y)> path;
        public MoveType moveType;
        public SlotState slotState;
        public (int x, int y) index;
        public bool walkable;
    }

    public enum MoveType { normal, castling, enpassant, twoStep, passive }
    public struct Move
    {
        public MoveType moveType;
        public Slot slot;

        public void Create(MoveType _moveType, (int, int) index)
        {
            moveType = _moveType;
            slot = Board.current.GetSlotFromIndex(index);
        }
    }
}