using System;
using UnityEngine;

public class CustomizeMenuSystem : MonoBehaviour
{
    public static CustomizeMenuSystem current;
    public SaveFile sf;


    private void Awake()
    {
        sf = SaveManager.Load();
        current = this;
    }
    public bool white = true;
    public Action OnPieceTabUpdate;
    public void PieceTabUpdate()
    {
        OnPieceTabUpdate.Invoke();
        SkinWheel.current.UpdateUI();
        if (white)
            cp.hexInput.text = ColorUtility.ToHtmlStringRGBA(new Color(sf.whiteColor.Item1, sf.whiteColor.Item2, sf.whiteColor.Item3));
        else
            cp.hexInput.text = ColorUtility.ToHtmlStringRGBA(new Color(sf.blackColor.Item1, sf.blackColor.Item2, sf.blackColor.Item3));
    }

    public Piece.PieceId currentPiece = Piece.PieceId.pawn;
    public Action OnPieceUpdate;
    public void PieceUpdate()
    {
        OnPieceUpdate.Invoke();
        SkinWheel.current.UpdateUI();
    }

    public FlexibleColorPicker cp; //color stuff
    private void Start()
    {
        cp.hexInput.text = ColorUtility.ToHtmlStringRGBA(new Color(sf.whiteColor.Item1, sf.whiteColor.Item2, sf.whiteColor.Item3));
    }

    private void FixedUpdate()
    {
        if (white)
            sf.whiteColor = (cp.color.r, cp.color.g, cp.color.b);
        else
            sf.blackColor = (cp.color.r, cp.color.g, cp.color.b);
    }
}
