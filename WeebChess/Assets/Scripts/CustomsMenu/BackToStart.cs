using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStart : MonoBehaviour
{
    public void Back()
    {
        SaveManager.Save(CustomizeMenuSystem.current.sf);
        SceneManager.LoadScene("StartScreen");
    }
}
