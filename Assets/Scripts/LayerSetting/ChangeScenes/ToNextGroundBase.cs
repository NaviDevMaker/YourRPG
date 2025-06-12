using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextGroundBase : MonoBehaviour
{
    [SerializeField] ToNextGroundBase toNextGround;//生成するシーン遷移用のオブジェクト
    [SerializeField] SceneNameDatas sceneNameDatas;//シーンの名前が入ったデータ
    [SerializeField] Vector2 genePos;
    [SerializeField] int[] sceneIndex = new int[2];//二つのシーンのインデックス
    [SerializeField] int[] enterIndex;//プレイヤーのほうに渡すためのインデックス

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
        Debug.Log($"シーンインデックスは{openSceneChangeIndex}");
        
    }


    private void OnCollisionEnter2D(Collision2D collision)//プレイヤーが触れた時にシーンを移動させる関数
    {
        
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {

            InLastStage();//ラストステージかどうかを確認する
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[sceneIndex[1]])
            {
                player.EnterIndex = enterIndex[0];//次の遷移すべきシーンのenterIndexをいれる(Groundに戻るなら0)
                openSceneChangeIndex = sceneIndex[0];
            }
            else if (SceneManager.GetActiveScene().name == sceneNameDatas.SceneNames[sceneIndex[0]])
            {
                player.EnterIndex = enterIndex[1];//次の遷移すべきシーンのenterIndexをいれる
                Debug.Log($"{player.EnterIndex}");
                openSceneChangeIndex = sceneIndex[1];
            }
            

            Debug.Log("次のところへ！");
           

            StartCoroutine(StartSceneChange());
            //player.Constraint = true;
            //ChangeSceneBase tmpChangeSceneBase = player.OpenSceneBases[openSceneChangeIndex];
            //StartCoroutine(tmpChangeSceneBase.ChangeScene(tmpChangeSceneBase));

            Debug.Log(openSceneChangeIndex);
        }
    }

   IEnumerator StartSceneChange()//シーンを変える
    {
        
        player.transform.position = this.gameObject.transform.position;
        player.Constraint = true;
        if(enterCasle != null)
        {
            Debug.Log("城だよ");
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
