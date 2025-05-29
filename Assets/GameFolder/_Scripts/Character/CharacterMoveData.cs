using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Data
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Data) + "/" + nameof(CharacterMoveData))]
    public class CharacterMoveData : ScriptableObject
    {
        public float SideMoveSpeed;
        public float ForwardMoveSpeed;
    }
}
