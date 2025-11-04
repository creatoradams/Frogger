using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private BasicMovement frogger;
    private Home[] homes;
    public GameObject gameOverMenu;

    private int score;
    private int lives;
    private int time;

    private void Awake()
    {

        // Will search through scene and return an array
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<BasicMovement>();

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
        NewLevel();

    }

    // Maintains score, lives
    private void NewLevel()
    {

        for(int i = 0; i < homes.Length; i++)
        {

            homes[i].enabled = false;

        }

        Respawn();

    }

    private void Respawn()
    {

        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));

    }

    private IEnumerator Timer(int duration)
    {

        time = duration;
        while (time > 0)
        {

            yield return new WaitForSeconds(1);
            time--;

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

        if (Cleared())
        {

            SetScore(score + 1000);
            SetLives(lives + 1);
            Invoke(nameof(NewLevel), 1f);

        }
        else
        {

            Invoke(nameof(Respawn), 1f);

        }

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
        // Add UI

    }

    private void SetLives(int lives)
    {

        this.lives = lives;
        // Add UI

    }

}
