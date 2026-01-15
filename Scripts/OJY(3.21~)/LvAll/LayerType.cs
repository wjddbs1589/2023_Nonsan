using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerType : MonoBehaviour
{
    public LayerTypeEnum TypeOfLayer = LayerTypeEnum.Enemy;

    [System.Serializable]
    public enum LayerTypeEnum
    {
        Plaster,
        Metal,
        Foliage,
        Rock,
        Wood,
        Brick,
        Concrete,
        Dirt,
        Glass,
        Water,
        Enemy,
        CarEnemy
    }
}
