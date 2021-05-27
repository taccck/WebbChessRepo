using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public GameObject menu;
    bool open;
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            open = !open;
            menu.SetActive(open);
        }
    }
}
