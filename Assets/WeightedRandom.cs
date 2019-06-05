using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom
{
    Dictionary<int, int> _probabilities;

    public WeightedRandom()
        : this(new Dictionary<int, int>())
    { }

    public WeightedRandom(Dictionary<int, int> probabilities)
    {
        _probabilities = probabilities;
    }

    /// <summary>
    /// 根据存入的加权概率随机返回一个int
    /// </summary>
    /// <returns></returns>
    public int GetInt()
    {
        throw new MissingComponentException(); //TODO
    }

    /// <summary>
    /// 存入一个值和它的加权概率，如果已经有这个值的概率则会覆盖之前的概率，如果概率小于等于0，则会移除这个值和它的概率
    /// </summary>
    /// <param name="value"></param>
    /// <param name="probability"></param>
    public void SetProbability(int value,int probability)
    {

    }

    /// <summary>
    /// 移除一个值和它的概率
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool RemoveProbability(int value)
    {
        throw new MissingComponentException(); //TODO
    }

    public static int GetInt(Dictionary<int, int> probabilities)
    {
        return new WeightedRandom(probabilities).GetInt();
    }
}
