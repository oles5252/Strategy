using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TextHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject textBubble;
    public string nextScene;
    public GameObject panel;
    public Text text;
    public Image prompter;
    List<string> words;
    TextGenerator textGen;
    TextGenerationSettings settings;
    bool canClick = false;
    public AudioSource source;
    public float waitForWord = 0.12f;
    string currentSpeaker;

    // Start is called before the first frame update
    void Start()
    {
        textGen = new TextGenerator();
        settings = new TextGenerationSettings();
        settings.font = text.font;
        settings.fontSize = text.fontSize;
        
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            words = new List<string>((Resources.Load<TextAsset>("Story/prologue").text).Split(new[] { ' ' }));
            StartCoroutine(presentNextLine());
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            words = new List<string>((Resources.Load<TextAsset>("Story/tutorial").text).Split(new[] { ' ' }));
            StartCoroutine(presentNextLine());
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            words = new List<string>((Resources.Load<TextAsset>("Story/fellowship").text).Split(new[] { ' ' }));
            StartCoroutine(presentNextLine());
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            words = new List<string>((Resources.Load<TextAsset>("Story/alone").text).Split(new[] { ' ' }));
            StartCoroutine(presentNextLine());
        }
    }

    public void presentDialog(string dataPath)
    {
        panel.gameObject.SetActive(true);
        words = new List<string>((Resources.Load<TextAsset>(dataPath).text).Split(new[] { ' ' }));
        StartCoroutine(presentNextLine());
    }


// Update is called once per frame
void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            words = null;
        }
    }

    IEnumerator presentNextLine()
    {
        if (words == null)
        {
            if (nextScene != null && nextScene != "")
            {
                SceneManager.LoadScene(nextScene);
            }
            panel.SetActive(false);
            yield break;
        }

        text.text = "";
        int counter = 0;
        float textWidth = 0;
        List<string> currentText = new List<string>();
        if (words[0].Equals("Tisiphone:"))
        {
            currentSpeaker = "Tisiphone: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Sisyphus:"))
        {
            currentSpeaker = "Sisyphus: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Gorb:"))
        {
            currentSpeaker = "Gorb: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Garb:"))
        {
            currentSpeaker = "Garb: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Gurd:"))
        {
            currentSpeaker = "Gurd: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Iphi:"))
        {
            currentSpeaker = "Iphi: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Dartfoot:"))
        {
            currentSpeaker = "Dartfoot: ";
            words.RemoveRange(0, 1);
        }
        else if (words[0].Equals("Phos:"))
        {
            currentSpeaker = "Phos: ";
            words.RemoveRange(0, 1);
        }
        if (currentSpeaker != null && currentSpeaker != "")
        {
            if (currentSpeaker.Equals("Tisiphone: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Tisiphone")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Sisyphus: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Sisyphus")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Garb: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Garb")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Gorb: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Gorb")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Iphi: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Iphi")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Dartfoot: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Dartfoot")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Phos: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Phos")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
            else if (currentSpeaker.Equals("Gurd: "))
            {
                for (int i = 0; i < TileMap.Instance.units.GetLength(0); i++)
                {
                    for (int j = 0; j < TileMap.Instance.units.GetLength(1); j++)
                    {
                        if (TileMap.Instance.units[i, j].name == "Gurd")
                        {
                            textBubble.transform.position = new Vector2(i, j);
                        }
                    }
                }
            }
        }
        if(currentSpeaker != null)
        {
            currentText.Add(currentSpeaker);
        }
        foreach(string word in words)
        {
            print("word: " + word);
            if (word.Equals("Tisiphone:"))
            {
                currentSpeaker = "Tisiphone: ";
                break;
            }
            else if (word.Equals("Sisyphus:"))
            {
                currentSpeaker = "Sisyphus: ";
                break;
            }
            else if (word.Equals("Gorb:"))
            {
                currentSpeaker = "Gorb: ";
                break;
            }
            else if (word.Equals("Garb:"))
            {
                currentSpeaker = "Garb: ";
                break;
            }
            else if (word.Equals("Gurd:"))
            {
                currentSpeaker = "Gurd: ";
                break;
            }
            else if (word.Equals("Iphi:"))
            {
                currentSpeaker = "Iphi: ";
                break;
            }
            else if (word.Equals("Dartfoot:"))
            {
                currentSpeaker = "Dartfoot: ";
                break;
            }
            else if (word.Equals("Phos:"))
            {
                currentSpeaker = "Phos: ";
                break;
            }
            else
            {
                currentText.Add(word);
                textWidth = textGen.GetPreferredWidth(" " + string.Join(" ", currentText) + " ", settings);
                print("text width: " + textWidth);
                if (textWidth > text.rectTransform.rect.width * 1f)
                {
                    break;
                }
                counter++;
            }
        }
        if (words.Count > counter+1)
        {
            words.RemoveRange(0, counter+1);
        }
        else
        {
            words = null;
        }
        foreach (string word in currentText)
        {
            source.Play();
            text.text += word + " ";
            yield return new WaitForSeconds(waitForWord);
        }
        StartCoroutine(blinkPrompter());
        canClick = true;
        yield break;
    }

    IEnumerator blinkPrompter()
    {
        while (true)
        {
            prompter.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            prompter.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick == true)
        {
            canClick = false;
            waitForWord = 0.12f;
            StopAllCoroutines();
            prompter.gameObject.SetActive(false);
            StartCoroutine(presentNextLine());
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (canClick == false)
        {
            waitForWord = 0.06f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        waitForWord = 0.12f;
    }
}
