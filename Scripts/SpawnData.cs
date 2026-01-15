using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "Scirptable object/Spawn Data", order = int.MaxValue)]
public class SpawnData : ScriptableObject
{
    [Serializable]
    public struct StageSpawnData
    {
        //public Stage Stage;
        public int AreaNum;
    }
}
