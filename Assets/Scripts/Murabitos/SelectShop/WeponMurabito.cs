using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponMurabito : SelectShopBase
{

    [SerializeField] List<WeponBase> storeWepons;//�V���b�v�̕���̃��X�g
    [SerializeField] WeponList weponList;
   
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        //�v���C���[�ƐڐG�����Ƃ�
        if (player != null)
        {
            InsertWepons(player.PreEnterIndex);
            wepons = storeWepons;
            Debug.Log(wepons);
            Debug.Log("�v���C���[�Ƃ��b���I");
            StartCoroutine(base.StartOption());//�h�ł̉�b�C�x���g���n�߂�
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
