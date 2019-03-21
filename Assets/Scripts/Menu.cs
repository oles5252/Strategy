using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Menu : MonoBehaviour
{
    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {

        GameController.Instance.data.SaveNew();
        SceneManager.LoadScene("GameIntro");
    }

    public void ContinueGame()
    {
        GameController.Instance.data.Load();
        SceneManager.LoadScene(GameController.Instance.data.levelName);
    }

    public void Settings()
    {

    }

}
