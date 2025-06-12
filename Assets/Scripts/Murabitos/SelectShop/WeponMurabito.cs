using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponMurabito : SelectShopBase
{

    [SerializeField] List<WeponBase> storeWepons;//ショップの武器のリスト
    [SerializeField] WeponList weponList;
   
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //プレイヤーと接触したとき
        if (player != null)
        {
            InsertWepons(player.PreEnterIndex);
            wepons = storeWepons;
            Debug.Log(wepons);
            Debug.Log("プレイヤーとお話し！");
            StartCoroutine(base.StartOption());//宿での会話イベントを始める
        }
    }

    void InsertWepons(int index)
    {
        switch (index)
        {
            case 2:
                wepons = weponList.TownSelledWepons;
                break;
            case 11:
                wepons = weponList.Town1SelledWepons;
                break;
        }

    }
      
    
}
