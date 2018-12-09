using System.Collections;
using UniRx;
using UnityEngine;

/// <summary>
/// 上下左右のキー入力に対してイベントを発行する
/// </summary>
public class KeyBordInputHandler : MonoBehaviour
{
    /// <summary>
    /// 入力されたキーコードを渡す
    /// </summary>
    Subject<KeyCode> keyCodeSubject = new Subject<KeyCode>();
    /// <summary>
    /// 入力されたキーコードを渡す
    /// </summary>
    Subject<KeyCode> upKeyCodeSubject = new Subject<KeyCode>();

    /// <summary>
    /// キーボードが押された時のイベント発行者
    /// </summary>
    public IObservable<KeyCode> OnKeyDown
    {
        get { return keyCodeSubject; }
    }

    /// <summary>
    /// キーボードが離された時のイベント発行者
    /// </summary>
    public IObservable<KeyCode> OnKeyUp
    {
        get { return upKeyCodeSubject; }
    }

    void Start()
    {
        //StartCoroutine(checkKeyDown());
        //StartCoroutine(checkKeyUp());
    }

    /// <summary>
    /// キーが押されたらキーコード発行
    /// </summary>
    /// <returns>処理中</returns>
    IEnumerator checkKeyDown()
    {
        while (true)
        {
            // 二回書くのくそだるい
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                keyCodeSubject.OnNext(KeyCode.UpArrow);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                keyCodeSubject.OnNext(KeyCode.DownArrow);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                keyCodeSubject.OnNext(KeyCode.LeftArrow);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                keyCodeSubject.OnNext(KeyCode.RightArrow);
            }
            yield return null;
        }
    }

    /// <summary>
    /// キーが離されたらキーコード発行
    /// </summary>
    /// <returns>処理中</returns>
    IEnumerator checkKeyUp()
    {
        while (true)
        {
            // 二回書くのくそだるい
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                upKeyCodeSubject.OnNext(KeyCode.UpArrow);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                upKeyCodeSubject.OnNext(KeyCode.DownArrow);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                upKeyCodeSubject.OnNext(KeyCode.LeftArrow);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                upKeyCodeSubject.OnNext(KeyCode.RightArrow);
            }
            yield return null;
        }
    }
}