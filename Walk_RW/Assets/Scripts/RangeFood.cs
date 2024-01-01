using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeFood : MonoBehaviour
{
    public static RangeFood _instance;

    //单例
    public static RangeFood instance
    {
        get { return _instance; }
    }

    [Header("食物的所有图片链表")] public List<Sprite> FoodSpriteList;
    [Header("食物预设")] public GameObject FoodPrefab;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        instance.AddFood(); //开始时添加一个食物

    }

    //获取单例
    public static RangeFood getInstance()
    {
        return instance;
    }

    //添加一个食物
    public void AddFood()
    {
        GameObject food = GameObject.Instantiate(FoodPrefab); //克隆一个食物的预制体
        food.GetComponent<Image>().sprite = FoodSpriteList[Random.Range(0, FoodSpriteList.Count - 1)]; //随机一个食物的图片
        food.transform.SetParent(this.transform.parent, false);
        food.transform.localPosition = new Vector3(Random.Range(Const.WidthminX, Const.WidthmaxX),
            Random.Range(Const.HeightminY, Const.HeightmaxY), 0); //在这里给食物赋值一个随机的位置
        food.transform.localScale = Vector3.one; //让缩放为1
    }
}