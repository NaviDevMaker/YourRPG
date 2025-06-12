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
    [SerializeField] float duration = 1f; // アニメーションの時間
    [SerializeField] float returnDulation;//剣の戻るときのアニメーション
    [SerializeField] float fadeDuration = 1f;//フェイドの時間
    [SerializeField] float startXPosition = -500f; // 画像の開始位置（左側にオフスクリーン）
    [SerializeField] float endXPosition = 0f; // 画像の終了位置（画面内）
    [SerializeField] List<string> sceneNames;//アセット上のシーンの名前
    [SerializeField] List<string> realSceneNames;//始まりの草原、などの本当のシーンの名前

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

        // 初期位置を設定
        swoadImage.rectTransform.anchoredPosition = new Vector2(startXPosition, swoadImage.rectTransform.anchoredPosition.y);
        mapText.rectTransform.anchoredPosition = new Vector2(mapText.rectTransform.anchoredPosition.x, mapText.rectTransform.anchoredPosition.y);
        // 横からスライドインするアニメーション
       

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
            Debug.Log("エンディング");
        }


    }

    void ExecuteSwoadAnimation(string sceneName)
    {
        
        Vector2 newSize = originalSize;
            // 文字数に応じてサイズを調整
        float newWidth = newSize.x + (sceneName.Length * 75f);
        mapImage.rectTransform.sizeDelta = new Vector2(newWidth, originalSize.y);
        
        //mapText.text = " ";
        swoadImage.rectTransform.DOAnchorPosX(endXPosition, duration)//SetEase(Ease.OutBounce)
           .OnComplete(() =>
           {
               
               // テキストとイメージの透明度を元に戻す
               mapText.DOFade(1f, 0f);  // 透明度をすぐに1に設定
               mapImage.DOFade(1f, 0f); // 透明度をすぐに1に設定
               mapImage.gameObject.SetActive(true);
               mapText.gameObject.SetActive(true);
               Debug.Log("Scene name: " + sceneName);
               mapText.text = sceneName;
               // スライドインが完了した後にフェードアウトを開始
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
