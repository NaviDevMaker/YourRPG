using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingBossBattle: ISpecialBossHandler
{
    List<int> siblingBossIndexs = new List<int>();
    private BOSSBase siblingBoss;

    public BOSSBase SiblingBoss { get => siblingBoss; set => siblingBoss = value; }
    public List<int> SiblingBossIndexs { get => siblingBossIndexs; set => siblingBossIndexs = value; }

    public bool EndBattleAtSiblingBoss()
    {
        bool correctOrder = false;
        for (int i = 1; i <= siblingBossIndexs.Count; i++)
        {
            if (siblingBossIndexs[i - 1] == i)
            {
                correctOrder = true;
            }
            else
            {
                correctOrder = false;
            }
        }
        return correctOrder;
    }

    public IEnumerator CheckSiblingBoss(int Index,SceneObjectManager sceneObjectManager)
    {
        Transform bossEncountCanvas = BossEncount.Instance.gameObject.transform;
        if (siblingBossIndexs.Count > 0) //!EndBattleAtSiblingBoss()
        {
            Transform siblingBossTra = sceneObjectManager.GridsBossDontDestroy[Index].gameObject.transform;

            if (EndBattleAtSiblingBoss())
            {
                
                if (siblingBossIndexs.Count == 3)
                {
                    FloarToSecond_Casle floarTo = GameObject.FindObjectOfType<FloarToSecond_Casle>();
                    floarTo.OnCompletedTerm = true;
                    PlayerController.Instance.Constraint = true;
                    sceneObjectManager.DetectDefeat[Index] = true;
                    bossEncountCanvas.GetChild(0).gameObject.SetActive(true);
                    BossDialog bossDialog = GameObject.FindObjectOfType<BossDialog>();
                    yield return bossDialog.TypeDialog(siblingBoss.BossDialogContent[3], auto: false);
                    bossEncountCanvas.GetChild(0).gameObject.SetActive(false);
                    siblingBossTra.GetChild(2).gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    PlayerController.Instance.Constraint = false;

                }
                else
                {
                    siblingBossTra.GetChild(siblingBossIndexs.Count - 1).gameObject.SetActive(false);
                }
            }
            else
            {
                BossDialog bossDialog = siblingBossTra.GetChild(siblingBossIndexs[siblingBossIndexs.Count - 1] - 1).gameObject.GetComponent<BossDialog>();
                bossEncountCanvas.GetChild(0).gameObject.SetActive(true);
                yield return bossDialog.TypeDialog(siblingBoss.BossDialogContent[2], auto: false);
                bossEncountCanvas.GetChild(0).gameObject.SetActive(false);

                foreach (var siblingBossIndex in siblingBossIndexs)
                {
                    siblingBossTra.GetChild(siblingBossIndex - 1).gameObject.SetActive(true);
                }

                siblingBossIndexs.Clear();
            }

        }
    }

    public void HandleDefeat(int bosslayerIndex, SceneObjectManager sceneObjectManager)
    {
        if (SiblingBossIndexs.Count == 0)
        {

            sceneObjectManager.GridsBossDontDestroy[bosslayerIndex].gameObject.SetActive(false);
            sceneObjectManager.DetectDefeat[bosslayerIndex] = true;

        }
        else
        {
            CoroutineRunner.Instance.RunCoroutine(CheckSiblingBoss(bosslayerIndex, sceneObjectManager));
        }
    }

   
}
