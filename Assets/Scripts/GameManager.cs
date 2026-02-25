using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RespawnType
{
    Enter,
    Exit,
    Load,
    None
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string currentScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.DoFadeOut();
        yield return fadeScreen.fadeEffectCo;

        // 씬 로드 전 메모리 저장 (파일저장 X)
        SaveManager.instance.UpdateSceneToMemory();

        // 씬 로드 실행
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while(!op.isDone) yield return null;

        // 씬 로드 완료 후 데이터 주입
        if (respawnType == RespawnType.Load)
            SaveManager.instance.LoadGame();
        else if (respawnType == RespawnType.Enter || respawnType == RespawnType.Exit)
        {
            SaveManager.instance.SyncMemoryToScene();

            Player player = Player.instance;
            player.Teleport(GetWaypointPosition(respawnType));
        }

        // 씬 전환 후 참조를 잃을 가능성이 높음
        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn();
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance != null)
            return UI.instance.fadeScreen;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var waypoint in waypoints)
        {
            if (waypoint.GetWaypointType() == type)
            {
                waypoint.SetCanBeTriggered(false);
                return waypoint.GetPosition();
            }
        }

        return Vector3.zero;
    }
    
}
