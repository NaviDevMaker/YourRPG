using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundParent : MonoBehaviour
{       
    //出口が同じレイヤーの呼び出すための親オブジェクト
    public static GroundParent Instance {get;private set;}

    [SerializeField] List<ToNextGroundBase> toNextGroundBases;//シーン遷移用のプレファブ等の情報が入ったもの
    [SerializeField] ToNextGroundBase testObj;//テスト用、最初に行く次のとこ
    [SerializeField] SceneNameDatas sceneNameDatas;//シーンの名前が入ったデータ
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
        Debug.Log($"最初のカウントは{toNextGroundBases[2].Count}");
        //testObj = Instantiate(testObj,this.transform);//テストだよ
        //PlayerController.Instance.EnterIndex = 14;
        //testObj.transform.localPosition = Vector3.zero;  // ローカルポジションをリセット
        SceneManager.sceneLoaded += ExecuteMethod;
    }

    void ExecuteMethod(Scene scene,LoadSceneMode mode)//シーン遷移時の処理を実行、具体的にはプレファブの生成
    {
        
      
        //int foreachCount = 0;
        //Destroy(testObj);
        foreach (var toNextGroundBase in toNextGroundBases)
        {
            Debug.Log($"{toNextGroundBase.ToNextGround.name}(Clone)");

            //遷移するシーンの名前が特定のシーン遷移用のオブジェクトを消さない所の場合
            if (scene.name == sceneNameDatas.SceneNames[toNextGroundBase.SceneIndex[0]] || scene.name == sceneNameDatas.SceneNames[toNextGroundBase.SceneIndex[1]])
            {

                Debug.Log($"カウントは{toNextGroundBase.Count}");
                if(toNextGroundBase.Count == 0)
                {
                    Debug.Log("だめ");
                    ToNextGroundBase sceneChangeObj = Instantiate(toNextGroundBase.ToNextGround, this.gameObject.transform);//this.gameObject.transform
                    Debug.Log(sceneChangeObj.name);
                    generatedGameObjects.Add(sceneChangeObj);
                    Debug.Log(generatedGameObjects[0].name);
                    sceneChangeObj.transform.position = toNextGroundBase.GenePos;
                   
                   
                    //sceneChangeObj.transform.localPosition = Vector3.zero;//PlayerController.Instance.gameObject.transform
                    toNextGroundBase.Count = 1;//カウントを１にしないとまた生成されてしまう
                    Debug.Log(toNextGroundBase.Count);
                    
                }
                    
            }//消すべきシーンの場合
            else
            {
                Debug.Log("消す");
                //generatedGameObjects.Add(null);
                // 逆順でループする
                for (int i = generatedGameObjects.Count - 1; i >= 0; i--)
                {
                    var obj = generatedGameObjects[i];
                    if (obj != null && obj.gameObject.name == $"{toNextGroundBase.ToNextGround.name}(Clone)")
                    {
                        Destroy(obj.gameObject);
                        generatedGameObjects.RemoveAt(i);  // 指定したインデックスの要素を削除
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
                //        Debug.Log("成功");
                //        Destroy(generatedGameObjects[i]);
                //        generatedGameObjects.Remove(generatedGameObjects[i]);
                //        toNextGroundBase.Count = 0;
                //    }
                //}






            }
            //toNextGroundBase.OnSceneChanged(scene,mode);
            //Debug.Log(foreachCount);
        }

        Debug.Log($"カウントは{generatedGameObjects.Count}");




    }

}
