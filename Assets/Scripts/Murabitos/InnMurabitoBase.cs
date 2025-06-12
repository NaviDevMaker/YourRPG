using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//���̑��ŕʂ̑��l��������s�����߂Ɋg�������l���ăx�[�X��
public class InnMurabitoBase : DialogBase
{

    int selectedIndex = 0;//�I������Ă���I���ƃX�y�[�X�������ꂽ�Ƃ��̑I������v�����邽��
    SelectableText[] Responce = new SelectableText[2];//�͂��A�������̃j��
    Image optionImage;//�I�v�V�����̃C���[�W
    Image dialogImage;//�_�C�A���O�̃C���[�W
    private Fade fade;//�t�F�C�h�̃X�N���v�g

    public UnityAction OnSelectedYes;
    private void Start()
    {
       
        //�V�[���ύX���Ɏ��s����
        SceneManager.activeSceneChanged += OnSceneChanged;
        //�h����o��ƃA�^�b�`����Ă���I�u�W�F�N�g�͑��݂��Ȃ��Ȃ�B���̂Ƃ��C���X�y�N�^�[��ł̃A�^�b�`���ƊO��Ă��܂�����B
        fade = GameObject.FindObjectOfType<Fade>().GetComponent<Fade>();
        if (fade != null) Debug.Log("�Ȃ��");
        //�͂��A�������̑I�����̃I�u�W�F�N�g�ɃA�^�b�`����Ă���X�N���v�g�̎擾�����邽��
        Transform innDiaTrans = InnDiaLogCanvas.Instance.gameObject.transform;
        Transform child = innDiaTrans.GetChild(1);
        for (int i = 0; i < Responce.Length; i++)
        {                    
            Responce[i] = child.GetChild(i).GetComponent<SelectableText>();

        }
        optionImage = InnDiaLogCanvas.Instance.gameObject.transform.Find("ResponceImage").GetComponent<Image>();
        dialogImage = InnDiaLogCanvas.Instance.gameObject.transform.Find("innDialogImage").GetComponent<Image>();       

    }
    //�h�̃_�C�A���O�̈�A�̗���
    enum InnState
    {
        Idle,
        OptionAction,
        OptionEnd,
    }

    InnState innState;


    private void Update()
    {
        switch (innState)
        {
            case InnState.Idle://�����n�܂��Ă��Ȃ����
                break;
            case InnState.OptionAction:
                if (!isRunning) // �R���[�`�������s���łȂ��ꍇ�ɂ̂ݎ��s
                {
                    StartCoroutine(StartInnAction());
                }
                break;
            case InnState.OptionEnd:
                OptionEnd();
                break;
            

        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("��");
        }

    }

    bool isRunning = false; // �R���[�`�������s�����ǂ����𔻒肷�邽�߂̃t���O
    public virtual IEnumerator StartOption()//�h�ł̑I���̊J�n
    {

        PlayerController.Instance.Constraint = true;
        Debug.Log(PlayerController.Instance.Constraint);
        dialogImage.gameObject.SetActive(true);
        yield return OutputDialog("���x�݂ɂȂ��܂����H");
        Text.text = "���x�݂ɂȂ��܂����H";
        innState = InnState.OptionAction;


    }
    public virtual IEnumerator OutputDialog(string DialogContent)//�_�C�A���O�̏o��
    {
        yield return base.TypeDialog(DialogContent, auto:false) ;
    }

    public void OptionDialog()//�_�C�A���O�̑I�����̑I������Ă���e�L�X�g�̐F�̐ݒ�
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))//�^���I�ɑI�����Ă���悤�Ɍ�����
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, Responce.Length - 1);//�l�͈̔͂�ݒ�
        for (int i = 0; i < Responce.Length; i++)
        {
            if (selectedIndex == i)
            {
                Responce[i].SetSelectedColor(true);
            }
            else
            {
                Responce[i].SetSelectedColor(false);
            }
        }

    }

    public virtual IEnumerator StartInnAction()//�I�����ɌĂ΂��
    {
        selectedIndex = 0;//�O��̃e�L�X�g�̑I�����͂���
        optionImage.gameObject.SetActive(true);
        isRunning = true; // �R���[�`�����J�n�����̂Ńt���O�𗧂Ă�
        bool optionSelected = false; 
        //�I�������܂ŌJ��Ԃ�
        while (!optionSelected)
        {
            //�I��
            OptionDialog();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                optionSelected = true;
              

                if (selectedIndex == 0)
                {
                    PlayerController.Instance.Battler.HP = PlayerController.Instance.Battler.MaxHP;
                    Debug.Log("�I������I");
                    yield return StartCoroutine(ExecuteInnFade());//�h�ɔ��܂��Ă���Ƃ��̉��o �AStopCoroutine���g�����߂ɂ����Ă��̌Ăѕ������Ă���iFade���Łj                 
                    optionImage.gameObject.SetActive(false);
                    dialogImage.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1.0f);//�����ɉ��̃_�C�A���O���n�܂�̂�h������
                    dialogImage.gameObject.SetActive(true);
                    yield return OutputDialog("���������F���Ă��܂��A�݂₽�l");
                    dialogImage.gameObject.SetActive(false);

                }
                else if (selectedIndex == 1)
                {
                    optionImage.gameObject.SetActive(false);
                    dialogImage.gameObject.SetActive(false);
                }
            }
            yield return null; // �t���[�����ƂɃ��[�v���p��
        }
        
        innState = InnState.OptionEnd;//�I���̏I��
        isRunning = false; // �R���[�`�����I�������̂Ńt���O�����Z�b�g


    }

    void OptionEnd()
    {
        PlayerController.Instance.Constraint = false;
        innState = InnState.Idle;//������idel�ɂ��ǂ��Ȃ���Constraint��������false�̂܂܂ɂȂ�V�[���J�ڒ��ɓ���

    }

    IEnumerator ExecuteInnFade()
    {
        fade.FadeIn(0.5f);
        OnSelectedYes?.Invoke();
        yield return new WaitForSeconds(4.0f);
        fade.FadeOut(0.5f);

    }

   void OnSceneChanged(Scene oldScene,Scene newScene)
   {
        //���тƂɃA�^�b�`�������̂�ۑ����邽�߂ɂ��̏����B���l�̌����ڂ��ꏊ�ɂ���Ă��ς��ꍇ���̏����͕ύX����i�\��j
        if (newScene.name != "Inn")
        {
            this.gameObject.SetActive(false);
        }
        else if (newScene.name == "Inn")
        {
            this.gameObject.SetActive(true);
        }
    }

    


}
