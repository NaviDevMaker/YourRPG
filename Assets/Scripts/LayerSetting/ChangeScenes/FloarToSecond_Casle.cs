using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloarToSecond_Casle : DialogBase
{

    enum OptionStatus
    {
        Idle,
        Select,
        Execute,
    }

    PlayerController player;

    OptionStatus optionStatus;
    int casleEnterIndex = 1;//最初は条件をクリアしていないのでfalse
    int selectedIndex = 0;

    [SerializeField] Vector2 playerPos;
    [SerializeField] List<string> stairsDiaLogContent;
    [SerializeField] Image stairDialogImage;
    [SerializeField] Image selectOptionImage;
    [SerializeField] List<Text> selectText;
    [SerializeField] int sceneIndex;//次のフロアとなるシーンチェンジのインデックス

    private bool onCompletedTerm = false;
    public  bool OnCompletedTerm {set => onCompletedTerm = value; }
    //[SerializeField] ChangeSceneBase sceneToSecondFloar;

   


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
                StartCoroutine(ExecuteOption());
                break;


        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("みやた");
        if (collision.gameObject == PlayerController.Instance.gameObject && !IsTyping)
        {
            if(onCompletedTerm == true)
            {
                casleEnterIndex = 0;
            }
            
            if (casleEnterIndex == 0)
            {
                IsTyping = true;
                Debug.Log("tanomu");
                StartCoroutine(TypeDialog(stairsDiaLogContent[casleEnterIndex], auto: false));

            }
            else if (casleEnterIndex == 1)
            {
                IsTyping = true;
                StartCoroutine(TypeDialog(stairsDiaLogContent[casleEnterIndex], auto: false));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject) IsTyping = false;

    }
    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        PlayerController.Instance.Constraint = true;
        stairDialogImage.gameObject.SetActive(true);
        yield return base.TypeDialog(line, auto, keyOperate);
        Text.text = line;

        selectedIndex = 0;
        if (casleEnterIndex == 0)
        {
            optionStatus = OptionStatus.Select;
        }
        else
        {
            stairDialogImage.gameObject.SetActive(false);
            player.Constraint = false;
            optionStatus = OptionStatus.Idle;
        }


        //stairDialogImage.gameObject.SetActive(false);
    }

    void SelectingOption()
    {

        selectText[selectedIndex].color = Color.red;
        selectOptionImage.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectText[selectedIndex].color = Color.white;
            selectedIndex++;
            selectText[selectedIndex].color = Color.red;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectText[selectedIndex].color = Color.white;
            selectedIndex--;
            selectText[selectedIndex].color = Color.red;

        }

        Mathf.Clamp(selectedIndex, 0, selectText.Count);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var text in selectText)
            {
                text.color = Color.white;
            }
            optionStatus = OptionStatus.Execute;
        }

    }

    IEnumerator ExecuteOption()
    {

        if (selectedIndex == 0)
        {
            optionStatus = OptionStatus.Idle;
            player.transform.position = playerPos;
            yield return player.OpenSceneBases[sceneIndex].ChangeScene(player.OpenSceneBases[sceneIndex]);
            player.Constraint = false;
            stairDialogImage.gameObject.SetActive(false);
            selectOptionImage.gameObject.SetActive(false);
        }
        else
        {
            PlayerController.Instance.Constraint = false;
            optionStatus = OptionStatus.Idle;
            stairDialogImage.gameObject.SetActive(false);
            selectOptionImage.gameObject.SetActive(false);
        }
    }
}


