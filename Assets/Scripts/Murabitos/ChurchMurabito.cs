using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChurchMurabito : DialogBase
{
    
    Image image;
    bool isActive = false;//�_�C�A���O�̃C�x���g���A�N�e�B�u���ǂ���

    public Image Image { get => image;}

    private void Start()
    {
        //�V�[���ǂݍ��ݎ��ɃR���|�[�l���g�̎擾
        image = GameObject.Find("ChurchImage").GetComponent<Image>();
        image.gameObject.SetActive(false);
        Debug.Log(image);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ڐG�����I�u�W�F�N�g���v���C���[��������
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            if (!isActive)
            {
                StartCoroutine(StartChurchEvent());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //�v���C���[����x�R���C�_�[�͈̔͂��甲����܂ŃC�x���g���J�n�ł��Ȃ��悤�ɂ���
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            isActive = false;
        }
    }

    private IEnumerator StartChurchEvent()//����̐l���b���C�x���g
    {
        Debug.Log(image);
        image.gameObject.SetActive(true);
        Debug.Log("����");
        PlayerController.Instance.Constraint = true;
        yield return StartCoroutine(base.TypeDialog("�_�̂����삪����񂱂Ƃ�", auto: false));
        image.gameObject.SetActive(false);
        PlayerController.Instance.Constraint = false;
        isActive = true;
    }
}
