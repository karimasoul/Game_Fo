using UnityEngine;

public class UnitMoveToBase : MonoBehaviour
{

    public Transform targetBase;
    public float speed = 2f;
    private bool isMoving = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(isMoving && targetBase != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetBase.position, speed * Time.deltaTime);
        }
    }

    public void GoToBase(Transform baseTarget)
    {
        targetBase = baseTarget;
        isMoving = true;

    }
}
