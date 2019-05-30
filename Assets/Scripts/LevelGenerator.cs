using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int rooms;
    [SerializeField]
    private int sizeOfRoom;
    [SerializeField]
    private int sizeOfRoad;
    [SerializeField]
    private int monstersPerLevel;
    [SerializeField]
    private int chestsPerLevel;
    [SerializeField]
    private int percentageOfWallsInRooms; //32
    [SerializeField]
    private int percentageOfWallsInRoads; //25
    [SerializeField]
    private int percentageOfWallsInBorderOfRooms; //75

    public Tilemap groundLayer;
    public Tilemap blockingLayer;
    public Tilemap entryDecorationsLayer;
    public Tilemap exitDecorationsLayer;

    public Tile floorTile;
    public Tile sideWallTile;
    public Tile roofWallTile;

    public Tile enemyTile1;
    public Tile enemyTile2;
    public Tile enemyTile3;

    public Tile chestTile1;
    public Tile chestTile2;
    public Tile chestTile3;
    public Tile chestTile4;

    enum Content
    {
        Floor = 0,
        RoofWall = 1,
        SideWall = 3,

        ChestTile1 = 11,
        ChestTile2 = 12,
        ChestTile3 = 13,
        ChestTile4 = 14,

        Enemy1 = 21,
        Enemy2 = 22,
        Enemy3 = 23,

        Player = 50,

        EntryTilemap = 61,
        PlayerAndEntryTilemap = 62,

        ExitTilemap = 65,
        PlayerAndExitTilemap = 66,
    }

    void Start()
    {
        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map;
        map = new int[mapSize, mapSize];
        // 
        GenerateLevel(ref map, mapSize);
        // Воспроизведение карты
        RenderMap(map, mapSize);
    }

    void Update()
    {

    }

    int SetOffset(int i, int delta)
    {
        return (((i / 2) + delta) * sizeOfRoom) + ((i / 2) * sizeOfRoad);
    }

    void GenerateLevel(ref int[,] map, int mapSize)
    {
        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        // Генерация схемы карты
        MapPlan(ref mapScheme);
        // Заполнение карты Tiles
        GenerateMap(ref mapScheme, ref map, mapSize, mapSchemeSize);
        // Добавление "боковых" стен
        AddSideWalls(ref map, mapSize);
    }
    
    // Заполнение карты Tiles
    void GenerateMap(ref int[,] mapScheme, ref int[,] map, int mapSize, int mapSchemeSize)
    {
        // Добавление сундуков и противников
        GenerateObjects(mapScheme, ref map, mapSize, mapSchemeSize);
        // Заполнение комнат
        GenerateRooms(ref mapScheme, ref map, mapSize, mapSchemeSize);
    }

    // Добавление сундуков и противников
    void GenerateObjects(int[,] mapScheme, ref int[,] map, int mapSize, int mapSchemeSize)
    {
        int monsters = 0;
        int chests = 0;
        int i, j, offsetX, offsetY, x, y;
        while (monsters < monstersPerLevel)
        {
            i = Random.Range(1, mapSchemeSize + 1);
            j = Random.Range(1, mapSchemeSize + 1);
            if (mapScheme[i, j] == 0)
            {
                while (true)
                {
                    offsetX = Random.Range(0, sizeOfRoom - 1);
                    offsetY = Random.Range(0, sizeOfRoom - 1);
                    x = SetOffset(i, 0) + offsetX;
                    y = SetOffset(j, 0) + offsetY;
                    if (map[x, y] == 0)
                    {
                        int k = Random.Range(1, 4);
                        switch (k)
                        {
                            case 1:
                                map[x, y] = (int)Content.Enemy1;
                                break;
                            case 2:
                                map[x, y] = (int)Content.Enemy2;
                                break;
                            case 3:
                                map[x, y] = (int)Content.Enemy3;
                                break;
                            default:
                                break;
                        }
                        monsters++;
                        break;
                    }
                }
            }
        }
        while (chests < chestsPerLevel)
        {
            i = Random.Range(1, mapSchemeSize + 1);
            j = Random.Range(1, mapSchemeSize + 1);
            if (mapScheme[i, j] == 0)
            {
                while (true)
                {
                    offsetX = Random.Range(1, sizeOfRoom - 2);
                    offsetY = Random.Range(1, sizeOfRoom - 2);
                    x = ((i / 2) * sizeOfRoom) + ((i / 2) * sizeOfRoad) + offsetX;
                    y = ((j / 2) * sizeOfRoom) + ((j / 2) * sizeOfRoad) + offsetY;
                    if (map[x, y] == 0 && map[x + 1, y] == 0 && map[x, y + 1] == 0 && map[x + 1, y + 1] == 0)
                    {
                        map[x, y] = (int)Content.ChestTile1;
                        map[x, y + 1] = (int)Content.ChestTile2;
                        map[x + 1, y] = (int)Content.ChestTile4;
                        map[x + 1, y + 1] = (int)Content.ChestTile3;
                        chests++;
                        break;
                    }
                }
            }
        }
    }

    // Заполнение комнат
    void GenerateRooms(ref int[,] mapScheme, ref int[,] map, int mapSize, int mapSchemeSize)
    {
        for (int x = 0; x < mapSchemeSize; x++)
        {
            for (int y = 0; y < mapSchemeSize; y++)
            {
                // Заполнение не основных комнат
                if (mapScheme[x + 1, y + 1] == 0 && (x % 2 == 0 && y % 2 == 0))
                {
                    GenerateRoomsWall(x, y, ref map);
                }
                // Заполнение дорог
                else if ((x % 2 == 1 || y % 2 == 1))
                {
                    if (x % 2 == 1 && y % 2 == 1) // Перекресток
                    {
                        GenerateRoadsWall(ref map, x, y, sizeOfRoad, sizeOfRoad, 1, 1, ref mapScheme);
                    }
                    if (x % 2 == 1 && y % 2 == 0)
                    {
                        GenerateRoadsWall(ref map, x, y, sizeOfRoad, sizeOfRoom, 1, 0, ref mapScheme);
                    }
                    if (x % 2 == 0 && y % 2 == 1)
                    {
                        GenerateRoadsWall(ref map, x, y, sizeOfRoom, sizeOfRoad, 0, 1, ref mapScheme);
                    }
                }
                // Заполнение важных комнат
                else if (mapScheme[x + 1, y + 1] != 0 && (x % 2 == 0 && y % 2 == 0))
                {
                    GenerateEntryAndExitRooms(x, y, ref map);
                }
            }
        }
    }

    // Заполнение важных комнат
    void GenerateEntryAndExitRooms(int x, int y, ref int[,] map)
    {
        int offsetX = SetOffset(x, 0);
        int offsetY = SetOffset(y, 0);
        for (int k = 0; k < sizeOfRoom; k++)
        {
            for (int m = 0; m < sizeOfRoom; m++)
            {
                map[offsetX + k, offsetY + m] = 0;
            }
        }
        if (y > 1)
        {
            map[offsetX + 4, offsetY + 6] = (int)Content.Player;
            map[offsetX, offsetY] = (int)Content.EntryTilemap;
        }
        if (y == 0)
        {
            map[offsetX, offsetY] = (int)Content.ExitTilemap;
        }
    }

    // Добавление "боковых" стен
    void AddSideWalls(ref int[,] map, int mapSize)
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y] == 0 && map[x, y + 1] == 1)
                {
                    map[x, y + 1] = (int)Content.SideWall;
                }
            }
        }
    }

    // Заполнение не основных комнат
    void GenerateRoomsWall(int x, int y, ref int[,] map)
    {
        x = SetOffset(x, 0);
        y = SetOffset(y, 0);
        // Добавление стен вокруг комнаты
        for (int k = 0; k < sizeOfRoom - 1; k++)
        {
            GenerateRoomsOuterWall(x, y, k + 1, 0, ref map);
            GenerateRoomsOuterWall(x, y, 0, k, ref map);
            GenerateRoomsOuterWall(x, y, k, sizeOfRoom - 1, ref map);
            GenerateRoomsOuterWall(x, y, sizeOfRoom - 1, k + 1, ref map);
        }

        // Добавление стен внутри комнаты
        GenerateRoomsInternalWall(x, y, ref map);
    }

    // Добавление стен вокруг комнаты
    void GenerateRoomsOuterWall(int offsetX, int offsetY, int deltaX, int deltaY, ref int[,] map)
    {
        if (Random.Range(0, 101) > percentageOfWallsInBorderOfRooms)
        {
            map[offsetX + deltaX, offsetY + deltaY] = (int)Content.Floor;
        }
        else
        {
            map[offsetX + deltaX, offsetY + deltaY] = (int)Content.RoofWall;
        }
    }

    // Добавление стен внутри комнаты
    void GenerateRoomsInternalWall(int offsetX, int offsetY, ref int[,] map)
    {
        for (int x = 1; x < sizeOfRoom - 1; x++)
        {
            for (int y = 1; y < sizeOfRoom - 1; y++)
            {
                if (Random.Range(0, 101) <= percentageOfWallsInRooms && map[offsetX + x, offsetY + y] == 0)
                {
                    map[offsetX + x, offsetY + y] = (int)Content.RoofWall;
                }
            }
        }
    }

    // Заполнение дорог стенами
    void GenerateRoadsWall(ref int[,] map, int i, int j, int sizeX, int sizeY, int deltaX, int deltaY, ref int[,] mapScheme)
    {
        int offsetX = SetOffset(i, deltaX);
        int offsetY = SetOffset(j, deltaY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (mapScheme[i + 1, j + 1] != 0)
                {
                    map[offsetX + x, offsetY + y] = (int)Content.Floor;
                }
                else
                {
                    if (Random.Range(0, 101) > percentageOfWallsInRoads)
                    {
                        map[offsetX + x, offsetY + y] = (int)Content.Floor;
                    }
                    else
                    {
                        map[offsetX + x, offsetY + y] = (int)Content.RoofWall;
                    }
                }
            }
        }
    }

    // Генерация схемы карты
    void MapPlan(ref int[,] mapScheme)
    {
        // Создание границ схемы карты
        MapSchemesBorder(ref mapScheme);
        // Создание стартовой и финишной комнаты
        EntryAndExitGeneration(ref mapScheme);
    }

    // Создание границ схемы карты
    void MapSchemesBorder(ref int[,] mapScheme)
    {
        for (int i = 0; i < (int)Mathf.Sqrt(mapScheme.Length); i++)
        {
            mapScheme[0, i] = -1;
            mapScheme[i, 0] = -1;
            mapScheme[(int)Mathf.Sqrt(mapScheme.Length) - 1, i] = -1;
            mapScheme[i, (int)Mathf.Sqrt(mapScheme.Length) - 1] = -1;
        }
    }

    // Создание стартовой и финишной комнаты и проложения между ними дороги
    void EntryAndExitGeneration(ref int[,] mapScheme)
    {
        int finishX = Random.Range(0, rooms) * 2 + 1, finishY = 1; // Координаты X и Y для начальной и конечной комнаты
        int startX = Random.Range(0, rooms) * 2 + 1, startY = rooms * 2 - 1;
        // Координаты дороги рядом с комнатой
        int roadX = 1;
        // Массив, в котором хранятся координаты перекрестков, через которые должна пройти "основная дорога"
        int[] crossRoad = new int[rooms - 1];

        mapScheme[finishX, finishY] = 1;
        mapScheme[startX, startY] = 1;

        // Создание дороги рядом с началом
        GenerateСrossRoadAndRoad(ref mapScheme, startX, startY, 1, ref crossRoad, ref roadX);
        // Создание дороги рядом с финишом
        GenerateСrossRoadAndRoad(ref mapScheme, finishX, finishY, 1, ref crossRoad, ref roadX);
        // Путь от входа до выхода
        GeneratePathFromEntryToExit(ref mapScheme, ref crossRoad, ref roadX);
    }

    // Путь от входа до выхода
    void GeneratePathFromEntryToExit(ref int[,] mapScheme, ref int[] crossRoad, ref int roadX)
    {
        // Создание перекрестков
        for (int m = 1; m < rooms - 1; m++)
        {
            crossRoad[m] = Random.Range(0, rooms - 1) * 2 + 2;
            mapScheme[crossRoad[m], m * 2 + 2] = 1;
        }

        // Прокладываем путь к основным перекресткам
        for (int m = rooms - 2; m >= 0; m--)
        {
            while (roadX != crossRoad[m])
            {
                roadX += roadX > crossRoad[m] ? -1 : 1;
                mapScheme[roadX, m * 2 + 2] = 1;
            }
            if (m > 0)
            {
                mapScheme[roadX, m * 2 + 1] = 1;
                mapScheme[roadX, m * 2] = 1;
            }
        }
    }

    // Создание перекрестка и дороги рядом с комнатой
    void GenerateСrossRoadAndRoad(ref int[,] mapScheme, int x, int y, int number, ref int[] crossRoad, ref int roadX)
    {
        int road;
        road = Random.Range(0, 4); // Создание перекрестка рядом с комнотой
        while (true)
        {
            if (road == 0)
            {
                if (mapScheme[x + 1, y + 1] != -1)
                {
                    GenerateСrossRoad(ref mapScheme, x, y, number, 1, 1, ref crossRoad, ref roadX);
                    break;
                }
                road++;
            }
            if (road == 1)
            {
                if (mapScheme[x - 1, y + 1] != -1)
                {
                    GenerateСrossRoad(ref mapScheme, x, y, number, -1, 1, ref crossRoad, ref roadX);
                    break;
                }
                road++;
            }
            if (road == 2)
            {
                if (mapScheme[x + 1, y - 1] != -1)
                {
                    GenerateСrossRoad(ref mapScheme, x, y, number, 1, -1, ref crossRoad, ref roadX);
                    break;
                }
                road++;
            }
            if (road == 3)
            {
                if (mapScheme[x - 1, y - 1] != -1)
                {
                    GenerateСrossRoad(ref mapScheme, x, y, number, -1, -1, ref crossRoad, ref roadX);
                    break;
                }
                road = 0;
            }
        }
    }

    // Создание перекрестка
    void GenerateСrossRoad(ref int[,] mapScheme, int x, int y, int number, int deltaX, int deltaY, ref int[] crossRoad, ref int roadX)
    {
        mapScheme[x + deltaX, y + deltaY] = number;
        if (Random.Range(0, 2) == 0)
        {
            mapScheme[x, y + deltaY] = number;
        }
        else
        {
            mapScheme[x + deltaX, y] = number;
        }
        if (number == 1 && y == 1)
        {
            crossRoad[0] = x + deltaX;
        }
        if (number == 1 && y != 1)
        {
            roadX = x + deltaX;
        }
    }

    // Воспроизведение карты
    void RenderMap(int[,] map, int mapSize)
    {
        // Заполнение карты полом
        GroundMap(mapSize);
        // Создание границ карты
        CreateMapBorder(map, mapSize);
    }

    // Заполнение карты полом
    void GroundMap(int mapSize)
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                groundLayer.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }
    }

    // Создание границ карты
    void CreateMapBorder(int[,] map, int mapSize)
    {
        for (int x = 0; x < mapSize + 10; x++)
        {
            for (int y = 1; y < 10 + 1; y++)
            {
                blockingLayer.SetTile(new Vector3Int(x, -y, 0), roofWallTile);
                blockingLayer.SetTile(new Vector3Int(x - 10, mapSize + y - 1, 0), roofWallTile); // Верхняя и нижняя стена карты

                blockingLayer.SetTile(new Vector3Int(-y, x - 10, 0), roofWallTile);
                blockingLayer.SetTile(new Vector3Int(mapSize + y - 1, x, 0), roofWallTile); // Левая и правая стена карты
            }
        }

        for (int x = 0; x < mapSize; x++)
        {
            if (map[x, mapSize - 1] == 0)
            {
                blockingLayer.SetTile(new Vector3Int(x, mapSize, 0), sideWallTile);
            }
        }
    }
}