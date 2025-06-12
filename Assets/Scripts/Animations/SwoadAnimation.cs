using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SwoadAnimation : MonoBehaviour
{
    [SerializeField] Image swoadImage;
    [SerializeField] Image mapImage;
    [SerializeField] Text mapText;
    // Start is called before the first frame update
    [SerializeField] float duration = 1f; // �A�j���[�V�����̎���
    [SerializeField] float returnDulation;//���̖߂�Ƃ��̃A�j���[�V����
    [SerializeField] float fadeDuration = 1f;//�t�F�C�h�̎���
    [SerializeField] float startXPosition = -500f; // �摜�̊J�n�ʒu�i�����ɃI�t�X�N���[���j
    [SerializeField] float endXPosition = 0f; // �摜�̏I���ʒu�i��ʓ��j
    [SerializeField] List<string> sceneNames;//�A�Z�b�g��̃V�[���̖��O
    [SerializeField] List<string> realSceneNames;//�n�܂�̑����A�Ȃǂ̖{���̃V�[���̖��O

    [SerializeField] AnimSceneNames animSceneNames;
    Vector2 originalSize;
    void Start()
    {
        originalSize = mapImage.rectTransform.sizeDelta;
        //ExecuteSwoadAnimation();
        SceneManager.sceneLoaded += OnSceceLoaded;
        //mapText.gameObject.SetActive(false);
        //mapImage.gameObject.SetActive(false);
        swoadImage.gameObject.SetActive(true);

        // �����ʒu��ݒ�
        swoadImage.rectTransform.anchoredPosition = new Vector2(startXPosition, swoadImage.rectTransform.anchoredPosition.y);
        mapText.rectTransform.anchoredPosition = new Vector2(mapText.rectTransform.anchoredPosition.x, mapText.rectTransform.anchoredPosition.y);
        // ������X���C�h�C������A�j���[�V����
       

    }

    void OnSceceLoaded(Scene scene,LoadSceneMode mode)
    {
        
        try
        {
            int sceneIndex = animSceneNames.SceneNames.IndexOf(scene.name);
            //ExecuteSwoadAnimation(animSceneNames.RealSceneNames[sceneIndex]);
            // Check if the scene name was found
            if (sceneIndex >= 0)
            {
                ExecuteSwoadAnimation(animSceneNames.RealSceneNames[sceneIndex]);
            }
            else
            {
                Debug.LogWarning($"Scene name '{scene.name}' not found in SceneNames list.");
            }

        }
        catch (System.Exception)
        {
            Debug.Log("�G���f�B���O");
        }


    }

    void ExecuteSwoadAnimation(string sceneName)
    {
        
        Vector2 newSize = originalSize;
            // �������ɉ����ăT�C�Y�𒲐�
        float newWidth = newSize.x + (sceneName.Length * 75f);
        mapImage.rectTransform.sizeDelta = new Vector2(newWidth, originalSize.y);
        
        //mapText.text = " ";
        swoadImage.rectTransform.DOAnchorPosX(endXPosition, duration)//SetEase(Ease.OutBounce)
           .OnComplete(() =>
           {
               
               // �e�L�X�g�ƃC���[�W�̓����x�����ɖ߂�
               mapText.DOFade(1f, 0f);  // �����x��������1�ɐݒ�
               mapImage.DOFade(1f, 0f); // �����x��������1�ɐݒ�
               mapImage.gameObject.SetActive(true);
               mapText.gameObject.SetActive(true);
               Debug.Log("Scene name: " + sceneName);
               mapText.text = sceneName;
               // �X���C�h�C��������������Ƀt�F�[�h�A�E�g���J�n
               Sequence sequence = DOTween.Sequence()
          .Append(swoadImage.rectTransform.DOAnchorPosX(endXPosition + 10, returnDulation).SetDelay(1f))
          .Append(swoadImage.rectTransform.DOAnchorPosX(startXPosition + 10, returnDulation));
               mapText.DOFade(0f, fadeDuration).SetDelay(1f);
               mapImage.DOFade(0f, fadeDuration).SetDelay(1f);
               
               
           });

        //mapImage.gameObject.SetActive(false);
        //mapText.gameObject.SetActive(false);

    }

}
