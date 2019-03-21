using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public string fileName;

    public SaveData data;

    public List<GameObject> prefabs;

    //Establish GameManager as singleton
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        fileName = Application.persistentDataPath + "/" + "SaveData.dat";
        data = new SaveData();
        DontDestroyOnLoad(this);
    }
}
