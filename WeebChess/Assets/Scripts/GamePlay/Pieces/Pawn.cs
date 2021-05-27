using System.Collections.Generic;

public class Pawn : Piece
{
    public bool enpassantable;
    int colorDirection;

    protected override void Start()
    {
        name = "Pawn";
        id = PieceId.pawn;
        colorDirection = white ? 1 : -1; //white moves in poistiv y axis, black moves in negativ

        Board.current.OnSetEnpassantToFalse += SetEnpassantToFalse;
        Board.current.OnCheckForPromotion += CheckForPormotion;

        base.Start();
    }

    public override void Unsub()
    {
        base.Unsub();
        Board.current.OnSetEnpassantToFalse -= SetEnpassantToFalse;
        Board.current.OnCheckForPromotion -= CheckForPormotion;
    }

    protected override SlotRespons[] GetPath()
    {
        List<SlotRespons> slots = new List<SlotRespons>();
        SlotRespons currSlot;

        currSlot = CheckForwardSlot(1 * colorDirection, MoveType.passive);
        slots.Add(currSlot);
        if (currSlot.walkable && unmoved)
        {
            currSlot = CheckForwardSlot(2 * colorDirection, MoveType.twoStep); //2 step
            slots.Add(currSlot);
        }

        currSlot = CheckDiagonalSlot(1, 1 * colorDirection); //right
        slots.Add(currSlot);
        if (!currSlot.walkable) //can only do enpassant when the pawn can't move diagonally normally
        {
            currSlot = Enpassant(1, 1 * colorDirection);
            slots.Add(currSlot);
        }

        currSlot = CheckDiagonalSlot(-1, 1 * colorDirection); //left
        slots.Add(currSlot);
        if (!currSlot.walkable)
        {
            currSlot = Enpassant(-1, 1 * colorDirection);
            slots.Add(currSlot);
        }

        return slots.ToArray();
    }

    SlotRespons CheckForwardSlot(int yDisplace, MoveType moveType)
    {
        SlotRespons slotRespons = CheckSlotAdvanced(0, yDisplace, moveType, null);
        if (slotRespons.slotState == SlotState.empty) //has to be empty because pawns cant remove pieces on vertical movments
            slotRespons.walkable = true;

        return slotRespons;
    }

    SlotRespons CheckDiagonalSlot(int xDisplace, int yDisplace)
    {
        SlotRespons slotRespons = CheckSlotAdvanced(xDisplace, yDisplace, MoveType.normal, null);

        if (slotRespons.slotState == SlotState.enemyPiece) //has to have an enemy piece on it to walk diagonally
            slotRespons.walkable = true;

        return slotRespons;
    }

    SlotRespons Enpassant(int xDisplace, int yDisplace)
    {
        SlotRespons slotRespons = CheckSlot(xDisplace, yDisplace, MoveType.enpassant, null);
        if (slotRespons.walkable) //if true, the slot is empty since enemy pieces have alredy been cheked for in CheckDiagonalSlot()
        {
            if (CheckSlotAdvanced(xDisplace, 0, MoveType.normal, null).slotState == SlotState.enemyPiece) //make sure slot next to pawn has enemy
            {
                Piece piece = Board.current.GetSlotFromIndex((slot.x + xDisplace, slot.y)).Piece;
                if (piece.id == PieceId.pawn) //if the enemy piece is a pawn
                {
                    Pawn pawn = piece.GetComponent<Pawn>();
                    if (pawn.enpassantable) //and that pawns just took two steps
                    {
                        slotRespons.walkable = true;
                        return slotRespons;
                    }
                }
            }
        }

        slotRespons.walkable = false;
        return slotRespons;
    }

    void SetEnpassantToFalse()
    {
        enpassantable = false;
    }

    void CheckForPormotion()
    {
        if (colorDirection > 0 && slot.y == 7)
            PromotionMenu.current.Open(this);

        if (colorDirection < 0 && slot.y == 0)
            PromotionMenu.current.Open(this);
    }
}