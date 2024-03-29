﻿using MtC.Tools.Random;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

[TestFixture]
public class WeightedRandomTest
{
    WeightedRandom _weightedRandom;
    Dictionary<int, int> _probebilities
    {
        get
        {
            return GetProbabilities(_weightedRandom);
        }
    }

    int _totalProbability
    {
        get
        {
            return GetTotalProbabilitiy(_weightedRandom);
        }
    }

    [SetUp]
    public void SetUp()
    {
        _weightedRandom = new WeightedRandom();
    }

    [TearDown]
    public void TearDown()
    {
        _weightedRandom = null;
    }

    /// <summary>
    /// 无参构造
    /// </summary>
    [Test]
    public void WeightedRandom_Normal()
    {
        _weightedRandom = new WeightedRandom();

        Assert.AreEqual(0, _probebilities.Count);
        Assert.AreEqual(0, _totalProbability);
    }

    /// <summary>
    /// 传几率构造
    /// </summary>
    [Test]
    public void WeightedRandom_Dictionary()
    {
        Dictionary<int, int> probebilities = new Dictionary<int, int>()
        {
            { 10, 1 },
            { 20, 2 },
            { 30, 3 },
        };

        _weightedRandom = new WeightedRandom(probebilities);

        Assert.AreEqual(probebilities, _probebilities);
        Assert.AreEqual(6, _totalProbability);
    }

    /// <summary>
    /// 传几率构造，传0几率的情况
    /// </summary>
    [Test]
    public void WeightedRandom_Dictionary_ZeroProbability()
    {
        Dictionary<int, int> probebilities = new Dictionary<int, int>()
        {
            { 10, 1 },
            { 20, 2 },
            { 30, 0 },
        };

        _weightedRandom = new WeightedRandom(probebilities);

        Assert.AreEqual(2, _probebilities.Count);
        Assert.AreEqual(3, _totalProbability);
    }

    /// <summary>
    /// 传几率构造，传负数几率的情况
    /// </summary>
    [Test]
    public void WeightedRandom_Dictionary_NegativeProbability()
    {
        Dictionary<int, int> probebilities = new Dictionary<int, int>()
        {
            { 10, 1 },
            { 20, 2 },
            { 30, -10 },
        };

        _weightedRandom = new WeightedRandom(probebilities);

        Assert.AreEqual(2, _probebilities.Count);
        Assert.AreEqual(3, _totalProbability);
    }

    /// <summary>
    /// 传几率构造，传入的总几率导致溢出的情况
    /// </summary>
    [Test]
    public void WeightedRandom_Dictionary_Overflow()
    {
        Dictionary<int, int> probebilities = new Dictionary<int, int>()
        {
            { 10, int.MaxValue-1 },
            { 20, 10 },
        };

        try
        {
            _weightedRandom = new WeightedRandom(probebilities);

            Assert.Fail();
        }
        catch (OverflowException)
        {
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// 存入新几率
    /// </summary>
    [Test]
    public void SetProbability_New()
    {
        int value = 10;
        int probebility = 1;

        _weightedRandom.SetProbability(value, probebility);

        Assert.AreEqual(probebility, _probebilities[value]);
        Assert.AreEqual(probebility, _totalProbability);
    }

    /// <summary>
    /// 覆盖旧的几率
    /// </summary>
    [Test]
    public void SetProbability_Overlay()
    {
        int value = 10;
        int oldProbebility = 1;
        int newProbebility = 1;

        _weightedRandom.SetProbability(value, oldProbebility);
        _weightedRandom.SetProbability(value, newProbebility);

        Assert.AreEqual(newProbebility, _probebilities[value]); // 值变成新的
        Assert.AreEqual(1, _probebilities.Count); // 数量还是一个不能变
        Assert.AreEqual(newProbebility, _totalProbability);
    }

    /// <summary>
    /// 存入0，效果应该是移除
    /// </summary>
    [Test]
    public void SetProbability_Zero()
    {
        int value = 10;

        _weightedRandom.SetProbability(value, 10);
        _weightedRandom.SetProbability(value, 0);

        Assert.AreEqual(0, _probebilities.Count);
        Assert.AreEqual(0, _totalProbability);
    }

    /// <summary>
    /// 存入负数，效果应该是移除
    /// </summary>
    [Test]
    public void SetProbability_Negative()
    {
        int value = 10;

        _weightedRandom.SetProbability(value, 10);
        _weightedRandom.SetProbability(value, -1);

        Assert.AreEqual(0, _probebilities.Count);
        Assert.AreEqual(0, _totalProbability);
    }

    /// <summary>
    /// 存入新几率导致溢出
    /// </summary>
    [Test]
    public void SetProbability_New_Overflow()
    {
        int firstValue = 1;
        int overflowValue = 2;
        int firstProbability = 100;
        int overflowProbability = int.MaxValue - 1;

        _weightedRandom.SetProbability(firstValue, firstProbability);

        try
        {
            _weightedRandom.SetProbability(overflowValue, overflowProbability);

            Assert.Fail();
        }
        catch (OverflowException)
        {

        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// 覆盖旧的几率导致溢出
    /// </summary>
    [Test]
    public void SetProbability_Overlay_Overflow()
    {
        int firstValue = 1;
        int overflowValue = 2;
        int firstProbability = 100;
        int overflowOldProbability = 1;
        int overflowNewProbability = int.MaxValue - 1;

        _weightedRandom.SetProbability(firstValue, firstProbability);
        _weightedRandom.SetProbability(overflowValue, overflowOldProbability);

        try
        {
            _weightedRandom.SetProbability(overflowValue, overflowNewProbability);

            Assert.Fail();
        }
        catch (OverflowException)
        {

        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// 移除存在的几率
    /// </summary>
    [Test]
    public void RemoveProbability_Exist()
    {
        int value = 10;

        _weightedRandom.SetProbability(value, 10);
        bool result = _weightedRandom.RemoveProbability(value);

        Assert.AreEqual(true, result);
        Assert.AreEqual(0, _totalProbability);
    }

    /// <summary>
    /// 移除不存在的几率
    /// </summary>
    [Test]
    public void RemoveProbability_NonExist()
    {
        int value = 10;

        bool result = _weightedRandom.RemoveProbability(value);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, _totalProbability);
    }

    /// <summary>
    /// 没有任何一个值和几率但获取随机数的情况
    /// </summary>
    [Test]
    public void GetInt_DontHaveProbability()
    {
        try
        {
            _weightedRandom.GetInt();

            Assert.Fail();
        }
        catch (ArgumentException)
        {
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    Dictionary<int, int> GetProbabilities(WeightedRandom weightedRandom)
    {
        Type weightRandomType = typeof(WeightedRandom);
        FieldInfo probabilitiesField = weightRandomType.GetField("_probabilities", BindingFlags.NonPublic | BindingFlags.Instance); // GetField()是要加BindingFlags的，这里加的是 “非公开”、“实例变量”
        return (Dictionary<int, int>)probabilitiesField.GetValue(weightedRandom);
    }

    int GetTotalProbabilitiy(WeightedRandom weightedRandom)
    {
        Type weightRandomType = typeof(WeightedRandom);
        FieldInfo probabilitiesField = weightRandomType.GetField("_totalProbability", BindingFlags.NonPublic | BindingFlags.Instance);
        return (int)probabilitiesField.GetValue(weightedRandom);
    }
}
