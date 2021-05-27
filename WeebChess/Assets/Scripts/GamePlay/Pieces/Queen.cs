using System.Collections.Generic;

public class Queen : Piece
{
    protected override void Start()
    {
        name = "Queen";
        id = PieceId.queen;
        base.Start();
    }

    protected override SlotRespons[] GetPath()
    {
        List<SlotRespons> slots = new List<SlotRespons>();
        SlotRespons currSlot;

        bool up = true; //doesn't check further in direction if something is blocking the path
        List<(int, int)> upSlots = new List<(int, int)>();

        bool down = true;
        List<(int, int)> downSlots = new List<(int, int)>();

        bool right = true;
        List<(int, int)> rightSlots = new List<(int, int)>();

        bool left = true;
        List<(int, int)> leftSlots = new List<(int, int)>();

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
            if (up)
            {
                currSlot = CheckContinousSlot(0, distance, MoveType.normal, upSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    up = false;
                else
                    upSlots.Add(currSlot.index);
            }

            if (down)
            {
                currSlot = CheckContinousSlot(0, -distance, MoveType.normal, downSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    down = false;
                else
                    downSlots.Add(currSlot.index);
            }

            if (right)
            {
                currSlot = CheckContinousSlot(distance, 0, MoveType.normal, rightSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    right = false;
                else
                    rightSlots.Add(currSlot.index);
            }

            if (left)
            {
                currSlot = CheckContinousSlot(-distance, 0, MoveType.normal, leftSlots);
                slots.Add(currSlot);
                if (!currSlot.walkable || currSlot.slotState == SlotState.enemyPiece)
                    left = false;
                else
                    leftSlots.Add(currSlot.index);
            }

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

            if (!up && !down && !right && !left && !topR && !topL && !bottomR && !bottomL)
                break;
        }

        return slots.ToArray();
    }
}