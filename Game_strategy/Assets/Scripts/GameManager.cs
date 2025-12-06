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

       // yield return new WaitForSeconds(2f);
        

        //transform.position = faudrai que je precise une position
        transform.position = new Vector3(28.00f, 14.21f, 2.93f);
        if (prefabToSpawn != null)
        {
           // GameObject newUnit = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        //rajoute remp
        //transform.position = new Vector3(28.07f, 14.95f, 2.93f);
        //if (rempToSpawn != null)
        //{
          //  GameObject newUnit = Instantiate(rempToSpawn, transform.position, Quaternion.identity);
        //}
        //

        currentTime = maxTime;
        UpdateScoreUI();
    }

    void Update()
    {
        
        if (gameOverImage.activeSelf) return; // jeu stopp�

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

        if (killedEnemies >= totalEnemies)
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
        //gameOverImage.SetActive(true); mettre un canva de victory plutot 
        //gameOverImage.GetComponentInChildren<TextMeshProUGUI>().text = "VICTOIRE !";
        Time.timeScale = 0f;
    }
}