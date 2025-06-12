using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MurabitoInfo : ScriptableObject
{
    [SerializeField] string murabitoDialogContent;
    [SerializeField] Sprite murabitoAppear;
    [SerializeField] Vector3 murabitoPos;
    [SerializeField] float waitTime;//‰½•b‚²‚Æ‚É•à‚­‚©
    //ˆê•à‚Å•à‚­•à”
    [SerializeField] int walkX;
    [SerializeField] int walkY;
    //‰½•à‚Å•ûŒü“]Š·‚·‚é‚©
    [SerializeField] int changeDirectionCount;//‰½•à‚Å•ûŒü“]Š·‚·‚é‚©
    [SerializeField] float currentTime;
    [SerializeField] int murabitoIndex;
    bool changeDirection;

    [SerializeField] int storeCount = 0;
   

    public string MurabitoDialogContent { get => murabitoDialogContent;}
    public Sprite MurabitoAppear { get => murabitoAppear;}
    public float WaitTime { get => waitTime;}
    public int WalkX { get => walkX;}
    public int WalkY { get => walkY;}
    public int ChangeDirectionCount { get => changeDirectionCount;}
    public float CurrentTime { get => currentTime; set => currentTime = value; }
    

    public int StoreCount { get => storeCount;}
    public int MurabitoIndex { get => murabitoIndex;}
    public Vector3 MurabitoPos { get => murabitoPos;}
    public bool ChangeDirection { get => changeDirection; set => changeDirection = value; }
}
