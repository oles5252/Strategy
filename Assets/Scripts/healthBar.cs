using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class healthBar : MonoBehaviour
{
    // Update is called once per frame
    public void updateHealth()
    { 
        float percenthp = (float)TileMap.Instance.units[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 0.75f)].currenthp / (float)TileMap.Instance.units[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 0.75f)].maxhp;
        print(percenthp);
        transform.localScale = new Vector3(percenthp, transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3((percenthp - 1f)/2f, transform.localPosition.y, transform.localPosition.z);
    }
}
