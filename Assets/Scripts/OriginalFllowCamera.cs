using UnityEngine;
using System.Collections;

public class OriginalFllowCamera : MonoBehaviour
{

    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    GameObject player;

    /// <summary>
    /// プレイヤーとカメラ間のオフセット距離を格納する Public 変数
    /// </summary>
    Vector3 offset;


    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        //プレイヤーとカメラ間の距離を取得してそのオフセット値を計算し、格納します。
        offset = transform.position - player.transform.position;
    }

    // 各フレームで、Update の後に LateUpdate が呼び出されます。
    void LateUpdate()
    {
        //カメラの transform 位置をプレイヤーのものと等しく設定します。ただし、計算されたオフセット距離によるずれも加えます。
        transform.position = player.transform.position + offset;
    }
}