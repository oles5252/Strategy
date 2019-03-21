using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapPan : MonoBehaviour
{
    public int Boundary = 20; // distance from edge scrolling starts
    public int speed = 2;
    private int screenWidth;
    private int screenHeight;
    float maxPanX;
    float maxPanY;
    private int currentPanX;
    private int currentPanY;
    public TileMap map;
    float findXOffset;
    float findYOffset;
    
    void Start()
    {
        maxPanX = (map.mapSizeX - 1f) / 2f;
        maxPanY = (map.mapSizeY - 1f) / 2f;

        Vector3 pos = new Vector3((map.mapSizeX - 1f) / 2f, (map.mapSizeY - 1f) / 2f, transform.position.z);

        transform.position = pos;
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        findXOffset = map.mapSizeX - Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, 0)).x;
        findYOffset = map.mapSizeY - Camera.main.ScreenToWorldPoint(new Vector3(0, screenHeight)).y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - maxPanX < findXOffset){
            if (Input.mousePosition.x > screenWidth - Boundary)
            {
                Vector3 pos = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.position = pos; // move on +X axis
            }
        }
        if (transform.position.x - maxPanX > -findXOffset) {
            if (Input.mousePosition.x < 0 + Boundary)
            {
                Vector3 pos = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.position = pos; // move on -X axis
            }
        }
        if (transform.position.y - maxPanY < findYOffset)
        {
            if (Input.mousePosition.y > screenHeight - Boundary)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
                transform.position = pos; // move on +Y axis
            }
        }
        if (transform.position.y - maxPanY > -findYOffset)
        {
            if (Input.mousePosition.y < 0 + Boundary)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
                transform.position = pos; // move on -Y axis
            }
        }
    }
}
