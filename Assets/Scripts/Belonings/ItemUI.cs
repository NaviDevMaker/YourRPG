using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : BelongingUIBase
{
    [SerializeField] StatusUI statusUI;
    [SerializeField] RectTransform movesParent;//親

    List<int> counts = new List<int>();//アイテムの数をいれる
    [SerializeField] List<ItemMoveBase> itemList;//回復アイテムのリスト
    [SerializeField] List<WeponBase> weponList;
    [SerializeField] Image swoadImage;
    [SerializeField] Transform swoadRot;
    [SerializeField] SelectableText itemTextPrefab;
    List<SelectableText> selectableTexts = new List<SelectableText>();//selectableTextを集める
    List<ItemMoveBase> previousItems = new List<ItemMoveBase>();//アイテムが一個目だった場合にその要素をいれるリスト
    string currentWeponName;
    public int selectedIndex { get; set; } = 0;
    int allCount = 0;//アイテムリスト内での選ばれたアイテムの場所
    public UnityAction<List<ItemMoveBase>,List<WeponBase>> OnClick_M_Item;

    GameObject swoad;

    [SerializeField] GridLayoutGroup gridLayoutGroup;
    public List<ItemMoveBase> ItemList { get => itemList; }
    public List<WeponBase> WeponList { get => weponList;}

    //CancellationTokenSource cls = new CancellationTokenSource();
    //CancellationToken clt;
    enum SelectItem 
    {
        Idle,
        Select,
        ItemExecute,
        WeponWearning,
    }

    
    SelectItem selectItem;

    private void Awake()
    {
       
        //スタート時にアイドルにするとアクティブになったときにセレクトになったselectItemの値が変わってしまう
        selectItem = SelectItem.Idle;
    }

    //private void Start()
    //{
    //    clt = cls.Token;
    //}

    private void Update()
    {
        
        //アイテム選択の一連の流れ
        switch (selectItem)
        {
            case SelectItem.Idle:
                break;
            case SelectItem.Select:
                HandleItemActionUpdate();
                break;
            case SelectItem.ItemExecute:
                UseItem(selectableTexts);               
                break;
            case SelectItem.WeponWearning:
                WeponSelect();
                break;


        }

        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    cls.Cancel();
        //    cls = new CancellationTokenSource(); // 新しいCancellationTokenSourceを作成
        //    clt = cls.Token; // 新しいTokenを取得
        //}

    }

    //Mを押したときにUIを生成する
    public async void InitItem(List<ItemMoveBase> havingItem,List<WeponBase> havingWepon)
    {
        itemList = havingItem;//スクリプタブルオブジェクトの情報をこちらに代入
        weponList = havingWepon;//上に同じ
        await UniTask.Delay(100);
        base.OpenManage();


        if (!opend)
        {
            DeleteMoveText(selectableTexts);
            return;
        }
        else
        {
            player.Constraint = true;
            selectItem = SelectItem.Select;
        }



        gridLayoutGroup.enabled = true;
        Debug.Log(gridLayoutGroup.enabled);
        Init(movesParent,selectableTexts);
        

        Debug.Log(currentWeponName);
    }

    //選択中の処理
    void HandleItemActionUpdate()
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("スペースを感知");
            Debug.Log($"選ばれたのは{selectedIndex}");
            if(selectableTexts.Count != 0)
            {
                if (selectedIndex <= counts.Count - 1) selectItem = SelectItem.ItemExecute;//itemList.Count - 1
                else selectItem = SelectItem.WeponWearning;

            }
            else 
            {
                selectItem = SelectItem.Idle;
            }
            
        }
    }

    //アイテムを使ったときの処理を行う
    public void UseItem(List<SelectableText> selectableTexts)
    {
        ItemMoveBase itemMoveBase = itemList[GetItemPlace(selectedIndex)];

        HealType healType = itemMoveBase.HealType;

        bool canItemAction = default;

        switch (healType)
        {
            case HealType.HP:
                if (player.Battler.HP == player.Battler.MaxHP)
                {
                    canItemAction = false;
                }
                else canItemAction = true;
                break;
            case HealType.MP:
                if (player.Battler.MagicPoint == player.Battler.MaxMP)
                {
                    canItemAction = false;
                }
                else canItemAction = true;
                break;
            default:
                break;


        }

        //if (player.Battler.HP == player.Battler.MaxHP)
        //{
        //    canItemAction = false;
        //}
        //else
        //{
        //    canItemAction = true;
        //}

        Debug.Log(canItemAction);

        if(canItemAction)
        {
            HealPlayer(itemMoveBase);
            statusUI.OnUsedItem_Heal(player.Battler.HP);
            SortOutText(selectableTexts);
            
            selectItem = SelectItem.Select;
        }
        else if (!canItemAction)
        {
            Debug.Log("スペースキーが押されていない、またはアイテム使用不可");
            selectItem = SelectItem.Select;
        }
               
       
    }

    void WeponSelect()
    {
        //Vector2 itemTextPos = new Vector2();
        Text selectedWeponText = selectableTexts[selectedIndex].Text;
        //Debug.Log("装備!");
        if (player.Battler.AT != player.Battler.AttackHolder[player.Battler.Level - 1])//装備していたら
        {

            if (currentWeponName == WeponList[selectedIndex - counts.Count].WeponName)
            {
                Vector2 itemTextPos = new Vector2(selectedWeponText.transform.position.x - 20, selectedWeponText.transform.position.y);
                Destroy(swoad);
                selectedWeponText.transform.position = itemTextPos;
                player.Battler.AT = player.Battler.AttackHolder[player.Battler.Level - 1];
                statusUI.OnSelectedWepon(player.Battler.AT);
                currentWeponName = null;
                selectItem = SelectItem.Select;
                return;
            }
            else
            {
                selectItem = SelectItem.Select;
                return;
            }
               


        }

        //テキストの位置をずらし剣をテキストの横につける。装備している剣を示す。
        WearWepon(selectedWeponText).Forget();
        //itemTextPos = new Vector2(selectedWepon.transform.position.x + 20, selectedWepon.transform.position.y);//剣のテキストの位置
        //selectedWepon.transform.position = itemTextPos;
        //Transform setSwoadImageTra = selectedWepon.transform;
        //Vector2 swoadPos = new Vector2(setSwoadImageTra.position.x - 100, setSwoadImageTra.position.y + 10);
        //swoad = Instantiate(swoadImage.gameObject, swoadPos, swoadRot.rotation, setSwoadImageTra);
        player.Battler.AT += weponList[selectedIndex - counts.Count].WeponAT;//アイテムのテキストの後に武器は置かれるため//itemList.Count
        statusUI.OnSelectedWepon(player.Battler.AT);

        currentWeponName = weponList[selectedIndex - counts.Count].WeponName;
        Debug.Log($"武器は{currentWeponName}");
        selectItem = SelectItem.Select;
        //isWeaning = true;
    }

    async UniTask WearWepon(Text selectedWepon)
    {
        Debug.Log("装備");
        Debug.Log(selectedWepon.text);
        Debug.Log($"現在の位置: {selectedWepon.rectTransform.anchoredPosition}");
        Debug.Log("Before change: " + gridLayoutGroup.enabled);
        await UniTask.Yield();
        gridLayoutGroup.enabled = false; // or gridLayout.gameObject.SetActive(false);
        Debug.Log("After change: " + gridLayoutGroup.enabled);

        Vector2 weponTextPos = new Vector2(selectedWepon.rectTransform.anchoredPosition.x + 20, selectedWepon.rectTransform.anchoredPosition.y);//剣のテキストの位置
        selectedWepon.rectTransform.anchoredPosition = weponTextPos;
        Debug.Log($"更新後の位置: {selectedWepon.rectTransform.anchoredPosition}");

        Transform setSwoadImageTra = selectedWepon.transform;
        Vector2 swoadPos = new Vector2(setSwoadImageTra.position.x - 100, setSwoadImageTra.position.y + 10);
        swoad = Instantiate(swoadImage.gameObject, swoadPos, swoadRot.rotation, setSwoadImageTra);

        //await Task.Yield();
        //gridLayoutGroup.enabled = true;
        //await UniTask.Delay(TimeSpan.FromSeconds(5.0f),cancellationToken:clt);
        //gridLayoutGroup.enabled = false;
    }

    //次のUI生成時に前回のUIが残るのを防ぐ
    public void DeleteMoveText(List<SelectableText> selectableTexts)
    {
        foreach (var text in selectableTexts)
        {
            Destroy(text.gameObject);

        }

       previousItems.Clear();
       counts.Clear();// if (counts != null) 
       selectableTexts.Clear();
        
    }

    //新たなアイテムテキストをUIに反映
    public void Init(Transform movesParent,List<SelectableText> selectableTexts,bool isBattle = false)
    {
        selectedIndex = 0;
        Debug.Log(currentWeponName);
        
        int selectableTextIndex = 0;
        int previousCount = 0;
     
        previousItems = new List<ItemMoveBase>(new ItemMoveBase[itemList.Count]); // 初期サイズをitemListに合わせる
        previousItems[0] = null;
        for (int i = 0; i < itemList.Count;i++ )
        {
            for (int j = 0; j <= previousItems.Count - 1; j++)
            {
                if (previousItems[j] != null && itemList[i].Name == previousItems[j].Name)//itemList[i].Name == previousItem.Name
                {
                    //Debug.Log(count);
                    //Debug.Log("回数");

                    counts[j]++;//selectedIndex
                    selectableTexts[j].SetText($"{itemList[i].Name}×{counts[j]}");//selectedIndex
                }
            }
                if(!previousItems.Contains(itemList[i]))
                {

                    SelectableText itemText = Instantiate(itemTextPrefab, movesParent);
                    itemText.SetText(itemList[i].Name);
                    selectableTexts.Add(itemText);
                    previousItems[previousCount] = itemList[i];
                    selectableTextIndex = selectableTexts.Count - 1;
                    counts.Add(0);
                    counts[selectableTextIndex] = 1;//selectedIndex
                    previousCount++;
                }
            
        }

        if(!isBattle)
        {
            int weponIndex = -10;
            for (int i = 0; i < weponList.Count; i++)
            {
                SelectableText weponText = Instantiate(itemTextPrefab, movesParent);
                if(weponList[i].WeponName == currentWeponName)
                {
                    weponIndex = i;
                    Debug.Log("装備しています");
           
                }
                weponText.SetText(weponList[i].WeponName);
                selectableTexts.Add(weponText);
            }

            try
            {
                // WearWepon の実行
                 WearWepon(selectableTexts[weponIndex + counts.Count].Text).Forget();
            }
            catch (Exception ex)
            {
                // エラーを無視してログを出力
                Debug.LogWarning($"WearWepon の実行中にエラーが発生しました: {ex.Message}");
            }

        }


    }

    public int GetItemPlace(int selectedIndex)
    {
        Debug.Log(counts[0]);
        allCount = 0;//アイテムリスト内での選ばれたアイテムの場所
        for (int i = 0; i <= selectedIndex; i++)
        {
            Debug.Log(counts[i]);
            allCount += counts[i];
        }

        return allCount - 1;
    }
    void HealPlayer(ItemMoveBase itemMoveBase)
    {
        Debug.Log(GetItemPlace(selectedIndex));//Debug.Log(allCount);
        Debug.Log("スペースキーが押されたかつ、アイテムが使用可能");

        //

        if(itemMoveBase.HealType == HealType.HP)
        {
            ItemMove_Heal _Heal = itemMoveBase as ItemMove_Heal;
            player.Battler.HP += _Heal.ItemHeal;
            player.Battler.HP = Mathf.Clamp(player.Battler.HP, 0, player.Battler.MaxHP);

        }
        else if(itemMoveBase.HealType == HealType.MP)
        {

            ItemMove_MP _MP = itemMoveBase as ItemMove_MP;
            player.Battler.MagicPoint += _MP.MpHeal;
            player.Battler.MagicPoint = Mathf.Clamp(player.Battler.MagicPoint, 0, player.Battler.MaxMP);

        }
        //player.Battler.HP += itemList[GetItemPlace(selectedIndex)].ItemHeal;//itemList[selectedIndex].ItemHeal//player.Battler.HP += itemList[allCount - 1].ItemHeal;//itemList[selectedIndex].ItemHeal
        //player.Battler.HP = Mathf.Clamp(player.Battler.HP, 0, player.Battler.MaxHP);
    }

    
    void SortOutText(List<SelectableText> selectableTexts)
    {

        int selectedItemIndex = allCount - 1;

        if (counts[selectedIndex] != 1)//使ってもまだアイテムがある場合の処理
        {
            
            counts[selectedIndex]--;
            selectableTexts[selectedIndex].SetText($"{itemList[selectedItemIndex].Name}×{counts[selectedIndex]}");
            itemList.Remove(itemList[selectedItemIndex]);
        }
        else//アイテムがなくなった場合の処理
        {
            Destroy(selectableTexts[selectedIndex].gameObject);
            selectableTexts.Remove(selectableTexts[selectedIndex]);
            itemList.Remove(itemList[selectedItemIndex]);

          
        }

        for (int i = itemList.Count; i < selectableTexts.Count; i++)
        {
            if (currentWeponName == weponList[i - itemList.Count].WeponName)//selectableTexts[i - 1]
            {
                Debug.Log("成功");
                WearWepon(selectableTexts[i].Text).Forget();
                selectableTexts[i].SetText(weponList[i - itemList.Count].WeponName);
                continue;
            }

            selectableTexts[i].SetText(weponList[i].WeponName);

        }
    }
}
