using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenOptions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Customize()
    {
        SceneManager.LoadScene("Customize");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
