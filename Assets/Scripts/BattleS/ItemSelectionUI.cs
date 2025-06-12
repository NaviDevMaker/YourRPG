using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectionUI : Phase2SelectionUIBase
{

    public static ItemSelectionUI Instance { get; private set; }

    //バトルシステムで使用するために必要、バトルシステムで扱う変数を少なくするため
    [SerializeField] ItemUI itemUI;

   
    public ItemUI ItemUI { get => itemUI;}

    //どのシーンでも使うからシングルトン
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


