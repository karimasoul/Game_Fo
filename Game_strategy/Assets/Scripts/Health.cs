using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 50;
    private int currentHP;
    public bool isEnemy = false;
    

    void Start()
    {
        if(gameObject.name.StartsWith("ok"))
        {
            isEnemy = true;
            GameManager.Instance.RegisterEnemy();
        }

        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log(name + " a pris " + amount + " degats. HP restants: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(name + " est detruit !");
        Destroy(gameObject);
        if (isEnemy)
        {
            GameManager.Instance.EnemyKilled();

           
        }
    }
}