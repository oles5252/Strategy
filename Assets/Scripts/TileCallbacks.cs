using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding;
using UnityEngine.SceneManagement;

public class TileCallbacks : MonoBehaviour
{
    TileMap tm;
    public AudioClip playerSelect;
    public AudioClip enemySelect;
    public AudioClip moved;
    public AudioClip cancel;

    private void Start()
    {
        tm = TileMap.Instance;
    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {
        
    }

    private void OnMouseDown() //clicked on tile
    {
        tm.RefreshMap();
        if (tm.currentState == TileMap.gameState.playerTurn)
        {
            print("mouse down spotted");
            if (tm.selected == null)
            {
                switch (tm.units[(int)transform.position.x, (int)transform.position.y].alignment) {
                    case Alignment.Terrain:
                        //do nothing
                        break;

                    case Alignment.Player:
                        
                        AudioSource.PlayClipAtPoint(playerSelect, transform.position);
                        tm.selected = new TileMap.Selected();
                        tm.selected.x = (int)transform.position.x;
                        tm.selected.y = (int)transform.position.y;

                        if (tm.units[(int)transform.position.x, (int)transform.position.y].turnTaken == false)
                        {
                            // TileMap.Instance.units[(int)transform.position.x, (int)transform.position.y];
                            //do something to highlight where the unit can move
                            tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                        }
                        else
                        {
                            //play error sound cause unit cannot be selected
                            AudioSource.PlayClipAtPoint(cancel, transform.position);
                            tm.selected = new TileMap.Selected();
                            tm.selected.x = (int)transform.position.x;
                            tm.selected.y = (int)transform.position.y;
                        }
                        break;

                    case Alignment.Enemy:
                        AudioSource.PlayClipAtPoint(enemySelect, transform.position);
                        //show stats
                        tm.selected = new TileMap.Selected();
                        tm.selected.x = (int)transform.position.x;
                        tm.selected.y = (int)transform.position.y;

                        if (tm.units[tm.selected.x, tm.selected.y].mov != 0)
                        {

                            // TileMap.Instance.units[(int)transform.position.x, (int)transform.position.y];
                            //do something to highlight where the unit can move
                            tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                        }


                        break;
                }
            }
            else
            {

                if (tm.units[tm.selected.x, tm.selected.y].turnTaken == false)
                {
                    //calculate distance between unit and click:
                    List<Node> path = new Pathfinding().A_Star(new Node(tm.selected.x, tm.selected.y), new Node((int)transform.position.x, (int)transform.position.y));
                    foreach (Node node in path)
                    {
                        print("ClickPath: " + node.x + " , " + node.y);
                    }

                    //A PLAYER IS SELECTED
                    if (tm.units[tm.selected.x, tm.selected.y].alignment == Alignment.Player)
                    {

                        //empty terrain & can move
                        if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Terrain && path.Count - 1 <= tm.units[tm.selected.x, tm.selected.y].mov)
                        {
                            var unit = tm.units[tm.selected.x, tm.selected.y];
                            tm.units[(int)transform.position.x, (int)transform.position.y] = new Character(unit.alignment, unit.name, unit.maxhp, unit.currenthp, unit.atk, unit.def, unit.mov, unit.rng);
                            tm.units[tm.selected.x, tm.selected.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                            Node node = new Node(tm.selected.x, tm.selected.y);
                            //Node oldPlayer = tm.players.Find(n => n.x == tm.selected.x && n.y == tm.selected.y);
                            //oldPlayer.x = (int)transform.position.x;
                            //oldPlayer.y = (int)transform.position.y;
                            if (SceneManager.GetActiveScene().name != "Tutorial")
                            {
                                tm.units[(int)transform.position.x, (int)transform.position.y].turnTaken = true;
                            }
                            tm.selected = null;
                            tm.RefreshCharacters();
                            return;
                            //upate graphics to show unit is deselected
                        }

                        //empty terrain & cannot move
                        else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Terrain)
                        {
                            tm.selected = null;
                            return;
                        }

                        //clicked a enemy and can move to attack
                        else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Enemy && path.Count - 1 <= tm.units[tm.selected.x, tm.selected.y].mov + tm.units[tm.selected.x, tm.selected.y].rng && path.Count - 1 > tm.units[tm.selected.x, tm.selected.y].rng)
                        {
                            //move one space away
                            var unit = tm.units[tm.selected.x, tm.selected.y];
                            var rng = tm.units[tm.selected.x, tm.selected.y].rng;
                            tm.units[path[rng].x, path[rng].y] = new Character(unit.alignment, unit.name, unit.maxhp, unit.currenthp, unit.atk, unit.def, unit.mov, unit.rng);
                            tm.units[tm.selected.x, tm.selected.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                            tm.selected.x = path[rng].x;
                            tm.selected.y = path[rng].y;

                            //do attack
                            tm.units[(int)transform.position.x, (int)transform.position.y].currenthp -= Mathf.Max(tm.units[tm.selected.x, tm.selected.y].atk - tm.units[(int)transform.position.x, (int)transform.position.y].def, 0);
                            if (tm.units[(int)transform.position.x, (int)transform.position.y].currenthp <= 0) {
                                if (SceneManager.GetActiveScene().name == "Tutorial")
                                {
                                    FindObjectOfType<TextHandler>().nextScene = "Fellowship";
                                    FindObjectOfType<TextHandler>().presentDialog("Story/tutorial3");
                                }
                                else
                                {
                                    tm.units[(int)transform.position.x, (int)transform.position.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                                }
                            }
                            if (tm.units[(int)transform.position.x, (int)transform.position.y].currenthp <= 0 && SceneManager.GetActiveScene().name == "Tutorial")
                            {
                                tm.units[tm.selected.x, tm.selected.y].turnTaken = false;
                            }
                            else
                            {
                                tm.units[tm.selected.x, tm.selected.y].turnTaken = true;
                            }
                            tm.selected = null;
                            tm.RefreshCharacters();
                            //upate graphics to show unit is deselected

                            return;
                        }
                        //clicked an enemy and is next to enemy
                        else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Enemy && path.Count - 1 <= tm.units[tm.selected.x, tm.selected.y].mov + tm.units[tm.selected.x, tm.selected.y].rng && path.Count - 1 <= tm.units[tm.selected.x, tm.selected.y].rng)
                        { 
                            //do attack
                            tm.units[(int)transform.position.x, (int)transform.position.y].currenthp -= Mathf.Max(tm.units[tm.selected.x, tm.selected.y].atk - tm.units[(int)transform.position.x, (int)transform.position.y].def, 0);
                            if (tm.units[(int)transform.position.x, (int)transform.position.y].currenthp <= 0) {
                                if (SceneManager.GetActiveScene().name == "Tutorial")
                                {
                                    FindObjectOfType<TextHandler>().nextScene = "Fellowship";
                                    FindObjectOfType<TextHandler>().presentDialog("Story/tutorial3");
                                }
                                else
                                {
                                    tm.units[(int)transform.position.x, (int)transform.position.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                                }
                            }
                            if (tm.units[(int)transform.position.x, (int)transform.position.y].currenthp <= 0 && SceneManager.GetActiveScene().name == "Tutorial")
                            {
                                tm.units[tm.selected.x, tm.selected.y].turnTaken = false;
                            }
                            else
                            {
                                tm.units[tm.selected.x, tm.selected.y].turnTaken = true;
                            }
                            tm.selected = null;
                            tm.RefreshCharacters();
                            //upate graphics to show unit is deselected
                            return;
                        }

                        //clicked a monster and cannot move
                        else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Enemy)
                        {
                            tm.selected.x = (int)transform.position.x;
                            tm.selected.y = (int)transform.position.y;
                            if (TileMap.Instance.units[tm.selected.x, tm.selected.y].mov != 0)
                            {
                                //do something to highlight where the unit can move
                                tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                            }
                            return;
                        }

                        //selected a player
                        else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Player)
                        {
                            tm.selected.x = (int)transform.position.x;
                            tm.selected.y = (int)transform.position.y;
                            if (TileMap.Instance.units[tm.selected.x, tm.selected.y].mov != 0)
                            {
                                //do something to highlight where the unit can move
                                tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                            }
                            return;
                        }
                    }
                    //A MONSTER IS SELECTED
                    else if(tm.units[tm.selected.x, tm.selected.y].alignment == Alignment.Enemy)
                    {
                        if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Terrain)
                        {
                            tm.selected = null;
                            return;
                        }else if(tm.units[(int)transform.position.x, (int)transform.position.y].alignment != Alignment.Terrain)
                        {
                            tm.selected.x = (int)transform.position.x;
                            tm.selected.y = (int)transform.position.y;
                            if (TileMap.Instance.units[tm.selected.x, tm.selected.y].mov != 0)
                            {
                                //do something to highlight where the unit can move
                                tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                            }
                            return;
                        }
                    }
                }
                else //turn has been taken
                {
                    if ((tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Player) || (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Enemy))
                    {
                        tm.selected.x = (int)transform.position.x;
                        tm.selected.y = (int)transform.position.y;
                        if (TileMap.Instance.units[tm.selected.x, tm.selected.y].mov != 0)
                        {
                            //do something to highlight where the unit can move
                            tm.highlightMovement(new Node(tm.selected.x, tm.selected.y));
                        }
                        return;
                    }
                    else if (tm.units[(int)transform.position.x, (int)transform.position.y].alignment == Alignment.Terrain) {
                        tm.selected = null;
                        return;
                    }
                }
            }

        }
    }
}
