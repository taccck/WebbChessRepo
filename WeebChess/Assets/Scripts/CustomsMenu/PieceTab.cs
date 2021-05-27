using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PieceTab : MonoBehaviour, IPointerClickHandler
{
    public GameObject border;
    Image img;
    public bool white;
    private void Start()
    {
        img = GetComponent<Image>();
        CustomizeMenuSystem.current.OnPieceTabUpdate += UpdateUI;
    }

    void UpdateUI()
    {
        if (CustomizeMenuSystem.current.white == white)
        {
            border.SetActive(false);
            img.color = new Color(87 / 255f, 87 / 255f, 87 / 255f);
        }
        else
        {
            border.SetActive(true);
            img.color = new Color(125 / 255f, 125 / 255f, 125 / 255f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SaveManager.Save(CustomizeMenuSystem.current.sf);
        CustomizeMenuSystem.current.white = white;
        CustomizeMenuSystem.current.PieceTabUpdate();
    }
}
