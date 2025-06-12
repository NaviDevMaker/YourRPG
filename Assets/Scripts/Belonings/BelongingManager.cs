using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelongingManager : MonoBehaviour
{
    public static BelongingManager Instance { get;private set; }
    [SerializeField] GoldUI goldUI;
    [SerializeField] StatusUI statusUI;
    [SerializeField] ItemUI itemUI;

    [SerializeField] StorePlayerItems storePlayerItems;

    public GoldUI GoldUI { get => goldUI;}

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

        InitItemList();
        Debug.Log(storePlayerItems.CurrentWepons);
        statusUI.OnClick_M_Status += statusUI.OnClicked_M;
        itemUI.OnClick_M_Item += itemUI.InitItem;

        StatusCanvas.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ItemCanvas.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            //ここでこの関数を使うことによって非アクティブのゴールドUIで出来ないUpdate内の処理が可能
            ManageGoldUI();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            
            Debug.Log("M確認");
            ManageStatusUI();
            ManageItemUI();

        }
        //CheckKeyCode();
    }
    void ManageGoldUI()//UI表示の管理
    {
        goldUI.OnClick_G?.Invoke();
        Debug.Log(goldUI.OnClick_G);
    }

    void ManageStatusUI()
    {
        statusUI.OnClick_M_Status?.Invoke();
        Debug.Log (statusUI.OnClick_M_Status);

    }

    void ManageItemUI()
    {
        //itemUI.ItemList.Clear();
        itemUI.OnClick_M_Item?.Invoke(storePlayerItems.CurrentItems,storePlayerItems.CurrentWepons);
    }
    public void SuspendMenu(bool BattleStart)//バトルが開始した時にメニューを開けなくする
    {
        if(BattleStart)
        {
            ItemCanvas.Instance.gameObject.SetActive(false);
            StatusCanvas.Instance.gameObject.SetActive(false);
            GoldCanvas.Instance.gameObject.SetActive(false);

        }else if(!BattleStart)
        {
            ItemCanvas.Instance.gameObject.SetActive(true);
            StatusCanvas.Instance.gameObject.SetActive(true);
            GoldCanvas.Instance.gameObject.SetActive(true);
        }
    }

    //武器や道具屋で参照するため、それらのショップからのアクセスはitemUIのついたオブジェクトがシーンロード時にアクティブでなきゃいけないので不完全
    void InitItemList()
    {
        if(storePlayerItems.CurrentItems != null)
        {
            foreach (var item in itemUI.ItemList)
            {
                storePlayerItems.CurrentItems.Remove(item);
            }

            storePlayerItems.CurrentItems.Clear();
        }

        if(storePlayerItems.CurrentWepons != null)
        {
            foreach (var wepon in itemUI.WeponList)
            {
                storePlayerItems.CurrentWepons.Remove(wepon);
            }

            storePlayerItems.CurrentWepons.Clear();
        }

        foreach (var item in itemUI.ItemList)
        {
            storePlayerItems.CurrentItems.Add(item);
        }

        foreach (var wepon in itemUI.WeponList)
        {
            storePlayerItems.CurrentWepons.Add(wepon);
        }
    }

   
}
