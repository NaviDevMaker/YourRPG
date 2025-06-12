using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurabitoAtTown : NpcMurabitoBase
{
   [SerializeField] List<MurabitoInfo> murabitoInfos;
    
    public  override void Start()
    {
        foreach (var mu in murabitoInfos)
        {

        }
        base.Start();
        base.SetMurabito(this.gameObject.transform, murabitoInfos);
     
    }
    private void Update()
    {
        if(!this.isDialogActive)
        {
            foreach (var murabitoInfo in murabitoInfos)
            {
                //murabitoInfo.CurrentTime += Time.deltaTime;
                currentTimes[murabitoInfo.MurabitoIndex] += Time.deltaTime;
                //Debug.Log(murabitoInfo.CurrentTime);
            }
            // 村人ごとに歩く処理を開始
            ExecuteMurabitoWalk(murabitoInfos);
        }
       
                 
    }

    private void ExecuteMurabitoWalk(List<MurabitoInfo> murabitoInfos)
    {
        　//各村人の歩きを実行
          foreach (var murabitoInfo in murabitoInfos)
          {
                if(currentTimes[murabitoInfo.MurabitoIndex] >= murabitoInfo.WaitTime)//murabitoInfo.CurrentTime
                {
                    StartWalking(murabitoInfo);
                 currentTimes[murabitoInfo.MurabitoIndex] = 0f;//murabitoInfo.CurrentTime = 0;
                }
        
          }
        
    }
        
    
}
