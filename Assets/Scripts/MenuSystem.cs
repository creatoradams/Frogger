using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainMenu;
    public GameObject instructions;
    public GameObject settings;

    [Header("Pause Menu UI")]
    public GameObject pause;

    [Header("Settings")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    private bool isPaused = false;

    private void Start()
    {
        ShowMainMenu();

        if (musicToggle != null)
        {
            musicToggle.SetIsOnWithoutNotify(AudioManager.Instance.musicEnabled);
            musicToggle.onValueChanged.AddListener(val => AudioManager.Instance.SetMusicEnabled(val));

        }

        if (sfxToggle != null)
        {
            sfxToggle.SetIsOnWithoutNotify(true);
            sfxToggle.onValueChanged.AddListener(val =>
            {
                AudioManager.Instance.sfxSource.mute = !val;
                AudioManager.Instance.uiSource.mute = !val;
            });
        }
    }

    private void Update()
    {
        // Pause/unpause during gameplay (check for pause key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause != null)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }

    // MAIN MENU
    
    public void PlayGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiSource.clip);
        SceneManager.LoadScene("Frogger"); 
    }

    // Opens instructions
    public void ShowInstrcutions()
    {
        mainMenu.SetActive(false);
        instructions.SetActive(true);
    }

    // Returns to main menu from instructiosn
    public void InstructionsFromMain()
    {
        instructions.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Opens settings
    public void ShowSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);

    }

    // Returns to main menu from settings
    public void SettingsFromMain()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // PAUSE MENU   

    // Pause game and show menu
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pause.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // create this scene
    }

    // SETTINGS

    // Toggles sound effects on/off
    public void ToggleMusic()
    {
        AudioManager.Instance.SetMusicEnabled(musicToggle.isOn);

    }

    public void ToggleSFX()
    {
        bool enabled = sfxToggle.isOn;
        AudioManager.Instance.sfxSource.mute = !enabled;
        AudioManager.Instance.uiSource.mute = !enabled;
    }

    // UI MANAGEMENT

    public void ShowMainMenu()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (instructions != null) instructions.SetActive(false);
        if (settings != null) settings.SetActive(false);
        if (pause != null) pause.SetActive(false);
    }
}
