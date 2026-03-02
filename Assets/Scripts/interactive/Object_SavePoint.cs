using UnityEngine;

public class Object_SavePoint : MonoBehaviour, ISaveable, IInteractable
{
    [SerializeField] private GameObject visualGuide; // "E를 눌러 저장" 같은 UI 가이드
    private bool isPlayerNearby = false;


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

    public void Interact()
    {
        Debug.Log("Save Game!!");
        SaveManager.instance.SaveGame();
    }
}
