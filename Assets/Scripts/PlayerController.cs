using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] Battler battler;//プレイヤーにバトラーとしての変数をいれるため

    [SerializeField] List<ChangeSceneBase> openSceneBases;//シーン遷移の情報を使うため。シーン遷移時のテキストやシーン名が場合によって違うのでリスト                                                          
    [SerializeField] int enterIndex;//isWalkable内で当たったレイヤーの情報を場合分けするため。また、シーン遷移時にも使用。

    [SerializeField] LayerMask getBossLayer;//レイヤーセッティングに入れてはいけない
    [SerializeField]  LayerSetting layerSettings;

    public UnityAction<Battler> OnEncounts; //Encountしたときに実行したい関数を外部から登録出来る
    public UnityAction<Battler,LayerMask,int> OnEncountsBoss;//BOSSに遭遇した時に発動する
    public UnityAction<int> MurabitoEvent;
    public UnityAction ChangeSceneEvent;
  
    Animator p_animator;//プレイヤーのアニメーター

    bool isMoving; //動いているかどうか
    bool constraint;//シーン遷移中にプレイヤーが動かないようにするためのフラグ

    Vector2 storePlayerPos;//シーン遷移前のプレイヤーのポジションを保持
    public Battler Battler { get => battler;}
 
    public int EnterIndex { get => enterIndex; set => enterIndex = value; }

    public int PreEnterIndex { get; private set; }
    public LayerMask GetBossLayer { get => getBossLayer;}
    public bool Constraint { get => constraint; set => constraint = value; }
    public List<ChangeSceneBase> OpenSceneBases { get => openSceneBases;}


    // Start is called before the first frame update
    private void Awake()//シングルトン,シーン遷移後も値は使うから
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        
        p_animator = GetComponent<Animator>();
        battler.Init(isEnemy:false);//BattlerにはPlayerのベースとなるスクリプタブルオブジェクトがアタッチされている。それらの値を集める。一度集めたらそれ以降別途で更新されていくので大丈夫
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(enterIndex);
        }

        if(isMoving == false) //プレイヤーが動いていない時に入力を受け付ける  
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (Constraint)//もしシーン遷移中の時はx、yの値を０にすることによって動けなくさせている(用途は動けなくなる時に全般)
            {
                x = 0;
                y = 0;
            }
            //transform.position += new Vector3(x, y, 0);
            if(x != 0)//斜め移動をしないようにしている
            {
                y = 0;
            }

            if(x != 0 || y != 0)//x,yの値が０でないときに歩行のアニメーションをするようにしている
            {
                p_animator.SetFloat("InputX", x);
                p_animator.SetFloat("InputY", y);
                StartCoroutine(Move(new Vector2(x, y)));
            }
            
        }

       p_animator.SetBool("IsMoving", isMoving);//動いているときは歩行のアニメーション

        

        



    }

    //１マス徐々に近づける
    IEnumerator Move(Vector3 direction)
    {
       
        isMoving = true;
        Vector3 targetPos = transform.position + direction;
        //現在とターゲットの場所が違うなら、近づけ続ける

        if(IsWalkable(targetPos) == false) //ここにレイヤーがあるなら先に進めないようにする。
        {
            isMoving = false;
            StartCoroutine(CheckForTown());//ここでもし町のレイヤーだったら町にはいる。（シーンを遷移させる）
            StartCoroutine(CheckForEncountBoss(targetPos));//もしボスだったら関数を発動
            StartCoroutine(CheckForInn());
            StartCoroutine(CheckForCave());
            StartCoroutine(CheckForChrch());
            yield break;
        }

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)//進む地点に到達するまで進み続ける
        {

            //近づける
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);//(現在地、目標値、速度）：目標値に近づける
            StartCoroutine(CheckForInn());//プレイヤーが外に出ようとしたとき
            StartCoroutine(CheckForChrch());//プレイヤーが外に出ようとしたとき
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        //敵に会うか調べる 
        if(!ChangeSceneBase.IsChangeScene) CheckForEncounts();//歩き終わった地点に敵に遭遇するか確かめる


    }

    //自分の場所から円のrayを飛ばして草原Layerに当たったら、ランダムエンカウント
    void CheckForEncounts()
    {
        Collider2D encount;
        //foreach (LayerMask encountlayer in layerSettings.EncountLayers)

        //{
            //移動した地点に、敵がいるか判断する
            encount = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EncountLayer);//encountレイヤーのもつコライダーの取得
            Debug.Log($"エリアは{encount}");
        if (encount)//もしエンカウントするレイヤーなら
        {
            Debug.Log("チャンス");
            float valueRandom = Random.Range(0, 100);
            //ランダムエンカウント
            if (valueRandom < 20)
            {
                Battler enemy = encount.GetComponent<EncountArea>().GetRandomBattler();//BackGroundにアタッチされているスクリプト
                OnEncounts?.Invoke(enemy);//もしOnEncountsに関数が登録されていれば実行する
                Debug.Log("遭遇");
            }

            //break;
        }

           
        //}
        
        //Collider2D encount = Physics2D.OverlapCircle(transform.position, 0.2f,layerSettings.EncountLayers);//encountレイヤーのもつコライダーの取得
        
       
    }

    //あたったレイヤーがボスかどうか確認する
    IEnumerator CheckForEncountBoss(Vector3 targetPos)
    {
        int bosslayerIndex = -1;
        Collider2D encountBoss = null;
        //移動した地点に、敵がいるか判断する
        foreach (var simbolLayer in layerSettings.SimbolLayers)
        {
            encountBoss = Physics2D.OverlapCircle(targetPos, 0.2f, simbolLayer);//encountレイヤーのもつコライダーの取得
            if (encountBoss)
            {
                bosslayerIndex = layerSettings.SimbolLayers.IndexOf(simbolLayer);
                break;

            }
        }
        if (encountBoss)//もしレイヤーがBOSSだったら 
        {
            //レイヤーについているコンポーネントを取得
            EncountBOSS enemyBOSS = encountBoss.GetComponent<EncountBOSS>();
            getBossLayer = layerSettings.SimbolLayers[bosslayerIndex];
            Battler BossBattler = enemyBOSS.GetBOSSBattler();//BossのTileMapにアタッチされているスクリプト

           
            //もしプレイヤーのレベルがボスと戦うに値するなら
            if (battler.Level >= enemyBOSS.BossBattler.BossBase.SuitableLevel)
            {
                
                BossDialog BossDialog = encountBoss.GetComponent<BossDialog>();
                int bossIndex = BossBattler.BossBase.BossIndex;
                constraint = true;
                yield return BossDialog.TypeDialog(BossBattler.BossBase.BossDialogContent[1], auto: false);//"うるせぇなぁ、ガキ!!ぶっ倒してやるよ！"
                Debug.Log("成功");
                BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                Debug.Log(bossIndex);
                if(bossIndex != 0)
                {
                    Debug.Log("戦闘開始！");
                    OnEncountsBoss?.Invoke(BossBattler, getBossLayer, bossIndex);//もしOnEncountに関数が登録されていれば実行する
                }
                else
                {
                    OnEncountsBoss?.Invoke(BossBattler, getBossLayer,0);//もしOnEncountに関数が登録されていれば実行する
                }
               
                
                
                Debug.Log("BOSSに遭遇");
                constraint = false;
            }
            else//もしプレイヤーのレベルが満たしていないなら
            {
                Debug.Log("まだはやいようだ");
                BossDialog BossDialog = encountBoss.GetComponent<BossDialog>();
                constraint = true;
                enemyBOSS.BossEncountCanvas.gameObject.SetActive(true);
                yield return BossDialog.TypeDialog(BossBattler.BossBase.BossDialogContent[0],auto:false);//"....反応がないようだ"
                enemyBOSS.BossEncountCanvas.gameObject.SetActive(false);
                constraint = false;
                //yield break;

            }

          //BOSSDialog?.Invoke();
          
          
        }

    }

    //今から移動する位置に移動できるか判定する関数
    bool  IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]))//もし町のレイヤーだったらインデックスを１にして進めないようにする
        {
            if (enterIndex == 0) enterIndex = 1;//レイヤーを渡す代わりの役割
            else if (enterIndex == 9) enterIndex = 10;
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]))//もし橋のレイヤーだったらインデックスを２にして進めないようにする
        {
            if (enterIndex == 1) enterIndex = 2;
            else if(enterIndex == 10) enterIndex = 11;//レイヤーを渡す代わりの役割
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]))//宿のレイヤーだったら
        {
            if (enterIndex != 11) enterIndex = 3;//レイヤーを渡す代わりの役割
            else if (enterIndex == 11) enterIndex = 12;

            Debug.Log(enterIndex);
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]))//もし洞窟のレイヤーだったら
        {
            enterIndex = 5;
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]) == false;


        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[4]))//もし洞窟の出口だったら
        {
            enterIndex = 6;
            return false;
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[5]))//もし教会だったら
        {
            if (enterIndex != 11) enterIndex = 7;
            else if (enterIndex == 11) enterIndex = 13;

            return false;
        }

        //foreach (var signBoard in layerSettings.SignBoardLayers)
        //{
        //    Collider2D signLayer = Physics2D.OverlapCircle(targetPos, 0.2f, signBoard);
        //    if(signLayer)
        //    {
        //        int signBoardIndex = layerSettings.SignBoardLayers.IndexOf(signBoard);
        //        SignBoardEvent?.Invoke(signBoardIndex);
        //        return false;
        //    }
        //}
        foreach (LayerMask murabitoLayer in layerSettings.MurabitoLayers)
        {
            Collider2D murabito = Physics2D.OverlapCircle(targetPos, 0.2f, murabitoLayer);
            if (murabito)
            {
            
                int murabitoLayerIndex = layerSettings.MurabitoLayers.IndexOf(murabitoLayer);
                Debug.Log($"村びとレイヤーインでっクスは{murabitoLayerIndex}");
                MurabitoEvent?.Invoke(murabitoLayerIndex);
                return false;
            }
        }

        foreach (var bosslayer in layerSettings.SimbolLayers)
        {
            if(Physics2D.OverlapCircle(targetPos, 0.2f, bosslayer))
            {
                return false;
            }
        }
        // targetPos1を中心に円形のRayを作る：SolidObjectLayerにぶつかったらtrueが帰ってくる.だからfalse
        return Physics2D.OverlapCircle(targetPos, 0.2f, layerSettings.SolidObjectLayer)  == false; //ぶつかっていない＝歩けるだから
                                         //targetPos
    }
    IEnumerator CheckForTown()//ChangeSceneBase SceneBase//町のレイヤーに当たったか確認する
    {
        Collider2D town = null;
        Collider2D brighe = null;
        int LayerTownIndex1 = 0;//町にはいるためのインデックス
        int LayerTownIndex2 = 0;//草原に戻るためのインデックス
        if (enterIndex == 1 || enterIndex == 10)//町のレイヤーを取得した場合の処理
        {
            Debug.Log(enterIndex);
            //OpenSceneBases[0].SceneIndex = 1;//ここで画面のロード中に出てくるヒントのテキストを変えるためのインデックスを渡す
            town = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]);   
            Debug.Log(town);
            Debug.Log(brighe);
        }
        else if(enterIndex == 2 || enterIndex == 11)//橋のレイヤーを取得した場合の処理
        {
            
            brighe = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]);
            Debug.Log(town);
            Debug.Log(brighe);
        }

        if (town != null)//町のレイヤーがヌルではなかったら
        {
            LayerTownIndex1 = town.gameObject.layer;
            
            // ここで LayerTownIndex を使った処理を行う
        }

        if (brighe != null) //橋のレイヤーがヌルではなかったら

        {
            LayerTownIndex2 = brighe.gameObject.layer;
            // ここで LayerTownIndex を使った処理を行う
        }
       
        //レイヤーの名前を取得
        string LayerTownName = LayerMask.LayerToName(LayerTownIndex1);//町のレイヤー
        string LayerTownName2 = LayerMask.LayerToName(LayerTownIndex2);//橋のレイヤー
        foreach (LayerMask layerMask in layerSettings.EnterLayer)//既存のリスト内にあるレイヤーの名前を比べる
        {
            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);

            //わざわざ二度のチェックが入るのは後々ゲームの進み具合におうじて入れる、入れないとこを作るため。
            if (LayerTownName == layerName && enterIndex == 1)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(5.5f, 1.5f);
                //タウンレイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//シーン遷移を実行
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 2;//最初の草原の町の中にはいるのでインデックスを変更
                PreEnterIndex = 2;
                Debug.Log(enterIndex);



            }
            else if (LayerTownName == layerName && enterIndex == 10)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(5.5f, 1.5f);
                //タウンレイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//シーン遷移を実行
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 11;//次の草原の町の中にはいるのでインデックスを変更
                PreEnterIndex = 11;
                Debug.Log(enterIndex);


            }
            else if (LayerTownName2 == layerName && enterIndex == 2)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(4, 1);
                constraint = true;
                Debug.Log("Exit town");
                transform.position = storePlayerPos;
                yield return openSceneBases[1].ChangeScene(openSceneBases[1]);//シーン遷移を実行
                constraint = false;
                enterIndex = 0;//町の外に出るのでインデックスを変更

            }
            else if(LayerTownName2 == layerName && enterIndex == 11)
            {

                storePlayerPos = new Vector2(-1, 5);
                constraint = true;
                Debug.Log("Exit town");
                transform.position = storePlayerPos;
                yield return openSceneBases[5].ChangeScene(openSceneBases[5]);//シーン遷移を実行
                constraint = false;
                enterIndex = 10;//町の外に出るのでインデックスを変更
            }

           
        }
        

    }

    IEnumerator CheckForInn()
    {
        Collider2D inn = null;
        Collider2D door = null;
        int LayerTownIndex3 = 0;//宿に入るためのインデックス
        //int LayerTownIndex1 = 0;//町内に戻るためのインデックス
        if (enterIndex == 3 || enterIndex == 12)//宿のレイヤーを取得した場合の処理
        {
            inn = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]);
            Debug.Log(inn);
            Debug.Log(door);
        }
        else if (enterIndex == 2)//町内のレイヤーを取得した場合の処理
        {
            //door = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]);          
        }

        if (inn != null)//町のレイヤーがヌルではなかったら
        {
            LayerTownIndex3 = inn.gameObject.layer;

            // ここで LayerTownIndex を使った処理を行う
        }

        //if (door != null) //ドアのレイヤーがヌルではなかったら

        //{
        //    LayerTownIndex1 = door.gameObject.layer;
        //    // ここで LayerTownIndex を使った処理を行う
        //}

        //レイヤーの名前を取得
        string LayerTownName3 = LayerMask.LayerToName(LayerTownIndex3);//宿のレイヤー
        //string LayerTownName = LayerMask.LayerToName(LayerTownIndex1);//町のレイヤー
        foreach (LayerMask layerMask in layerSettings.EnterLayer)//既存のリスト内にあるレイヤーの名前を比べる
        {
                     
            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName3 == layerName && enterIndex == 3)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(0, -3.0f);
                //宿レイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[2].ChangeScene(openSceneBases[2]);//シーン遷移を実行
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 4;//宿の中にはいるのでインデックスを変更


            }
            else if (LayerTownName3 == layerName && enterIndex == 12)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(0, -3.0f);
                //宿レイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[2].ChangeScene(openSceneBases[2]);//シーン遷移を実行
                constraint = false;
                Debug.Log(constraint);
                //enterIndex = 4;//宿の中にはいるのでインデックスを変更
            }
            else if (transform.position.y <= -5.0f && enterIndex == 4)//名前が同じかつインデックスの値が同じだったら
            {
                Debug.Log("町に戻るよ！");
                constraint = true;
                Debug.Log("Exit town");
                //transform.position = new Vector2(0, -3.0f);
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//シーン遷移を実行                
                constraint = false;
                enterIndex = 2;//町内に戻るのでインデックスを変更

            }
            else if (transform.position.y <= -5.0f && enterIndex == 12)//名前が同じかつインデックスの値が同じだったら
            {
                Debug.Log("町に戻るよ！");
                constraint = true;
                Debug.Log("Exit town");
                //transform.position = new Vector2(0, -3.0f);
                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//シーン遷移を実行                
                constraint = false;
                enterIndex = 11;//町内に戻るのでインデックスを変更

            }


        }


    }

    IEnumerator CheckForCave()
    {
       
        Collider2D cave = null;
        Collider2D exit = null;
        int LayerTownIndex4 = 0;//洞窟に入るためのインデックス
        int LayerTownIndex5 = 0;
        if (enterIndex == 5)//洞窟のレイヤーを取得した場合の処理
        {
            //OpenSceneBases[2].SceneIndex = 3;//ここで画面のロード中に出てくるヒントのテキストを変えるためのインデックスを渡す
            cave = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]);
            Debug.Log(cave);
            Debug.Log(exit);
        }else if(enterIndex == 6)
        {
            exit = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[4]);
        }

        if (cave != null)//洞窟のレイヤーがヌルではなかったら
        {
            LayerTownIndex4 = cave.gameObject.layer;

            // ここで LayerTownIndex を使った処理を行う
        }

        if(exit != null)
        {
            LayerTownIndex5 = exit.gameObject.layer;
        }
        //レイヤーの名前を取得
        string LayerTownName4 = LayerMask.LayerToName(LayerTownIndex4);//洞窟のレイヤー
        string LayerTownName5 = LayerMask.LayerToName(LayerTownIndex5);//洞窟のレイヤー

        foreach (LayerMask layerMask in layerSettings.EnterLayer)//既存のリスト内にあるレイヤーの名前を比べる
        {

            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName4 == layerName && enterIndex == 5)//名前が同じかつインデックスの値が同じだったら
            {
                
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
       

    
                //洞窟レイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(-11, 11, 0);
                yield return openSceneBases[3].ChangeScene(openSceneBases[3]);//シーン遷移を実行
                //transform.position = new Vector3(-11, 11, 0);
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 6;//洞窟の中にはいるのでインデックスを変更


            }
            else if (LayerTownName5 == layerName && enterIndex == 6)//名前が同じかつインデックスの値が同じだったら　transform.position.y <= -5.0f
            {
                Debug.Log("町に戻るよ！");
                constraint = true;
                Debug.Log("Exit town");
                //洞窟に入ったときの場所にプレイヤーを戻す
                transform.position = storePlayerPos;
                
                
                yield return openSceneBases[1].ChangeScene(openSceneBases[1]);//シーン遷移を実行                
                constraint = false;
                enterIndex = 0;//草原に戻るのでインデックスを変更

            }


        }

    }

    IEnumerator CheckForChrch()
    {

        Collider2D church = null;
        Collider2D exit = null;
        int LayerTownIndex6 = 0;//洞窟に入るためのインデックス
        int LayerTownIndex7 = 0;
        if (enterIndex == 7 || enterIndex == 13)//教会のレイヤーを取得した場合の処理
        {
            church = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[5]);
            Debug.Log(church);
            Debug.Log(exit);
        }

        if (church != null)//教会のレイヤーがヌルではなかったら
        {
            LayerTownIndex6 = church.gameObject.layer;

            // ここで LayerTownIndex を使った処理を行う
        }
        //レイヤーの名前を取得
        string LayerTownName6 = LayerMask.LayerToName(LayerTownIndex6);//洞窟のレイヤー
        string LayerTownName7 = LayerMask.LayerToName(LayerTownIndex7);//洞窟のレイヤー

        foreach (LayerMask layerMask in layerSettings.EnterLayer)//既存のリスト内にあるレイヤーの名前を比べる
        {

            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName6 == layerName && enterIndex == 7)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
                //教会レイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(0, 0, 0);
                yield return openSceneBases[4].ChangeScene(openSceneBases[4]);//シーン遷移を実行               
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 8;//教会の中にはいるのでインデックスを変更
                Debug.Log($"教会のインデックスは{enterIndex}");


            }
            else if (LayerTownName6 == layerName && enterIndex == 13)//名前が同じかつインデックスの値が同じだったら
            {
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
                //教会レイヤーのインデックスをLayerMaskに設定
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(0, 0, 0);
                yield return openSceneBases[4].ChangeScene(openSceneBases[4]);//シーン遷移を実行               
                constraint = false;
                Debug.Log(constraint);
                //enterIndex = 8;//教会の中にはいるのでインデックスを変更
                Debug.Log($"教会のインデックスは{enterIndex}");


            }
            else if (transform.position.y <= -5 && enterIndex == 8)//名前が同じかつインデックスの値が同じだったら　
            {
                Debug.Log($"プレイヤーポジ{storePlayerPos}");
                Debug.Log("町に戻るよ！");
                constraint = true;
                Debug.Log("Exit town");
                //Vector3 temporaryPos = new Vector3(-4, -4,0);
                
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//シーン遷移を実行
                transform.position = new Vector3(-4, -4, 0); ;
                if(storePlayerPos != new Vector2(0,0)) transform.position = storePlayerPos;


                //else 

                constraint = false;
                enterIndex = 2;//町内に戻るのでインデックスを変更

            }
            else if (transform.position.y <= -5 && enterIndex == 13)//名前が同じかつインデックスの値が同じだったら　
            {
                Debug.Log($"プレイヤーポジ{storePlayerPos}");
                Debug.Log("町に戻るよ！");
                constraint = true;
                Debug.Log("Exit town");
                //Vector3 temporaryPos = new Vector3(-4, -4,0);

                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//シーン遷移を実行
                transform.position = new Vector3(-4, -4, 0); ;
                if (storePlayerPos != new Vector2(0, 0)) transform.position = storePlayerPos;


                //else 

                constraint = false;
                enterIndex = 11;//NextGroundの町内に戻るのでインデックスを変更

            }


        }

    }
    


}











