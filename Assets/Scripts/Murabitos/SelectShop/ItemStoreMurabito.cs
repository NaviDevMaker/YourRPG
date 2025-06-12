using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemStoreMurabito : SelectShopBase
{

    [SerializeField] List<ItemMove_Heal> storeItems;//ショップのアイテムのリスト
    [SerializeField] ItemList itemList;
     
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //プレイヤーと接触したとき
        if (player!= null)
        {
            InsertItems(player.PreEnterIndex);
            items = storeItems;
            Debug.Log(items);
            Debug.Log("プレイヤーとお話し！");
            StartCoroutine(base.StartOption());//宿での会話イベントを始める
        }
    }

    void InsertItems(int index)
    {
        switch (index)
        {
            case 2:
                storeItems = itemList.TownSelledItems;
                break;
            case 11:
                storeItems = itemList.Town1SelledItems;
                break;
        }

    }


}


