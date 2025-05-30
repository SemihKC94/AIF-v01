using System.Collections.Generic;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Test
{
    public class ReorderableListTest : MonoBehaviour
    {
        [ReorderableList]
        public int[] intArray;

        [ReorderableList]
        public List<Vector3> vectorList;

        [ReorderableList]
        public List<SomeStruct> structList;

        [ReorderableList]
        public GameObject[] gameObjectsList;

        [ReorderableList]
        public List<Transform> transformsList;

        [ReorderableList]
        public List<MonoBehaviour> monoBehavioursList;
    }

    [System.Serializable]
    public struct SomeStruct
    {
        public int Int;
        public float Float;
        public Vector3 Vector;
    }
}
