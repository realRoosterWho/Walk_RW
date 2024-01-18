using System.Collections.Generic;
using UnityEngine;

public class RangeFood : MonoBehaviour
{
    [Header("食物的所有图片链表")] public List<Sprite> FoodSpriteList;
    [Header("食物预设")] public GameObject FoodPrefab;
    [Header("确保场上只有一个food")]bool isOnlyFood = true;

    void Start()
    {
        AddFood(); //开始时添加一个食物
    }

    //添加一个食物
    public void AddFood()
    {
        GameObject food = GameObject.Instantiate(FoodPrefab); //克隆一个食物的预制体
        food.transform.parent = this.transform.parent;
        food.transform.localPosition = new Vector3(Random.Range(Const.WidthminX, Const.WidthmaxX),
            Random.Range(Const.HeightminY, Const.HeightmaxY), 0); //在这里给食物赋值一个随机的位置
        food.transform.localScale = Vector3.one; //让缩放为1

    }
    
    public void FixedUpdate()
    {
        if (isOnlyFood == true)
        {
            //检查场上是否只有一个食物，如果有超过一个，把其他的食物都销毁
            GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
            if (foods.Length > 1)
            {
                for (int i = 1; i < foods.Length; i++)
                {
                    Destroy(foods[i]);
                }
            }
        }
    }
}