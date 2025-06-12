using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BarrierEvent : DialogBase
{

    [SerializeField] Image barrierDialogImage;

    [SerializeField] List<string> dialogContents;
    [SerializeField] List<Barrier_Child> barriers;

    [SerializeField] Renderer barrierRenderer;
    PlayerController player;

    [SerializeField] LightEffect lightEffect;

   
    List<bool> BarrierStatus = new List<bool>();

    Coroutine changeBarrierAlpha;
    private void Start()
    {
        //lightEffect.DestroyBarrier += (() => Destroy(this.gameObject));
        //lightEffect.DialogAction += DialogAction(TypeDialog(dialogContents[1],auto:false)) ;
        player = PlayerController.Instance;
        changeBarrierAlpha = StartCoroutine(ChangeBarrierAlfha());
  
        foreach (var barrier in barriers)
        {
            barrier.OnDestroyedBarrier += AddDestroyedList;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {
           
            IsTyping = true;
            StartCoroutine(ChangeDialogCotent(isBrokenAllBarrier()));
        }
    }

    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
               
        barrierDialogImage.gameObject.SetActive(IsTyping);       
        yield return base.TypeDialog(line, auto, keyOperate);
        player.Constraint = false;
        IsTyping = false;
        barrierDialogImage.gameObject.SetActive(IsTyping);
    }

    //void DialogAction()
    //{
    //    StartCoroutine(TypeDialog(dialogContents[1], auto: false));
    //}

    IEnumerator ChangeDialogCotent(bool isDestroyedBarrier)
    {
        if(isDestroyedBarrier)
        {
            StopCoroutine(changeBarrierAlpha);
            player.Constraint = true;
            

            yield return lightEffect.BarrierDestoryEvent();

            yield return TypeDialog(dialogContents[1], auto: false);
            Destroy(this.gameObject);
            //yield return TypeDialog(dialogContents[1], auto: false);
            

        }
        else
        {
            float X = 1.2f;
            float Y = 1.2f;

            player.Constraint = true;

            yield return TypeDialog(dialogContents[0], auto: false);
            if (player.transform.position.x < 0) X = -X;
            if (player.transform.position.y < 2.75f) Y = -Y;

            Debug.Log($"{X}{Y}");
           

            //Vector3 playerPos = new Vector3(plX, player.transform.position.y + Y);
            player.transform.Translate(X,Y,0f,Space.World);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject) IsTyping = false;
    }

    void AddDestroyedList(Barrier_Child barrier)
    {
        BarrierStatus.Add(true);
        barriers.Remove(barrier);
        Debug.Log(BarrierStatus);
    }

    bool isBrokenAllBarrier()
    {
        if (barriers.Count == 0)
        {
            
            return true;
        }    
        else
        {
            return false;
        }
    }

    IEnumerator ChangeBarrierAlfha()
    {
            while (true)
            {
                // アルファ値の初期値
                float alphaValue = 0.6f;
                bool increaseAlpha = false;

                // コルーチンを無限に実行するループ
                while (true)
                {
                    // アルファ値を増減させる処理
                    if (increaseAlpha)
                    {
                        alphaValue += 0.2f * Time.deltaTime;
                        if (alphaValue >= 0.7f)
                        {
                            alphaValue = 0.7f;
                            increaseAlpha = false;
                        }
                    }
                    else
                    {
                        alphaValue -= 0.2f * Time.deltaTime;
                        if (alphaValue <= 0.3f)
                        {
                            alphaValue = 0.3f;
                            increaseAlpha = true;
                        }
                    }

                    // アルファ値を更新
                    Color color = barrierRenderer.material.color;
                    color.a = alphaValue;
                    barrierRenderer.material.color = color;

                    // 次のフレームまで待機
                    yield return null;
                }
            }
        
        
    }

   

    


}
