using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetInActiveObj : MonoBehaviour
{
    public static GetInActiveObj Instence { get; private set; }

    private void Awake()
    {
        if(Instence == null)
        {
            Instence = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instence != null)
        {
            Destroy(this.gameObject);
        }
    }
    public T GetSpecifecObj<T>() where T: Component
    {
        // 現在のシーンを取得
        var scene = SceneManager.GetActiveScene();

        // ルートオブジェクトをすべて取得
        var rootObjects = scene.GetRootGameObjects();

        // 非アクティブを含むすべての子オブジェクトを検索
        foreach (var rootObject in rootObjects)
        {
            var result = rootObject.GetComponentInChildren<T>(true); // true で非アクティブも対象
            if (result != null)
                return result;
        }

        return null; // 見つからなかった場合
    }
}

