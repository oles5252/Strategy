using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding
{

    public int heuristic_cost_estimate(Node start, Node goal)
    {
        return Mathf.Abs(start.x - goal.x) + Mathf.Abs(start.y - goal.y);
    }

    public List<Node> reconstruct_path(Node[,] cameFrom, Node current) {
        List<Node> total_path = new List<Node>();
        total_path.Add(current);
        while (cameFrom[current.x, current.y] != null) {
            current = cameFrom[current.x, current.y];
            total_path.Add(current);
        }
        return total_path;
    }

    public bool arrayContains(Node[,] cameFrom, Node current)
    {
        bool contains = false;
        for(int i = 0; i < cameFrom.GetLength(0); i++)
        {
            for(int j = 0; j < cameFrom.GetLength(1); j++)
            {
                if(cameFrom[i,j] != null && cameFrom[i,j].x == current.x && cameFrom[i,j].y == current.y)
                {
                    contains = true;
                }
            }
        }
        return contains;
    }

    public List<Node> PossibleMoves(Node start, int mov)
    {
        //set of Nodes evaled
        List<Node> closedSet = new List<Node>();

        //returnable closedSet (does not include other players/enemies)
        List<Node> returnableSet = new List<Node>();

        //set of unevaled
        List<Node> openSet = new List<Node>();
        openSet.Add(start);

        //Most efficient previous Node
        Node[,] cameFrom = new Node[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];

        //cost to get to Node
        int[,] gScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < gScore.GetLength(0); ++row)
        {
            for (int col = 0; col < gScore.GetLength(1); ++col)
            {
                gScore[row, col] = 99999;
            }
        }
        gScore[start.x, start.y] = 0;

        int[,] fScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < fScore.GetLength(0); ++row)
        {
            for (int col = 0; col < fScore.GetLength(1); ++col)
            {
                fScore[row, col] = 99999;
            }
        }

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            if (reconstruct_path(cameFrom, current).Count > mov+1)
            {
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            Node neighborPlayer = null;
            Node neighborEnemy = null;
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Player)
            {
                neighborPlayer = new Node(current.x, current.y);
            }
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Enemy)
            {
                neighborEnemy = new Node(current.x, current.y);
            }
            if (neighborPlayer != null || neighborEnemy != null)
            {
                returnableSet.Remove(current);
            }
            else
            {
                returnableSet.Add(current);
            }

            if (returnableSet.Contains(current) || (current.x == start.x && current.y == start.y)) {
                foreach (Node neighbor in TileMap.Instance.GetNeighbors(current))
                {
                    bool flag = false;
                    foreach (Node node in closedSet)
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = true; //ignore neighbor
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue; //ignore neighbor 
                    }

                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Player)
                    {
                        neighborPlayer = new Node(neighbor.x, neighbor.y);
                    }
                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Enemy)
                    {
                        neighborEnemy = new Node(neighbor.x, neighbor.y);
                    }
                    int tentative_gScore;
                    if (neighborPlayer != null || neighborEnemy != null)
                    {
                        tentative_gScore = gScore[current.x, current.y] + 10000000;
                        returnableSet.Remove(neighbor);
                    }
                    else
                    {
                        tentative_gScore = gScore[current.x, current.y] + TileMap.Instance.tileTypes[(int)TileMap.Instance.tiles[neighbor.x, neighbor.y]].tileWeight;
                    }

                    flag = true;
                    foreach (Node node in openSet) //(!openSet.Contains(neighbor))
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentative_gScore >= gScore[neighbor.x, neighbor.y])
                    {
                        continue;
                    }

                    cameFrom[neighbor.x, neighbor.y] = current;
                    gScore[neighbor.x, neighbor.y] = tentative_gScore;
                    fScore[neighbor.x, neighbor.y] = gScore[neighbor.x, neighbor.y];
                }
            }
        }

        return returnableSet;
    }

    public List<Node> PossibleAttack(Node start, int mov, int range)
    {
        //set of Nodes evaled
        List<Node> closedSet = new List<Node>();

        //returnable closedSet (does not include other players/enemies)
        List<Node> returnableSet = new List<Node>();

        //set of unevaled
        List<Node> openSet = new List<Node>();
        openSet.Add(start);

        //Most efficient previous Node
        Node[,] cameFrom = new Node[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];

        //cost to get to Node
        int[,] gScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < gScore.GetLength(0); ++row)
        {
            for (int col = 0; col < gScore.GetLength(1); ++col)
            {
                gScore[row, col] = 99999;
            }
        }
        gScore[start.x, start.y] = 0;

        int[,] fScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < fScore.GetLength(0); ++row)
        {
            for (int col = 0; col < fScore.GetLength(1); ++col)
            {
                fScore[row, col] = 99999;
            }
        }

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            if (reconstruct_path(cameFrom, current).Count > mov + 1 + range)
            {
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            Node neighborPlayer = null;
            Node neighborEnemy = null;
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Player)
            {
                neighborPlayer = new Node(current.x, current.y);
            }
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Enemy)
            {
                neighborEnemy = new Node(current.x, current.y);
            }
            if (neighborPlayer != null)
            {
                returnableSet.Remove(current);
            }
            else
            {
                returnableSet.Add(current);
            }

            if ((returnableSet.Contains(current) && neighborEnemy == null) || (current.x == start.x && current.y == start.y))
            {
                foreach (Node neighbor in TileMap.Instance.GetNeighbors(current))
                {
                    bool flag = false;
                    foreach (Node node in closedSet)
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = true; //ignore neighbor
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue; //ignore neighbor 
                    }

                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Player)
                    {
                        neighborPlayer = new Node(neighbor.x, neighbor.y);
                    }
                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Enemy)
                    {
                        neighborEnemy = new Node(neighbor.x, neighbor.y);
                    }
                    int tentative_gScore;
                    if (neighborPlayer != null || neighborEnemy != null)
                    {
                        tentative_gScore = gScore[current.x, current.y] + 10000000;
                        returnableSet.Remove(neighbor);
                    }
                    else
                    {
                        tentative_gScore = gScore[current.x, current.y] + TileMap.Instance.tileTypes[(int)TileMap.Instance.tiles[neighbor.x, neighbor.y]].tileWeight;
                    }

                    flag = true;
                    foreach (Node node in openSet) //(!openSet.Contains(neighbor))
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentative_gScore >= gScore[neighbor.x, neighbor.y])
                    {
                        continue;
                    }

                    cameFrom[neighbor.x, neighbor.y] = current;
                    gScore[neighbor.x, neighbor.y] = tentative_gScore;
                    fScore[neighbor.x, neighbor.y] = gScore[neighbor.x, neighbor.y];
                }
            }
        }

        return returnableSet;


    }



    public List<Node> EnemyPossibleAttack(Node start, int mov, int range)
    {
        //set of Nodes evaled
        List<Node> closedSet = new List<Node>();

        //returnable closedSet (does not include other players/enemies)
        List<Node> returnableSet = new List<Node>();

        //set of unevaled
        List<Node> openSet = new List<Node>();
        openSet.Add(start);

        //Most efficient previous Node
        Node[,] cameFrom = new Node[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];

        //cost to get to Node
        int[,] gScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < gScore.GetLength(0); ++row)
        {
            for (int col = 0; col < gScore.GetLength(1); ++col)
            {
                gScore[row, col] = 99999;
            }
        }
        gScore[start.x, start.y] = 0;

        int[,] fScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < fScore.GetLength(0); ++row)
        {
            for (int col = 0; col < fScore.GetLength(1); ++col)
            {
                fScore[row, col] = 99999;
            }
        }

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            if (reconstruct_path(cameFrom, current).Count > mov + 1 + range)
            {
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            Node neighborPlayer = null;
            Node neighborEnemy = null;
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Player)
            {
                neighborPlayer = new Node(current.x, current.y);
            }
            if (TileMap.Instance.units[current.x, current.y].alignment == Alignment.Enemy)
            {
                neighborEnemy = new Node(current.x, current.y);
            }
            if (neighborEnemy != null)
            {
                returnableSet.Remove(current);
            }
            else
            {
                returnableSet.Add(current);
            }

            if ((returnableSet.Contains(current) && neighborPlayer == null) || (current.x == start.x && current.y == start.y))
            {
                foreach (Node neighbor in TileMap.Instance.GetNeighbors(current))
                {
                    bool flag = false;
                    foreach (Node node in closedSet)
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = true; //ignore neighbor
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue; //ignore neighbor 
                    }

                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Player)
                    {
                        neighborPlayer = new Node(neighbor.x, neighbor.y);
                    }
                    if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Enemy)
                    {
                        neighborEnemy = new Node(neighbor.x, neighbor.y);
                    }
                    int tentative_gScore;
                    if (neighborPlayer != null || neighborEnemy != null)
                    {
                        tentative_gScore = gScore[current.x, current.y] + 10000000;
                        returnableSet.Remove(neighbor);
                    }
                    else
                    {
                        tentative_gScore = gScore[current.x, current.y] + TileMap.Instance.tileTypes[(int)TileMap.Instance.tiles[neighbor.x, neighbor.y]].tileWeight;
                    }

                    flag = true;
                    foreach (Node node in openSet) //(!openSet.Contains(neighbor))
                    {
                        if (node.x == neighbor.x && node.y == neighbor.y)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentative_gScore >= gScore[neighbor.x, neighbor.y])
                    {
                        continue;
                    }

                    cameFrom[neighbor.x, neighbor.y] = current;
                    gScore[neighbor.x, neighbor.y] = tentative_gScore;
                    fScore[neighbor.x, neighbor.y] = gScore[neighbor.x, neighbor.y];
                }
            }
        }

        return returnableSet;


    }








    public List<Node> A_Star(Node start, Node goal)
    {
        //set of Nodes evaled
        List<Node> closedSet = new List<Node>();

        //set of unevaled
        List<Node> openSet = new List<Node>();
        openSet.Add(start);

        //Most efficient previous Node
        Node[,] cameFrom = new Node[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];

        //cost to get to Node
        int[,] gScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < gScore.GetLength(0); ++row)
        {
            for (int col = 0; col < gScore.GetLength(1); ++col)
            {
                gScore[row, col] = 99999;
            }
        }
        gScore[start.x, start.y] = 0;

        int[,] fScore = new int[TileMap.Instance.mapSizeX, TileMap.Instance.mapSizeY];
        for (int row = 0; row < fScore.GetLength(0); ++row)
        {
            for (int col = 0; col < fScore.GetLength(1); ++col)
            {
                fScore[row, col] = 99999;
            }
        }

        fScore[start.x, start.y] = heuristic_cost_estimate(start, goal);

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            foreach (Node node in openSet)
            {
                if (fScore[current.x, current.y] > fScore[node.x, node.y])
                {
                    current = node;
                }
            }
            if (current.x == goal.x && current.y == goal.y)
            {
                return reconstruct_path(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neighbor in TileMap.Instance.GetNeighbors(current))
            {
                bool flag = false;
                foreach (Node node in closedSet)
                {
                    if (node.x == neighbor.x && node.y == neighbor.y)
                    {
                        flag = true; //ignore neighbor
                        break;
                    }
                }
                if (flag)
                {
                    continue; //ignore neighbor 
                }

                Node neighborPlayer = null;
                Node neighborEnemy = null;
                if(TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Player)
                {
                    neighborPlayer = new Node(neighbor.x, neighbor.y);
                }
                if (TileMap.Instance.units[neighbor.x, neighbor.y].alignment == Alignment.Enemy)
                {
                    neighborEnemy = new Node(neighbor.x, neighbor.y);
                }
                int tentative_gScore;
                if (neighborPlayer != null || neighborEnemy != null)
                {
                    tentative_gScore = gScore[current.x, current.y] + 10000000;
                }
                else
                {
                    tentative_gScore = gScore[current.x, current.y] + TileMap.Instance.tileTypes[(int)TileMap.Instance.tiles[neighbor.x, neighbor.y]].tileWeight;
                }

                flag = true;
                foreach (Node node in openSet) //(!openSet.Contains(neighbor))
                {
                    if (node.x == neighbor.x && node.y == neighbor.y)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    openSet.Add(neighbor);
                }
                else if (tentative_gScore >= gScore[neighbor.x, neighbor.y])
                {
                    continue;
                }

                cameFrom[neighbor.x, neighbor.y] = current;
                gScore[neighbor.x, neighbor.y] = tentative_gScore;
                fScore[neighbor.x, neighbor.y] = gScore[neighbor.x, neighbor.y] + heuristic_cost_estimate(neighbor, goal);
            }
        }

        return null;


    }
}
