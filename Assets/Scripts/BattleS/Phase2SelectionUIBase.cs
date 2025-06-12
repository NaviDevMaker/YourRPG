using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2SelectionUIBase : MonoBehaviour
{
    [SerializeField] protected RectTransform moveParent;//戦闘時に生成するUIの親
    [SerializeField] protected SelectableText textPrefab;//生成するテキスト
    protected List<SelectableText> selectableTexts = new List<SelectableText>();//生成したテキストを順に入れていく

    public int selectedIndex { get; private set; } = 0;
    public int SelectedIndex { get => selectedIndex; }

    //プレイヤーが選択したオプションを表示、道具か戦うのどちらにも対応するために引数は二つ
    public virtual void Init(List<Move> moves = null,List<ItemMoveBase> items = null)
    {
        if(moves != null)
        {
            //自分の子要素から集める
            SetMovesUISize(moves,items);
        }else
        {
            //自分の子要素から集める
            SetMovesUISize(moves,items);
        }
       


    }

    //UIのサイズをテキストの数に応じて変更
    public virtual void SetMovesUISize(List<Move> moves, List<ItemMoveBase> items)
    {

        if(moves != null)
        {
            Vector2 uiSize = moveParent.sizeDelta;
            uiSize.y = 45 + 45 * moves.Count;
            moveParent.sizeDelta = uiSize;

            for (int i = 0; i < moves.Count; i++)
            {
                SelectableText moveText = Instantiate(textPrefab, moveParent);
                moveText.SetText(moves[i].Base.Name);
                selectableTexts.Add(moveText);
            }
        }
        else
        {
            Vector2 uiSize = moveParent.sizeDelta;
            uiSize.y = 45 + 45 * selectableTexts.Count;
            moveParent.sizeDelta = uiSize;

            //for (int i = 0; i < items.Count; i++)
            //{
            //    SelectableText itemText = Instantiate(textPrefab, moveParent);
            //    itemText.SetText(items[i].Name);
            //    selectableTexts.Add(itemText);
            //}
        }
        

    }

    //選択中
    public virtual void HandleActionUpdate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))//疑似的に選択しているように見せる
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Count - 1);
        for (int i = 0; i < selectableTexts.Count; i++)
        {
            if (selectedIndex == i)
            {
                selectableTexts[i].SetSelectedColor(true);
            }
            else
            {
                selectableTexts[i].SetSelectedColor(false);
            }
        }
    }

    //UIの表示
    public virtual void Open()
    {
        selectedIndex = 0;
        gameObject.SetActive(true);
        Debug.Log("Open was Implement");
    }

    //UIの非表示
    public virtual void Close()
    {

        gameObject.SetActive(false);

    }

    //前回のUIの削除
    public virtual void DeleteMoveText()
    {
        foreach (var text in selectableTexts)
        {
            Destroy(text.gameObject);

        }

        selectableTexts.Clear();
    }


}

