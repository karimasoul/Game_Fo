using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            
            Destroy(gameObject);
            GridManager.Instance.SetNodeWalkable(gameObject.transform.position, true);
            
            return; //à remetre?
        }

        // Mouvement vers la cible
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Collision (distance très faible)
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target.GetComponent<Health>()?.TakeDamage(damage);
            if (target == null)
            {
                // GameManager.Instance.EnemyKilled();
                Destroy(gameObject);
                GridManager.Instance.SetNodeWalkable(gameObject.transform.position, true);
            }
            //if (target != null && target.childCount > 0)
            //{
                //Transform closest = GetClosestTarget();
            //}
          
        }
    }
}