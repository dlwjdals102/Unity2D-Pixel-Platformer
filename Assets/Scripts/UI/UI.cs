using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    private PlayerInputSet input;

    public UI_Options optionsUI { get; private set; }
    public UI_FadeScreen fadeScreen { get; private set; }
    public UI_DeathScreen deathScreen { get; private set; }

    private void Awake()
    {
        instance = this;

        optionsUI = GetComponentInChildren<UI_Options>(true);
        fadeScreen = GetComponentInChildren<UI_FadeScreen>(true);
        deathScreen = GetComponentInChildren<UI_DeathScreen>(true);
    }

    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.OptionUI.performed += ctx => ToggleOptionsUI();
    }

    public void ToggleOptionsUI()
    {
        bool enable = optionsUI.gameObject.activeSelf; 

        enable = !enable;   
        optionsUI.gameObject.SetActive(enable);

        StopPlayerControls(enable);
        Time.timeScale = enable ? 0f : 1f;
    }

    public void OpenDeathScreenUI()
    {
        deathScreen.gameObject.SetActive(true);
        input.Disable();
    }

    public void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }
}
