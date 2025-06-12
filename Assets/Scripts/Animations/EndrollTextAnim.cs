using DG.Tweening; // DOTween���g�p���ăX���[�Y�ȃX�N���[��������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class EndrollTextAnim : MonoBehaviour
{
    [SerializeField] List<Text> texts; // �e�L�X�g�I�u�W�F�N�g�̃��X�g
    [SerializeField] float borderLine; // �e�L�X�g��������Y���W�̋��E��
    [SerializeField] float duration; // �X�N���[�����x
    [SerializeField] RectTransform parentCanvas; // �e�L�����o�X�i�X�N���[���̊���W�j
    [SerializeField] EndrollContent endrollContent;

    [SerializeField] EndRollPlayerAnim endRollPlayerAnim;

    CancellationTokenSource cls = new CancellationTokenSource();
    CancellationToken clt;
    private Queue<string> contentQueue; // �e�L�X�g�����Ԃɏ������邽�߂̃L���[
    Text text;

    
    void Start()
    {
        Debug.Log("�X�^�[�g�G���f�B���O");

        contentQueue = new Queue<string>(endrollContent.LogContents); // ���X�g���L���[�ɕϊ�
        clt = cls.Token;
        StartCoroutine(StartRoll(clt)); // �G���h���[�����J�n
        endRollPlayerAnim.StartAnim(clt);
    }

    IEnumerator StartRoll(CancellationToken clt)
    {
        Debug.Log("�X�^�[�g�G���f�B���O");
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(RollCoroutine(clt));
    }

    IEnumerator RollCoroutine(CancellationToken clt)
    {
    
        foreach (var text in texts)
        {
            text.gameObject.SetActive(false); // �ŏ��͑S�Ĕ�\���ɂ��Ă���
        }

        while (contentQueue.Count > 0)
        {
            // �L���[���玟�̕�������擾
            string nextContent = contentQueue.Dequeue();

            StartCoroutine(GetNextAvailableText());
            // �\������Text�I�u�W�F�N�g���擾
            Text activeText = this.text;

            // �e�L�X�g��ݒ肵�ĕ\��
            activeText.text = nextContent;
            activeText.gameObject.SetActive(true);

            // �e�L�X�g����ʉ������ֈړ�
            RectTransform textTransform = activeText.GetComponent<RectTransform>();
            textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, -parentCanvas.rect.height);
            textTransform.DOAnchorPosY(borderLine, duration).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(1.0f);
            // �e�L�X�g���\���ɂ���
            //activeText.gameObject.SetActive(false);
        }

        Debug.Log("�I���");
        cls.Cancel();
    }

    IEnumerator GetNextAvailableText()
    {
        // ��\����Ԃ�Text�I�u�W�F�N�g��T���ĕԂ�
        foreach (var text in texts)
        {
            if (!text.gameObject.activeSelf)
            {
                this.text = text;
                yield break;
            }
        }

        //yield return new WaitForSeconds(0.5f);

        // �S�Ďg�p���̏ꍇ�͍ŏ��̃e�L�X�g���ė��p�i���S�̂��߂̃t�H�[���o�b�N�j
        for (int i = 0; i < texts.Count; i++)
        {
            if (i == texts.Count - 1)
            {
                this.text = texts[0];
                yield return new WaitForSeconds(1.0f);    
            }

            texts[i].gameObject.SetActive(false); // ��\���ɂ���


        }
        

        yield return null;
    }
}