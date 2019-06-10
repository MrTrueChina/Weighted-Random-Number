using MtC.Tools.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于演示带权重随机数生成的组件
/// </summary>
public class WeightRandomDisplayer : MonoBehaviour
{
#pragma warning disable 0649 // 这个版本的 Unity 和 VS 相性不好，SerilizeField 会显示没有初始化警告，用这个预处理指令清除掉
    [SerializeField]
    InputField _probabilitiesInputField;
#pragma warning disable 0649
    [SerializeField]
    Text _resultText;

    int _loopTime = 10000;

    private void Start()
    {
        _probabilitiesInputField.text = "1:1\n2:2\n3:3\n4:4";
    }

    public void Show()
    {
        /*
         *  if(输入框的文字格式是正确的、可以解析的)
         *      正式进行演示
         *  else
         *      输出错误
         */
        Dictionary<int, int> probabilities;
        try
        {
            probabilities = InputToDictionary();
            DoShow(probabilities);
        }
        catch (OverflowException e)
        {
            if (e.Message == "所有值的几率总和超出了 int 型的范围")
                _resultText.text = "无法获取随机数，因为所有值的几率之和超出了 int 型的范围";
            else
                _resultText.text = "无法获取随机数，因为有至少一个数字或几率超出了 int 型的范围";
        }
        catch (FormatException)
        {
            _resultText.text = "无法获取随机数，因为输入的几率格式不正确，请按照如下格式输入：\n数字:几率\n数字:几率\n……\n中间的冒号是英文冒号";
        }
        catch (ArgumentException)
        {
            _resultText.text = "无法获取随机数，可能是因为输入的几率中有重复的数字，同一个数字请不要输入一个以上的几率";
        }
        catch (Exception e)
        {
            _resultText.text = "无法获取随机数，因为出现了设计之外的bug，详细信息请看输出";
            Debug.LogError(e);
        }
    }

    Dictionary<int, int> InputToDictionary()
    {
        Dictionary<int, int> probabilities = new Dictionary<int, int>();

        string[] probabilityStrings = _probabilitiesInputField.text.Split('\n');

        foreach (string probabilityString in probabilityStrings)
        {
            string[] item = probabilityString.Split(':');
            probabilities.Add(int.Parse(item[0]), int.Parse(item[1]));
        }

        return probabilities;
    }

    void DoShow(Dictionary<int, int> probabilities)
    {
        WeightedRandom weightedRandom = new WeightedRandom(probabilities);
        Dictionary<int, int> numberTime = probabilities.ToDictionary(p => p.Key, p => 0); // 第二个 Lambda 以 KeyValuePair 为参数，但直接返回 0

        for (int i = 0; i < _loopTime; i++)
            numberTime[weightedRandom.GetInt()]++;

        StringBuilder sb = new StringBuilder("获取" + _loopTime + "次随机数\n");

        foreach (KeyValuePair<int, int> item in numberTime)
            sb.Append(item.Key + "：" + item.Value + "次：" + item.Value / (float)_loopTime * 100 + "%\n");

        _resultText.text = sb.ToString();
    }
}
