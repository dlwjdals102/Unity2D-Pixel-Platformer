using System.Collections.Generic;
using UnityEngine;

public class UI_GameSlotsMenu : MonoBehaviour
{
    private UI_GameSlot[] gameSlots;

    private void Awake()
    {
        gameSlots = GetComponentsInChildren<UI_GameSlot>();
    }

    private void OnEnable()
    {
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        Dictionary<string, GameData> profilesGameData = SaveManager.instance.GetAllProfilesGameData();

        foreach(UI_GameSlot gameSlot in gameSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(gameSlot.GetProfileId(), out profileData);
            gameSlot.SetData(profileData);
        }
    }

    public void OnGameSlotClicked(UI_GameSlot gameSlot)
    {
        DisableMenuButtons();

        // 1. 프로필 변경 및 데이터 로드
        SaveManager.instance.ChangeSelectedProfileId(gameSlot.GetProfileId());

        // 2. 데이터 유무에 따른 분기 처리
        if (!SaveManager.instance.HasGameData())
        {
            // 데이터가 없으면 새 게임 데이터 생성
            SaveManager.instance.NewGame();
            // 새 게임 시작 씬으로 이동
            GameManager.instance.ChangeScene("Level_0", RespawnType.Load);
        }
        else
        {
            // 데이터가 있으면 저장된 씬으로 이동
            string lastScene = SaveManager.instance.GetSavedGameData().lastSceneName;
            GameManager.instance.ChangeScene(lastScene, RespawnType.Load);
        }
    }

    private void DisableMenuButtons()
    {
        foreach(var gameSlot in gameSlots)
        {
            gameSlot.SetInteractable(false);
        }
    }
}
