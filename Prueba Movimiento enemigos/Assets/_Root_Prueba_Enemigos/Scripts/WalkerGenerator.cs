using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {
        floor, wall, empty
    }

    public Grid[,] gridHandler;
    public List<WalkerObject> walkers;
    public Tilemap tileMap;
    public Tile floor;
    public Tile wall;
    public int mapWidth = 30;
    public int mapHeight = 30;
    public int maximumWalkers = 10;
    public int tileCount = default;
    public float fillPercentage = 0.4f;
    public float waitTime = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[mapWidth, mapHeight];
        for (int x = 0; x < gridHandler.GetLength(1); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.empty;
            }
        }
        walkers = new List<WalkerObject>();
        Vector3Int tileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);
        WalkerObject curWalker = new WalkerObject(new Vector2(tileCenter.x, tileCenter.y), GetDirection(), 0.5f);
        gridHandler[tileCenter.x, tileCenter.y] = Grid.floor;
        tileMap.SetTile(tileCenter, floor);
        walkers.Add(curWalker);
        tileCount++;
        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)tileCount / (float)gridHandler.Length < fillPercentage)
        {

            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.position.x, (int)curWalker.position.y, 0);
                if (gridHandler[curPos.x, curPos.y] != Grid.floor)
                {
                    tileMap.SetTile(curPos, floor);
                    tileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.floor;
                    hasCreatedFloor = true;
                }
            }
            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        StartCoroutine(CreateWalls());
    }
    

    void ChanceToRemove()
    {
        int updateCount = walkers.Count;
        for (int i = 0; i < updateCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count > 1)
            {
                walkers.RemoveAt(i);
                break;
            }
        }
    }
    void ChanceToRedirect()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange)
            {
                WalkerObject curWalker = walkers[i];
                curWalker.direction = GetDirection();
                walkers[i] = curWalker;
            }
            

        }
    }
    
    void ChanceToCreate()
    {
        int updateCount = walkers.Count;
        for (int i = 0; i < updateCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count < maximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = walkers[i].position;
                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            WalkerObject foundWalker = walkers[i];
            foundWalker.position += foundWalker.direction;
            foundWalker.position.x = Mathf.Clamp(foundWalker.position.x, 1, gridHandler.GetLength(0) - 2);
            foundWalker.position.y = Mathf.Clamp(foundWalker.position.y, 1, gridHandler.GetLength(0) - 2);
            walkers[i] = foundWalker;
        }
    }

    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.floor)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.empty)
                    {
                        tileMap.SetTile(new Vector3Int(x + 1, y, 0), wall);
                        gridHandler[x + 1, y] = Grid.wall;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.empty)
                    {
                        tileMap.SetTile(new Vector3Int(x - 1, y, 0), wall);
                        gridHandler[x - 1, y] = Grid.wall;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.empty)
                    {
                        tileMap.SetTile(new Vector3Int(x, y + 1, 0), wall);
                        gridHandler[x, y + 1] = Grid.wall;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.empty)
                    {
                        tileMap.SetTile(new Vector3Int(x, y - 1, 0), wall);
                        gridHandler[x, y - 1] = Grid.wall;
                        hasCreatedWall = true;
                    }
                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }

                }
            }
        }
    }
}
