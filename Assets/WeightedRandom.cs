using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeightedRandom
{
    Dictionary<int, int> _probabilities;
    int _totalProbability;

    public WeightedRandom()
        : this(new Dictionary<int, int>())
    { }

    /// <summary>
    /// Dictionary 的Key 对应值，Value 对应这个值的几率
    /// </summary>
    /// <param name="probabilities"></param>
    public WeightedRandom(Dictionary<int, int> probabilities)
    {
        _probabilities = probabilities.Where(p => p.Value > 0).ToDictionary(p => p.Key, p => p.Value); // 取出传入的 Dictionary 中的几率为正数的元素并转换为一个新的 Dictionary 保存起来
        //Dictionary.Where()：获取符合指定标准的元素存入一个 IEnumerable 中返回。一般参数是 Lambda 表达式，参数方法的标准是：参数是单个 KeyValuePair，返回值是表示是否符合标准的 bool
        //IEnumerable.ToDictionary()：将 IEnumerable 的元素按照指定标准转换为 Dictionary。参数是两个方法，参数都是单个 KeyValuePair，第一个返回值是 Dictionary 的 Key，第二个的返回值则是 Dictionary 的 Value
        //这两个方法都是 System.Linq 提供的扩展方法
        //参数叫 p 是取了 probability 的首字母

        UpdateTotalProbability();
    }

    /// <summary>
    /// 根据存入的加权几率随机返回一个int
    /// </summary>
    /// <returns></returns>
    public int GetInt()
    {
        /*
         *  没有几率时不能获取随机数
         */
        /*
         *  根据总几率获取这次的几率
         *  根据这次的几率和几率表返回值
         */
        if (_totalProbability <= 0)
            throw new System.ArgumentException("不能在没有几率的情况下获取随机数");

        int currentProbability = GetCurrentProbability();
        return GetValueByCurrentProbability(currentProbability);
    }

    int GetCurrentProbability()
    {
        return Random.Range(0, _totalProbability);
    }

    int GetValueByCurrentProbability(int randomProbability)
    {
        /*
         *  遍历所有几率
         *  {
         *      几率 -= 当前几率
         *      if(几率 < 0)
         *          return 当前几率对应的值
         *  }
         *  
         *  抛异常
         */
        foreach (KeyValuePair<int, int> currentProbability in _probabilities)
            if ((randomProbability -= currentProbability.Value) < 0)
                return currentProbability.Key;

        throw new System.ArgumentOutOfRangeException("随机几率超出了总几率范围");
    }

    /// <summary>
    /// 存入一个值和它的加权几率，如果已经有这个值的几率则会覆盖之前的几率，如果几率小于等于0，则会移除这个值和它的几率
    /// </summary>
    /// <param name="value"></param>
    /// <param name="probability"></param>
    public void SetProbability(int value, int probability)
    {
        /*
         *  if(值不存在)
         *      存入
         *  else
         *      修改
         *      
         *  更新总几率
         */
        if (!_probabilities.ContainsKey(value))
            DoSetProbability(value, probability);
        else
            UpdateProbability(value, probability);

        UpdateTotalProbability();
    }

    void DoSetProbability(int value, int probability)
    {
        _probabilities.Add(value, probability);
    }

    void UpdateProbability(int value, int probability)
    {
        /*
         *  if(几率大于0)
         *      更新
         *  else
         *      移除
         */
        if (probability > 0)
            DoUpdateProbability(value, probability);
        else
            RemoveProbability(value);
    }

    void DoUpdateProbability(int value, int probability)
    {
        _probabilities[value] = probability;
    }

    void UpdateTotalProbability()
    {
        _totalProbability = 0;

        foreach (int probability in _probabilities.Values)
            _totalProbability += probability;
    }

    /// <summary>
    /// 移除一个值和它的几率
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool RemoveProbability(int value)
    {
        bool result = _probabilities.Remove(value);

        UpdateTotalProbability();

        return result;
    }

    /// <summary>
    /// 根据传入的加权几率随机返回一个int，注意 Dictionary 的 Key 对应值，Value 对应这个值的几率
    /// </summary>
    /// <param name="probabilities"></param>
    /// <returns></returns>
    public static int GetInt(Dictionary<int, int> probabilities)
    {
        return new WeightedRandom(probabilities).GetInt();
    }
}
