using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//他の村で別の村人がこれを行うために拡張性を考えてベース化
public class InnMurabitoBase : DialogBase
{

    int selectedIndex = 0;//選択されている選択とスペースを押されたときの選択を一致させるため
    SelectableText[] Responce = new SelectableText[2];//はい、いいえのニ択
    Image optionImage;//オプションのイメージ
    Image dialogImage;//ダイアログのイメージ
    private Fade fade;//フェイドのスクリプト

    public UnityAction OnSelectedYes;
    private void Start()
    {
       
        //シーン変更時に実行する
        SceneManager.activeSceneChanged += OnSceneChanged;
        //宿から出るとアタッチされているオブジェクトは存在しなくなる。そのときインスペクター上でのアタッチだと外れてしまうから。
        fade = GameObject.FindObjectOfType<Fade>().GetComponent<Fade>();
        if (fade != null) Debug.Log("なんで");
        //はい、いいえの選択肢のオブジェクトにアタッチされているスクリプトの取得をするため
        Transform innDiaTrans = InnDiaLogCanvas.Instance.gameObject.transform;
        Transform child = innDiaTrans.GetChild(1);
        for (int i = 0; i < Responce.Length; i++)
        {                    
            Responce[i] = child.GetChild(i).GetComponent<SelectableText>();

        }
        optionImage = InnDiaLogCanvas.Instance.gameObject.transform.Find("ResponceImage").GetComponent<Image>();
        dialogImage = InnDiaLogCanvas.Instance.gameObject.transform.Find("innDialogImage").GetComponent<Image>();       

    }
    //宿のダイアログの一連の流れ
    enum InnState
    {
        Idle,
        OptionAction,
        OptionEnd,
    }

    InnState innState;


    private void Update()
    {
        switch (innState)
        {
            case InnState.Idle://何も始まっていない状態
                break;
            case InnState.OptionAction:
                if (!isRunning) // コルーチンが実行中でない場合にのみ実行
                {
                    StartCoroutine(StartInnAction());
                }
                break;
            case InnState.OptionEnd:
                OptionEnd();
                break;
            

        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("下");
        }

    }

    bool isRunning = false; // コルーチンが実行中かどうかを判定するためのフラグ
    public virtual IEnumerator StartOption()//宿での選択の開始
    {

        PlayerController.Instance.Constraint = true;
        Debug.Log(PlayerController.Instance.Constraint);
        dialogImage.gameObject.SetActive(true);
        yield return OutputDialog("お休みになられますか？");
        Text.text = "お休みになられますか？";
        innState = InnState.OptionAction;


    }
    public virtual IEnumerator OutputDialog(string DialogContent)//ダイアログの出力
    {
        yield return base.TypeDialog(DialogContent, auto:false) ;
    }

    public void OptionDialog()//ダイアログの選択時の選択されているテキストの色の設定
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))//疑似的に選択しているように見せる
        {
            selectedIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, Responce.Length - 1);//値の範囲を設定
        for (int i = 0; i < Responce.Length; i++)
        {
            if (selectedIndex == i)
            {
                Responce[i].SetSelectedColor(true);
            }
            else
            {
                Responce[i].SetSelectedColor(false);
            }
        }

    }

    public virtual IEnumerator StartInnAction()//選択時に呼ばれる
    {
        selectedIndex = 0;//前回のテキストの選択をはずす
        optionImage.gameObject.SetActive(true);
        isRunning = true; // コルーチンが開始されるのでフラグを立てる
        bool optionSelected = false; 
        //選択されるまで繰り返す
        while (!optionSelected)
        {
            //選択中
            OptionDialog();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                optionSelected = true;
              

                if (selectedIndex == 0)
                {
                    PlayerController.Instance.Battler.HP = PlayerController.Instance.Battler.MaxHP;
                    Debug.Log("終わった！");
                    yield return StartCoroutine(ExecuteInnFade());//宿に泊まっているときの演出 、StopCoroutineを使うためにあえてこの呼び方をしている（Fade内で）                 
                    optionImage.gameObject.SetActive(false);
                    dialogImage.gameObject.SetActive(false);
                    yield return new WaitForSeconds(1.0f);//すぐに下のダイアログが始まるのを防ぐため
                    dialogImage.gameObject.SetActive(true);
                    yield return OutputDialog("ご無事を祈っています、みやた様");
                    dialogImage.gameObject.SetActive(false);

                }
                else if (selectedIndex == 1)
                {
                    optionImage.gameObject.SetActive(false);
                    dialogImage.gameObject.SetActive(false);
                }
            }
            yield return null; // フレームごとにループを継続
        }
        
        innState = InnState.OptionEnd;//選択の終了
        isRunning = false; // コルーチンが終了したのでフラグをリセット


    }

    void OptionEnd()
    {
        PlayerController.Instance.Constraint = false;
        innState = InnState.Idle;//ここでidelにもどさないとConstraintがずっとfalseのままになりシーン遷移中に動く

    }

    IEnumerator ExecuteInnFade()
    {
        fade.FadeIn(0.5f);
        OnSelectedYes?.Invoke();
        yield return new WaitForSeconds(4.0f);
        fade.FadeOut(0.5f);

    }

   void OnSceneChanged(Scene oldScene,Scene newScene)
   {
        //村びとにアタッチしたものを保存するためにこの処理。村人の見た目が場所によってか変わる場合下の処理は変更する（予定）
        if (newScene.name != "Inn")
        {
            this.gameObject.SetActive(false);
        }
        else if (newScene.name == "Inn")
        {
            this.gameObject.SetActive(true);
        }
    }

    


}
