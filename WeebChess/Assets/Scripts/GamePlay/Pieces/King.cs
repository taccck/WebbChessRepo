using System.Collections.Generic;

public class King : Piece
{
    protected override void Start()
    {
        name = "King";
        id = PieceId.king;
        base.Start();
    }

    protected override SlotRespons[] GetPath()
    {
        List<SlotRespons> slots = new List<SlotRespons>();
        SlotRespons currSlot;

        currSlot = CheckSlot(0, 1, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(0, -1, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(1, 0, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(-1, 0, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(1, 1, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(-1, 1, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(1, -1, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(-1, -1, MoveType.normal, null);
        slots.Add(currSlot);

        //casteling
        if (unmoved)
        {
            bool emptyBetweenKingAndRook = true;
            for (int i = 1; i < 3; i++)
            {
                currSlot = CheckSlotAdvanced(i, 0, MoveType.castling, null);
                Slot findMe = Board.current.GetSlotFromIndex(currSlot.index);
                if (currSlot.slotState != SlotState.empty || IsSlotAPotentialThreat(findMe))
                {
                    emptyBetweenKingAndRook = false;
                    break;
                }
            }

            if (emptyBetweenKingAndRook)
            {
                currSlot = Castling(3, 0, MoveType.castling, null);
                slots.Add(currSlot);
            }

            for (int i = -1; i > -4; i--)
            {
                currSlot = CheckSlotAdvanced(i, 0, MoveType.castling, null);
                Slot findMe = Board.current.GetSlotFromIndex(currSlot.index);
                if (currSlot.slotState != SlotState.empty || IsSlotAPotentialThreat(findMe))
                {
                    emptyBetweenKingAndRook = false;
                    break;
                }
            }

            if (emptyBetweenKingAndRook)
            {
                currSlot = Castling(-4, 0, MoveType.castling, null);
                slots.Add(currSlot);
            }
        }
        return slots.ToArray();
    }

    SlotRespons Castling(int xDisplace, int yDisplace, MoveType moveType, List<(int, int)> path)
    {
        SlotRespons slotRespons = CheckSlotAdvanced(xDisplace, yDisplace, moveType, path);
        if (slotRespons.slotState == SlotState.friendlyPiece)
        {
            Piece rook = Board.current.GetSlotFromIndex(slotRespons.index).Piece;

            if (rook.id == PieceId.rook && rook.unmoved)
                slotRespons.walkable = true; //should also change index
        }

        return slotRespons;
    }

    public override void CheckLegallity()
    {
        List<Move> legalMoves = new List<Move>();
        foreach (Move m in moves)
        {
            bool addMove = true;

            if (IsSlotAPotentialThreat(m.slot))
                addMove = false;

            Piece currPiece = m.slot.Piece;
            if (currPiece != null)
            {
                if (currPiece.guarded)
                    addMove = false;
            }

            if (addMove) legalMoves.Add(m);
        }

        moves = legalMoves.ToArray();
    }

    bool IsSlotAPotentialThreat(Slot findMe)
    {
        foreach (Slot s in Board.current.GetPotentailThreat(white))
        {
            if (findMe == s) return true;
        }

        return false;
    }
}
