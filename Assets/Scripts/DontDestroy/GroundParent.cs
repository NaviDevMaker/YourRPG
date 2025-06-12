using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundParent : MonoBehaviour
{       
    //�o�����������C���[�̌Ăяo�����߂̐e�I�u�W�F�N�g
    public static GroundParent Instance {get;private set;}

    [SerializeField] List<ToNextGroundBase> toNextGroundBases;//�V�[���J�ڗp�̃v���t�@�u���̏�񂪓���������
    [SerializeField] ToNextGroundBase testObj;//�e�X�g�p�A�ŏ��ɍs�����̂Ƃ�
    [SerializeField] SceneNameDatas sceneNameDatas;//�V�[���̖��O���������f�[�^
    List<ToNextGroundBase> generatedGameObjects = new List<ToNextGroundBase>();

    //int count = 0;
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

    private void Start()
    {
        foreach (var Base in toNextGroundBases)
        {
            Base.Count = 0;
        }
        Debug.Log($"�ŏ��̃J�E���g��{toNextGroundBases[2].Count}");
        //testObj = Instantiate(testObj,this.transform);//�e�X�g����
        //PlayerController.Instance.EnterIndex = 14;
        //testObj.transform.localPosition = Vector3.zero;  // ���[�J���|�W�V���������Z�b�g
        SceneManager.sceneLoaded += ExecuteMethod;
    }

    void ExecuteMethod(Scene scene,LoadSceneMode mode)//�V�[���J�ڎ��̏��������s�A��̓I�ɂ̓v���t�@�u�̐���
    {
        
      
        //int foreachCount = 0;
        //Destroy(testObj);
        foreach (var toNextGroundBase in toNextGroundBases)
        {
            Debug.Log($"{toNextGroundBase.ToNextGround.name}(Clone)");

            //�J�ڂ���V�[���̖��O������̃V�[���J�ڗp�̃I�u�W�F�N�g�������Ȃ����̏ꍇ
            if (scene.name == sceneNameDatas.SceneNames[toNextGroundBase.SceneIndex[0]] || scene.name == sceneNameDatas.SceneNames[toNextGroundBase.SceneIndex[1]])
            {

                Debug.Log($"�J�E���g��{toNextGroundBase.Count}");
                if(toNextGroundBase.Count == 0)
                {
                    Debug.Log("����");
                    ToNextGroundBase sceneChangeObj = Instantiate(toNextGroundBase.ToNextGround, this.gameObject.transform);//this.gameObject.transform
                    Debug.Log(sceneChangeObj.name);
                    generatedGameObjects.Add(sceneChangeObj);
                    Debug.Log(generatedGameObjects[0].name);
                    sceneChangeObj.transform.position = toNextGroundBase.GenePos;
                   
                   
                    //sceneChangeObj.transform.localPosition = Vector3.zero;//PlayerController.Instance.gameObject.transform
                    toNextGroundBase.Count = 1;//�J�E���g���P�ɂ��Ȃ��Ƃ܂���������Ă��܂�
                    Debug.Log(toNextGroundBase.Count);
                    
                }
                    
            }//�����ׂ��V�[���̏ꍇ
            else
            {
                Debug.Log("����");
                //generatedGameObjects.Add(null);
                // �t���Ń��[�v����
                for (int i = generatedGameObjects.Count - 1; i >= 0; i--)
                {
                    var obj = generatedGameObjects[i];
                    if (obj != null && obj.gameObject.name == $"{toNextGroundBase.ToNextGround.name}(Clone)")
                    {
                        Destroy(obj.gameObject);
                        generatedGameObjects.RemoveAt(i);  // �w�肵���C���f�b�N�X�̗v�f���폜
                        toNextGroundBase.Count = 0;
                    }
                }
                //foreach (var obj in generatedGameObjects)
                //{
                //    if (obj != null && obj.gameObject.name == $"{toNextGroundBase.ToNextGround.name}(Clone)")
                //    {
                //        Destroy(obj.gameObject);
                //        generatedGameObjects.Remove(obj);
                //        toNextGroundBase.Count = 0;
                //    }
                //}
                //generatedGameObjects.Clear();
                //generatedGameObjects.Add(null);
                ////Debug.Log(sceneChangeObj);

                //for (int i = 0; i < generatedGameObjects.Count - 1; i++)
                //{
                //    Debug.Log(generatedGameObjects[i].name);
                //    //generatedGameObjects[i] == generatedGameObjects[foreachCount - 1]
                //    if (generatedGameObjects[i].name == $"{toNextGroundBase.ToNextGround.name}(Clone)")
                //    {
                //        Debug.Log("����");
                //        Destroy(generatedGameObjects[i]);
                //        generatedGameObjects.Remove(generatedGameObjects[i]);
                //        toNextGroundBase.Count = 0;
                //    }
                //}






            }
            //toNextGroundBase.OnSceneChanged(scene,mode);
            //Debug.Log(foreachCount);
        }

        Debug.Log($"�J�E���g��{generatedGameObjects.Count}");




    }

}
