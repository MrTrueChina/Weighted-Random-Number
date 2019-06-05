using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightRandomDisplayer : MonoBehaviour
{
    Dictionary<int, int> _probabilitys = new Dictionary<int, int>
    {
        { 0, 1 },
        { 5, 3 },
        { 10, 1 },
    };
    WeightedRandom _weightedRandom;

    private void Awake()
    {
        _weightedRandom = new WeightedRandom(_probabilitys);
    }

    private void Update()
    {
        Debug.Log(_weightedRandom.GetInt());
    }
}
