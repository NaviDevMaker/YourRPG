using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RemoveJson_Test : MonoBehaviour
{

    const string fileName = "save.json";
    string pass => Path.Combine(Application.persistentDataPath, fileName);
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (File.Exists(pass)) File.Delete(pass);
        }
    }
}
