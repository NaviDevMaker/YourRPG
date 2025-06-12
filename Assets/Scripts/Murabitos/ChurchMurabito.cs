using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChurchMurabito : DialogBase
{
    
    Image image;
    bool isActive = false;//ダイアログのイベントがアクティブかどうか

    public Image Image { get => image;}

    private void Start()
    {
        //シーン読み込み時にコンポーネントの取得
        image = GameObject.Find("ChurchImage").GetComponent<Image>();
        image.gameObject.SetActive(false);
        Debug.Log(image);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //接触したオブジェクトがプレイヤーだったら
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            if (!isActive)
            {
                StartCoroutine(StartChurchEvent());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //プレイヤーが一度コライダーの範囲から抜けるまでイベントを開始できないようにする
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            isActive = false;
        }
    }

    private IEnumerator StartChurchEvent()//教会の人が話すイベント
    {
        Debug.Log(image);
        image.gameObject.SetActive(true);
        Debug.Log("教会");
        PlayerController.Instance.Constraint = true;
        yield return StartCoroutine(base.TypeDialog("神のご加護があらんことを", auto: false));
        image.gameObject.SetActive(false);
        PlayerController.Instance.Constraint = false;
        isActive = true;
    }
}
