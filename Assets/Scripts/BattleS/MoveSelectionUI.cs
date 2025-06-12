using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionUI : Phase2SelectionUIBase
{
    public static MoveSelectionUI Instance { get; private set; }

    //どのシーンでも使うからシングルトン
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    public override void Init(List<Move> moves = null, List<ItemMoveBase> items = null)
    {
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

    public override  void Close()
    {
        base.Close();
    }

    public override void DeleteMoveText()
    {
        base.DeleteMoveText();
    }
        

}
