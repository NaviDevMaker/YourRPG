using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemStore : SceneToNextBase
{

    public override void Start()
    {
        base.Start();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("è§ìXÇ÷");
        base.OnCollisionEnter2D(collision);
    }
}