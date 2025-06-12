using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

[System.Serializable]
public class OrbPropatiey
{
    [SerializeField] Sprite orbSprite;
    [SerializeField] Vector2 leftOrbWorldPos;
    [SerializeField] Vector2 rightOrbWorldPos;
    [SerializeField] ParticleSystem orbPrt;
    [SerializeField] float moveAmount;

    public Sprite OrbSprite { get => orbSprite;}
    public Vector2 LeftOrbWorldPos { get => leftOrbWorldPos;}
    public ParticleSystem OrbPrt { get => orbPrt;}
    public float MoveAmount { get => moveAmount;}
    public Vector2 RightOrbWorldPos { get => rightOrbWorldPos;}
}

public class BossDOTweenAction : MonoBehaviour
{
    Transform gridPos;
    [SerializeField] OrbPropatiey orbPropatiey;
  
   
    void Start()
    {
        DOTween.Init();
        gridPos = GameObject.FindGameObjectWithTag("Floar4Ground").gameObject.transform;
        //transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f),1.0f);
    }//終了時のスケール

    public async UniTask PreLastBossVibrato(float duration, float strength, int vibrato, float randomness, bool fadeOut = false)
    {

       List<UniTask> _shakeTweeners = new List<UniTask>();
       //Vector3 _initPosition = new Vector3();

        Transform playerTra = PlayerController.Instance.gameObject.transform;
        
        Dictionary<int, Transform> _targetTras = new Dictionary<int, Transform>()
        {
            { 0 ,playerTra},
            { 1,gridPos},
        };
       
       
        //_initPosition = targetTra.position;
        // 前回の処理が残っていれば停止して初期位置に戻す
        //if (_shakeTweeners != null)
        //{
        //    foreach (var _shakeTweener in _shakeTweeners)
        //    {
        //        _shakeTweener.Kill();
        //    }
        //    //_shakeTweener.Kill();
        //    //gameObject.transform.position = _initPosition;
        //}
        // 揺れ開始

        for (int i = 0; i < 2; i++)
        {
           if(_targetTras.TryGetValue(i, out Transform _targetTra))//
            {
                 var _shakeTweener = _targetTra.DOShakePosition(duration, strength, vibrato, randomness, fadeOut).ToUniTask();
                _shakeTweeners.Add(_shakeTweener);
            }
            
        }

        await UniTask.WhenAll(_shakeTweeners);
        //_shakeTweener = targetTra.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }

    public async UniTask LastBossVibrato(Transform bossTra, float duration, float strength, int vibrato, float randomness, bool fadeOut = false)
    {

        Vector2 newPos = new Vector2(bossTra.position.x, 0f);
        await bossTra.DOMove(newPos, duration).ToUniTask();

        await PreLastBossVibrato(duration,strength,vibrato,randomness,fadeOut);
        Debug.Log("降臨");
    }
    public async UniTask FadeImage()
    {
        float fadeTime = 3.0f;
        Fade.Instance.FadeIn(3.0f);
        await UniTask.Delay(TimeSpan.FromSeconds(fadeTime));
        Fade.Instance.FadeOut(3.0f);
        await UniTask.Delay(TimeSpan.FromSeconds(fadeTime));

    }

    public IEnumerator BossIntoOrb(int bossCount)
    {
        GameObject orbObj = new GameObject("OrbObject");

        if (bossCount == 0) orbObj.transform.position = orbPropatiey.LeftOrbWorldPos;
        else if (bossCount == 1) orbObj.transform.position = orbPropatiey.RightOrbWorldPos;


        SpriteRenderer orbSp =  orbObj.AddComponent<SpriteRenderer>();
        MoveOrb moveOrb = orbObj.AddComponent<MoveOrb>();
        moveOrb.OrbPrt = orbPropatiey.OrbPrt;

        yield return null;
        Vector2 orbPos = orbObj.transform.position;
        Vector2 newPos = new Vector2(orbPos.x, orbPos.y - 1.5f);
        moveOrb.GeneratedPrt.transform.position = newPos;
        orbSp.sortingLayerName = "Actor";
        orbSp.sprite = orbPropatiey.OrbSprite;

        ShakeOrb(moveOrb);

        Sequence sequence = DOTween.Sequence();
        // 1.5 秒かけて左に 10 m 移動する動きを覚えさせます。
        sequence.Append(orbObj.transform.DOMoveX(orbPropatiey.MoveAmount, 1.0f, false).SetEase(Ease.InOutQuart).SetRelative());
        // 1.5 秒かけて右に 10 m 移動する動きを覚えさせます。
        sequence.Append(orbObj.transform.DOMoveX(-orbPropatiey.MoveAmount, 1.0f, false).SetEase(Ease.InOutQuart).SetRelative());

        // 無制限に繰り返すことを指示します。
        sequence.SetLoops(-1, LoopType.Restart);


        Debug.Log("Sequence created.");

        // シーケンス開始を確認
        if (sequence.IsActive())
        {
            Debug.Log("Sequence is active and running.");
        }
        else
        {
            Debug.LogError("Sequence is not active.");
        }
        yield return new WaitUntil(() => moveOrb.IsUped);

        sequence.Kill();
        
 
    }

    async void ShakeOrb(MoveOrb moveOrb)
    {
        while(!moveOrb.IsUped)
        {
            Debug.Log("Scaling up...");
            await moveOrb.gameObject.transform.DOScale(Vector3.one * 1.2f, 1.0f).SetEase(Ease.InOutQuart);
            Debug.Log("Scaling down...");

            await moveOrb.gameObject.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.InOutQuart);

        }
    }

  

}
