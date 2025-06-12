using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecialBossHandler
{
    void HandleDefeat(int bosslayerIndex, SceneObjectManager sceneObjectManager);
}
