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
    public List<ParticleSystem> explosionParticle;
    public int score;
    public AudioClip shootSound;
    public AudioClip dieSound;
    public AudioClip crashSound;
    public AudioSource Sound;
    public RawImage[] heart;
    public bool isGameActive;
    private float spawnRate = 1f;
    int randNum;
    int num;
    public int life;
    Target targetScript;

    // Start is called before the first frame update
    void Start()
    {
        quitButton = GameObject.Find("QuitButton");
        Sound = GetComponent<AudioSource>();
        life =3;
    }

    // Update is called once per frame
    void Update()
    {
        randNum = Random.Range(1,4);
        StartCoroutine(OnTapping());
        
        if(score<0&&life>0)
        {
            life-=1;
            score = 0;
            UpdateScore(0);
        }

        //Display heart
        if(life==2)
        {
            heart[2].gameObject.SetActive(false);
        }
        if(life==1)
        {
            heart[1].gameObject.SetActive(false);
        }
        if(life==0)
        {
            heart[0].gameObject.SetActive(false);
        }
        
        if(life<=0) 
        {
            GameOver(true);
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

   private IEnumerator OnTapping ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && (hit.collider.tag == "Unit" || hit.collider.tag == "BadObject"))
        {
            targetScript = hit.collider.GetComponent<Target>();
            Vector3 spawnLocation = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
            UpdateScore(targetScript.pointValue);
            Destroy(hit.collider.gameObject);
            if(hit.collider.tag == "BadObject")
            {
                Instantiate(explosionParticle[0], spawnLocation, explosionParticle[0].transform.rotation);
                Sound.PlayOneShot(crashSound,0.6f);
            }
            else if(hit.collider.tag == "Unit")
            {
                Sound.PlayOneShot(shootSound,1f);
                Instantiate(explosionParticle[randNum], spawnLocation, explosionParticle[randNum].transform.rotation); 
            }    
        }
        yield return null;
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver(bool isOver)
    {
        if(isOver&&life<=0) 
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
