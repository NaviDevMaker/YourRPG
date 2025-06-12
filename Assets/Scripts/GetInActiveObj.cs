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
        // ���݂̃V�[�����擾
        var scene = SceneManager.GetActiveScene();

        // ���[�g�I�u�W�F�N�g�����ׂĎ擾
        var rootObjects = scene.GetRootGameObjects();

        // ��A�N�e�B�u���܂ނ��ׂĂ̎q�I�u�W�F�N�g������
        foreach (var rootObject in rootObjects)
        {
            var result = rootObject.GetComponentInChildren<T>(true); // true �Ŕ�A�N�e�B�u���Ώ�
            if (result != null)
                return result;
        }

        return null; // ������Ȃ������ꍇ
    }
}

