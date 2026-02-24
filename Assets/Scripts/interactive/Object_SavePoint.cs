using UnityEngine;

public class Object_SavePoint : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject visualGuide; // "E를 눌러 저장" 같은 UI 가이드
    private bool isPlayerNearby = false;

    private void Update()
    {
        // 플레이어가 근처에 있고, 상호작용 키(E)를 누르면 저장
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        Debug.Log("Save Game!!");
        SaveManager.instance.SaveGame();

        // 시각적 피드백 (선택 사항: 저장 애니메이션이나 사운드)
    }

    // 플레이어 감지 (Trigger2D 사용)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (visualGuide != null) 
                visualGuide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (visualGuide != null) 
                visualGuide.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }

    public void LoadData(GameData data)
    {
        Player player = Player.instance;

        if (player != null)
            player.Teleport(data.playerPosition);
    }
}
