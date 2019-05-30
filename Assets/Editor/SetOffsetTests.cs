using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class SetOffsetTests
{
    [Test]
    public void OffSet_I0AndDelta0_0()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;

        int i = 0;
        int delta = 0;
        int expected = 0;

        // ACT
        levelGenerator.Construct(0, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        int offset = levelGenerator.SetOffset(i, delta);

        // ASSERT
        Assert.AreEqual(expected, offset);
    }

    [Test]
    public void OffSet_I5AndDelta0_28()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;

        int i = 5;
        int delta = 0;
        int expected = 28;

        // ACT
        levelGenerator.Construct(0, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        int offset = levelGenerator.SetOffset(i, delta);

        // ASSERT
        Assert.That(offset, Is.EqualTo(expected));
    }

    [Test]
    public void OffSet_I5AndDelta5_88()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;

        int i = 5;
        int delta = 5;
        int expected = 88;

        // ACT
        levelGenerator.Construct(0, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        int offset = levelGenerator.SetOffset(i, delta);

        // ASSERT
        Assert.That(offset, Is.EqualTo(expected));
    }

    [Test]
    public void OffSet_I0AndDelta5_60()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;

        int i = 0;
        int delta = 5;
        int expected = 60;

        // ACT
        levelGenerator.Construct(0, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        int offset = levelGenerator.SetOffset(i, delta);

        // ASSERT
        Assert.That(offset, Is.EqualTo(expected));
    }
    
    [Test]
    public void OffSet_Imin2AndDeltamin1_min26()
    {
        // ARRANGE
        var levelGenerator = new GameObject().AddComponent<LevelGenerator>();

        int sizeOfRoom = 12;
        int sizeOfRoad = 2;

        int i = -2;
        int delta = -1;
        int expected = -26;

        // ACT
        levelGenerator.Construct(0, sizeOfRoom, sizeOfRoad, 0, 0, 0, 0, 0);
        int offset = levelGenerator.SetOffset(i, delta);

        // ASSERT
        Assert.That(offset, Is.EqualTo(expected));
    }
}
