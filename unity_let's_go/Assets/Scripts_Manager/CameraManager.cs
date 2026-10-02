using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 追いかける対象
    [SerializeField] private Transform target;

    //カメラの移動範囲を制限
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
       // ターゲットがnull(データなし)じゃなければ
    if (target != null)
    {
    // Clamp(変数, 最小値, 最大値);
    float x = Mathf.Clamp(target.position.x, minX, maxX);
    float y = Mathf.Clamp(target.position.y, minY, maxY);

    // このオブジェクトの座標をターゲットの座標にする
    // カメラのZ座標は固定
    transform.position = new Vector3(x, y, transform.position.z);
    }
    }
}