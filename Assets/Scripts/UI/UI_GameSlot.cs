using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] TextMeshProUGUI text;

    private Button gameSlotButton;

    private void Awake()
    {
        gameSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // 저장한 시간, 이미지, scene 이름정도 넣어주면 베스트

        if (data == null)
        {
            text.text = "Empty";
        }
        else
        {
            text.text = "Exist";
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        gameSlotButton.interactable = interactable;
    }
}
