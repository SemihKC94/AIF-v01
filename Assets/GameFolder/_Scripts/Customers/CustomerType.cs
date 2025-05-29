using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Customers
{
    public enum CustomerType
    {
        Broke = 0,
        Poor = 1,
        Middling = 2,
        Rich = 3
    }

    [System.Serializable]
    public struct CustomerPath
    {
        public Vector3 _loadPoint;
        public float loadTime;
        public float unloadTime;
    }
}
