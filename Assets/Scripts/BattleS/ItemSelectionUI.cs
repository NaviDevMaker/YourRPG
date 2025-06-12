using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectionUI : Phase2SelectionUIBase
{

    public static ItemSelectionUI Instance { get; private set; }

    //�o�g���V�X�e���Ŏg�p���邽�߂ɕK�v�A�o�g���V�X�e���ň����ϐ������Ȃ����邽��
    [SerializeField] ItemUI itemUI;

   
    public ItemUI ItemUI { get => itemUI;}

    //�ǂ̃V�[���ł��g������V���O���g��
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
    }

    public override void Init(List<Move> moves = null, List<ItemMoveBase> items = null)
    {
        ItemUI.Init(moveParent, selectableTexts,isBattle:true);
        base.Init(moves,items);
    }
    public override void SetMovesUISize(List<Move> moves = null, List<ItemMoveBase> items = null)
    {
        base.SetMovesUISize(moves,items);
    }

    public override void HandleActionUpdate()
    {
        base.HandleActionUpdate();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void DeleteMoveText()
    {
        base.DeleteMoveText();
    }

    public void WhenUsedItem()
    {
        ItemUI.selectedIndex = this.selectedIndex;
        ItemUI.UseItem(selectableTexts);
        ItemUI.DeleteMoveText(selectableTexts);
        ItemUI.Init(moveParent,selectableTexts,isBattle:true);
    }

  




}


