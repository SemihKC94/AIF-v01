using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Save
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Save) + "/" + nameof(IntValue))]
    public class IntValue : SaveVariable<int>
    {
        protected override void OnEnable()
        {
            RuntimeValue = (int)GetDefaultValue;
        }

        public override void RestoreState(object obj)
        {
            RuntimeValue = (int)obj;
        }
    }
}
