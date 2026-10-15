using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public Vector3 pointA = new Vector3(20, 300, 0);  // 開始座標
    public Vector3 pointB = new Vector3(18, 300, 0);   // 目的座標
    public float speed = 1000f;                         // 移動速度

    void Start()
    {
        transform.localPosition = pointA;
    }

    void Update()
    {
        // 【修正】第1引数を localPosition に直しました
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, pointB, speed * Time.deltaTime);
    }
}