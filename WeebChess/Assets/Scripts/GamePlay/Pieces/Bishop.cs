using System.Collections.Generic;

public class Bishop : Piece
{
    protected override void Start()
    {
        name = "Bishop";
        id = PieceId.bishop;
        base.Start();
    }

    protected override SlotRespons[] GetPath()
    {
        List<SlotRespons> slots = new List<SlotRespons>();
        SlotRespons currSlot;

        bool topR = true;
        List<(int, int)> topRSlots = new List<(int, int)>();

        bool topL = true;
        List<(int, int)> topLSlots = new List<(int, int)>();

        bool bottomR = true;
        List<(int, int)> bottomRSlots = new List<(int, int)>();

        bool bottomL = true;
        List<(int, int)> bottomLSlots = new List<(int, int)>();

        for (int distance = 1; distance < 8; distance++)
        {
            if (topR)
            {
                currSlot = CheckContinousSlot(distance, distance, MoveType.normal, topRSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece) //stop at enemy, but add the slot to path
                    topR = false;
                else
                    topRSlots.Add(currSlot.index);
            }

            if (topL)
            {
                currSlot = CheckContinousSlot(-distance, distance, MoveType.normal, topLSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    topL = false;
                else
                    topLSlots.Add(currSlot.index);
            }

            if (bottomR)
            {
                currSlot = CheckContinousSlot(distance, -distance, MoveType.normal, bottomRSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    bottomR = false;
                else
                    bottomRSlots.Add(currSlot.index);
            }


            if (bottomL)
            {
                currSlot = CheckContinousSlot(-distance, -distance, MoveType.normal, bottomLSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    bottomL = false;
                else
                    bottomLSlots.Add(currSlot.index);
            }

            if (!topR && !topL && !bottomR && !bottomL)
                break;
        }

        return slots.ToArray();
    }
}