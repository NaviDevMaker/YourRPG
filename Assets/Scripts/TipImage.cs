using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipImage : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
   public  void InitTexts(string EnterText)//�V�[����J�ڂ���ۂɃe�L�X�g�����邽�߂̃e�L�X�g�R���|�[�l���g���K�v�Ȃ���
   {
        text = GetComponentInChildren<Text>();
        text.text += EnterText;//�V�[���J�ڎ��̏����œ����ꂽ�e�L�X�g������B
      
   }

    
}
