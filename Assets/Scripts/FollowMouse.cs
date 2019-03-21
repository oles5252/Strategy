using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Component cursor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        cursor = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(pos.x, pos.y, cursor.transform.position.z);
    }

     private void OnMouseDown()
     {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(pos.x, pos.y, cursor.transform.position.z);
        //potentially change cursor to display mousedown
    }
}
