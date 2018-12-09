using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 加速度
    /// </summary>
    readonly float ACCELARATION = 0.01f;
    /// <summary>
    /// 最大X速度
    /// </summary>
    readonly float MAX_VELOCITY_X = 3f;
    /// <summary>
    /// 最大Z速度
    /// </summary>
    readonly float MAX_VELOCITY_Z = 3f;

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

    IEnumerator Jump()
    {
        nowJumpping = true;
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
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

        transform.LookAt(lookedAtSphere);

        StartCoroutine(updateMotion());
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
                // directionのX軸とZ軸の方向を向かせる
                transform.rotation = Quaternion.LookRotation(new Vector3
                    (direction.x, 0, direction.z));
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
        float vx = Input.GetAxisRaw("Horizontal") * Time.deltaTime * MAX_VELOCITY_X;

        //W・Sキー、↑↓キーで前後移動
        float vz = Input.GetAxisRaw("Vertical") * Time.deltaTime * MAX_VELOCITY_Z;

        //現在の位置＋入力した数値の場所に移動する
        rb.MovePosition(transform.position + new Vector3(vx, 0, vz));

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
}