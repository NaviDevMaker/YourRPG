using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimSceneNames : ScriptableObject
{
    [SerializeField] List<string> sceneNames;//�A�Z�b�g��̃V�[���̖��O
    [SerializeField] List<string> realSceneNames;//�n�܂�̑����A�Ȃǂ̖{���̃V�[���̖��O

    public List<string> SceneNames { get => sceneNames;}
    public List<string> RealSceneNames { get => realSceneNames;}
}
