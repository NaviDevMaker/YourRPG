using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //[SerializeField] 
    ChurchMurabito churchMurabito;

    [SerializeField] BattleAudio battleAudio;

    Fade fade;

    [SerializeField] SceneObjectManager sceneObjectManager;//�{�X�̃��X�g���擾���邽��

    [SerializeField] PlayerController player;

    [SerializeField] BattleSystem battleSystem;//�o�g���V�X�e���̊Ǘ�

    [SerializeField] BelongingManager belongingManager;//�������n���Ǘ����Ă���X�N���v�g

    private  LayerMask bossLayerMask; //�{�X��̎��Ƀ{�X�̃��C���[���擾

    SpecialBossBattle specialBossBattle;
    
    [SerializeField] LayerSetting layerSetting;

    [SerializeField] SceneNameDatas sceneNameDatas;

    public static GameController Instance { get; private set; }

   
    private void Awake()//�V���O���g��
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            specialBossBattle = new SpecialBossBattle();
        }else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        fade = GameObject.FindObjectOfType<Fade>();
        specialBossBattle.InitBossInfo();
        SetEvents();
       
      
    }
    void StartBattle(Battler enemyBattler)//�G�l�~�[�̏����擾���A�o�g�����J�n
    {
        battleAudio.PlayBattleAudio();
        enemyBattler.Init(isEnemy:true);
        player.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(player.Battler,enemyBattler);
        belongingManager.SuspendMenu(true);
    }

    void StartBossBattle(Battler enemyBattler,LayerMask BosslayerMask,int bossIndex = 0)//�G�l�~�[�̏����擾���A�o�g�����J�n
    {
        Debug.Log("�o�g���J�n");
        if (bossIndex != 0)
        {
            if(SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[9])
            {
                specialBossBattle.SiblingBossBatlle.SiblingBossIndexs.Add(bossIndex);
                specialBossBattle.SiblingBossBatlle.SiblingBoss = enemyBattler.BossBase;
                Debug.Log("����");
            }else if(SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[11])
            {
                Debug.Log("��������");
                //���X�{�X�̏���
                StartCoroutine(StartSemiLastBossBattle(bossIndex,BosslayerMask,enemyBattler));
                return;
               

            }

        }
        //Debug.Log(siblingBossBatlle.SiblingBossIndexs[0]);
         BossBattle(BosslayerMask, enemyBattler);
        //bossLayerMask = BosslayerMask;
        //enemyBattler.Init();
        //player.gameObject.SetActive(false);
        //battleSystem.gameObject.SetActive(true);
        //battleSystem.BattleStart(player.Battler, enemyBattler);
        //belongingManager.SuspendMenu(true);
    }
    void EndBattle()//�o�g������UI���\���ɂ��A�����ɖ߂��A�S�[���h�̃A�b�v�f�[�g
    {
        battleAudio.StopBattleAudio();
        belongingManager.GoldUI.UpdateGoldUI();
        player.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
        belongingManager.SuspendMenu(false);

        if(battleSystem.IsDieidPlayer)
        {
            StartCoroutine(ToChurch());
            battleSystem.IsDieidPlayer = false;
        }
    }

    void WhetherDefeatBoss(bool isDefeat)//�o�g���I�����ɓ|�����G���{�X�L�������������ǂ����m�F����
    {
       
        int BossLayerIndex = bossLayerMask;//�{�X�Ɛ���Ă������i�G���J�E���g���Ƀ{�X���C���[�ɐڐG���Ă�����j�{�X�ł͂Ȃ�������O������
        int bosslayerIndex = -1;//����Ă��Ȃ�������-1
        Debug.Log(BossLayerIndex);
        //�{�X�������ǂ������f����
        if(BossLayerIndex != 0)
        {
          bosslayerIndex = layerSetting.SimbolLayers.IndexOf(bossLayerMask);
            
        }
        if(isDefeat == true)
        {
            
            string activeScene = SceneManager.GetActiveScene().name;
            int sceneIndex = sceneNameDatas.SceneNames.IndexOf(activeScene);
            if(sceneIndex == 9 || sceneIndex == 10 ||sceneIndex == 11)
            {
                specialBossBattle.ExecuteBossEvent(bosslayerIndex, sceneObjectManager, activeScene);
            }
            //else if(sceneIndex == 10)
            //{
            //    EmergeStair emergeStair = GameObject.FindObjectOfType<EmergeStair>();
            //    eme;
            //}
            else
            {
                sceneObjectManager.GridsBossDontDestroy[bosslayerIndex].gameObject.SetActive(false);
                sceneObjectManager.DetectDefeat[bosslayerIndex] = true;

            }

        }

      
        //Debug.Log(EndBattleAtSiblingBoss());
       
        //sceneObjectManager.GridsDontDestroy[0] = null;
        //�����łO�ɂ��Ȃ��ƃo�g���I�����ɂ��܂ł��{�X��|��������ɂȂ��Ă��܂�(�G���G��|���Ă��{�X��|��������ɂȂ��Ă��܂�)
        BossLayerIndex = 0;
        
    }

    private IEnumerator ToChurch()
    {
        if(specialBossBattle != null)
        {
            specialBossBattle.SiblingBossBatlle.SiblingBossIndexs.Clear();
            specialBossBattle.LastBoss.DefeatCounts.Clear();
        }
        
        yield return fade.FadeIn(2.0f);
        SceneManager.LoadScene(sceneNameDatas.SceneNames[4]);//"Church"
        player.Constraint = true;
        // �V�[�����[�h����������܂ő҂�
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Church");
        churchMurabito = FindObjectOfType<ChurchMurabito>();
        player.gameObject.transform.position = new Vector2(0, 0);
        //Debug.Log(churchMurabito);
        churchMurabito.Image.gameObject.SetActive(true);
        yield return churchMurabito.TypeDialog("�_��A�݂₽�l�ɂ��͂�", auto: false);
        player.Battler.HP = 1;
        player.Constraint = false;
        churchMurabito.Image.gameObject.SetActive(false);
        player.EnterIndex = 8;//�����O�ɏo�邽��

    }


    void BossBattle(LayerMask BosslayerMask,Battler enemyBattler)
    {
        Debug.Log("�ǂ��Ȃ�H");
        bossLayerMask = BosslayerMask;
        enemyBattler.Init(isEnemy:true);
        player.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(player.Battler, enemyBattler);
        belongingManager.SuspendMenu(true);
    }

    private IEnumerator StartSemiLastBossBattle(int bossIndex, LayerMask BosslayerMask, Battler enemyBattler)
    {
        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        // LastBossDialog����������܂őҋ@
        yield return StartCoroutine(specialBossBattle.LastBoss.LastBossDialog(bossIndex));

        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        Debug.Log("���s");
        // ������Ƀ{�X�o�g���J�n
        BossBattle(BosslayerMask, enemyBattler);
    }

    void LastBossBattle(Battler lastBossBattler)
    {
        bossLayerMask = layerSetting.SimbolLayers[6];
        BossBattle(layerSetting.SimbolLayers[6], lastBossBattler);
    }

    void SetEvents()
    {
        player.OnEncounts = StartBattle;//�G���J�E���g���ɍs����֐����Z�b�g
        player.OnEncountsBoss = StartBossBattle;

        battleSystem.OnBattleOver = EndBattle;//�o�g���I�����ɍs���֐����Z�b�g
        battleSystem.OnDefeatBoss = WhetherDefeatBoss;
        specialBossBattle.LastBoss.OnEmergeLastBoss = LastBossBattle;
        specialBossBattle.LastBoss.OnEndStory = EndStory;
    }
    IEnumerator EndStory()
    {
        player.Constraint = true;

        int bosslayerIndex = layerSetting.SimbolLayers.IndexOf(bossLayerMask);
        sceneObjectManager.GridsBossDontDestroy[bosslayerIndex].gameObject.SetActive(false);
        sceneObjectManager.DetectDefeat[bosslayerIndex] = true;
        Debug.Log("Story Clear");

        yield return new WaitForSeconds(2.0f);
        yield return fade.FadeIn(3.0f);
        if (fade.CutoutRange >= 1.0) fade.CutoutRange = 1.0f;
        SceneManager.LoadSceneAsync(sceneNameDatas.SceneNames[12]);
        yield return new WaitForSeconds(1.0f);
        yield return fade.FadeOut(1.0f);
    }
}
