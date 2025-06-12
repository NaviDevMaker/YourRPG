using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NpcMurabitoBase : DialogBase
{
    protected PlayerController playerController;

    [SerializeField] GameObject image;//ダイアログのイメージのオブジェクト
    [SerializeField] LayerMask murabitoLayerMasks;
    //村人のスクリプタブルオブジェクトの動く距離をリストに移す
     List<int> walkX = new List<int>();
     List<int> walkY = new List<int>();
     List<int> storeDirectionCounts = new List<int>();//何歩歩いたかを保存する
     List<Transform> murabitoPositions = new List<Transform>();//村人の最初の出現場所を設定
     List<bool> changeDirection= new List<bool>();//村びとごとの方向転換の確認
     List<Coroutine> WalkCoroutines = new List<Coroutine>();//コルーチンを格納
     List<string> DialogContents = new List<string>();
     protected List<float> currentTimes = new List<float>();
    int getFromPlayer;
     bool confirmPlayerHit = false;//プレイヤーのあたりを確認
     protected bool isDialogActive = false;//ダイアログがアクティブ時、継承先で使うのでprotected

    public virtual void Start()//継承先で使うため
    {
        //if (playerController.MurabitoEvent != null) playerController.MurabitoEvent = null;
        Debug.Log("ゲット！");
        playerController = GameObject.FindObjectOfType<PlayerController>();//GameObject.FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
        playerController.MurabitoEvent += ConfirmMurabitoEvent;
        //StartCoroutine(TypeDialog("おはよ", false));
        
    }
   
    void ConfirmMurabitoEvent(int murabitoLayerIndex)//村びとと当たったときのイベント
    {
        //StartCoroutine(TypeDialog("おはよ", auto: false));
        Debug.Log("感知");
        getFromPlayer = murabitoLayerIndex;
        confirmPlayerHit = true;
    }

    //void SetConfirmFlug(int murabitoIndex)
    //{
    //    confirmPlayerHits[murabitoIndex] = true;
    //}
    public override IEnumerator TypeDialog(string line, bool auto,bool keyOperate = true)
    {
        if(Text.text != null)
        {
            Debug.Log("綺麗");
            Text.text = null;
            
        }

        //if (isDialogActive)
        //{
        //    yield break; // すでにダイアログがアクティブなら処理を終了
        //}
        //ダイアログ関係のオブジェクトの管理
        Debug.Log("48,またやん");
        image.SetActive(true);
        playerController.Constraint = true;//プレイヤーの動きを止める
        //Debug.Log("アクティブ");
        //yield return StartCoroutine(TypeDialog(line, auto));
        yield return base.TypeDialog(line,auto);

        image.SetActive(false);
        isDialogActive = false;
        Debug.Log(isDialogActive);
        //Debug.Log("非アクティブ");

    }

    public virtual void  StartWalking(MurabitoInfo murabitoInfo)//コルーチンを止める処理があるので必要
    {
        int murabitoIndex = murabitoInfo.MurabitoIndex;
        Debug.Log(murabitoIndex);
        if (WalkCoroutines[murabitoIndex] != null)
        {
            Debug.Log("ダイアログ");
            StopCoroutine(WalkCoroutines[murabitoIndex]);
            //WalkCoroutines[murabitoIndex] = null;//コルーチンを停止したら null にする
            

        }

        WalkCoroutines[murabitoIndex] = StartCoroutine(WalkMurabito(murabitoInfo));
        Debug.Log(WalkCoroutines[murabitoIndex]);
    }

    //村人の歩く処理
    public  IEnumerator  WalkMurabito(MurabitoInfo murabitoInfo)
    {
        
        //リストに割り当てる為に必要
        int murabitoIndex = murabitoInfo.MurabitoIndex;
        //プレイヤーのあたりを確認
        if (confirmPlayerHit)//&& !isDialogの場合一歩進んでしまうが確定で文は大丈夫
        {
            Debug.Log("お話");
            if (!isDialogActive)//文の重複バグをなくすため
            {
                isDialogActive = true;
                yield return StartCoroutine(TypeDialog(DialogContents[getFromPlayer], auto: false));// murabitoInfo.MurabitoDialogContent
            }
            else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
           
            //murabitoInfo.CurrentTime = 0;
            Debug.Log(WalkCoroutines[murabitoIndex]);

            //Vector3 stopPos = new Vector3(murabitoPositions[murabitoIndex].position.x, murabitoPositions[murabitoIndex].position.y,0);
           
           

            //isDialogActive = false;//テスト用
            playerController.Constraint = false;
            confirmPlayerHit = false;
            //isDialogActive = false;
            //StopCoroutine(WalkCoroutines[murabitoIndex]);

            Debug.Log(WalkCoroutines[murabitoIndex]);



        }

        if (murabitoIndex == 1)
        {
            Debug.Log($"歩数は{storeDirectionCounts[1]}");

        }
        Debug.Log(murabitoIndex);
       
        Debug.Log(WalkCoroutines[murabitoIndex]);
        storeDirectionCounts[murabitoIndex] = Mathf.Clamp(storeDirectionCounts[murabitoIndex], 0, murabitoInfo.ChangeDirectionCount);

                //for (int i= 0; i <  storeDirectionCounts.Count; i++)
            //{
            //    if(murabitoInfo.MurabitoIndex == i)
            //    {

            //    }
            //}   
            
            //村人の歩く方向を変える
            if (storeDirectionCounts[murabitoIndex] == murabitoInfo.ChangeDirectionCount)
            {
                Debug.Log("方向変わる");
                changeDirection[murabitoIndex] = true;
                walkX[murabitoIndex] = -walkX[murabitoIndex];
                walkY[murabitoIndex] = -walkY[murabitoIndex];
                Debug.Log($"Xは{walkX[murabitoIndex]}");
                Debug.Log($"Yは{walkY[murabitoIndex]}");

            }
            
            
           
            //村びとを歩かせる
            Vector3 newMurabitoPos = new Vector3(murabitoPositions[murabitoIndex].position.x + walkX[murabitoIndex], murabitoPositions[murabitoIndex].position.y + walkY[murabitoIndex], 0);
            while ((newMurabitoPos - murabitoPositions[murabitoIndex].position).sqrMagnitude >= Mathf.Epsilon)//村人がゆっくり歩くため
            {
                murabitoPositions[murabitoIndex].position = Vector3.MoveTowards(murabitoPositions[murabitoIndex].position, newMurabitoPos, 5f * Time.deltaTime);

                yield return null;// フレームごとの更新を待つために必要
            }
            murabitoPositions[murabitoIndex].position = newMurabitoPos;

           
            //歩数のカウント、正の方向だったら＋、負の方向だったらー
            if (storeDirectionCounts[murabitoIndex] != murabitoInfo.ChangeDirectionCount && !changeDirection[murabitoIndex]) storeDirectionCounts[murabitoIndex]++;//歩数のカウント
            else if (changeDirection[murabitoIndex])//最初と逆方向に歩いているときの処理
            {

                    if(storeDirectionCounts[murabitoIndex] != 0) storeDirectionCounts[murabitoIndex]--;//村びとが何歩歩いたか

                    //負の方向に一定のカウント分動き終わったら
                    if (storeDirectionCounts[murabitoIndex] == 0)
                    {

                        Debug.Log($"あかん");
                        changeDirection[murabitoIndex] = false;
                        walkX[murabitoIndex] = -walkX[murabitoIndex];
                        walkY[murabitoIndex] = -walkY[murabitoIndex];
                        Debug.Log($"Xは{walkX[murabitoIndex]}");
                        Debug.Log($"Yは{walkY[murabitoIndex]}");
                        if (WalkCoroutines[murabitoIndex] != null)
                        {
                            StopCoroutine(WalkCoroutines[murabitoIndex]);
                            //Debug.Log($"Stopped Walk Coroutine: {WalkCoroutines[murabitoIndex]}");




                            //murabitoinfo.walkcoroutine = null;  // ここでも null にする
                        }
                    }
            }

           


    }







    //村人の情報をリストに移す
    public void SetMurabito(Transform parentTransform, List<MurabitoInfo> murabitoInfos)
    {
       
        storeDirectionCounts.Clear();
        foreach (var murabitoinfo in murabitoInfos)
        {
            storeDirectionCounts.Add(murabitoinfo.StoreCount);
            walkX.Add(murabitoinfo.WalkX);
            walkY.Add(murabitoinfo.WalkY);
            DialogContents.Add(murabitoinfo.MurabitoDialogContent);
            changeDirection.Add(murabitoinfo.ChangeDirection);
            WalkCoroutines.Add(null);
            currentTimes.Add(0f);
            //confirmPlayerHits.Add(false);
            
        }
        Debug.Log($"カウントたちは{storeDirectionCounts.Count}{walkX.Count}{walkY.Count}{changeDirection.Count}");
        int childCount = parentTransform.childCount;

        // 子オブジェクトを順に取得する
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = parentTransform.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            SpriteRenderer spriteRenderer = childObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = murabitoInfos[i].MurabitoAppear;

            childObject.transform.position = murabitoInfos[i].MurabitoPos;
            murabitoPositions.Add(childTransform);

        }
    }
}
