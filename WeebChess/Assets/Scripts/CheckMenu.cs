using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckMenu : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public GameObject butt;

    private void Start()
    {
        Board.current.OnUpdateCheckMenu += UpdateUI;
    }

    void UpdateUI()
    {
        tmp.text = GameManager.current.checkText;
        if (GameManager.current.end)
            butt.SetActive(true);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
