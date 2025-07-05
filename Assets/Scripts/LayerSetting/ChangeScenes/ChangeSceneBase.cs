using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;//Directionaryを使うため


[System.Serializable]

public class ChangeSceneBase
{
    public enum TipTextType
    {
       Patern1,
       Patern2,
       Patern3,
       Patern4,
       Patern5,
    }

    Dictionary<TipTextType, string> tipTextTipes = new Dictionary<TipTextType, string>
    {
        { TipTextType.Patern1, "町で道具や武器を買えます" },
        { TipTextType.Patern2, "敵を倒すと経験値を得られる" },
        { TipTextType.Patern3, "適度に宿で休息を取りましょう" },
        { TipTextType.Patern4, "ボス戦は逃げられません..." },
        { TipTextType.Patern5,"レアモンスターが存在するらしいです" }

    };

    //[SerializeField] ChangeSceneBase changeBase;
    [SerializeField] Image tipImage;//チップのイメージ
    [SerializeField] GameObject LoadAnim;//テキストやチップイメージの親オブジェクト
        
    [SerializeField] string sceneName; //遷移先のシーンの名前
    [SerializeField] Fade fade;//フェイドマネージャー
    //[SerializeField] int sceneIndex;
    [SerializeField]  List<Text> texts;//アニメーションするテキスト
    [SerializeField]  float bounceDuration;
    [SerializeField]  int posY;  //テキストの初期位置

    //public int SceneIndex { get => sceneIndex; set => sceneIndex = value; }
 
    private List<Sequence> sequences = new List<Sequence>();
    public string SceneName { get => sceneName;}
    public int PosY { get => posY;}
    public Image TipImage { get => tipImage;}
    public List<Text> Texts { get => texts;}

    public static bool IsChangeScene { get; private set; }

    //public List<string> GetCurrennScenes1 { get => getCurrennScenes; set => getCurrennScenes = value; }
    //public ChangeSceneBase ChangeBase { get => changeBase; set => changeBase = value; }

    public virtual IEnumerator ChangeScene(ChangeSceneBase changeSceneBase)//シーン遷移を実行
    {
        IsChangeScene = true;
        Debug.Log(IsChangeScene);
        var randomKey = tipTextTipes.Keys.ElementAt(Random.Range(0, tipTextTipes.Count));
        tipImage.GetComponent<TipImage>().InitTexts(tipTextTipes[randomKey]);
        DOTween.Init();
        StopAllAnimations();//アニメーションの開始前にすべてのアニメーションを停止するため
        ResetTextPositions(changeSceneBase);//テキストを初期位置に戻す
        sequences.Clear();//リスト内のアニメーションを空にする

        for (var i = 0; i < texts.Count; i++)//テキストのバウンドアニメーションを行う
        {
            LoadAnim.SetActive(true);
            changeSceneBase.TipImage.gameObject.SetActive(true);
            changeSceneBase.Texts[i].gameObject.SetActive(true);
            int BouncePos = PosY - 25;//移動する位置のPos.Y
            changeSceneBase.Texts[i].rectTransform.anchoredPosition = new Vector2((i - changeSceneBase.Texts.Count / 2) * 25 + 670, PosY);//800
            Sequence sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Restart)
                .SetDelay((bounceDuration / 2) * ((float)i / changeSceneBase.Texts.Count))
                .Append(changeSceneBase.Texts[i].rectTransform.DOAnchorPosY(BouncePos, bounceDuration / 4))
                .Append(changeSceneBase.Texts[i].rectTransform.DOAnchorPosY(PosY, changeSceneBase.bounceDuration / 4))
                 .AppendInterval((bounceDuration / 2) * ((float)(1 - i) / changeSceneBase.Texts.Count))
                 .SetDelay(1.0f);
            sequence.Play();
            sequences.Add(sequence);
        }
        
        //フェードインのあとシーン遷移
        fade.FadeIn(2f, () =>
        {
           
            LoadAnim.gameObject.SetActive(false);

            SceneManager.LoadSceneAsync(changeSceneBase.SceneName).completed += (asyncOperation) =>
            {//SceneManager.LoadSceneAsync(changeSceneBase.SceneName);
                fade.FadeOut(1.5f);
                foreach (var seq in sequences)//シーン遷移後にも不要なアニメーションが残らないようにするため
                {
                    seq.Kill();
                }

                tipImage.GetComponentInChildren<Text>().text = " ";//チップのテキストを消す
            };

        });

        yield return new WaitForSeconds(4.0f);//この後にプレイヤーは動けるようになる
        IsChangeScene = false;
        Debug.Log(IsChangeScene);

    }

    private void StopAllAnimations()
    {
        // すべての既存のSequenceを停止
        foreach (var seq in sequences)
        {
            seq.Kill();
        }
    }

    private void ResetTextPositions(ChangeSceneBase changeSceneBase)
    {
        for (var i = 0; i < changeSceneBase.Texts.Count; i++)
        {
            // 各TextのRectTransformを初期位置にリセット
            changeSceneBase.Texts[i].rectTransform.anchoredPosition = new Vector2((i - changeSceneBase.Texts.Count / 2) * 25 + 800, PosY);
        }
    }

   



}

  
  

