using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponStore : SceneToNextBase
{
   
    public override void Start()
    {
        base.Start();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("���퉮��");
        base.OnCollisionEnter2D(collision);
    }
}
