using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnMurabitoAtTown : InnMurabitoBase
{
    bool hasStartedCoroutine = false;//�ēx�h�̑��l�ɐG���܂ŉ�b�̃_�C�A���O���o���Ȃ�����


    public static InnMurabitoAtTown Instance { get; private set; }//Text���V�[���J�ڌ��null�ɂȂ邱�Ƃ�h������

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�ƐڐG�����Ƃ�
        if (collision.gameObject == PlayerController.Instance.gameObject && !hasStartedCoroutine)
        {
            Debug.Log("�v���C���[�Ƃ��b���I");
            StartCoroutine(base.StartOption());//�h�ł̉�b�C�x���g���n�߂�
            //StartCoroutine(StartInnAction());            
            //OptionDialog();
            //SelectedOption();
            hasStartedCoroutine = true;//��b�C�x���g���n�߂�A���ꂪ�Ȃ���

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            hasStartedCoroutine = false;
        }
    }

    public override IEnumerator StartInnAction()
    {
        yield return base.StartInnAction();
    }
}
