using UnityEngine;
using UnityEngine.UI;

public class AllyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject gameOverImage;

    void Start()
    {
        currentHealth = maxHealth;
        if (gameOverImage != null)
            gameOverImage.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Allié touché ! HP restant : " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Allié mort !");
            Destroy(gameObject);
            if (gameOverImage != null)
                gameOverImage.SetActive(true);
        }
    }
}