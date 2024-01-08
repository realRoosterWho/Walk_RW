using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodText : MonoBehaviour
{
    
    public TextMeshProUGUI foodText;
    public int foodnum;
    
    private Vector2 currenToScreenPoint;
    // Start is called before the first frame update
    void Start()
    {
        // 随机生成1~3的整数
        foodnum = Random.Range(1, 4);

        // 获取Food的RectTransform组件
        RectTransform foodTextRectTransform = foodText.GetComponent<RectTransform>();

        // 设置TextMeshProUGUI的RectTransform的位置为Food的RectTransform的位置
        Vector3 currentPosition = this.transform.position; // 获取当前GameObject的世界坐标
        Vector2 currentToScreenPoint = Camera.main.WorldToScreenPoint(currentPosition); // 转换为屏幕坐标
        Vector2 foodTextScreenPoint = new Vector2(currenToScreenPoint.x, currenToScreenPoint.y + 50);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(foodTextRectTransform, foodTextScreenPoint, null, out Vector2 foodTextLocalPoint);
        foodTextRectTransform.localPosition = foodTextLocalPoint;
        
        

        //将TextMeshProUGUI组件的文本设置为foodnum
        foodText.text = foodnum.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }
}
