using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class OtherFunctions
{
    [Test]
    public void EntryAndExitGeneration_Generate_EntryAndExitRooms_GeneratedRooms()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int EntryAndExitRooms = 0;
        int expected = 2;

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        levelGenerator.EntryAndExitGeneration(ref mapScheme);

        // ASSERT
        for (int x = 1; x < mapSchemeSize + 1; x += 2)
        {
            if (mapScheme[x, 1] == 1)
            {
                EntryAndExitRooms++;
            }
            if (mapScheme[x, mapSchemeSize] == 1)
            {
                EntryAndExitRooms++;
            }
        }

        Assert.AreEqual(expected, EntryAndExitRooms);
    }

    [Test]
    public void AddSideWalls_UnderWallOrWallOrSideWall()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;
        int monstersPerLevel = 10;
        int chestsPerLevel = 10;

        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map = new int[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    map[x, y] = 1;
                }
            }
        }

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, monstersPerLevel, chestsPerLevel, 0, 0, 0);
        levelGenerator.AddSideWalls(ref map, mapSize);

        // ASSERT
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y + 1] == 1)
                {
                    if (!(map[x, y] == 1 || map[x, y] == 3))
                    {
                        Assert.Fail();
                    }
                }
            }
        }
        Assert.Pass();
    }

    [Test]
    public void GenerateСrossRoadAndRoad_EntryRoom_Generate()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];
        
        int startX = Random.Range(0, rooms) * 2 + 1, startY = rooms * 2 - 1;
        
        int roadX = 1;
        int[] crossRoad = new int[rooms - 1];

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        levelGenerator.GenerateСrossRoadAndRoad(ref mapScheme, startX, startY, 1, ref crossRoad, ref roadX);

        // ASSERT
        if (mapScheme[startX + 1, startY + 1] == 1)
        {
            if (mapScheme[startX + 1, startY] == 1 || mapScheme[startX, startY + 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[startX - 1, startY - 1] == 1)
        {
            if (mapScheme[startX - 1, startY] == 1 || mapScheme[startX, startY - 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[startX + 1, startY - 1] == 1)
        {
            if (mapScheme[startX + 1, startY] == 1 || mapScheme[startX, startY - 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[startX - 1, startY + 1] == 1)
        {
            if (mapScheme[startX - 1, startY] == 1 || mapScheme[startX, startY + 1] == 1)
            {
                Assert.Pass();
            }
        }

        Assert.Fail();
    }

    [Test]
    public void GenerateСrossRoadAndRoad_ExitRoom_Generate()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int finishX = Random.Range(0, rooms) * 2 + 1, finishY = 1;

        int roadX = 1;
        int[] crossRoad = new int[rooms - 1];

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        levelGenerator.GenerateСrossRoadAndRoad(ref mapScheme, finishX, finishY, 1, ref crossRoad, ref roadX);

        // ASSERT
        if (mapScheme[finishX + 1, finishY + 1] == 1)
        {
            if (mapScheme[finishX + 1, finishY] == 1 || mapScheme[finishX, finishY + 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[finishX - 1, finishY - 1] == 1)
        {
            if (mapScheme[finishX - 1, finishY] == 1 || mapScheme[finishX, finishY - 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[finishX + 1, finishY - 1] == 1)
        {
            if (mapScheme[finishX + 1, finishY] == 1 || mapScheme[finishX, finishY - 1] == 1)
            {
                Assert.Pass();
            }
        }
        if (mapScheme[finishX - 1, finishY + 1] == 1)
        {
            if (mapScheme[finishX - 1, finishY] == 1 || mapScheme[finishX, finishY + 1] == 1)
            {
                Assert.Pass();
            }
        }

        Assert.Fail();
    }

    [Test]
    public void EntryAndExitGeneration_Generate_PathFromEntryToExit_Road()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int road = 0;
        int expected = mapSchemeSize - 2;

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        levelGenerator.MapPlan(ref mapScheme);

        // ASSERT
        for (int y = 2; y < mapSchemeSize; y++)
        {
            for (int x = 2; x < mapSchemeSize; x++)
            {
                if (mapScheme[x ,y] == 1)
                {
                    road++;
                    break;
                }
            }
        }

        Assert.AreEqual(expected, road);
    }
}
