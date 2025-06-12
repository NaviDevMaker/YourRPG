using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Barrier_Child : DialogBase
{
    public UnityAction<Barrier_Child> OnDestroyedBarrier;
    [SerializeField] string dialogContent;
    [SerializeField] Image dialogImage;
    PlayerController player;
    private void Start()
    {
        player = PlayerController.Instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {
            StartCoroutine(TypeDialog(dialogContent, auto: false));
        }
    }

    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        Debug.Log("ê¨å˜");

        //BossEncount.Instance.gameObject.SetActive(true);
        player.Constraint = true;
        dialogImage.gameObject.SetActive(true);
        yield return base.TypeDialog(line, auto, keyOperate);
        player.Constraint = false;
        OnDestroyedBarrier?.Invoke(this);
        dialogImage.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
   
}
