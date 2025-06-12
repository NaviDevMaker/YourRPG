using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EndrollContent : ScriptableObject
{
    [SerializeField,TextArea(1,24)] List<string> logContents;

    public List<string> LogContents { get => logContents;}
}
