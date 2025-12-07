using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject prefabToSpawn;
    public GameObject rempToSpawn;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverImage;
    public GameObject victoryImage;
    public GameObject explication;

    [Header("Timer")]
    public float maxTime = 60f;
    private float currentTime;

    private int totalEnemies = 0;
    private int killedEnemies = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        explication.SetActive(true);
        Invoke(nameof(Hide), 2f);

       
        //transform.position = new Vector3(28.00f, 14.21f, 2.93f);
        if (prefabToSpawn != null)
        {
            //debug
           // GameObject newUnit = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }


        currentTime = maxTime;
        UpdateScoreUI();
    }

    void Update()
    {
        
        if (gameOverImage.activeSelf) return; 
        if (victoryImage.activeSelf) return;// end

        currentTime -= Time.deltaTime;
        timerText.text = "Temps : " + Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            GameOver();
        }
    }
    void Hide()
    {
        explication.SetActive(false);
    }

    public void RegisterEnemy()
    {
        totalEnemies++;
        UpdateScoreUI();
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        UpdateScoreUI();

        if (killedEnemies == totalEnemies)
        {
            Victory();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Points : " + killedEnemies + " / " + totalEnemies;
    }

    public void GameOver()
    {
        gameOverImage.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Victory()
    {
        victoryImage.SetActive(true); 
        Time.timeScale = 0f;
    }
}