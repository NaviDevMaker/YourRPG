using DG.Tweening; // DOTweenを使用してスムーズなスクロールを実現
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class EndrollTextAnim : MonoBehaviour
{
    [SerializeField] List<Text> texts; // テキストオブジェクトのリスト
    [SerializeField] float borderLine; // テキストが消えるY座標の境界線
    [SerializeField] float duration; // スクロール速度
    [SerializeField] RectTransform parentCanvas; // 親キャンバス（スクロールの基準座標）
    [SerializeField] EndrollContent endrollContent;

    [SerializeField] EndRollPlayerAnim endRollPlayerAnim;

    CancellationTokenSource cls = new CancellationTokenSource();
    CancellationToken clt;
    private Queue<string> contentQueue; // テキストを順番に処理するためのキュー
    Text text;

    
    void Start()
    {
        Debug.Log("スタートエンディング");

        contentQueue = new Queue<string>(endrollContent.LogContents); // リストをキューに変換
        clt = cls.Token;
        StartCoroutine(StartRoll(clt)); // エンドロールを開始
        endRollPlayerAnim.StartAnim(clt);
    }

    IEnumerator StartRoll(CancellationToken clt)
    {
        Debug.Log("スタートエンディング");
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(RollCoroutine(clt));
    }

    IEnumerator RollCoroutine(CancellationToken clt)
    {
    
        foreach (var text in texts)
        {
            text.gameObject.SetActive(false); // 最初は全て非表示にしておく
        }

        while (contentQueue.Count > 0)
        {
            // キューから次の文字列を取得
            string nextContent = contentQueue.Dequeue();

            StartCoroutine(GetNextAvailableText());
            // 表示するTextオブジェクトを取得
            Text activeText = this.text;

            // テキストを設定して表示
            activeText.text = nextContent;
            activeText.gameObject.SetActive(true);

            // テキストを画面下から上へ移動
            RectTransform textTransform = activeText.GetComponent<RectTransform>();
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, -parentCanvas.rect.height);
            textTransform.DOAnchorPosY(borderLine, duration).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(1.0f);
            // テキストを非表示にする
            //activeText.gameObject.SetActive(false);
        }

        Debug.Log("終わり");
        cls.Cancel();
    }

    IEnumerator GetNextAvailableText()
    {
        // 非表示状態のTextオブジェクトを探して返す
        foreach (var text in texts)
        {
            if (!text.gameObject.activeSelf)
            {
                this.text = text;
                yield break;
            }
        }

        //yield return new WaitForSeconds(0.5f);

        // 全て使用中の場合は最初のテキストを再利用（安全のためのフォールバック）
        for (int i = 0; i < texts.Count; i++)
        {
            if (i == texts.Count - 1)
            {
                this.text = texts[0];
                yield return new WaitForSeconds(1.0f);    
            }

            texts[i].gameObject.SetActive(false); // 非表示にする


        }
        

        yield return null;
    }
}