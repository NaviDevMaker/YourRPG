using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectManager : MonoBehaviour
{
    //�O���b�h�i�{�X�j�𒬂Ȃǂɓ��������\���ɂ���A���̏�������̃X�N���v�g�ŊǗ����邽��
    public static SceneObjectManager Instance { get; private set; }
    [SerializeField] Grid[] gridsBossDontDestroy; // �{�X��grid���\���ɂ��邽�߂Ƀ{�X�����X�g�ŏW�߂�K�v������
    //[SerializeField] GameObject BossEncount;

    [SerializeField] List<int> ExistBossScene;//
    [SerializeField] SceneNameDatas sceneNameDatas;
    private bool[] detectDefeat;//�V�[���J�ڎ��Ƀ{�X���|����Ă��邩�ǂ������m�F�A�ǂ̃{�X���|����Ă��邩�m�邽�߂Ƀ��X�g�ɂ��ĊǗ�
    public bool[] DetectDefeat { get => detectDefeat; set => detectDefeat = value; }
    public Grid[] GridsBossDontDestroy { get => gridsBossDontDestroy; }

    bool gridSceneActive = false;

    Dictionary<int, int> dictionaryMaps;
    
    //int nowScene = 0;
    private void Awake()
    {
        //DontDestroyOnLoad(BossEncount);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        dictionaryMaps = new Dictionary<int, int>
        {
            {1,0},//�ŏ��̑���
            {3,1},//���A
            {5,2},//��ڂ̑���
            {8,3},//��̃{�X�P
            {9,4},//��̃{�X2
            {10,5},//��̃{�X�R
            {11,6},//��̃{�X�S
        };
       


    }
    // Start is called before the first frame update
    void Start()
    {
        // ���X�g����̏ꍇ�Afalse ��ǉ����ď�����

        detectDefeat = new bool[gridsBossDontDestroy.Length];
        //detectDefeat[0] = false;
        SceneManager.sceneLoaded += OnSceneChanged;// �V�[�������[�h���ꂽ�Ƃ��ɑO�̃V�[���̃O���b�h���\�� // SceneManager.activeSceneChanged += OnSceneChanged;
        for (int i = 0; i < gridsBossDontDestroy.Length; i++)
        {
            detectDefeat[i] = false;
        
            if (i != 0) gridsBossDontDestroy[i].gameObject.SetActive(false);
        }
        //SceneManager.sceneLoaded += OnSceneLoaded; //�V�[�����߂��Ă����Ƃ��ɍēx�A�N�e�B�u��

    }




    private void OnDestroy()
    {
        // �C�x���g����폜
        SceneManager.sceneLoaded -= OnSceneChanged;// SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene newScene,LoadSceneMode mode )//Scene newScene
    {
        string newSceneName = newScene.name;
        //string oldSceneName = oldScene.name;//�h���璬�ɖ߂�Ƃ��Ƀ{�X�O���b�h���\���ɂ��邽�߂ɕK�v
        int newSceneIndex = sceneNameDatas.SceneNames.IndexOf(newSceneName);
        //int oldsceneIndex = sceneNameDatas.SceneNames.IndexOf(oldSceneName);//��ɓ���

        Debug.Log(newSceneIndex);
        if (dictionaryMaps.TryGetValue(newSceneIndex, out int bossIndex))
        {
            
            // �V�[���C���f�b�N�X�ɑΉ�����{�X�����݂���ꍇ�A���̃{�X�̃O���b�h��ݒ�
            ChangeBossGrid(bossIndex);
        }
       
        else if (!ConfirmBossExist(newSceneIndex))
        {
            StartCoroutine(BossGrid.Instance.ExecuteDisable(gridSceneActive));
            //gridSceneActive = true;
        }

       

    }

    bool ConfirmBossExist(int newSceneIndex)//�{�X���m��ł��Ȃ��V�[���Ń{�X�̃O���b�h���\���ɂ���
    {
        bool bossExist = false;
        foreach (var Boss in ExistBossScene)//�{�X������V�[�����ǂ���
        {

            if (Boss != newSceneIndex) bossExist = false;//�{�X�������������Ȃ��͂��̃V�[���ł̓{�X���\��
            else if (Boss == newSceneIndex) bossExist = true;//����Ȃ�O���b�h�̓A�N�e�B�u


        }

        return bossExist;
    }

    void ChangeBossGrid(int bossIndex)
    {
        gridSceneActive = true;
        //DontDestroyOnload�̌�ɌĂяo�����ƂŃA�N�e�B�u�A��A�N�e�B�u��ύX���邱�Ƃ��o����A�ŏ������\���ɂ���ƃA�^�b�`���O��邩��
        StartCoroutine(BossGrid.Instance.ExecuteDisable(gridSceneActive));//�{�X�O���b�h�̐e�̃L�����o�X
        for (int i = 0; i < gridsBossDontDestroy.Length; i++)
        {
            if (i == bossIndex) continue;//�Y������{�X�ȊO�͔�\���ɂ���
            gridsBossDontDestroy[i].gameObject.SetActive(false);
        }
        if (DetectDefeat[bossIndex] == true)//�{�X����������Ă������ǂ����̊m�F
        {
            if (gridsBossDontDestroy[bossIndex] != null)//���X�g��null����Ȃ�������
            {
                gridsBossDontDestroy[bossIndex].gameObject.SetActive(false);
            }
        }
        else
        {

            if (gridsBossDontDestroy[bossIndex] != null) gridsBossDontDestroy[bossIndex].gameObject.SetActive(true);

        }
        gridSceneActive = false;//�V�[�����ς��O�Ɏ��̃V�[�����炱�̃V�[���ɖ߂��ė���Ƃ��ɃA�N�e�B�u�����邽��

    }

   



}
