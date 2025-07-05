using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }

    public UnityAction OnBattleOver;//�o�g���I����
    public UnityAction<bool> OnDefeatBoss;//�{�X��|������

    private bool isDieidPlayer = false;//�v���C���[�����񂾂Ƃ��A����ɔ�΂�����
    bool isDefeat;//�{�X���|���ꂽ���ǂ����̔���
    bool skipPlayerTurn;//������̂Ɏ��s�����Ƃ�
    private bool isTypingDialog = false;//�I�𒆂̘A�ł��Ă��e�L�X�g�̏d����h������
    bool canUseItem = true;
 
    public bool IsDieidPlayer { get => isDieidPlayer; set => isDieidPlayer = value; }

    int checkPlayerMove = 0;//�v���C���[����������������̂ǂ����I��������
    enum State�@//�퓬�̗���
    { 
        Start,
        ActionSelection,
        MoveSelection,
        RunTurns,
        BattleOver,
    }

    State state;

    //�퓬���ɕK�v�ȕϐ�
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
    public void BattleStart(Battler player, Battler enemy)//�o�g���̎n�܂�
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

   
    IEnumerator SetupBattle(Battler player,Battler enemy)//�����ƓG�̏����W�߂�
    {
        actionSelectionUI.SetWhiteColor();
        playerUnit.Setup(player);
        enemyUnit.Setup(enemy);
        yield return battleDialog.TypeDialog($"{enemy.Base.Name}�����ꂽ�I\n�ǂ�����H");
        ActionSelection();
    }

    void BattleOver()//�o�g���I��
    {
        Debug.Log(isDefeat);
        itemSelectionUI.DeleteMoveText();
        moveSelectionUI.DeleteMoveText();
        OnBattleOver?.Invoke();
        OnDefeatBoss?.Invoke(isDefeat);
        

    }

    void ActionSelection()//�o�g�����̍ŏ��̑I��
    {
      
        actionSelectionUI.Open();
        state = State.ActionSelection;
        

    }

    void MoveSelection()//�키�I����
    {
        checkPlayerMove = 0;
        state = State.MoveSelection;
        moveSelectionUI.Open();
        

    }

    void ItemSelection()//����I����
    {
        checkPlayerMove = 1;
        state = State.MoveSelection;
        itemSelectionUI.Open();
    }
     IEnumerator RunTurns()//���ۂ̐퓬
    {
        
        state = State.RunTurns;//�O�̂Ƃ���ł̓��͂̏������J��Ԃ��Ȃ�����

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

                yield return battleDialog.TypeDialog($"{enemyUnit.Battler.Base.Name}��|�����I", auto: false);

                //���x���A�b�v�ɂ��V�������U���o����A�o���l�𓾂�B
                playerUnit.Battler.HasExp += enemyUnit.Battler.Base.Exp;

                yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}�͌o���l{enemyUnit.Battler.Base.Exp}�𓾂��I", false);
                playerUnit.Battler.GetGold(enemyUnit.Battler.Base);
                yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}��{enemyUnit.Battler.Base.DropGold}G�𓾂�!", auto: false);
                while (playerUnit.Battler.isLevelUp())
                {
                    playerUnit.UpdateStatus();
                    playerUnit.UpdateUI();

                    //���x���A�b�v�̏���
                    yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}��Level.{playerUnit.Battler.Level}�ɂȂ����I", auto: false);
                    //����̃��x���ŋZ���o����
                    //�Z���o�������̏���
                    Move learnedMove = playerUnit.Battler.LearnedMove();
                    if (learnedMove != null)
                    {
                        yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}��{learnedMove.Base.Name}���o�����I", auto: false);
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
            yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}�͓|���ꂽ�I",auto:false );

            isDieidPlayer = true;
            isDefeat = false;
            BattleOver();
            yield break;

        }

        yield return battleDialog.TypeDialog("�ǂ�����H");

        skipPlayerTurn = false;//�����v���C���[��������Ȃ������玟�̃^�[���͑I���ł���悤��false�ɂ���
        ActionSelection(); ;//������state��ς���Ɠ��]�ڂ̂Ƃ���window��open����Ȃ��Ȃ�
        

    }

    //�U����̃_�C�A���O�̏o��
    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit, ActionMoveBase move = null, ItemMoveBase itemmove = null,bool endTurn = false)//�Z���s���ق��Ǝ󂯂�ق��ɂ킯��
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

        //Battler instance1 = new Battler(sourceBattler);//enemy�̃C���X�^���X
        //Battler instance2 = new Battler(targetBattler);//Player�̃C���X�^���X

        //sourceUnit.Battler._AboutBuff.originalAT = instance1._AboutBuff.originalAT;

       if(endTurn)
       {

            Battler sourceBattler = sourceUnit.Battler;//enemy
            Battler targetBattler = targetUnit.Battler;//player

            Debug.Log($"�o�t�̊m�F {targetBattler.Base.Name}{targetBattler._AboutBuff.IsBuffed_Attack}");
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
        sourceUnit.UpdateUI();//�v���C���[�̑I���^�[���I������UI�̕ύX
        targetUnit.UpdateUI();//����̃^�[���I������UI�̕ύX

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

    IEnumerator HandleActionSelection()//�ŏ��̑I��
    {
        actionSelectionUI.HandleActionUpdate();

        if(!isTypingDialog && Input.GetKeyDown(KeyCode.Space))
        {
            if(actionSelectionUI.SelectedIndex == 0)
            {
                Debug.Log("��������");
                MoveSelection();
            }
            else if(actionSelectionUI.SelectedIndex == 1)
            {
                ItemSelection();
            }
            else if(actionSelectionUI.SelectedIndex == 2)
            {
                //������̏ꍇ
                if (enemyUnit.Battler.BossBase != null)//�{�X�Ƃ̐퓬�̏ꍇ
                {
                    ActionSelection();
                    yield return battleDialog.TypeDialog("�������Ȃ��I�I");
                }
                else//�ʏ�̐퓬�̏ꍇ
                {
                    int ratioEscape = Random.Range(0, 3);  //int ratioEscape = Random.Range(0, 3);//Random.Range(0, 3);//�I���l�͊܂܂Ȃ�����3,2/3�͓�����������
                    Debug.Log(ratioEscape);
                    if(ratioEscape == 1)
                    {
                        state = State.BattleOver;//������state��ς��Ȃ��Ƃ��̃��\�b�h���p������A�X�y�[�X���������т��̏����������ƍs���ăe�L�X�g�������Ă���
                        yield return battleDialog.TypeDialog("���܂������؂ꂽ�I",auto:false);

                        BattleOver();
                    }else if(ratioEscape == 0)
                    {
                            state = State.RunTurns;//������state��ς��Ȃ��Ƃ��̃��\�b�h���p������A�X�y�[�X���������т��̏����������ƍs���ăe�L�X�g�������Ă���
                        �@�@yield return battleDialog.TypeDialog("��荞�܂ꂽ�I");
                            skipPlayerTurn = true;
                            StartCoroutine(RunTurns());
                       
                       
                        
                    }
                }
                   
            }
        }
    }

    void HandleMoveActionSelection()//�I�΂ꂽ���U�Ǝ��s��
    {
        moveSelectionUI.HandleActionUpdate();//�ǂ̋Z���I�΂�邩
        itemSelectionUI.HandleActionUpdate();//�ǂ̓���I�΂�邩

        if (playerUnit.Battler.HP == playerUnit.Battler.MaxHP && itemSelectionUI.gameObject.activeSelf)
        {
            canUseItem = false;
        }

        

        if (InputKeySpace(ableAction:canUseItem))//Input.GetKeyDown(KeyCode.Space)
        {
            var selectedMoveBase = playerUnit.Battler.Moves[moveSelectionUI.SelectedIndex].Base;

            if (selectedMoveBase is AttackMove playerMove)
            {
                // AttackMove �Ƃ��ė��p�ł���
                if (playerUnit.Battler.MagicPoint < playerMove.MagicPoint)
                {
                    state = State.MoveSelection;
                    return;
                }
            }

            moveSelectionUI.Close();
            actionSelectionUI.Close();
            itemSelectionUI.Close();
            //�U���̏������s��
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
                    resultText = $"{targetBattler.Base.Name}�̍U���͏㏸�̌��ʂ��؂ꂽ�I�I";
                    targetBattler._AboutBuff.BuffedDulation_AT = 0;
                    targetBattler._AboutBuff.IsBuffed_Attack = false;
                    yield return battleDialog.TypeDialog(resultText, auto: false);
                }
                else targetBattler._AboutBuff.BuffedDulation_AT++;
                Debug.Log("�U���o�t�`�F�b�N");
                break; 
            case 1:
                if (sourceBattler._AboutBuff.DebuffedDulation_AT == 1)
                {
                    sourceBattler.AT = sourceBattler._AboutBuff.OriginalAT;
                    resultText = $"{sourceBattler.Base.Name}�̍U���͂����Ƃɖ߂����I�I";
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
                    resultText = $"{targetBattler.Base.Name}�̎���͏㏸�̌��ʂ��؂ꂽ�I�I";
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
                    resultText = $"{targetBattler.Base.Name}�̎���͂����Ƃɖ߂����I�I";
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
