using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMapCharacterPosition : MonoBehaviour
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    Player player;
    /// <summary>
    /// 3dマップ
    /// </summary>
    [SerializeField]
    GameObject map3d;
    /// <summary>
    /// 2dマップ
    /// </summary>
    [SerializeField]
    Image map2d;

    /// <summary>
    /// マップ比率
    /// </summary>
    float mapScaleRatio = 0.0f;

    // Use this for initialization
    void Start()
    {
        mapScaleRatio = (map2d.rectTransform.sizeDelta.x / (map3d.transform.localScale.x * map3d.GetComponent<MeshFilter>().mesh.bounds.size.x));
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = GetPosition();
        transform.rotation = Quaternion.AngleAxis(-player.transform.rotation.y * Mathf.Rad2Deg + 180, Vector3.forward);
    }

    Vector3 GetPosition()
    {
        var position = player.transform.position;
        position *= mapScaleRatio;
        position = new Vector3(position.x, position.z, 0);
        return position;
    }
}
