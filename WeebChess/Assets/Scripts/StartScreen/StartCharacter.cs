using UnityEngine;
using UnityEngine.UI;

public class StartCharacter : MonoBehaviour
{
    public SkinDB skinDB;
    private void Start()
    {
        SaveManager.NewSave();
        SaveFile sf = SaveManager.Load();
        GetComponent<Image>().sprite = skinDB.skins[sf.whiteKing];
    }
}
