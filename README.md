# 加权随机数
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

## 使用方法
使用方法有两种：通过静态方法和通过非静态方法。

### 使用静态方法：
```C#
public static int WeightedRandom.GetInt(Dictionary<int, int> probabilities);

//按照 Dictionary<数字, 这个数字的几率> 格式传入参数，即可按权重获取随机数
```
#### 例子：
```C#
int number = WeightedRandom.GetInt(
    new Dictionary<int, int>()
    {
        { 10, 1 },
        { 20, 2 },
        { 30, 3 }
    }
);

//这个例子会按照 1:2:3 的几率获取到 10/20/30 中的一个数字
```
通过静态方法比较方便并且线程安全，但速度较慢。<br/>
<br/>

### 使用非静态方法：
```C#
//首先创建 WeightedRandom 对象，可以选择在创建时存入数字和几率
public WeightedRandom();
public WeightedRandom(Dictionary<int, int> probabilities);

//可以通过 Set 和 Remove 方法调整几率
public void SetProbability(int value, int probability);
public bool RemoveProbability(int value);

//通过 GetInt 方法获取加权随机数
public int WeightedRandom.GetInt();
```
#### 例子：
```C#
WeightedRandom random = new WeightedRandom();
random.SetProbability(10, 1);
random.SetProbability(20, 2);
random.SetProbability(30, 3);
int number = random.GetInt();

/////////////////////////////////////////////

WeightedRandom random = new WeightedRandom(
    new Dictionary<int, int>()
    {
        { 10, 1 },
        { 20, 2 },
        { 30, 3 }
    }
);
int number = random.GetInt();

//这两个例子同样会按照 1:2:3 的几率获取到 10/20/30 中的一个数字
```
通过非静态方法虽然多了创建对象的步骤但通过对象获取随机数的速度比通过静态方法获取随机数更快。
这个使用方法是线程不安全的，但如果创建对象后不再修改几率，多个线程是可以同时获取随机数的。

<br/>

## 文件夹内容
| 文件夹 | 内容 |
| ------ | :------ |
| Assets/WeightedRandom.cs | 加权随机数代码 |
| Assets/Demo | 演示场景和脚本 |
| Assets/Editor/Tests | 测试代码 |
