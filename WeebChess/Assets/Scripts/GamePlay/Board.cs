using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
    public static Board current;
    public GameObject slot;
    public GameObject pawn;
    public GameObject rook;
    public GameObject knight;
    public GameObject bishop;
    public GameObject queen;
    public GameObject king;
    public GameObject black;
    public GameObject white;
    public Slot[,] slots = new Slot[8, 8];

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        MakeBoard();

        SetBoard();
    }

    void MakeBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                slots[x, y] = Instantiate(slot, transform).GetComponent<Slot>();
                slots[x, y].transform.position = new Vector2(x + transform.position.x, y + transform.position.y);
                slots[x, y].index = (x, y);
                slots[x, y].name = NumToChar(x + 1) + (y + 1).ToString();

                if (x % 2 == 0) //set color
                {
                    if (y % 2 == 0)
                        slots[x, y].IdleColor = PlayerCustoms.current.black;
                    else
                        slots[x, y].IdleColor = PlayerCustoms.current.white;
                }
                else
                {
                    if (y % 2 == 0)
                        slots[x, y].IdleColor = PlayerCustoms.current.white;
                    else
                        slots[x, y].IdleColor = PlayerCustoms.current.black;
                }
            }
        }
    }

    char NumToChar(int num)
    {
        switch (num)
        {
            case 1:
                return 'a';
            case 2:
                return 'b';
            case 3:
                return 'c';
            case 4:
                return 'd';
            case 5:
                return 'e';
            case 6:
                return 'f';
            case 7:
                return 'g';
            case 8:
                return 'h';
            default:
                return 'q';
        }
    }

    void SetBoard()
    {
        //pawns
        for (int x = 0; x < 8; x++)
            AddPiece(x, 1, pawn, true);

        for (int x = 0; x < 8; x++)
            AddPiece(x, 6, pawn, false);

        //rooks
        AddPiece(0, 0, rook, true);
        AddPiece(7, 0, rook, true);
        AddPiece(0, 7, rook, false);
        AddPiece(7, 7, rook, false);

        //knights
        AddPiece(1, 0, knight, true);
        AddPiece(6, 0, knight, true);
        AddPiece(1, 7, knight, false);
        AddPiece(6, 7, knight, false);

        //Bishop
        AddPiece(2, 0, bishop, true);
        AddPiece(5, 0, bishop, true);
        AddPiece(2, 7, bishop, false);
        AddPiece(5, 7, bishop, false);

        //Queens
        AddPiece(3, 0, queen, true);
        AddPiece(3, 7, queen, false);

        //king
        AddPiece(4, 0, king, true);
        AddPiece(4, 7, king, false);
    }

    public void AddPiece(int _x, int _y, GameObject _piece, bool _white)
    {
        GameObject parent = _white ? white : black;
        slots[_x, _y].Piece = Instantiate(_piece, parent.transform).GetComponent<Piece>();
        slots[_x, _y].Piece.white = _white;
        slots[_x, _y].Piece.slot = (_x, _y);
        slots[_x, _y].Piece.MovePiece();
    }

    public Slot GetSlotFromIndex((int x, int y) index)
    {
        return slots[index.x, index.y];
    }

    public Slot[] GetSlotFromIndex((int x, int y)[] indexes)
    {
        List<Slot> slots = new List<Slot>();
        foreach ((int, int) i in indexes)
            slots.Add(GetSlotFromIndex(i));

        return slots.ToArray();
    }

    public event Action OnUpdatePath;
    public void UpdatePath()
    {
        OnUpdatePath.Invoke();
    }

    public event Action OnSetEnpassantToFalse;
    public void SetEnpassantToFalse()
    {
        OnSetEnpassantToFalse.Invoke();
    }

    public event Action OnSetGuardedToFalse;
    public void SetGuardedToFalse()
    {
        OnSetGuardedToFalse.Invoke();
    }

    public event Action OnCheckForPromotion;
    public void CheckForPromotion()
    {
        OnCheckForPromotion.Invoke();
    }

    public event Action OnUpdateCheckMenu;
    public void UpdateCheckMenu()
    {
        OnUpdateCheckMenu.Invoke();
    }

    public event Action OnCheckLegality;
    public void CheckLegality()
    {
        OnCheckLegality.Invoke();
    }

    public int WhiteMoveCount { get; private set; }
    HashSet<Slot> potentialWhiteThreat = new HashSet<Slot>();
    List<Threat> whiteThreat = new List<Threat>();
    public Threat[] GetWhiteThreat() { return whiteThreat.ToArray(); }

    public int BlackMoveCount { get; private set; }
    HashSet<Slot> potentialBlackThreat = new HashSet<Slot>();
    List<Threat> blackThreat = new List<Threat>();
    public Threat[] GetBlackThreat() { return blackThreat.ToArray(); }
    public struct Threat
    {
        public Slot[] Interuptable { get; private set; }
        public Piece Threatening { get; private set; }
        public Piece Threatened { get; private set; }

        public void Create(Slot[] interuptable, Piece threatening, Piece threatened)
        {
            Interuptable = interuptable;
            Threatening = threatening;
            Threatened = threatened;
        }
    }

    public void AddThreat(Slot[] interuptable, Piece threatening, Piece threatened, bool white)
    {
        Threat threat = new Threat();
        threat.Create(interuptable, threatening, threatened);
        if (white)
            whiteThreat.Add(threat);
        else
            blackThreat.Add(threat);
    }

    public void AddAttackSlot((int x, int y) index, bool white)
    {
        if (white)
            potentialWhiteThreat.Add(GetSlotFromIndex(index));
        else
            potentialBlackThreat.Add(GetSlotFromIndex(index));
    }

    public Slot[] GetPotentailThreat(bool white)
    {
        if (white)
        {
            Slot[] potentialThreat = new Slot[potentialBlackThreat.Count];
            potentialBlackThreat.CopyTo(potentialThreat);
            return potentialThreat;
        }
        else
        {
            Slot[] potentialThreat = new Slot[potentialWhiteThreat.Count];
            potentialWhiteThreat.CopyTo(potentialThreat);
            return potentialThreat;
        }
    }

    public void AddMoveCount(int amount, bool white)
    {
        if (white)
            WhiteMoveCount += amount;
        else
            BlackMoveCount += amount;
    }

    public void ClearThreats()
    {
        blackThreat = new List<Threat>();
        whiteThreat = new List<Threat>();
        potentialWhiteThreat = new HashSet<Slot>();
        potentialBlackThreat = new HashSet<Slot>();
        WhiteMoveCount = 0;
        BlackMoveCount = 0;
    }

    public Threat[] IsPieceThreatened(Piece piece)
    {
        List<Threat> threatsToPiece = new List<Threat>();
        List<Threat> threats = (piece.white) ? blackThreat : whiteThreat;

        foreach (Threat threat in threats)
        {
            if (threat.Threatened == piece)
                threatsToPiece.Add(threat);
        }

        return threatsToPiece.ToArray();
    }
}
