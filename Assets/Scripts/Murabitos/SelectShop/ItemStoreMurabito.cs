using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemStoreMurabito : SelectShopBase
{

    [SerializeField] List<ItemMove_Heal> storeItems;//�V���b�v�̃A�C�e���̃��X�g
    [SerializeField] ItemList itemList;
     
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //�v���C���[�ƐڐG�����Ƃ�
        if (player!= null)
        {
            InsertItems(player.PreEnterIndex);
            items = storeItems;
            Debug.Log(items);
            Debug.Log("�v���C���[�Ƃ��b���I");
            StartCoroutine(base.StartOption());//�h�ł̉�b�C�x���g���n�߂�
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


