using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverImage != null)
            gameOverImage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
