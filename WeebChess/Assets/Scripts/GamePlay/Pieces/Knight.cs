using System.Collections.Generic;

public class Knight : Piece
{
    protected override void Start()
    {
        name = "Knight";
        id = PieceId.knight;
        base.Start();
    }

    protected override SlotRespons[] GetPath()
    {
        List<SlotRespons> slots = new List<SlotRespons>();
        SlotRespons currSlot;

        //top right
        currSlot = CheckSlot(1, 2, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(2, 1, MoveType.normal, null);
        slots.Add(currSlot);

        //top left
        currSlot = CheckSlot(-1, 2, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(-2, 1, MoveType.normal, null);
        slots.Add(currSlot);

        //bottom right
        currSlot = CheckSlot(1, -2, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(2, -1, MoveType.normal, null);
        slots.Add(currSlot);

        //bottom left
        currSlot = CheckSlot(-1, -2, MoveType.normal, null);
        slots.Add(currSlot);

        currSlot = CheckSlot(-2, -1, MoveType.normal, null);
        slots.Add(currSlot);

        return slots.ToArray();
    }
}