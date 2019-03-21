using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    TileMap tm;
    public Text text;
    bool slide = false;

    // Start is called before the first frame update
    void Start()
    {
        tm = TileMap.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(tm.selected != null)
        {
            if(!slide)
            {
                GetComponent<Animation>().Play("CharacterInfo_Slide");
                slide = true;
            }
            text.text = tm.units[tm.selected.x, tm.selected.y].name + "\n" + (tm.units[tm.selected.x, tm.selected.y].alignment).ToString() + "\nHP: " + tm.units[tm.selected.x, tm.selected.y].currenthp + "/" + tm.units[tm.selected.x, tm.selected.y].maxhp + "\nATK: " + tm.units[tm.selected.x, tm.selected.y].atk + "\nDEF: " + tm.units[tm.selected.x, tm.selected.y].def + "\nMOV: " + tm.units[tm.selected.x, tm.selected.y].mov;
        }
        else
        {
            if (slide)
            {
                GetComponent<Animation>().Play("CharacterInfo_SlideOut");
                slide = false;
            }
            text.text = "";
        }
    }
}
