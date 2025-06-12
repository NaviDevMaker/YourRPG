using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmName : DialogBase
{
    enum OptionStatus
    {
        Idle,
        Select,
        Execute,
    }

    PlayerController player;

    OptionStatus optionStatus;

    public int selectedIndex { get; private set; } = 0;
 
    [SerializeField] List<string> dialogContents;
    [SerializeField] Image dialogImage;
    [SerializeField] Image selectOptionImage;
    [SerializeField] List<Text> selectTexts;
   
   public bool IsPressed { get; private set; } = false;
    public List<string> DialogContents { get => dialogContents;}

    private void Start()
    {
        player = PlayerController.Instance;
        optionStatus = OptionStatus.Idle;
    }

    private void Update()
    {
        switch (optionStatus)
        {
            case OptionStatus.Idle:
                break;
            case OptionStatus.Select:
                SelectingOption();
                break;
            case OptionStatus.Execute:
                ExecuteOption();
                break;


        }


    }
   
    public void StartOptionSelect()
    {
        selectedIndex = 0;
        IsPressed = false;
        StartCoroutine(TypeDialogToSelect(dialogContents[0]));
    }
    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        dialogImage.gameObject.SetActive(true);
        yield return base.TypeDialog(line, auto, keyOperate);
        Text.text = line;       
    }

    public IEnumerator TypeDialogToSelect(string line, bool auto = true, bool keyOperate = true)
    {     
        yield return TypeDialog(line, auto, keyOperate);
        optionStatus = OptionStatus.Select;
    }
   
    void SelectingOption()
    {

        selectOptionImage.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
          
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //selectTexts[selectedIndex].color = Color.white;
            selectedIndex--;
            //selectTexts[selectedIndex].color = Color.yellow;

        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectTexts.Count - 1);

        for (int i = 0; i < selectTexts.Count; i++)
        {
            if (i == selectedIndex) selectTexts[i].color = Color.yellow;
            else selectTexts[i].color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPressed = true;
            foreach (var text in selectTexts)
            {
                text.color = Color.white;
            }
            optionStatus = OptionStatus.Execute;
        }

    }

    void ExecuteOption()
    {

        if (selectedIndex == 0)
        {

            optionStatus = OptionStatus.Idle;

            ChangeActive(false);
           
        }
        else
        {
            optionStatus = OptionStatus.Idle;
           
        }
    }

    public void ChangeActive(bool isActive)
    {
        dialogImage.gameObject.SetActive(false);
        selectOptionImage.gameObject.SetActive(false);
    }




}
