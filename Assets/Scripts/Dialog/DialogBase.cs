using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogBase : MonoBehaviour
{
    
   

    //�ꕶ���Â��b�Z�[�W��\������
    [SerializeField] Text text;//�퓬���̃e�L�X�g
    [SerializeField] float letterPerSeconds;//�ꕶ���Â��͂���镶���̎��Ԃ̊Ԋu

    bool isTyping = false;
    public Text Text { get => text; set => text = value; }
    public bool IsTyping { get => isTyping; set => isTyping = value; }

    public virtual IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)//�������Ȃ����ɂ͎����I��true������
    {

        if (!keyOperate && text.text != "")
        {
            yield break;
        }

        if (text.text != "")
        {
            text.text = "";
        }
        

        foreach (char letter in line)//���ԍ��Ńe�L�X�g���o��
        {
                text.text += letter;
                yield return new WaitForSeconds(letterPerSeconds);
        }                   

        if (auto)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else //�X�y�[�X�L�[���������܂Ŏ��̃e�L�X�g�ɂ͐i�܂Ȃ��i���̏����܂ł����Ȃ�)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            text.text = "";

        }


    }
}
