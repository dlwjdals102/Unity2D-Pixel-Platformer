using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SaveManager.instance.SaveGame();
    }

    public void QuitToMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }

    public void RespawnBTN()
    {
        string lastScene = SaveManager.instance.GetSavedGameData().lastSceneName;
        GameManager.instance.ChangeScene(lastScene, RespawnType.Load);
    }
}
