using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    Player player;
    /// <summary>
    /// プレイヤー初期位置
    /// </summary>
    Vector3 initialPlayerPosition;

	// Use this for initialization
	void Start () {
        initialPlayerPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
	}

    public void Restart()
    {
        player.SetPosition(initialPlayerPosition, false);
    }
}
