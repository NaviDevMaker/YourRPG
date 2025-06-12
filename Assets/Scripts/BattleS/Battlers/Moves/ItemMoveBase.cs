using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveBase : MoveBase
{
    [SerializeField] int prace;
    [SerializeField] HealType healType;

    public int Prace { get => prace; }
    public HealType HealType { get => healType;}
}

public enum HealType
{ 
    HP,
    MP,
}

