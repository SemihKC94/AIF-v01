﻿using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.MetaAttributes;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Test
{
    public class FoldoutTest : MonoBehaviour
    {
        [Foldout("Integers")]
        public int int0;
        [Foldout("Integers")]
        public int int1;

        [Foldout("Floats")]
        public float float0;
        [Foldout("Floats")]
        public float float1;

        [Foldout("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider0;
        [Foldout("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [Foldout("Transforms")]
        public Transform trans0;
        [Foldout("Transforms")]
        public Transform trans1;
    }
}
