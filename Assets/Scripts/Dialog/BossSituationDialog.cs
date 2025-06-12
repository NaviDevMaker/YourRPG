using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSituationDialog : DialogBase
{
    [SerializeField] List<string> dialogContents;
    [SerializeField] Image dialogImage;
    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        return base.TypeDialog(line, auto, keyOperate);
    }

    public IEnumerator SituationTypeDialog(int index)
    {
        dialogImage.gameObject.SetActive(true);

        switch (index)
        {
            case 0:
                yield return TypeDialog(dialogContents[0], auto: false);
                break;
            case 1:
                yield return TypeDialog(dialogContents[1], auto: false);
                yield return TypeDialog(dialogContents[2], auto: false);
                break;

        }

        
        dialogImage.gameObject.SetActive(false);

    }
}
