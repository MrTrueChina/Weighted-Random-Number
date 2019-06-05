using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;

/// <summary>
/// 用于测试 Dictionary 的部分方法的效果
/// </summary>
[TestFixture]
public class DictionaryTest
{
    Dictionary<int, int> _dictionary;

    [SetUp]
    public void SetUp()
    {
        _dictionary = new Dictionary<int, int>();
    }

    [TearDown]
    public void TearDown()
    {
        _dictionary = null;
    }

    /// <summary>
    /// 测试 Dictionary 是不是有序的
    /// </summary>
    [Test]
    public void IsOrderly()
    {
        List<int> keys;

        _dictionary.Add(0, 0);
        _dictionary.Add(1, 1);
        _dictionary.Add(2, 2);
        _dictionary.Add(3, 3);
        _dictionary.Add(4, 4);
        _dictionary.Add(5, 5);

        keys = _dictionary.Keys.ToList();
        for(int i = 1; i < keys.Count;i++)
            if(keys[i]<keys[i-1])
            {
                Debug.Log("Dictionary 不是按照存入顺序保持有序的");
                return;
            }

        _dictionary.Clear();

        _dictionary.Add(5, 5);
        _dictionary.Add(4, 4);
        _dictionary.Add(3, 3);
        _dictionary.Add(2, 2);
        _dictionary.Add(1, 1);
        _dictionary.Add(0, 0);

        keys = _dictionary.Keys.ToList();
        for (int i = 1; i < keys.Count; i++)
            if (keys[i] > keys[i - 1])
            {
                Debug.Log("Dictionary 不是按照存入顺序保持有序的");
                return;
            }

        Debug.Log("Dictionary 是按照存入顺序保持有序的");
    }
}
