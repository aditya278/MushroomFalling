using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] mushrooms;
    public Transform[] spawnPoints;
    float spawnRate = 1.5f;
    BoyMovement Boy;

    public bool isGameActive;
    int highScore;

    //Canvas Objects
    public GameObject MainMenuCanvas;
    public GameObject GamePlayCanvas;
    public GameObject GameOverCanvas;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI PointsText;
    public TextMeshProUGUI HighScoreText;

    //Audio Clips
    public AudioClip loseClip;
    public AudioClip startClip;
    public AudioClip backgroundClip;
    public AudioSource gameAudio;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuCanvas.SetActive(true);
        GamePlayCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);

        Time.timeScale = 1;

        Boy = GameObject.FindGameObjectWithTag("Player").GetComponent<BoyMovement>();
        highScore = PlayerPrefs.GetInt("HighScore");
        HighScoreText.text = "High Score: " + highScore;

        StartCoroutine(SpawnMushrooms(spawnRate));
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            if (!gameAudio.isPlaying)
            {
                gameAudio.loop = true;
                gameAudio.clip = backgroundClip;
                gameAudio.Play();
            }

            HealthText.text = "Health: " + Boy.health;
            PointsText.text = "Points: " + Boy.score;

            if (Boy.health == 0)
            {
                Boy.boyAnimator.SetBool("IsDead", true);
                StartCoroutine(GameOver(1.5f));
            }

            if(Boy.score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", Boy.score);
            }
        }
    }

    IEnumerator GameOver(float seconds)
    {
        isGameActive = false;
        yield return new WaitForSecondsRealtime(seconds);
        MainMenuCanvas.SetActive(false);
        GamePlayCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);

        gameAudio.loop = false;
        gameAudio.clip = loseClip;
        gameAudio.Play();
    }

    IEnumerator SpawnMushrooms(float seconds)
    {
        int i = Random.Range(0, 10);
        i = i > 5 ? 1 : 0;

        int j = Random.Range(0, 40) / 10;
        yield return new WaitForSeconds(seconds);

        var position = spawnPoints[j].position;
        if(isGameActive)
            Instantiate(mushrooms[i], position, Quaternion.identity);

        float newSeconds = Random.Range(1f, spawnRate);
        StartCoroutine(SpawnMushrooms(newSeconds));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameActive = true;
        MainMenuCanvas.SetActive(false);
        GamePlayCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);

        gameAudio.loop = false;
        gameAudio.clip = startClip;
        gameAudio.Play();
    }

}
