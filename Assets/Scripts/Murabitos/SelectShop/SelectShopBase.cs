using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SelectShopBase : DialogBase
{
    enum SelectMachandise
    {
        Idle,
        OptionAction,
        CountBuyItem, 
        OptionSelected,
        OptionEnd,
    }

    SelectMachandise selectMachandise;
    int selectedIndex = 0;//選択されている選択とスペースを押されたときの選択を一致させるため
    [SerializeField] List<string> dialogContents;
    [SerializeField] Transform parentTransform;
    

    [SerializeField] SelectableText selectableText;//セレクタブルテキストのプレファブ
    //[SerializeField] Text countTextPrefab;//テキストのプレファブ
    protected ItemUI itemUI;//シーンが変わったらアタッチがはずれるので都度ゲットするため,継承先で使うため

    [SerializeField] Image optionImage;//オプションのイメージ
    [SerializeField] Image dialogImage;//ダイアログのイメージ

    [SerializeField] StorePlayerItems currentPlayerItems;

    

    List<int> buyCounts = new List<int>();
    int _buyCount = 1;


    bool isPressed = false;

    KeyCode pressedKey = new KeyCode();

    int buyCount 
    {
        get => _buyCount;
        set
        {
            _buyCount = value;
            UpdateCount();
        }
     }

    List<SelectableText> options = new List<SelectableText>();//プレファブ確認用、ウィンドウを閉じた後にテキストが残るのを防ぐため
    //List<Text> counts = new List<Text>();//プレファブ確認用、ウィンドウを閉じた後にテキストが残るのを防ぐ


    protected List<WeponBase> wepons;
    protected List<ItemMove_Heal> items;

    bool isTyped = false;
    bool isExecuting = false;

    float pressedTime = 0f;
    // Start is called before the first frame update
    public virtual void Start()
    {
        wepons = new List<WeponBase>();
        items = new List<ItemMove_Heal>();

        selectMachandise = SelectMachandise.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(selectMachandise);
        switch (selectMachandise)
        {
            case SelectMachandise.Idle:
                break;
            case SelectMachandise.OptionAction:
                StartCoroutine(SelectAction());
                break;
            case SelectMachandise.CountBuyItem:
               CountItem();
                break;
            case SelectMachandise.OptionSelected:
                if(!isExecuting) StartCoroutine(ExecuteOption(wepons, items, currentPlayerItems.CurrentWepons, currentPlayerItems.CurrentItems));

                break;
            case SelectMachandise.OptionEnd:
               StartCoroutine(EndDialog());
                break;
        }

    }


    protected IEnumerator StartOption()
    {
        //BelongingManager.Instance.SuspendMenu(true);
        selectedIndex = 0;//前回のテキストの選択をはずす
        PlayerController.Instance.Constraint = true;
        Debug.Log(PlayerController.Instance.Constraint);
        dialogImage.gameObject.SetActive(true);
        yield return base.TypeDialog(dialogContents[0], auto: false);
        dialogImage.gameObject.SetActive(false);
        optionImage.gameObject.SetActive(true);
        InitOption(wepons, items);
        Text.text = dialogContents[0];
        options[selectedIndex].SetSelectedColor(true);
        selectMachandise = SelectMachandise.OptionAction;
        
    }

    private IEnumerator SelectAction()//選択時に呼ばれる
    {

        //Debug.Log("成功");
        //options[selectedIndex].SetSelectedColor(true);
        
        //選択されるまで繰り返す
       

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("下");            
            selectedIndex++;
            options[selectedIndex].SetSelectedColor(true);
            options[selectedIndex - 1].SetSelectedColor(false);

        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {            
            selectedIndex--;
            options[selectedIndex].SetSelectedColor(true);
            options[selectedIndex + 1].SetSelectedColor(false);
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            selectedIndex = 0;
            yield return EndDialog();
            selectMachandise = SelectMachandise.Idle;

        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, options.Count - 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("間違い");
            selectMachandise = SelectMachandise.CountBuyItem;

        }
    }

    void CountItem()
    {

     

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isPressed = true;
            pressedKey = KeyCode.DownArrow;
            Debug.Log("下");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isPressed = true;
            pressedKey = KeyCode.UpArrow;
          
           
            //options[selectedIndex + 1].SetSelectedColor(false);
        }

        if(isPressed)
        {
            pressedTime += Time.deltaTime;
            if(pressedTime >= 0.07f)
            {
                switch (pressedKey)
                {
                    case KeyCode.DownArrow:
                        buyCount--;
                        break;
                    case KeyCode.UpArrow:
                        buyCount++;
                        break;

                }

                pressedTime = 0f;
            }
           
           
        }
        buyCount = Mathf.Clamp(buyCount, 1, 99);
        buyCounts[selectedIndex] = buyCount;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("間違い");
            selectMachandise = SelectMachandise.OptionSelected;
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            selectMachandise = SelectMachandise.OptionAction;
        }

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(key))
            {
                Debug.Log($"Key exit: {key}");
                isPressed = false;
                pressedTime = 0f;
            }
        }
    }

    //持ち物の欄に追加する
    IEnumerator ExecuteOption(List<WeponBase> wepons = null, List<ItemMove_Heal> items = null, List<WeponBase> weponList = null, List<ItemMoveBase> itemList = null)
    {
       
        isExecuting = true;
      
        if (SceneManager.GetActiveScene().name == "WeponStore")
        {
            if(weponList.Contains(wepons[selectedIndex]))
            {
                Debug.Log("持っている");
                selectMachandise = SelectMachandise.OptionAction;

            }else
            {
                weponList.Add(wepons[selectedIndex]);
                selectMachandise = SelectMachandise.OptionEnd;
            }
            
        } 
        else if(SceneManager.GetActiveScene().name == "ItemShop" || SceneManager.GetActiveScene().name == "CasleFloar1")
        {
            int overollPrece = buyCounts[selectedIndex] * items[selectedIndex].Prace;
            PlayerController player = GameObject.FindObjectOfType<PlayerController>();
            Debug.Log($"所持金: {player.Battler.HaveGold}, 必要金額: {items[selectedIndex].Prace}");

            if (player.Battler.HaveGold < overollPrece)//items[selectedIndex].Prace
            {
                Debug.Log(player.Battler.HaveGold);
                //if(!isTyped)
                //{
                    Debug.Log("お金が足りません！");
                    dialogImage.gameObject.SetActive(true);
                    //isTyped = true;
                    yield return base.TypeDialog(dialogContents[2], auto: false);
                    //isTyped = false;

                    dialogImage.gameObject.SetActive(false);
                    selectMachandise = SelectMachandise.OptionAction;
                    isExecuting = false;
                    yield break;
                //}
               
            }
            player.Battler.HaveGold -= overollPrece; //items[selectedIndex].Prace;
            itemList.Add(items[selectedIndex]);
            selectMachandise = SelectMachandise.OptionEnd;
            isExecuting = false;
        }
       

        
    }

    IEnumerator EndDialog()
    {
       
        if(!isTyped)
        {
            isTyped = true;
            dialogImage.gameObject.SetActive(true);

            switch (selectMachandise)
            {
                case SelectMachandise.OptionAction:
                    yield return base.TypeDialog(dialogContents[3], auto: false);
                    break;
                case SelectMachandise.OptionEnd:
                    yield return base.TypeDialog(dialogContents[1], auto: false);
                    break;

            }
            
            isTyped = false;
            PlayerController.Instance.Constraint = false;
            selectMachandise = SelectMachandise.Idle;
            optionImage.gameObject.SetActive(false);
            dialogImage.gameObject.SetActive(false);
            BelongingManager.Instance.SuspendMenu(false);
        }
       
    }
    void InitOption(List<WeponBase> wepons = null, List<ItemMove_Heal> items = null)
    {

        DeleteList();

        if (items.Count != 0)
        {
            Debug.Log("成功だよ");
            int count = 0;
            foreach (var item in items)
            {
                SelectableText option = Instantiate(selectableText, parentTransform);
                //Text countText = Instantiate(countTextPrefab,Vector3.zero,Quaternion.identity);
                options.Add(option);
                //counts.Add(countText);
                buyCounts.Add(1);
                option.SetText(items[count].Name + $" x{buyCounts[count].ToString()}" );
                count++;
            }
        }

        if (wepons.Count != 0)
        {
            int count = 0;
            foreach (var wepon in wepons)
            {

                SelectableText option = Instantiate(selectableText, parentTransform);
             
                options.Add(option);
               
                option.SetText(wepons[count].WeponName);
                count++;
            }
        } 
        
        
    }

    void UpdateCount()
    {
        options[selectedIndex].SetText(items[selectedIndex].Name + $" x{buyCounts[selectedIndex].ToString()}");
    }

    void DeleteList()
    {
        if (options != null)
        {
            foreach (var option in options)
            {
                Destroy(option.gameObject);
            }

            options.Clear();
        }

        if(buyCounts != null)
        {
           buyCounts.Clear();
        }
    }

 
}
