using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LayerSetting : ScriptableObject
{
    //�ǂɃR���C�_�[�ƃ��C���[��ݒ肷��
    [SerializeField] LayerMask solidObjectLayer; //�ǔ���
    //[SerializeField] List<LayerMask> encountLayers;    //�G����
    [SerializeField] LayerMask encountLayer;
    [SerializeField] List<LayerMask> simbolLayers;//�{�X����A����Boss�͂ЂƂ肾���g�������l���ă��X�g��    
    [SerializeField] List<LayerMask> enterLayer;//�V�[���J�ڎ��̔��� 
    [SerializeField] List<LayerMask> murabitoLayers;//���тƂƉ�b���邽��
    public LayerMask SolidObjectLayer { get => solidObjectLayer; set => solidObjectLayer = value; }
    //public List<LayerMask> EncountLayers { get => encountLayers; set => encountLayers = value; }
    public List<LayerMask> SimbolLayers { get => simbolLayers; set => simbolLayers = value; }
    public List<LayerMask> EnterLayer { get => enterLayer; set => enterLayer = value; }
    public List<LayerMask> MurabitoLayers { get => murabitoLayers; set => murabitoLayers = value; }
    public LayerMask EncountLayer { get => encountLayer;}
    //public List<LayerMask> SignBoardLayers { get => signBoardLayers; set => signBoardLayers = value; }
}
