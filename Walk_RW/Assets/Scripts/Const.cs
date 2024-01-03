using UnityEngine;

public class Const : MonoBehaviour
{
    public static float WidthminX;
    public static float WidthmaxX;
    public static float HeightminY;
    public static float HeightmaxY;
    public static float Timer = 0.5f;                //游戏速度
    public static int step = 50; //蛇头的移动距离

    void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        HeightminY = -cameraHeight / 2;
        HeightmaxY = cameraHeight / 2;
        WidthminX = -cameraHeight / 2 * screenRatio;
        WidthmaxX = cameraHeight / 2 * screenRatio;
    }
}