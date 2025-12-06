using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 50;
    private int currentHP;
    public bool isEnemy = false;
    //public bool remp = false;

    void Start()
    {
        if(gameObject.name.StartsWith("ok"))
        {
            isEnemy = true;
            GameManager.Instance.RegisterEnemy();
        }

        //if (gameObject.name.StartsWith("cry"))
          //  remp = true;
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log(name + " a pris " + amount + " d�g�ts. HP restants: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(name + " est d�truit !");
        Destroy(gameObject);
        if (isEnemy)
        {
            GameManager.Instance.EnemyKilled(); // a voir

           // if (remp)
            //{

              //  GridManager.Instance.SetNodeWalkable(gameObject.transform.position, true);
            //}
        }
    }
}