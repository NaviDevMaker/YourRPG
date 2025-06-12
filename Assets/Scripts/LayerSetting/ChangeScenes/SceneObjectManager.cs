using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectManager : MonoBehaviour
{
    //グリッド（ボス）を町などに入ったら非表示にする、その処理を一つのスクリプトで管理するため
    public static SceneObjectManager Instance { get; private set; }
    [SerializeField] Grid[] gridsBossDontDestroy; // ボスのgridを非表示にするためにボスをリストで集める必要がある
    //[SerializeField] GameObject BossEncount;

    [SerializeField] List<int> ExistBossScene;//
    [SerializeField] SceneNameDatas sceneNameDatas;
    private bool[] detectDefeat;//シーン遷移時にボスが倒されているかどうかを確認、どのボスが倒されているか知るためにリストにして管理
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
            {1,0},//最初の草原
            {3,1},//洞窟
            {5,2},//二つ目の草原
            {8,3},//城のボス１
            {9,4},//城のボス2
            {10,5},//城のボス３
            {11,6},//城のボス４
        };
       


    }
    // Start is called before the first frame update
    void Start()
    {
        // リストが空の場合、false を追加して初期化

        detectDefeat = new bool[gridsBossDontDestroy.Length];
        //detectDefeat[0] = false;
        SceneManager.sceneLoaded += OnSceneChanged;// シーンがロードされたときに前のシーンのグリッドを非表示 // SceneManager.activeSceneChanged += OnSceneChanged;
        for (int i = 0; i < gridsBossDontDestroy.Length; i++)
        {
            detectDefeat[i] = false;
        
            if (i != 0) gridsBossDontDestroy[i].gameObject.SetActive(false);
        }
        //SceneManager.sceneLoaded += OnSceneLoaded; //シーンが戻ってきたときに再度アクティブ化

    }




    private void OnDestroy()
    {
        // イベントから削除
        SceneManager.sceneLoaded -= OnSceneChanged;// SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene newScene,LoadSceneMode mode )//Scene newScene
    {
        string newSceneName = newScene.name;
        //string oldSceneName = oldScene.name;//宿から町に戻るときにボスグリッドを非表示にするために必要
        int newSceneIndex = sceneNameDatas.SceneNames.IndexOf(newSceneName);
        //int oldsceneIndex = sceneNameDatas.SceneNames.IndexOf(oldSceneName);//上に同じ

        Debug.Log(newSceneIndex);
        if (dictionaryMaps.TryGetValue(newSceneIndex, out int bossIndex))
        {
            
            // シーンインデックスに対応するボスが存在する場合、そのボスのグリッドを設定
            ChangeBossGrid(bossIndex);
        }
       
        else if (!ConfirmBossExist(newSceneIndex))
        {
            StartCoroutine(BossGrid.Instance.ExecuteDisable(gridSceneActive));
            //gridSceneActive = true;
        }

       

    }

    bool ConfirmBossExist(int newSceneIndex)//ボスが確定でいないシーンでボスのグリッドを非表示にする
    {
        bool bossExist = false;
        foreach (var Boss in ExistBossScene)//ボスがいるシーンかどうか
        {

            if (Boss != newSceneIndex) bossExist = false;//ボスがそもそもいないはずのシーンではボスを非表示
            else if (Boss == newSceneIndex) bossExist = true;//いるならグリッドはアクティブ


        }

        return bossExist;
    }

    void ChangeBossGrid(int bossIndex)
    {
        gridSceneActive = true;
        //DontDestroyOnloadの後に呼び出すことでアクティブ、非アクティブを変更することが出来る、最初から非表示にするとアタッチが外れるから
        StartCoroutine(BossGrid.Instance.ExecuteDisable(gridSceneActive));//ボスグリッドの親のキャンバス
        for (int i = 0; i < gridsBossDontDestroy.Length; i++)
        {
            if (i == bossIndex) continue;//該当するボス以外は非表示にする
            gridsBossDontDestroy[i].gameObject.SetActive(false);
        }
        if (DetectDefeat[bossIndex] == true)//ボスがたおされていたかどうかの確認
        {
            if (gridsBossDontDestroy[bossIndex] != null)//リストがnullじゃなかったら
            {
                gridsBossDontDestroy[bossIndex].gameObject.SetActive(false);
            }
        }
        else
        {

            if (gridsBossDontDestroy[bossIndex] != null) gridsBossDontDestroy[bossIndex].gameObject.SetActive(true);

        }
        gridSceneActive = false;//シーンが変わる前に次のシーンからこのシーンに戻って来るときにアクティブ化するため

    }

   



}
