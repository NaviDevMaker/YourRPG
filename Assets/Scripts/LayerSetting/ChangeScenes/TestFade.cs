using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFade : MonoBehaviour
{
    [SerializeField] Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        fade.CutoutRange = 1.0f;
        fade.FadeOut(1.0f);
    }

   
}
