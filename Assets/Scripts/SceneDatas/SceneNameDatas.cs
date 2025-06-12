using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneNameDatas : ScriptableObject
{
    [SerializeField] List<string> sceneNames;

    public List<string> SceneNames { get => sceneNames;}
}
