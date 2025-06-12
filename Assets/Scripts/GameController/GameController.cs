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

    [SerializeField] SceneObjectManager sceneObjectManager;//ボスのリストを取得するため

    [SerializeField] PlayerController player;

    [SerializeField] BattleSystem battleSystem;//バトルシステムの管理

    [SerializeField] BelongingManager belongingManager;//持ち物系を管理しているスクリプト

    private  LayerMask bossLayerMask; //ボス戦の時にボスのレイヤーを取得

    SpecialBossBattle specialBossBattle;
    
    [SerializeField] LayerSetting layerSetting;

    [SerializeField] SceneNameDatas sceneNameDatas;

    public static GameController Instance { get; private set; }

   
    private void Awake()//シングルトン
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
    void StartBattle(Battler enemyBattler)//エネミーの情報を取得し、バトルを開始
    {
        battleAudio.PlayBattleAudio();
        enemyBattler.Init(isEnemy:true);
        player.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        battleSystem.BattleStart(player.Battler,enemyBattler);
        belongingManager.SuspendMenu(true);
    }

    void StartBossBattle(Battler enemyBattler,LayerMask BosslayerMask,int bossIndex = 0)//エネミーの情報を取得し、バトルを開始
    {
        Debug.Log("バトル開始");
        if (bossIndex != 0)
        {
            if(SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[9])
            {
                specialBossBattle.SiblingBossBatlle.SiblingBossIndexs.Add(bossIndex);
                specialBossBattle.SiblingBossBatlle.SiblingBoss = enemyBattler.BossBase;
                Debug.Log("成功");
            }else if(SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[11])
            {
                Debug.Log("成功だよ");
                //ラスボスの処理
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
    void EndBattle()//バトル中のUIを非表示にし、草原に戻す、ゴールドのアップデート
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

    void WhetherDefeatBoss(bool isDefeat)//バトル終了時に倒した敵がボスキャラだったかどうか確認する
    {
       
        int BossLayerIndex = bossLayerMask;//ボスと戦っていたか（エンカウント時にボスレイヤーに接触していたら）ボスではなかったら０が入る
        int bosslayerIndex = -1;//戦っていなかったら-1
        Debug.Log(BossLayerIndex);
        //ボスだったどうか判断する
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
        //ここで０にしないとバトル終了時にいつまでもボスを倒した判定になってしまう(雑魚敵を倒してもボスを倒した判定になってしまう)
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
        // シーンロードが完了するまで待つ
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Church");
        churchMurabito = FindObjectOfType<ChurchMurabito>();
        player.gameObject.transform.position = new Vector2(0, 0);
        //Debug.Log(churchMurabito);
        churchMurabito.Image.gameObject.SetActive(true);
        yield return churchMurabito.TypeDialog("神よ、みやた様にお力を", auto: false);
        player.Battler.HP = 1;
        player.Constraint = false;
        churchMurabito.Image.gameObject.SetActive(false);
        player.EnterIndex = 8;//教会から外に出るため

    }


    void BossBattle(LayerMask BosslayerMask,Battler enemyBattler)
    {
        Debug.Log("どうなる？");
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
        // LastBossDialogが完了するまで待機
        yield return StartCoroutine(specialBossBattle.LastBoss.LastBossDialog(bossIndex));

        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        Debug.Log("失敗");
        // 完了後にボスバトル開始
        BossBattle(BosslayerMask, enemyBattler);
    }

    void LastBossBattle(Battler lastBossBattler)
    {
        bossLayerMask = layerSetting.SimbolLayers[6];
        BossBattle(layerSetting.SimbolLayers[6], lastBossBattler);
    }

    void SetEvents()
    {
        player.OnEncounts = StartBattle;//エンカウント時に行われる関数をセット
        player.OnEncountsBoss = StartBossBattle;

        battleSystem.OnBattleOver = EndBattle;//バトル終了時に行う関数をセット
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
