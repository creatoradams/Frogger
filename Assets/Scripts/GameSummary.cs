using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSummary : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelsText;
    public TextMeshProUGUI deathsText;

    private void Start() {

        scoreText.text = GameStats.finalScore.ToString();
        levelsText.text = GameStats.levelsCompleted.ToString();
        deathsText.text = GameStats.totalDeaths.ToString();
    }

    public void BackToMainMenu() {

        SceneManager.LoadScene("MainMenu");
    
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Frogger");
    }

}
