using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2SelectionUIBase : MonoBehaviour
{
    [SerializeField] protected RectTransform moveParent;//�퓬���ɐ�������UI�̐e
    [SerializeField] protected SelectableText textPrefab;//��������e�L�X�g
    protected List<SelectableText> selectableTexts = new List<SelectableText>();//���������e�L�X�g�����ɓ���Ă���

    public int selectedIndex { get; private set; } = 0;
    public int SelectedIndex { get => selectedIndex; }

    //�v���C���[���I�������I�v�V������\���A����키�̂ǂ���ɂ��Ή����邽�߂Ɉ����͓��
    public virtual void Init(List<Move> moves = null,List<ItemMoveBase> items = null)
    {
        if(moves != null)
        {
            //�����̎q�v�f����W�߂�
            SetMovesUISize(moves,items);
        }else
        {
            //�����̎q�v�f����W�߂�
            SetMovesUISize(moves,items);
        }
       


    }

    //UI�̃T�C�Y���e�L�X�g�̐��ɉ����ĕύX
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

    //�I��
    public virtual void HandleActionUpdate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))//�^���I�ɑI�����Ă���悤�Ɍ�����
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

    //UI�̕\��
    public virtual void Open()
    {
        selectedIndex = 0;
        gameObject.SetActive(true);
        Debug.Log("Open was Implement");
    }

    //UI�̔�\��
    public virtual void Close()
    {

        gameObject.SetActive(false);

    }

    //�O���UI�̍폜
    public virtual void DeleteMoveText()
    {
        foreach (var text in selectableTexts)
        {
            Destroy(text.gameObject);

        }

        selectableTexts.Clear();
    }


}

