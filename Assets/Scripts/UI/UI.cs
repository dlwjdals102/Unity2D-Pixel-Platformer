using UnityEngine;

public class UI : MonoBehaviour
{
    private PlayerInputSet input;

    public UI_Options optionsUI { get; private set; }

    private void Awake()
    {
        optionsUI = GetComponentInChildren<UI_Options>(true);
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

    public void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }
}
