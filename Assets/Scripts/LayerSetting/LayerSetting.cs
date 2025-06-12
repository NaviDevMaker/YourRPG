using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LayerSetting : ScriptableObject
{
    //壁にコライダーとレイヤーを設定する
    [SerializeField] LayerMask solidObjectLayer; //壁判定
    //[SerializeField] List<LayerMask> encountLayers;    //敵判定
    [SerializeField] LayerMask encountLayer;
    [SerializeField] List<LayerMask> simbolLayers;//ボス判定、今はBossはひとりだが拡張性も考えてリスト化    
    [SerializeField] List<LayerMask> enterLayer;//シーン遷移時の判定 
    [SerializeField] List<LayerMask> murabitoLayers;//村びとと会話するため
    public LayerMask SolidObjectLayer { get => solidObjectLayer; set => solidObjectLayer = value; }
    //public List<LayerMask> EncountLayers { get => encountLayers; set => encountLayers = value; }
    public List<LayerMask> SimbolLayers { get => simbolLayers; set => simbolLayers = value; }
    public List<LayerMask> EnterLayer { get => enterLayer; set => enterLayer = value; }
    public List<LayerMask> MurabitoLayers { get => murabitoLayers; set => murabitoLayers = value; }
    public LayerMask EncountLayer { get => encountLayer;}
    //public List<LayerMask> SignBoardLayers { get => signBoardLayers; set => signBoardLayers = value; }
}
