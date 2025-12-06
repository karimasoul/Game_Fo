using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    private Transform target;
    private bool hasHit = false;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            GridManager.Instance.SetNodeWalkable(transform.position, true);
            return;
        }

        // Déplacement
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Collision
        if (!hasHit && Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            hasHit = true;  
            target.GetComponent<Health>()?.TakeDamage(damage);

            Destroy(gameObject);
            GridManager.Instance.SetNodeWalkable(transform.position, true);
        }
    }
}
