using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionSelectionUI : MonoBehaviour
{
    public static ActionSelectionUI Instance { get; private set; }
    //�ǂ����I�����Ă��邩�c������
    // Start is called before the first frame update
    SelectableText[] selectableTexts;
    [SerializeField] int selectedIndex = 0;

    public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    public void Init()
    {
        selectableTexts = GetComponentsInChildren<SelectableText>();//�����̎q�v�f����W�߂�
    }

    public void HandleActionUpdate()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))//�^���I�ɑI�����Ă���悤�Ɍ�����
        {
            selectedIndex++;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);
        for(int i = 0;i < selectableTexts.Length;i++)
        {
            if(selectedIndex == i)
            {
                selectableTexts[i].SetSelectedColor(true);
            }
            else
            {
                selectableTexts[i].SetSelectedColor(false);
            }
        }
    }

    public void Open()
    {
        selectedIndex = 0;
        gameObject.SetActive(true);
                 
        Debug.Log("Open was Implement");
    }

    public void Close()
    {
        
        gameObject.SetActive(false);
    }

    public void SetWhiteColor()//�퓬�J�n���ɂ͂Ȃɂ��I������Ă��Ȃ���Ԃɂ���
    {
        foreach (SelectableText text in selectableTexts)
        {
            text.SetSelectedColor(false);
        }
    }


}
