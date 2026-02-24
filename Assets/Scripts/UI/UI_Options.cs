using UnityEngine;

public class UI_Options : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        Debug.Log("Click");
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }
}
