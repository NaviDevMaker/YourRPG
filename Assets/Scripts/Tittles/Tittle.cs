using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Tittle : MonoBehaviour
{

    [SerializeField] BattlerBase player;
    [SerializeField] InputField inputField;
    [SerializeField] SceneNameDatas sceneNameDatas;
    [SerializeField] RectTransform fieldRect;
    [SerializeField] ConfirmName confirmName;
    [SerializeField] ChangeSceneBase changeSceneBase;

    Vector2 originalScale;
    string playerName;

    Tween inputFieldAnim;

    private void Start()
    {
        WaitingInput();
    }
    public void GetPlayerName()
    {
        playerName = inputField.text.ToString();
        player.Name = playerName;
        inputField.interactable = false;

        StartCoroutine(ConfirmName());
     

        
    }

    public void InputtingName()
    {
        fieldRect.localScale = originalScale;
        inputFieldAnim.Kill();
    }

    public void Defocused()
    {
        WaitingInput();
    }
    public void WaitingInput()
    {
        originalScale = fieldRect.localScale;

        inputFieldAnim = fieldRect.DOScale(fieldRect.localScale * 1.3f, 1.0f)
           .SetEase(Ease.OutSine)
           .SetLoops(-1, LoopType.Yoyo);
    }

    IEnumerator ConfirmName()
    {
        confirmName.StartOptionSelect();
        yield return new WaitUntil(() => confirmName.IsPressed);

        if(confirmName.selectedIndex == 0)
        {
            yield return changeSceneBase.ChangeScene(changeSceneBase);
        }
        else if(confirmName.selectedIndex == 1)
        {
            yield return confirmName.TypeDialog(confirmName.DialogContents[1]);
            confirmName.ChangeActive(false);
        }

        inputField.interactable = true;
    }

}
