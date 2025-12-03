using UnityEngine;
using UnityEngine.Rendering;

public class teste_drag : MonoBehaviour
{
    [SerializeField] Transform mouse;
    public GameObject prefabToSpawn;
    public Transform ennemy;
   // private int teste = 0;

    private void OnMouseDrag()
    {
       
        

            transform.position = mouse.position;
            
        
    }
    private void OnMouseUp()
    {
        if(prefabToSpawn != null)
        {
            GameObject newUnit= Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            //UnitMoveToBase moveScript = newUnit.GetComponent<UnitMoveToBase>();
            //if (moveScript!=null && ennemy != null)
            //{
              //  moveScript.GoToBase(ennemy);
            //}
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
