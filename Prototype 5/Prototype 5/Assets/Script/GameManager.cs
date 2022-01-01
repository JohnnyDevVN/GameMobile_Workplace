using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    ///// Using InvokeRepeating
    // public List<GameObject> target;
    // private float spawnRate = 2f;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     //StartCoroutine(SpawnTarget());
    //     InvokeRepeating("SpawnTarget", spawnRate, spawnRate);
    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    // void SpawnTarget()
    // {
    //     // while(true)
    //     // {
    //         //yield return new WaitForSeconds(spawnRate);
    //         int index = Random.Range(0, target.Count);
    //         Instantiate(target[index]);
    //     // }
    // }
   
    public List<GameObject> target;
    public TextMeshProUGUI scoreText;
    public Button restartButton;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject quitButton;
    public int score;
    public AudioClip shootSound;
    public AudioClip dieSound;
    public AudioSource Sound;
    public bool isGameActive;
    private float spawnRate = 1f;
    // Start is called before the first frame update
    void Start()
    {
        quitButton = GameObject.Find("QuitButton");
        Sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(score<0) 
        {
            Debug.Log("Bad Score, Game Over!");
            GameOver(false);
        }
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, target.Count);
            Instantiate(target[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

    }

    public void GameOver(bool isBad)
    {
        if(!isBad) 
        {
            gameOverScreen.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);

            isGameActive = false;
        }
    }

    public void StartGame(int difficulty)
    {
        isGameActive=true;
        score = 0;
        spawnRate /= difficulty;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Get name is String or can use "Prototype 5" instead
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
