using UnityEngine;

public class mouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        //print(Input.mousePosition);
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        print(mousePos3D);
        mousePos3D.z = 0f;
        transform.position = mousePos3D;
        //print(mousePos);
    }
}
