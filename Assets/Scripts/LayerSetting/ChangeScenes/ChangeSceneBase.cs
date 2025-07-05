using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;//Directionary���g������


[System.Serializable]

public class ChangeSceneBase
{
    public enum TipTextType
    {
       Patern1,
       Patern2,
       Patern3,
       Patern4,
       Patern5,
    }

    Dictionary<TipTextType, string> tipTextTipes = new Dictionary<TipTextType, string>
    {
        { TipTextType.Patern1, "���œ���═��𔃂��܂�" },
        { TipTextType.Patern2, "�G��|���ƌo���l�𓾂���" },
        { TipTextType.Patern3, "�K�x�ɏh�ŋx�������܂��傤" },
        { TipTextType.Patern4, "�{�X��͓������܂���..." },
        { TipTextType.Patern5,"���A�����X�^�[�����݂���炵���ł�" }

    };

    //[SerializeField] ChangeSceneBase changeBase;
    [SerializeField] Image tipImage;//�`�b�v�̃C���[�W
    [SerializeField] GameObject LoadAnim;//�e�L�X�g��`�b�v�C���[�W�̐e�I�u�W�F�N�g
        
    [SerializeField] string sceneName; //�J�ڐ�̃V�[���̖��O
    [SerializeField] Fade fade;//�t�F�C�h�}�l�[�W���[
    //[SerializeField] int sceneIndex;
    [SerializeField]  List<Text> texts;//�A�j���[�V��������e�L�X�g
    [SerializeField]  float bounceDuration;
    [SerializeField]  int posY;  //�e�L�X�g�̏����ʒu

    //public int SceneIndex { get => sceneIndex; set => sceneIndex = value; }
 
    private List<Sequence> sequences = new List<Sequence>();
    public string SceneName { get => sceneName;}
    public int PosY { get => posY;}
    public Image TipImage { get => tipImage;}
    public List<Text> Texts { get => texts;}

    public static bool IsChangeScene { get; private set; }

    //public List<string> GetCurrennScenes1 { get => getCurrennScenes; set => getCurrennScenes = value; }
    //public ChangeSceneBase ChangeBase { get => changeBase; set => changeBase = value; }

    public virtual IEnumerator ChangeScene(ChangeSceneBase changeSceneBase)//�V�[���J�ڂ����s
    {
        IsChangeScene = true;
        Debug.Log(IsChangeScene);
        var randomKey = tipTextTipes.Keys.ElementAt(Random.Range(0, tipTextTipes.Count));
        tipImage.GetComponent<TipImage>().InitTexts(tipTextTipes[randomKey]);
        DOTween.Init();
        StopAllAnimations();//�A�j���[�V�����̊J�n�O�ɂ��ׂẴA�j���[�V�������~���邽��
        ResetTextPositions(changeSceneBase);//�e�L�X�g�������ʒu�ɖ߂�
        sequences.Clear();//���X�g���̃A�j���[�V��������ɂ���

        for (var i = 0; i < texts.Count; i++)//�e�L�X�g�̃o�E���h�A�j���[�V�������s��
        {
            LoadAnim.SetActive(true);
            changeSceneBase.TipImage.gameObject.SetActive(true);
            changeSceneBase.Texts[i].gameObject.SetActive(true);
            int BouncePos = PosY - 25;//�ړ�����ʒu��Pos.Y
            changeSceneBase.Texts[i].rectTransform.anchoredPosition = new Vector2((i - changeSceneBase.Texts.Count / 2) * 25 + 670, PosY);//800
            Sequence sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Restart)
                .SetDelay((bounceDuration / 2) * ((float)i / changeSceneBase.Texts.Count))
                .Append(changeSceneBase.Texts[i].rectTransform.DOAnchorPosY(BouncePos, bounceDuration / 4))
                .Append(changeSceneBase.Texts[i].rectTransform.DOAnchorPosY(PosY, changeSceneBase.bounceDuration / 4))
                 .AppendInterval((bounceDuration / 2) * ((float)(1 - i) / changeSceneBase.Texts.Count))
                 .SetDelay(1.0f);
            sequence.Play();
            sequences.Add(sequence);
        }
        
        //�t�F�[�h�C���̂��ƃV�[���J��
        fade.FadeIn(2f, () =>
        {
           
            LoadAnim.gameObject.SetActive(false);

            SceneManager.LoadSceneAsync(changeSceneBase.SceneName).completed += (asyncOperation) =>
            {//SceneManager.LoadSceneAsync(changeSceneBase.SceneName);
                fade.FadeOut(1.5f);
                foreach (var seq in sequences)//�V�[���J�ڌ�ɂ��s�v�ȃA�j���[�V�������c��Ȃ��悤�ɂ��邽��
                {
                    seq.Kill();
                }

                tipImage.GetComponentInChildren<Text>().text = " ";//�`�b�v�̃e�L�X�g������
            };

        });

        yield return new WaitForSeconds(4.0f);//���̌�Ƀv���C���[�͓�����悤�ɂȂ�
        IsChangeScene = false;
        Debug.Log(IsChangeScene);

    }

    private void StopAllAnimations()
    {
        // ���ׂĂ̊�����Sequence���~
        foreach (var seq in sequences)
        {
            seq.Kill();
        }
    }

    private void ResetTextPositions(ChangeSceneBase changeSceneBase)
    {
        for (var i = 0; i < changeSceneBase.Texts.Count; i++)
        {
            // �eText��RectTransform�������ʒu�Ƀ��Z�b�g
            changeSceneBase.Texts[i].rectTransform.anchoredPosition = new Vector2((i - changeSceneBase.Texts.Count / 2) * 25 + 800, PosY);
        }
    }

   



}

  
  

