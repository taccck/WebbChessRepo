using UnityEngine;

public class Slot : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public (int x, int y) index;
    public Piece Piece { get; set; }
    public Sprite activeSprite;
    public Sprite idleSprite;
    public Color activeColor;
    public Color IdleColor { get; set; }
    bool active = false;

    private void Start()
    {
        spriteRenderer.sprite = idleSprite;
        spriteRenderer.color = IdleColor;
    }

    bool clicked;
    private void OnMouseDown()
    {
        if (!PromotionMenu.current.menu.activeSelf)
        {
            clicked = true;
            GameManager.current.Click(this);
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    static bool hasDraged;
    Vector3 startMousePos;
    private void OnMouseDrag()
    {
        if (clicked && Piece != null)
        {
            Vector3 currMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (currMousePos != startMousePos && Piece != null) //only move the slots piece if it exists and the mouse has moved since it clicked
                hasDraged = true;

            if (hasDraged)
            {
                Piece.transform.position = new Vector2(currMousePos.x, currMousePos.y);
            }
        }
    }

    private void OnMouseOver()
    {
        currHovered = this; //remember what slot the mouse is at
    }

    static Slot currHovered;
    private void OnMouseUp()
    {
        if (hasDraged)
        {
            Piece.transform.position = transform.position; //reset position is can't move to hovered slot
            GameManager.current.Click(currHovered);
            hasDraged = false;
        }

        clicked = false;
    }

    public void SwapSlotState()
    {
        active = !active;
        if (active)
        {
            spriteRenderer.sprite = activeSprite;
            spriteRenderer.color = activeColor;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
            spriteRenderer.color = IdleColor;
        }
    }
}
