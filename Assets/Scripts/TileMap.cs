using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TileMap : MonoBehaviour
{

    //Establish TileMap as singleton
    private static TileMap _instance;
    public static TileMap Instance { get { return _instance; } }
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

    public TileType[] tileTypes;
    public int[,] tiles;

    public Character[,] units;
    public GameObject[,] unitObjects;
    public GameObject[,] tileObjects;

    public int mapSizeX = 3;
    public int mapSizeY = 6;

    public int turn = 1;

    SaveData data;

    public enum gameState
    {
        playerTurn,
        enemyTurn
    }
    public gameState currentState = 0;

    //tilemap will be responsible for the selected object
    public class Selected
    {
        public int x;
        public int y;
    }

    public Selected selected = null;


    // Start is called before the first frame update
    void Start()
    {

        //load savedata
        data = GameController.Instance.data;
        data.Load();

        tiles = new int[mapSizeX, mapSizeY];
        for(int x=0; x<mapSizeX; x++)
        {
            for(int y=0; y<mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        //initialize as all 0 units
        units = new Character[mapSizeX, mapSizeY];
        for(int i = 0; i < mapSizeX; i++)
        {
            for(int j = 0; j < mapSizeY; j++)
            {
                units[i, j] = new Character(Alignment.Terrain, "", 0, 0, 0 , 0, 0, 0);
            }
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            //set unit type 1
            //units[0, 0] = data.characters.Find(c => c.name == "Phos"); //human
            units[2, 1] = data.characters.Find(c => c.name == "Tisiphone"); //elf sister
                                                                            //units[3, 5] = data.characters.Find(c => c.name == "Sisyphus"); //elf brother
                                                                            //units[4,0] = data.characters.Find(c => c.name == "Iphi"); //dwarf
                                                                            //units[6,0] = data.characters.Find(c => c.name == "Dartfoot"); //halfling

            //set enemy type 2
            units[2, 4] = new Character(Alignment.Enemy, "Sisyphus", 3, 3, 1, 0, 2, 2); //elf king
                                                                                        //units[5, 7] = new Character(Alignment.Enemy, "Gorb", 2, 2, 2, 0, 2, 1); //goblin
                                                                                        //units[3, 7] = new Character(Alignment.Enemy, "Gorb", 2, 2, 2, 0, 2, 1); //goblin
                                                                                        //units[1, 7] = new Character(Alignment.Enemy, "Gorb", 2, 2, 2, 0, 2, 1); //goblin
        }
        if(SceneManager.GetActiveScene().name == "Fellowship")
        {
            //set unit type 1
            //units[0, 0] = data.characters.Find(c => c.name == "Phos"); //human
            units[1, 1] = data.characters.Find(c => c.name == "Tisiphone"); //elf sister
                                                                            //units[3, 5] = data.characters.Find(c => c.name == "Sisyphus"); //elf brother
                                                                            //units[4,0] = data.characters.Find(c => c.name == "Iphi"); //dwarf
                                                                            //units[6,0] = data.characters.Find(c => c.name == "Dartfoot"); //halfling

            //set enemy type 2
            //units[2, 4] = new Character(Alignment.Enemy, "Sisyphus", 3, 3, 1, 0, 2, 2); //elf king
            units[4, 6] = new Character(Alignment.Enemy, "Gurd", 4, 4, 3, 0, 2, 1); //goblin
            units[2, 6] = new Character(Alignment.Enemy, "Gorb", 4, 4, 2, 1, 2, 1); //goblin
            units[3, 7] = new Character(Alignment.Enemy, "Garb", 6, 6, 2, 0, 2, 1); //goblin

            //tiles[0, 2] = 1;
            //tiles[1, 3] = 1;
            //tiles[4, 3] = 1;
            //tiles[5, 3] = 1;
            //tiles[6, 3] = 1;
        }
        unitObjects = new GameObject[mapSizeX, mapSizeY];
        tileObjects = new GameObject[mapSizeX,mapSizeY];

        GenerateMap();
        GenerateCharacters();

    }

    private void Update()
    {
        if(currentState == gameState.enemyTurn)
        {
            //do enemy turn
            bool win = true;
            foreach (Character unit in units)
            {
                if(unit.alignment == Alignment.Enemy)
                {
                    win = false;
                }
            }
            if(win)
            {
                //win!!
                if(SceneManager.GetActiveScene().name == "Fellowship")
                {
                    SceneManager.LoadScene("");
                }
                return;
                
            }

            bool lose = true;
            //loop thru enemies
            for (int i = 0; i < units.GetLength(0); i++)
            {
                for(int j = 0; j < units.GetLength(1); j++) {
                    if (units[i, j].alignment == Alignment.Enemy)
                    {
                        List<Node> shortestPath = new List<Node>();

                        //loop thru players
                        for (int x = 0; x < units.GetLength(0); x++)
                        {
                            for (int y = 0; y < units.GetLength(1); y++)
                            {
                                if (units[x, y].alignment == Alignment.Player)
                                {
                                    lose = false;
                                    List<Node> path = new Pathfinding().A_Star(new Node(i, j), new Node(x, y)); //find closest player
                                    if (shortestPath.Count == 0)
                                    {
                                        shortestPath = path;
                                    }
                                    else if (shortestPath.Count > path.Count)
                                    {
                                        shortestPath = path;
                                    }
                                }
                            }
                        }

                        if (lose)
                        {
                            //end
                            return;
                        }
//                        if(shortestPath != nl)
  //                      shortestPath = new Pathfinding().A_Star(new Node(i, j), new Node(shortestPath[0].x, shortestPath[0].y));

                        //if shortestPath is > mov amount, we grab the path that we need:

                        Node target = new Node(0, 0);
                        //if shortestpath is in the move range, and the shortest path has more than just the first and last
                        if (shortestPath.Count - 1 <= units[i, j].mov + units[i, j].rng && shortestPath.Count -1 > units[i,j].rng)
                        {
                            //move next to player
                            target = shortestPath[units[i,j].rng]; //can't overlap player so we put it at neighbor node
                            var enemy = units[i, j];
                            units[target.x, target.y] = new Character(enemy.alignment, enemy.name, enemy.maxhp, enemy.currenthp, enemy.atk, enemy.def, enemy.mov, enemy.rng);
                            units[i, j] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);

                            //attack
                            Node player = shortestPath[0];
                            units[player.x, player.y].currenthp -= Mathf.Max(units[target.x, target.y].atk - units[player.x, player.y].def, 0);
                            if (units[player.x, player.y].currenthp <= 0) {
                                    units[player.x, player.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                            }
                            else if (SceneManager.GetActiveScene().name == "Tutorial")
                            {
                                FindObjectOfType<TextHandler>().presentDialog("Story/tutorial2");
                            }

                        }
                        else if(shortestPath.Count -1 <= units[i,j].rng)
                        {
                            //don't move

                            //attack
                            Node player = shortestPath[0];
                            units[player.x, player.y].currenthp -= Mathf.Max(units[i, j].atk - units[player.x, player.y].def, 0);
                            if (units[player.x, player.y].currenthp <= 0)
                            {

                                units[player.x, player.y] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                            }
                            else if (SceneManager.GetActiveScene().name == "Tutorial")
                            {
                                FindObjectOfType<TextHandler>().presentDialog("Story/tutorial2");
                            }
                        }
                        else
                        {
                            //move as far as possible
                            target = shortestPath[shortestPath.Count - 1 - units[i, j].mov];
                            var enemy = units[i, j];
                            units[target.x, target.y] = new Character(enemy.alignment, enemy.name, enemy.maxhp, enemy.currenthp, enemy.atk, enemy.def, enemy.mov, enemy.rng);
                            units[i, j] = new Character(Alignment.Terrain, "", 0, 0, 0, 0, 0, 0);
                        }



                        RefreshCharacters();
                    }
                }


            }
            if(turn == 1 && SceneManager.GetActiveScene().name == "Fellowship")
            {
                units[2, 8] = data.characters.Find(c => c.name == "Phos"); //human
                units[4, 7] = data.characters.Find(c => c.name == "Iphi"); //dwarf
                units[6, 8] = data.characters.Find(c => c.name == "Dartfoot"); //halfling
                RefreshCharacters();
                FindObjectOfType<TextHandler>().presentDialog("Story/fellowship2");
            }
            turn++;
            currentState = gameState.playerTurn;
        }
        else if (currentState == gameState.playerTurn)
        {
            //do player turn
            bool flag = true; //swap to enemy state
            foreach (Character unit in units)
            {
                if (unit != null && unit.alignment == Alignment.Player)
                {
                    if (unit.turnTaken == false)
                    {
                        flag = false;
                    }
                }

            }
            if (flag)
            {
                RefreshMap();

                foreach (Character unit in units)
                {
                    unit.turnTaken = false;
                }
                currentState = gameState.enemyTurn;
            }
        }
    }


    public void GenerateMap()
    {
        for(int x=0; x<mapSizeX; x++)
        {
            for(int y=0; y<mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                tileObjects[x,y] = (Instantiate(tt.tilePrefab, new Vector3(x, y, 0), Quaternion.identity));
            }
        }
    }

    public void GenerateCharacters()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (units[x, y].name != "")
                {
                    unitObjects[x, y] = (Instantiate(GameController.Instance.prefabs.Find(p => p.name == units[x, y].name), new Vector3(x, y, 0), Quaternion.identity));
                    if (unitObjects[x, y].GetComponentInChildren<healthBar>() != null) {
                        unitObjects[x, y].GetComponentInChildren<healthBar>().updateHealth();
                    }
                    if (units[x, y].alignment == Alignment.Player && units[x, y].turnTaken)
                    {
                        SpriteRenderer renderer = unitObjects[x, y].GetComponentInChildren<SpriteRenderer>();
                        renderer.color = new Color(renderer.color.r - (50f / 255f), renderer.color.g - (50f / 255f), renderer.color.b - (50f / 255f), renderer.color.a);
                    }
                }
            }
        }
    }

    public void RefreshCharacters()
    {
        foreach(GameObject unit in unitObjects)
        {
            Object.Destroy(unit);
        }
        unitObjects = new GameObject[mapSizeX, mapSizeY];
        GenerateCharacters();
    }

    public void RefreshMap()
    {
        foreach(GameObject tile in tileObjects)
        {
            Object.Destroy(tile);
        }
        tileObjects = new GameObject[mapSizeX, mapSizeY];
        GenerateMap();
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
                if (x == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y;

                if (checkX >= 0 && checkX < mapSizeX && checkY >= 0 && checkY < mapSizeY)
                {
                    neighbors.Add(new Node(checkX, checkY));
                }
        }
        for (int y = -1; y <= 1; y++)
        {
            if (y == 0)
                continue;

            int checkX = node.x;
            int checkY = node.y + y;

            if (checkX >= 0 && checkX < mapSizeX && checkY >= 0 && checkY < mapSizeY)
            {
                neighbors.Add(new Node(checkX, checkY));
            }
        }

        return neighbors;
    }

    public float distance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt((y2 - y1) * (y2 - y1)+ (x2 - x1) * (x2 - x1));
    }

    public void highlightMovement(Node selected)
    {
        if (selected != null && units[selected.x, selected.y].alignment == Alignment.Player && !units[selected.x, selected.y].turnTaken)
        {
            List<Node> closedAttack = new Pathfinding().PossibleAttack(selected, units[selected.x, selected.y].mov, units[selected.x, selected.y].rng);
            List<Node> closed = new Pathfinding().PossibleMoves(selected, units[selected.x, selected.y].mov);
            foreach (Node node in closedAttack)
            {
                tileObjects[node.x, node.y].GetComponent<SpriteRenderer>().color = new Color(255, 165, 0, 80);
            }
            foreach (Node node in closed)
            {
                tileObjects[node.x, node.y].GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 80);
            }
        }
        else if (selected != null && units[selected.x, selected.y].alignment == Alignment.Enemy && units[selected.x, selected.y].turnTaken == false)
        {
            List<Node> closedAttack = new Pathfinding().EnemyPossibleAttack(selected, units[selected.x, selected.y].mov, units[selected.x, selected.y].rng);
            List<Node> closed = new Pathfinding().PossibleMoves(selected, units[selected.x, selected.y].mov);
            foreach (Node node in closedAttack)
            {
                tileObjects[node.x, node.y].GetComponent<SpriteRenderer>().color = new Color(255, 165, 0, 80);
            }
            foreach (Node node in closed)
            {
                tileObjects[node.x, node.y].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 80);
            }
        }
    }

    IEnumerator showWin()
    {

        yield return new WaitForSeconds(1f);
    }
}
