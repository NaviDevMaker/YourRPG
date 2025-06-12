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
            //�����ł��̊֐����g�����Ƃɂ���Ĕ�A�N�e�B�u�̃S�[���hUI�ŏo���Ȃ�Update���̏������\
            ManageGoldUI();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            
            Debug.Log("M�m�F");
            ManageStatusUI();
            ManageItemUI();

        }
        //CheckKeyCode();
    }
    void ManageGoldUI()//UI�\���̊Ǘ�
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
    public void SuspendMenu(bool BattleStart)//�o�g�����J�n�������Ƀ��j���[���J���Ȃ�����
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

    //����⓹��ŎQ�Ƃ��邽�߁A�����̃V���b�v����̃A�N�Z�X��itemUI�̂����I�u�W�F�N�g���V�[�����[�h���ɃA�N�e�B�u�łȂ��Ⴂ���Ȃ��̂ŕs���S
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
