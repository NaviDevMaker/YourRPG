using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnMurabitoAtTown : InnMurabitoBase
{
    bool hasStartedCoroutine = false;//再度宿の村人に触れるまで会話のダイアログを出さないため


    public static InnMurabitoAtTown Instance { get; private set; }//Textがシーン遷移後にnullになることを防ぐため

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーと接触したとき
        if (collision.gameObject == PlayerController.Instance.gameObject && !hasStartedCoroutine)
        {
            Debug.Log("プレイヤーとお話し！");
            StartCoroutine(base.StartOption());//宿での会話イベントを始める
            //StartCoroutine(StartInnAction());            
            //OptionDialog();
            //SelectedOption();
            hasStartedCoroutine = true;//会話イベントが始める、これがないと

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            hasStartedCoroutine = false;
        }
    }

    public override IEnumerator StartInnAction()
    {
        yield return base.StartInnAction();
    }
}
