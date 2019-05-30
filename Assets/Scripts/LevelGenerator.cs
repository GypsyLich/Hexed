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

    public Tilemap groundLayer;
    public Tilemap blockingLayer;
    public Tilemap entryDecorationsLayer;
    public Tilemap exitDecorationsLayer;

    void Start()
    {
        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map;
        map = new int[mapSize, mapSize];
        // 
        GenerateLevel(ref map, mapSize);
    }

    void Update()
    {

    }

    void GenerateLevel(ref int[,] map, int mapSize)
    {
        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        // Генерация схемы карты
        MapPlan(ref mapScheme);
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
}