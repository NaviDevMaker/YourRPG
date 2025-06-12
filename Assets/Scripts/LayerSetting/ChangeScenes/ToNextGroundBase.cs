using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextGroundBase : MonoBehaviour
{
    [SerializeField] ToNextGroundBase toNextGround;//��������V�[���J�ڗp�̃I�u�W�F�N�g
    [SerializeField] SceneNameDatas sceneNameDatas;//�V�[���̖��O���������f�[�^
    [SerializeField] Vector2 genePos;
    [SerializeField] int[] sceneIndex = new int[2];//��̃V�[���̃C���f�b�N�X
    [SerializeField] int[] enterIndex;//�v���C���[�̂ق��ɓn�����߂̃C���f�b�N�X

    EnterCasle enterCasle;
 
    PlayerController player = PlayerController.Instance;
    int openSceneChangeIndex = 0;
    int count = 0;

  
   

    public int[] SceneIndex { get => sceneIndex;}
    public ToNextGroundBase ToNextGround { get => toNextGround;}
    public Vector2 GenePos { get => genePos;}
    public int Count { get => count; set => count = value; }

    private void Start()
    {
        //count = 0;
        //openSceneChangeIndex = sceneIndex[1];
        Debug.Log($"�V�[���C���f�b�N�X��{openSceneChangeIndex}");
        
    }


    private void OnCollisionEnter2D(Collision2D collision)//�v���C���[���G�ꂽ���ɃV�[�����ړ�������֐�
    {
        
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {

            InLastStage();//���X�g�X�e�[�W���ǂ������m�F����
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[sceneIndex[1]])
            {
                player.EnterIndex = enterIndex[0];//���̑J�ڂ��ׂ��V�[����enterIndex�������(Ground�ɖ߂�Ȃ�0)
                openSceneChangeIndex = sceneIndex[0];
            }
            else if (SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[sceneIndex[0]])
            {
                player.EnterIndex = enterIndex[1];//���̑J�ڂ��ׂ��V�[����enterIndex�������
                Debug.Log($"{player.EnterIndex}");
                openSceneChangeIndex = sceneIndex[1];
            }
            

            Debug.Log("���̂Ƃ���ցI");
           

            StartCoroutine(StartSceneChange());
            //player.Constraint = true;
            //ChangeSceneBase tmpChangeSceneBase = player.OpenSceneBases[openSceneChangeIndex];
            //StartCoroutine(tmpChangeSceneBase.ChangeScene(tmpChangeSceneBase));

            Debug.Log(openSceneChangeIndex);
        }
    }

   IEnumerator StartSceneChange()//�V�[����ς���
    {
        
        player.transform.position = this.gameObject.transform.position;
        player.Constraint = true;
        if(enterCasle != null)
        {
            Debug.Log("�邾��");
            yield return enterCasle.TypeDialog(enterCasle.DialogContent, auto:false);

        }
        ChangeSceneBase tmpChangeSceneBase = player.OpenSceneBases[openSceneChangeIndex];
        yield return tmpChangeSceneBase.ChangeScene(tmpChangeSceneBase);
        player.Constraint = false;
    }

    void InLastStage()
    {
        if(sceneNameDatas.SceneNames[sceneIndex[0]] == "LastStage") enterCasle = GameObject.FindObjectOfType<EnterCasle>();
    }

}
