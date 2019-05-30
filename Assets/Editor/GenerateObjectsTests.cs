using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class GenerateObjectsTests
{
    [Test]
    public void Generate_Chests10AndEnemy10_Same()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;
        int rooms = 4;
        int monstersPerLevel = 10, monsters = 0;
        int chestsPerLevel = 10, chests = 0;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];
        
        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map = new int[mapSize, mapSize];

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, monstersPerLevel, chestsPerLevel, 0, 0, 0);
        levelGenerator.GenerateObjects(mapScheme, ref map, mapSize, mapSchemeSize);

        // ASSERT
        for (int x = 0; x < mapSize - 1; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y] == 21 || map[x, y] == 22 || map[x, y] == 23)
                {
                    monsters++;
                }
                if (map[x, y] == 11 && map[x,  y + 1] == 12 && map[x + 1, y + 1] == 13 && map[x + 1, y] == 14)
                {
                    chests++;
                }
            }
        }
        Assert.AreEqual(monstersPerLevel, monsters);
        Assert.AreEqual(chestsPerLevel, chests);
    }

    [Test]
    public void Generate_ChestsAndEnemyLargerThanMapSize_SixteenthPartOfSize()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;
        int monstersPerLevel = 1000, monsters = 0;
        int chestsPerLevel = 2000, chests = 0;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map = new int[mapSize, mapSize];

        int expectedChests, expectedMonsters;

        if (chestsPerLevel * 16 > Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2))
        {
            expectedChests = (int)Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2) / 16;
        } 
        else
        {
            expectedChests = chestsPerLevel;
        }


        if (monstersPerLevel + (int)Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2) / 4 > Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2) ||
            (monstersPerLevel + chestsPerLevel * 16 > Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2) && chestsPerLevel * 16 <= Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2)))
        {
            expectedMonsters = (int)Mathf.Pow((mapSchemeSize + 1) / 2 * (sizeOfRoom - 2), 2) / 4;
        }
        else
        {
            expectedMonsters = monstersPerLevel;
        }

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, monstersPerLevel, chestsPerLevel, 0, 0, 0);
        levelGenerator.GenerateObjects(mapScheme, ref map, mapSize, mapSchemeSize);

        // ASSERT
        for (int x = 0; x < mapSize - 1; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y] == 21 || map[x, y] == 22 || map[x, y] == 23)
                {
                    monsters++;
                }
                if (map[x, y] == 11 && map[x, y + 1] == 12 && map[x + 1, y + 1] == 13 && map[x + 1, y] == 14)
                {
                    chests++;
                }
            }
        }
        Assert.AreEqual(expectedChests, chests);
        Assert.AreEqual(expectedMonsters, monsters);
    }

    [Test]
    public void Generate_ChestsNegativeAndEnemyNegative_Chests0AndEnemy0()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;
        int monstersPerLevel = -10, monsters = 0;
        int chestsPerLevel = -10, chests = 0;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map = new int[mapSize, mapSize];

        int expectedChests = 0, expectedMonsters = 0;

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, monstersPerLevel, chestsPerLevel, 0, 0, 0);
        levelGenerator.GenerateObjects(mapScheme, ref map, mapSize, mapSchemeSize);

        // ASSERT
        for (int x = 0; x < mapSize - 1; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y] == 21 || map[x, y] == 22 || map[x, y] == 23)
                {
                    monsters++;
                }
                if (map[x, y] == 11 && map[x, y + 1] == 12 && map[x + 1, y + 1] == 13 && map[x + 1, y] == 14)
                {
                    chests++;
                }
            }
        }
        Assert.AreEqual(expectedChests, chests);
        Assert.AreEqual(expectedMonsters, monsters);
    }

    [Test]
    public void Generate_Chests0AndEnemy0_Chests0AndEnemy0()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 6;
        int sizeOfRoad = 2;
        int rooms = 2;
        int monstersPerLevel = 0, monsters = 0;
        int chestsPerLevel = 0, chests = 0;

        int mapSchemeSize = rooms * 2 - 1;
        int[,] mapScheme = new int[mapSchemeSize + 2, mapSchemeSize + 2];

        int mapSize = rooms * sizeOfRoom + (rooms - 1) * sizeOfRoad;
        int[,] map = new int[mapSize, mapSize];

        int expectedChests = 0, expectedMonsters = 0;

        // ACT
        levelGenerator.Construct(rooms, sizeOfRoom, sizeOfRoad, monstersPerLevel, chestsPerLevel, 0, 0, 0);
        levelGenerator.GenerateObjects(mapScheme, ref map, mapSize, mapSchemeSize);

        // ASSERT
        for (int x = 0; x < mapSize - 1; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                if (map[x, y] == 21 || map[x, y] == 22 || map[x, y] == 23)
                {
                    monsters++;
                }
                if (map[x, y] == 11 && map[x, y + 1] == 12 && map[x + 1, y + 1] == 13 && map[x + 1, y] == 14)
                {
                    chests++;
                }
            }
        }
        Assert.AreEqual(expectedChests, chests);
        Assert.AreEqual(expectedMonsters, monsters);
    }
}
