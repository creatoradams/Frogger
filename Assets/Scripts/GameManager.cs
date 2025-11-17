using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{

    private BasicMovement frogger;
    private Home[] homes;
    public GameObject gameOverMenu;
    public GameObject levelCompleted;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelCompletedText;
    private float speedScale = 1.15f; // increase object speed per level by 15%.
    private int level = 0;
    private MoveCycle[] cycles;
    private int score;
    private int lives;
    private int time;

    private void Awake()
    {

        // Will search through scene and return an array
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<BasicMovement>();

        cycles = FindObjectsOfType<MoveCycle>(true); // initialize the lanes

    }

    private void Start()
    {

        NewGame();

    }

    // Reset game
    private void NewGame()
    {

        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);

        // reset level and lane speeds
        level = 0;
        foreach (var c in cycles)
        {
            c.ResetToBase();
        }
        NewLevel();
    }

    // Maintains score, lives
    private void NewLevel()
    {

        for(int i = 0; i < homes.Length; i++)
        {

            homes[i].enabled = false;

        }

        level++;
        if (level > 1) // make sure we are above the first level
        {
            // Increase speed of all lanes
            foreach (var c in cycles)
            {
                c.speed *= speedScale;
            }
        }
        Respawn();
    }

    private void Respawn()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.respawnSound);
        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));

    }

    private IEnumerator Timer(int duration)
    {

        time = duration;
        timerText.text = time.ToString();

        while (time > 0)
        {

            yield return new WaitForSeconds(1);
            time--;

            if (time == 10)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.timeWarningSound);
            }
           timerText.text = time.ToString();

        }

        frogger.Death();

    }

    public void Died()
    {

        SetLives(lives - 1);

        if (lives > 0)
        {

            Invoke(nameof(Respawn), 1f);

        }
        else
        {

            // Game over
            Invoke(nameof(GameOver), 1f);

        }

    }

    private void GameOver()
    {
        AudioManager.Instance.PlayWithDucking(AudioManager.Instance.gameOverSound, 2f);
        frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());

    }

    private IEnumerator PlayAgain()
    {

        bool playAgain = false;

        while (!playAgain)
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {

                playAgain = true;

            }

            yield return null;

        }

        NewGame();

    }

    public void AdvancedRow()
    {

        SetScore(score + 10);

    }

    public void HomeOccupied()
    {

        frogger.gameObject.SetActive(false);
        int bonusPoints = time * 20;
        SetScore(score + 50 + bonusPoints);

        // Play a different sound if time is left when the frog makes it home
        if (time >= 20)
        {
            AudioManager.Instance.PlayWithDucking(AudioManager.Instance.specialHomeSound);
        }
        else
        {
            AudioManager.Instance.PlayWithDucking(AudioManager.Instance.homeSound);
        }

        if (Cleared())
        {
            SetScore(score + 1000);
            SetLives(lives + 1);
            StartCoroutine(LevelCompletedSequence());
            //AudioManager.Instance.PlayWithDucking(AudioManager.Instance.levelCompletedSound, 2f);
            //Invoke(nameof(NewLevel), 1f);

        }
        else
        {

            Invoke(nameof(Respawn), 1f);

        }

    }

    private IEnumerator LevelCompletedSequence()
    {
        // setting text dynamically
        levelCompletedText.text = "Level " + level + " Completed!";

        levelCompleted.SetActive(true);
        frogger.gameObject.SetActive(false);

        AudioManager.Instance.PlayWithDucking(AudioManager.Instance.levelCompletedSound, 2f);

        yield return new WaitForSeconds(2f); // show text for 2 seconds

        levelCompleted.SetActive(false);
        NewLevel();
    }

    private bool Cleared()
    {

        for (int i = 0; i < homes.Length; i++)
        {

            if (!homes[i].enabled)
            {

                return false;

            }

        }

        return true;

    }

    private void SetScore(int score)
    {

        this.score = score;
       scoreText.text = score.ToString();
        // Add UI

    }

    private void SetLives(int lives)
    {

        this.lives = lives;
        livesText.text = lives.ToString();
        // Add UI

    }

}
