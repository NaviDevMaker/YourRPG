using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NpcMurabitoBase : DialogBase
{
    protected PlayerController playerController;

    [SerializeField] GameObject image;//�_�C�A���O�̃C���[�W�̃I�u�W�F�N�g
    [SerializeField] LayerMask murabitoLayerMasks;
    //���l�̃X�N���v�^�u���I�u�W�F�N�g�̓������������X�g�Ɉڂ�
     List<int> walkX = new List<int>();
     List<int> walkY = new List<int>();
     List<int> storeDirectionCounts = new List<int>();//��������������ۑ�����
     List<Transform> murabitoPositions = new List<Transform>();//���l�̍ŏ��̏o���ꏊ��ݒ�
     List<bool> changeDirection= new List<bool>();//���тƂ��Ƃ̕����]���̊m�F
     List<Coroutine> WalkCoroutines = new List<Coroutine>();//�R���[�`�����i�[
     List<string> DialogContents = new List<string>();
     protected List<float> currentTimes = new List<float>();
    int getFromPlayer;
     bool confirmPlayerHit = false;//�v���C���[�̂�������m�F
     protected bool isDialogActive = false;//�_�C�A���O���A�N�e�B�u���A�p����Ŏg���̂�protected

    public virtual void Start()//�p����Ŏg������
    {
        //if (playerController.MurabitoEvent != null) playerController.MurabitoEvent = null;
        Debug.Log("�Q�b�g�I");
        playerController = GameObject.FindObjectOfType<PlayerController>();//GameObject.FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
        playerController.MurabitoEvent += ConfirmMurabitoEvent;
        //StartCoroutine(TypeDialog("���͂�", false));
        
    }
   
    void ConfirmMurabitoEvent(int murabitoLayerIndex)//���тƂƓ��������Ƃ��̃C�x���g
    {
        //StartCoroutine(TypeDialog("���͂�", auto: false));
        Debug.Log("���m");
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
            Debug.Log("�Y��");
            Text.text = null;
            
        }

        //if (isDialogActive)
        //{
        //    yield break; // ���łɃ_�C�A���O���A�N�e�B�u�Ȃ珈�����I��
        //}
        //�_�C�A���O�֌W�̃I�u�W�F�N�g�̊Ǘ�
        Debug.Log("48,�܂����");
        image.SetActive(true);
        playerController.Constraint = true;//�v���C���[�̓������~�߂�
        //Debug.Log("�A�N�e�B�u");
        //yield return StartCoroutine(TypeDialog(line, auto));
        yield return base.TypeDialog(line,auto);

        image.SetActive(false);
        isDialogActive = false;
        Debug.Log(isDialogActive);
        //Debug.Log("��A�N�e�B�u");

    }

    public virtual void  StartWalking(MurabitoInfo murabitoInfo)//�R���[�`�����~�߂鏈��������̂ŕK�v
    {
        int murabitoIndex = murabitoInfo.MurabitoIndex;
        Debug.Log(murabitoIndex);
        if (WalkCoroutines[murabitoIndex] != null)
        {
            Debug.Log("�_�C�A���O");
            StopCoroutine(WalkCoroutines[murabitoIndex]);
            //WalkCoroutines[murabitoIndex] = null;//�R���[�`�����~������ null �ɂ���
            

        }

        WalkCoroutines[murabitoIndex] = StartCoroutine(WalkMurabito(murabitoInfo));
        Debug.Log(WalkCoroutines[murabitoIndex]);
    }

    //���l�̕�������
    public  IEnumerator  WalkMurabito(MurabitoInfo murabitoInfo)
    {
        
        //���X�g�Ɋ��蓖�Ă�ׂɕK�v
        int murabitoIndex = murabitoInfo.MurabitoIndex;
        //�v���C���[�̂�������m�F
        if (confirmPlayerHit)//&& !isDialog�̏ꍇ����i��ł��܂����m��ŕ��͑��v
        {
            Debug.Log("���b");
            if (!isDialogActive)//���̏d���o�O���Ȃ�������
            {
                isDialogActive = true;
                yield return StartCoroutine(TypeDialog(DialogContents[getFromPlayer], auto: false));// murabitoInfo.MurabitoDialogContent
            }
            else yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            
           
            //murabitoInfo.CurrentTime = 0;
            Debug.Log(WalkCoroutines[murabitoIndex]);

            //Vector3 stopPos = new Vector3(murabitoPositions[murabitoIndex].position.x, murabitoPositions[murabitoIndex].position.y,0);
           
           

            //isDialogActive = false;//�e�X�g�p
            playerController.Constraint = false;
            confirmPlayerHit = false;
            //isDialogActive = false;
            //StopCoroutine(WalkCoroutines[murabitoIndex]);

            Debug.Log(WalkCoroutines[murabitoIndex]);



        }

        if (murabitoIndex == 1)
        {
            Debug.Log($"������{storeDirectionCounts[1]}");

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
            
            //���l�̕���������ς���
            if (storeDirectionCounts[murabitoIndex] == murabitoInfo.ChangeDirectionCount)
            {
                Debug.Log("�����ς��");
                changeDirection[murabitoIndex] = true;
                walkX[murabitoIndex] = -walkX[murabitoIndex];
                walkY[murabitoIndex] = -walkY[murabitoIndex];
                Debug.Log($"X��{walkX[murabitoIndex]}");
                Debug.Log($"Y��{walkY[murabitoIndex]}");

            }
            
            
           
            //���тƂ��������
            Vector3 newMurabitoPos = new Vector3(murabitoPositions[murabitoIndex].position.x + walkX[murabitoIndex], murabitoPositions[murabitoIndex].position.y + walkY[murabitoIndex], 0);
            while ((newMurabitoPos - murabitoPositions[murabitoIndex].position).sqrMagnitude >= Mathf.Epsilon)//���l����������������
            {
                murabitoPositions[murabitoIndex].position = Vector3.MoveTowards(murabitoPositions[murabitoIndex].position, newMurabitoPos, 5f * Time.deltaTime);

                yield return null;// �t���[�����Ƃ̍X�V��҂��߂ɕK�v
            }
            murabitoPositions[murabitoIndex].position = newMurabitoPos;

           
            //�����̃J�E���g�A���̕�����������{�A���̕�����������[
            if (storeDirectionCounts[murabitoIndex] != murabitoInfo.ChangeDirectionCount && !changeDirection[murabitoIndex]) storeDirectionCounts[murabitoIndex]++;//�����̃J�E���g
            else if (changeDirection[murabitoIndex])//�ŏ��Ƌt�����ɕ����Ă���Ƃ��̏���
            {

                    if(storeDirectionCounts[murabitoIndex] != 0) storeDirectionCounts[murabitoIndex]--;//���тƂ�������������

                    //���̕����Ɉ��̃J�E���g�������I�������
                    if (storeDirectionCounts[murabitoIndex] == 0)
                    {

                        Debug.Log($"������");
                        changeDirection[murabitoIndex] = false;
                        walkX[murabitoIndex] = -walkX[murabitoIndex];
                        walkY[murabitoIndex] = -walkY[murabitoIndex];
                        Debug.Log($"X��{walkX[murabitoIndex]}");
                        Debug.Log($"Y��{walkY[murabitoIndex]}");
                        if (WalkCoroutines[murabitoIndex] != null)
                        {
                            StopCoroutine(WalkCoroutines[murabitoIndex]);
                            //Debug.Log($"Stopped Walk Coroutine: {WalkCoroutines[murabitoIndex]}");




                            //murabitoinfo.walkcoroutine = null;  // �����ł� null �ɂ���
                        }
                    }
            }

           


    }







    //���l�̏������X�g�Ɉڂ�
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
        Debug.Log($"�J�E���g������{storeDirectionCounts.Count}{walkX.Count}{walkY.Count}{changeDirection.Count}");
        int childCount = parentTransform.childCount;

        // �q�I�u�W�F�N�g�����Ɏ擾����
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
