using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }

    public UnityAction OnBattleOver;//バトル終了時
    public UnityAction<bool> OnDefeatBoss;//ボスを倒した時

    private bool isDieidPlayer = false;//プレイヤーが死んだとき、教会に飛ばすため
    bool isDefeat;//ボスが倒されたかどうかの判定
    bool skipPlayerTurn;//逃げるのに失敗したとき
    private bool isTypingDialog = false;//選択中の連打してもテキストの重複を防ぐため
    bool canUseItem = true;
 
    public bool IsDieidPlayer { get => isDieidPlayer; set => isDieidPlayer = value; }

    int checkPlayerMove = 0;//プレイヤーがたたかうか道具のどちらを選択したか
    enum State　//戦闘の流れ
    { 
        Start,
        ActionSelection,
        MoveSelection,
        RunTurns,
        BattleOver,
    }

    State state;

    //戦闘中に必要な変数
    [SerializeField] ActionSelectionUI actionSelectionUI;
    [SerializeField] MoveSelectionUI moveSelectionUI;
    [SerializeField] ItemSelectionUI itemSelectionUI;
    [SerializeField] DialogBase battleDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        this.gameObject.SetActive(false);
       
    }
    public void BattleStart(Battler player, Battler enemy)//バトルの始まり
    {
        isDefeat = false;
        state = State.Start;
        Debug.Log("Battel start");
        actionSelectionUI.Init();
        moveSelectionUI.Init(player.Moves);
        itemSelectionUI.Init(items:itemSelectionUI.ItemUI.ItemList);
        player.SetOriginalSta();
        StartCoroutine(SetupBattle(player,enemy));
    }

   
    IEnumerator SetupBattle(Battler player,Battler enemy)//自分と敵の情報を集める
    {
        actionSelectionUI.SetWhiteColor();
        playerUnit.Setup(player);
        enemyUnit.Setup(enemy);
        yield return battleDialog.TypeDialog($"{enemy.Base.Name}が現れた！\nどうする？");
        ActionSelection();
    }

    void BattleOver()//バトル終了
    {
        Debug.Log(isDefeat);
        itemSelectionUI.DeleteMoveText();
        moveSelectionUI.DeleteMoveText();
        OnBattleOver?.Invoke();
        OnDefeatBoss?.Invoke(isDefeat);
        

    }

    void ActionSelection()//バトル時の最初の選択
    {
      
        actionSelectionUI.Open();
        state = State.ActionSelection;
        

    }

    void MoveSelection()//戦う選択時
    {
        checkPlayerMove = 0;
        state = State.MoveSelection;
        moveSelectionUI.Open();
        

    }

    void ItemSelection()//道具選択時
    {
        checkPlayerMove = 1;
        state = State.MoveSelection;
        itemSelectionUI.Open();
    }
     IEnumerator RunTurns()//実際の戦闘
    {
        
        state = State.RunTurns;//前のところでの入力の処理を繰り返さないため

        ActionMoveBase playerMove = null;
        ItemMoveBase itemplayerMove = null;
        if (checkPlayerMove == 0)
        {
           playerMove = playerUnit.Battler.Moves[moveSelectionUI.SelectedIndex].Base as ActionMoveBase;

            if(playerMove != null && playerMove.Type == ActionType.Buff)
            {
                if (playerMove is BuffMove_Attack) playerUnit.Battler._AboutBuff.IsBuffed_Attack = true;
                else if (playerMove is Debuff_Attack) enemyUnit.Battler._AboutBuff.IsDebuffed_Attack = true;
                else if (playerMove is Buff_Defence) playerUnit.Battler._AboutBuff.IsBuffed_Defence = true;
                else if (playerMove is Debuff_Defence) enemyUnit.Battler._AboutBuff.IsDebuffed_Defence = true;
            }
        }else if(checkPlayerMove == 1)
        {
               itemSelectionUI.WhenUsedItem();
               itemplayerMove = itemSelectionUI.ItemUI.ItemList[itemSelectionUI.ItemUI.GetItemPlace(itemSelectionUI.selectedIndex)];
        }
        
        if(!skipPlayerTurn)
        {
            yield return RunMove(playerUnit, enemyUnit,playerMove, itemplayerMove);
            if (state == State.BattleOver)
            {

                yield return battleDialog.TypeDialog($"{enemyUnit.Battler.Base.Name}を倒した！", auto: false);

                //レベルアップによる新しいワザを覚える、経験値を得る。
                playerUnit.Battler.HasExp += enemyUnit.Battler.Base.Exp;

                yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は経験値{enemyUnit.Battler.Base.Exp}を得た！", false);
                playerUnit.Battler.GetGold(enemyUnit.Battler.Base);
                yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は{enemyUnit.Battler.Base.DropGold}Gを得た!", auto: false);
                while (playerUnit.Battler.isLevelUp())
                {
                    playerUnit.UpdateStatus();
                    playerUnit.UpdateUI();

                    //レベルアップの処理
                    yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}はLevel.{playerUnit.Battler.Level}になった！", auto: false);
                    //特定のレベルで技を覚える
                    //技を覚えた時の処理
                    Move learnedMove = playerUnit.Battler.LearnedMove();
                    if (learnedMove != null)
                    {
                        yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は{learnedMove.Base.Name}を覚えた！", auto: false);
                    }


                }

                BOSSBase bOSSBase = enemyUnit.Battler?.BossBase;
                if (bOSSBase != null) isDefeat = true;
                else isDefeat = false;

                BattleOver();
                yield break;

            }


           

        }

        Move enemyMove = enemyUnit.Battler.GetRandomMove();
        ActionMoveBase enemyActionMove = enemyMove.Base as ActionMoveBase;

      
        yield return RunMove(enemyUnit, playerUnit, enemyActionMove, null, endTurn: true);
        if (state == State.BattleOver)
        {
            yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は倒された！",auto:false );

            isDieidPlayer = true;
            isDefeat = false;
            BattleOver();
            yield break;

        }

        yield return battleDialog.TypeDialog("どうする？");

        skipPlayerTurn = false;//もしプレイヤーが逃げれなかったら次のターンは選択できるようにfalseにする
        ActionSelection(); ;//ここでstateを変えると二回転目のときにwindowがopenされなくなる
        

    }

    //攻撃後のダイアログの出力
    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit, ActionMoveBase move = null, ItemMoveBase itemmove = null,bool endTurn = false)//技を行うほうと受けるほうにわける
    {


        string resultText = "";
        isTypingDialog = true;
        if(move != null)
        {
            resultText = move.RunMoveResult(sourceUnit, targetUnit);
        }else if(itemmove != null)
        {
            resultText = itemmove.RunMoveResult(sourceUnit, targetUnit);
        }
        
        yield return battleDialog.TypeDialog(resultText, auto: false);

        //Battler instance1 = new Battler(sourceBattler);//enemyのインスタンス
        //Battler instance2 = new Battler(targetBattler);//Playerのインスタンス

        //sourceUnit.Battler._AboutBuff.originalAT = instance1._AboutBuff.originalAT;

       if(endTurn)
       {

            Battler sourceBattler = sourceUnit.Battler;//enemy
            Battler targetBattler = targetUnit.Battler;//player

            Debug.Log($"バフの確認 {targetBattler.Base.Name}{targetBattler._AboutBuff.IsBuffed_Attack}");
            if (targetBattler._AboutBuff.IsBuffed_Attack)
            {
                yield return CheckBuff(sourceBattler, targetBattler, 0);
            }

            if (sourceBattler._AboutBuff.IsDebuffed_Attack)
            {
                yield return CheckBuff(sourceBattler, targetBattler, 1);
            }

            if(targetBattler._AboutBuff.IsBuffed_Defence)
            {
                yield return CheckBuff(sourceBattler, targetBattler, 2);
            }

            if(sourceBattler._AboutBuff.IsDebuffed_Defence)
            {
                yield return CheckBuff(sourceBattler, targetBattler, 3);
            }


         
        }

        if (targetUnit.Battler.HP <= 0)
        {
            state = State.BattleOver;
        }
        isTypingDialog = false;
        sourceUnit.UpdateUI();//プレイヤーの選択ターン終了時のUIの変更
        targetUnit.UpdateUI();//相手のターン終了時のUIの変更

        playerUnit.ChangeHPColor();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Start:
                break;
            case State.ActionSelection:
                StartCoroutine(HandleActionSelection());
                break;
            case State.MoveSelection:
                HandleMoveActionSelection();
                break;
            case State.RunTurns:
                break;
            case State.BattleOver:
                break;

        }

        


    }

    IEnumerator HandleActionSelection()//最初の選択
    {
        actionSelectionUI.HandleActionUpdate();

        if(!isTypingDialog && Input.GetKeyDown(KeyCode.Space))
        {
            if(actionSelectionUI.SelectedIndex == 0)
            {
                Debug.Log("たたかう");
                MoveSelection();
            }
            else if(actionSelectionUI.SelectedIndex == 1)
            {
                ItemSelection();
            }
            else if(actionSelectionUI.SelectedIndex == 2)
            {
                //逃げるの場合
                if (enemyUnit.Battler.BossBase != null)//ボスとの戦闘の場合
                {
                    ActionSelection();
                    yield return battleDialog.TypeDialog("逃げられない！！");
                }
                else//通常の戦闘の場合
                {
                    int ratioEscape = Random.Range(0, 3);  //int ratioEscape = Random.Range(0, 3);//Random.Range(0, 3);//終了値は含まないため3,2/3は逃げたいから
                    Debug.Log(ratioEscape);
                    if(ratioEscape == 1)
                    {
                        state = State.BattleOver;//ここでstateを変えないとこのメソッドが継続され、スペースを押すたびこの処理がずっと行われてテキストが増えていく
                        yield return battleDialog.TypeDialog("うまく逃げ切れた！",auto:false);

                        BattleOver();
                    }else if(ratioEscape == 0)
                    {
                            state = State.RunTurns;//ここでstateを変えないとこのメソッドが継続され、スペースを押すたびこの処理がずっと行われてテキストが増えていく
                        　　yield return battleDialog.TypeDialog("回り込まれた！");
                            skipPlayerTurn = true;
                            StartCoroutine(RunTurns());
                       
                       
                        
                    }
                }
                   
            }
        }
    }

    void HandleMoveActionSelection()//選ばれたワザと実行へ
    {
        moveSelectionUI.HandleActionUpdate();//どの技が選ばれるか
        itemSelectionUI.HandleActionUpdate();//どの道具が選ばれるか

        if (playerUnit.Battler.HP == playerUnit.Battler.MaxHP && itemSelectionUI.gameObject.activeSelf)
        {
            canUseItem = false;
        }

        

        if (InputKeySpace(ableAction:canUseItem))//Input.GetKeyDown(KeyCode.Space)
        {
            var selectedMoveBase = playerUnit.Battler.Moves[moveSelectionUI.SelectedIndex].Base;

            if (selectedMoveBase is AttackMove playerMove)
            {
                // AttackMove として利用できる
                if (playerUnit.Battler.MagicPoint < playerMove.MagicPoint)
                {
                    state = State.MoveSelection;
                    return;
                }
            }

            moveSelectionUI.Close();
            actionSelectionUI.Close();
            itemSelectionUI.Close();
            //攻撃の処理を行う
            StartCoroutine(RunTurns());
        }else if (Input.GetKeyDown(KeyCode.X)) { 
            moveSelectionUI.Close();
            itemSelectionUI.Close();
            ActionSelection();


        }

        
    }


    IEnumerator CheckBuff(Battler sourceBattler,Battler targetBattler,int buffIndex)//Battler instanc
    {

        string resultText = "";
        switch (buffIndex)
        {
            case 0:
                if (targetBattler._AboutBuff.BuffedDulation_AT == 1)
                {
                    targetBattler.AT = targetBattler._AboutBuff.OriginalAT;
                    resultText = $"{targetBattler.Base.Name}の攻撃力上昇の効果が切れた！！";
                    targetBattler._AboutBuff.BuffedDulation_AT = 0;
                    targetBattler._AboutBuff.IsBuffed_Attack = false;
                    yield return battleDialog.TypeDialog(resultText, auto: false);
                }
                else targetBattler._AboutBuff.BuffedDulation_AT++;
                Debug.Log("攻撃バフチェック");
                break; 
            case 1:
                if (sourceBattler._AboutBuff.DebuffedDulation_AT == 1)
                {
                    sourceBattler.AT = sourceBattler._AboutBuff.OriginalAT;
                    resultText = $"{sourceBattler.Base.Name}の攻撃力がもとに戻った！！";
                    sourceBattler._AboutBuff.DebuffedDulation_AT = 0;
                    sourceBattler._AboutBuff.IsDebuffed_Attack = false;
                    yield return battleDialog.TypeDialog(resultText, auto: false);
                }
                else sourceBattler._AboutBuff.DebuffedDulation_AT++;
                break;
            case 2:
                if (targetBattler._AboutBuff.BuffedDulation_DF == 1)
                {
                    targetBattler.Defence = targetBattler._AboutBuff.OriginalDefence;
                    resultText = $"{targetBattler.Base.Name}の守備力上昇の効果が切れた！！";
                    targetBattler._AboutBuff.BuffedDulation_DF = 0;
                    targetBattler._AboutBuff.IsBuffed_Defence = false;
                    yield return battleDialog.TypeDialog(resultText, auto: false);
                }
                else targetBattler._AboutBuff.BuffedDulation_DF++;
                
                break;
            case 3:
                if (sourceBattler._AboutBuff.DebuffedDulation_DF == 1)
                {
                    sourceBattler.Defence= sourceBattler._AboutBuff.OriginalDefence;
                    resultText = $"{targetBattler.Base.Name}の守備力がもとに戻った！！";
                    sourceBattler._AboutBuff.DebuffedDulation_DF = 0;
                    sourceBattler._AboutBuff.IsDebuffed_Defence = false;
                    yield return battleDialog.TypeDialog(resultText, auto: false);
                }
                else sourceBattler._AboutBuff.DebuffedDulation_DF++;
                break;
            default:
                break;






        }

        yield return null;

    }
    bool InputKeySpace(KeyCode key = KeyCode.Space, bool ableAction = true)
    {
        canUseItem = true;
        return ableAction && Input.GetKeyDown(key);

    }



}
