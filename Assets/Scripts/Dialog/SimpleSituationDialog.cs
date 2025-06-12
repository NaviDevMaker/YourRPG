using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSituationDialog : DialogBase
{
    [SerializeField] List<string> dialogContents;
    [SerializeField] Image dialogImage;

    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        return base.TypeDialog(line, auto, keyOperate);
    }

    public IEnumerator OutPutDialog()
    {
        dialogImage.gameObject.SetActive(true);
        for (int i = 0; i < dialogContents.Count; i++)
        {
            yield return TypeDialog(dialogContents[i], auto: false);
        }

        dialogImage.gameObject.SetActive(false);

    }
}
