using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
public class EmergeStair : MonoBehaviour
{
    [SerializeField] GameObject stairObj;
    [SerializeField] float boader;
    [SerializeField] float upAmount;
    [SerializeField] SimpleSituationDialog situationDialog;
    [SerializeField] FloarToSecond_Casle floarToSecond;
    //[SerializeField] ParticleSystem emergePrt;

    //PlayerController player;

    bool isUped = true;
    bool isScaleUp = false;
    CancellationTokenSource cls = new CancellationTokenSource();
    CancellationToken clt;

    private void Start()
    {
        clt = cls.Token;
        //player = GameObject.FindObjectOfType<PlayerController>();
       
    }
    public async void StairMoveAnim(BoxCollider2D boxCollider2D)
    {

        Debug.Log("階段");
         StartCoroutine(UpStair());
        ShakeStair();
        ExpandScale();
        Debug.Log(isUped);
        await new WaitUntil(() => !isUped);
        await UniTask.Delay(TimeSpan.FromSeconds(3.0f));
        var dialogTask = situationDialog.OutPutDialog().ToUniTask();
        await dialogTask;
        Debug.Log("エフェクト終わり");
        boxCollider2D.enabled = true;
        floarToSecond.OnCompletedTerm = true;
        PlayerController.Instance.Constraint = false;
        //Destroy(emergePrt.gameObject);
    }
    IEnumerator UpStair()
    {
        Debug.Log(isUped);
        float posY =  stairObj.transform.position.y;
        while(posY <= boader)
        {
              

                stairObj.transform.position = new Vector2(stairObj.transform.position.x,stairObj.transform.position.y + upAmount);
                //stairObj.transform.position = setPos;
                posY = stairObj.transform.position.y;
                isUped = true;


            yield return null;
        }
        cls.Cancel();
        isUped = false;
    }

    async void ShakeStair()
    {

        while(!clt.IsCancellationRequested)
        {
            await stairObj.transform.DOPunchPosition(Vector3.right * 0.1f, 0.1f);
            await stairObj.transform.DOPunchPosition(Vector3.left * 0.1f, 0.1f);

        }
        Debug.Log("終了");
    }

    async void ExpandScale()
    {
        while(!clt.IsCancellationRequested && !isScaleUp)
        {
            isScaleUp = true;
            await stairObj.transform.DOScale(stairObj.transform.localScale * 1.15f,0.1f);
            await UniTask.Yield();
            isScaleUp = false;
        }
    }
}
