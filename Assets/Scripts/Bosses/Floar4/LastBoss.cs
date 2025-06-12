using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System;
public class LastBoss : ISpecialBossHandler
{
    BossDOTweenAction bossDOTweenAction;
    BossSituationDialog bossSituationDialog;
    SimpleSituationDialog storyClearDialog;

    Transform lastBossTra;
    List<BOSSBase> BossBases = new List<BOSSBase>();

    BossDialog bossDialog;

    List<bool> defeatCounts = new List<bool>();
    public bool IsDefeatedLastBoss { get; private set; } = false;
    public List<bool> DefeatCounts { get => defeatCounts; }

    int tmpChildIndex = 0;

    public UnityAction<Battler> OnEmergeLastBoss;
    public Func<IEnumerator> OnEndStory;
   
    public IEnumerator LastBossDialog(int childIndex)
    {
        tmpChildIndex = childIndex - 1;

        lastBossTra = GameObject.FindWithTag("CasleFloar4").transform;

        //nt childCount = lastBossTra.childCount;

        GetBossBase();

        Debug.Log(BossBases.Count);
      

        bossDialog = lastBossTra.GetChild(tmpChildIndex).gameObject.GetComponent<BossDialog>();

        if (bossDialog == null)
        {
            Debug.LogError("BossDialog component not found on the specified child object.");
        }
        if (defeatCounts.Count == 1)
        {
            yield return bossDialog.TypeDialog(BossBases[1].BossDialogContent[2], auto: false);//Ç‹ÇæèIÇÌÇÁÇÒÇº!
        }
        else
        {
            Debug.Log("åãâ ÇÕ");
            yield return bossDialog.TypeDialog(BossBases[0].BossDialogContent[0], auto: false);//äyÇµÇ‹ÇπÇƒÇ‡ÇÁÇ§ÇºÅAóEé“!! //1
        }
       
        //defeatCounts.Add(true);

    }

    public void HandleDefeat(int bosslayerIndex, SceneObjectManager sceneObjectManager)
    {
        if (BossEncount.Instance == null)
        {
            Debug.LogError("BossEncount.Instance is null. Ensure the BossEncount object is initialized and present in the scene.");
            return;
        }

        
        Debug.Log("ÉeÉXÉg");
        int index = 0;
        if (defeatCounts.Count == 1)
        {
            index = 3;
            CoroutineRunner.Instance.RunCoroutine(Boss4Dialog(index));
           
        }else if(defeatCounts.Count == 2)
        {
            index = 4;
            CoroutineRunner.Instance.RunCoroutine(Boss4Dialog(index));
        }
        else if(defeatCounts.Count == 3)
        {
            CoroutineRunner.Instance.RunCoroutine(LastBossDialog(isDefeated:true));

        }

    }
    IEnumerator Boss4Dialog(int dialogIndex)//ÇRÇ…ì¡Ç…à”ñ°ÇÕÇ»Ç¢
    {
        if(bossDOTweenAction == null) bossDOTweenAction = GameObject.FindObjectOfType<BossDOTweenAction>();
        if (bossSituationDialog == null) bossSituationDialog = GameObject.FindObjectOfType<BossSituationDialog>();


        int count = BossBases.Count - 1;
        Debug.Log(count);
        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        PlayerController.Instance.Constraint = true;
        yield return bossDialog.TypeDialog(BossBases[count].BossDialogContent[dialogIndex], auto: false);//ÇÆÇÌÇüÇüÇüÇüÇü...ÇÕÇ¡ÇÕÇ¡ÇÕÇ¡
        BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        lastBossTra.GetChild(tmpChildIndex).gameObject.SetActive(false);
        
        yield return bossDOTweenAction.BossIntoOrb(count);
        yield return bossSituationDialog.SituationTypeDialog(count);

        PlayerController.Instance.Constraint = false;
        if (dialogIndex == 4)
        {
            tmpChildIndex = 2;
            Debug.Log("óhÇÍÇÈÇÊ");
            GetBossBase();
            Transform bossTra = lastBossTra.transform.GetChild(tmpChildIndex).transform;
            SummonLastBoss(bossTra).Forget();

        }

    }
    async UniTaskVoid SummonLastBoss(Transform bossTra)
    {
      
        PlayerController.Instance.Constraint = true;
        bossDOTweenAction.PreLastBossVibrato(3.0f,1.0f,150, 100.0f).Forget();// strengh 2.0f,vibrato 100 randomness 180
                                                                                     
        await bossDOTweenAction.FadeImage();
 
        await bossDOTweenAction.LastBossVibrato(bossTra,1.0f,0.5f,50,100.0f);

        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));
        //PlayerController.Instance.Constraint = false;
        CoroutineRunner.Instance.RunCoroutine(LastBossDialog(isDefeated: false)) ;


        Debug.Log("ê¨å˜");
       
    }

    void GetBossBase()
    {
        EncountBOSS encountBoss = lastBossTra.GetChild(tmpChildIndex).gameObject.GetComponent<EncountBOSS>();
        BOSSBase BossBase = encountBoss.BossBattler.BossBase;
        BossBases.Add(BossBase);
    }

    IEnumerator LastBossDialog(bool isDefeated)
    {
        bossDialog = lastBossTra.GetChild(tmpChildIndex).gameObject.GetComponent<BossDialog>();
        if (!isDefeated)
        {           
            EncountBOSS encountBOSS = lastBossTra.GetChild(tmpChildIndex).GetComponent<EncountBOSS>();
            Battler battler = encountBOSS.BossBattler;

            BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                yield return bossDialog.TypeDialog(BossBases[tmpChildIndex].BossDialogContent[i], auto: false);
            }
           

            BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            OnEmergeLastBoss?.Invoke(battler);
        }
        else if(isDefeated)
        {
            storyClearDialog = GameObject.FindObjectOfType<SimpleSituationDialog>();
            BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            for (int i = 3; i < 6; i++)
            {
                yield return bossDialog.TypeDialog(BossBases[tmpChildIndex].BossDialogContent[i], auto: false);
            }

            BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            IsDefeatedLastBoss = true;
            PlayerController.Instance.Constraint = true;
            yield return storyClearDialog.OutPutDialog();
            CoroutineRunner.Instance.RunCoroutine(OnEndStory?.Invoke());
        }
       
    }

   
}
