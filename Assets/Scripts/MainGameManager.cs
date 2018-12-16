using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    Player player;
    ///<summary>
    /// クリアに進めるぜテキスト
    ///</summary>
    [SerializeField]
    TextMeshProUGUI sendToClearText;
    /// <summary>
    /// クリア可能パネル
    /// </summary>
    [SerializeField]
    MeshCollider clearPanel;
    /// <summary>
    /// プレイヤー初期位置
    /// </summary>
    Vector3 initialPlayerPosition;
    /// <summary>
    /// クリアに進めるなら真
    /// </summary>
    bool canClear = false;

    // Use this for initialization
    void Start()
    {
        initialPlayerPosition = player.transform.position;
        MessageBroker.Default.Receive<bool>().Subscribe(b => canClear = false);
        MessageBroker.Default.Receive<Collision>().Subscribe(col => toCanClear(col));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        sendToClearText.gameObject.SetActive(canClear);
        if (canClear)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene("Clear");
            }
        }
    }

    //Planeに触れている間作動
    void toCanClear(Collision col)
    {
        if (col.collider == clearPanel)
        {
            canClear = true;
        }
    }

    public void Restart()
    {
        player.SetPosition(initialPlayerPosition, false);
    }
}
