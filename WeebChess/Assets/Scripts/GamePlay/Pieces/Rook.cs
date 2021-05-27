using System.Collections.Generic;

public class Rook : Piece
{
    protected override void Start()
    {
        name = "Rook";
        id = PieceId.rook;
        base.Start();
        //Board.current.OnCheckSpecialMove += SpecialMove;
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

            if (!up && !down && !right && !left) //break if all directions are blocked
                break;
        }
        return slots.ToArray();
    }
}