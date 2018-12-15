using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 加速度
    /// </summary>
    readonly float ACCELARATION = 0.01f;
    /// <summary>
    /// 最大Z速度
    /// </summary>
    readonly float MAX_VELOCITY_Z = 1f;

    /// <summary>
    /// x,zの速度差分
    /// </summary>
    float diffVelocityX = 0.0f;
    float diffVelocityZ = 0.0f;
    //各方向への速度
    float velocityX = 0.0f;
    float velocityZ = 0.0f;

    //Rigidbodyを変数に入れる
    Rigidbody rb;
    //ジャンプ力
    float jumpForce = 5;
    //Animatorを入れる変数
    private Animator animator;
    //ユニティちゃんの位置を入れる
    Vector3 playerPos;
    //地面に接触しているか否か
    bool ground;

    /// <summary>
    /// キーボード入力ハンドラ
    /// </summary>
    [SerializeField]
    KeyBordInputHandler keyBordInputHandler;
    /// <summary>
    /// 見つめるスフィア
    /// </summary>
    [SerializeField]
    Transform lookedAtSphere;

    /// <summary>
    /// ジャンプ中
    /// </summary>
    bool nowJumpping;

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    /// <returns>処理中</returns>
    IEnumerator Jump()
    {
        nowJumpping = true;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //ジャンプのアニメーションをオンにする
        animator.SetBool("Jumping", true);
        while (true)
        {
            if (ground)
            {
                break;
            }
            yield return null;
        }
        //ジャンプのアニメーションをオフにする
        animator.SetBool("Jumping", false);
        nowJumpping = false;
    }

    void Start()
    {
        //Rigidbodyを取得
        rb = GetComponent<Rigidbody>();
        //ユニティちゃんのAnimatorにアクセスする
        animator = GetComponent<Animator>();
        //ユニティちゃんの現在より少し前の位置を保存
        playerPos = transform.position;
        nowJumpping = false;
    }

    IEnumerator updateMotion()
    {
        var oldPos = Vector3.zero;
        //移動距離が少しでもあった場合に方向転換
        while (true)
        {
            //ユニティちゃんの最新の位置から少し前の位置を引いて移動距離を割り出す
            Vector3 direction = playerPos - oldPos;

            if (direction.magnitude > 0.001f)
            {
                //走るアニメーションを再生
                animator.SetBool("Running", true);

            }
            else
            {
                //ベクトルの長さがない＝移動していない時は走るアニメーションはオフ
                animator.SetBool("Running", false);
            }
            oldPos = playerPos;
            yield return null;
        }
    }

    private void Update()
    {
        //W・Sキー、↑↓キーで前後移動
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (velocityZ < MAX_VELOCITY_Z)
            {
                velocityZ += ACCELARATION;
            }
        }
        else
        {
            if (Math.Abs(velocityZ) > 0.1f)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    velocityZ -= ACCELARATION * 2;
                }
                else
                {
                    velocityZ -= ACCELARATION;
                }
            }
            else
            {
                velocityZ = 0.0f;
            }
        }

        //現在の位置＋向いている方向にかかっている速度に移動する
        rb.MovePosition(transform.position + transform.forward * velocityZ);

        //姿勢を変更
        transform.Rotate(0, Input.GetAxisRaw("Horizontal") * 2, 0, Space.World);

        //ユニティちゃんの最新の位置から少し前の位置を引いて移動距離を割り出す
        Vector3 direction = playerPos - transform.position;
        if (direction.magnitude > 0.001f)
        {
            //走るアニメーションを再生
            animator.SetBool("Running", true);

        }
        else
        {
            //ベクトルの長さがない＝移動していない時は走るアニメーションはオフ
            animator.SetBool("Running", false);
        }

        //ユニティちゃんの位置を更新する
        playerPos = transform.position;

        if (Input.GetButton("Jump") && !nowJumpping && ground)
        {
            StartCoroutine("Jump");
        }
    }

    //Planeに触れている間作動
    void OnCollisionStay(Collision col)
    {
        ground = true;
    }

    //Planeから離れると作動
    void OnCollisionExit(Collision col)
    {
        ground = false;
    }

    public void SetPosition(Vector3 position, bool velocityContinue)
    {
        if (!velocityContinue)
        {
            velocityZ = 0.0f;
        }
        transform.position = position;
    }
}