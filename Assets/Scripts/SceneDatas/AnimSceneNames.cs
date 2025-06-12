using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimSceneNames : ScriptableObject
{
    [SerializeField] List<string> sceneNames;//アセット上のシーンの名前
    [SerializeField] List<string> realSceneNames;//始まりの草原、などの本当のシーンの名前

    public List<string> SceneNames { get => sceneNames;}
    public List<string> RealSceneNames { get => realSceneNames;}
}
