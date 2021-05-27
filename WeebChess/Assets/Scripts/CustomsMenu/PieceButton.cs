using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PieceButton : MonoBehaviour, IPointerClickHandler
{
    Image img;
    public Piece.PieceId piece;

    private void Start()
    {
        img = GetComponent<Image>();
        CustomizeMenuSystem.current.OnPieceUpdate += UpdateUI;
    }

    void UpdateUI()
    {
        if (CustomizeMenuSystem.current.currentPiece == piece)
        {
            img.color = new Color(65 / 255f, 65 / 255f, 65 / 255f);
        }
        else
        {
            img.color = new Color(87 / 255f, 87 / 255f, 87 / 255f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CustomizeMenuSystem.current.currentPiece = piece;
        CustomizeMenuSystem.current.PieceUpdate();
    }
}
