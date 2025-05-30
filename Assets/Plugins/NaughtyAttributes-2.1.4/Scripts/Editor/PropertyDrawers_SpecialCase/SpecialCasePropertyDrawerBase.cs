﻿using System;
using System.Collections.Generic;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes_SpecialCase;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.ValidatorAttributes;
using SKC.AIF.NaughtyAttributes_2._1._4.Editor.PropertyValidators;
using SKC.AIF.NaughtyAttributes_2._1._4.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Editor.PropertyDrawers_SpecialCase
{
    public abstract class SpecialCasePropertyDrawerBase
    {
        public void OnGUI(Rect rect, SerializedProperty property)
        {
            // Check if visible
            bool visible = PropertyUtility.IsVisible(property);
            if (!visible)
            {
                return;
            }

            // Validate
            ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
            foreach (var validatorAttribute in validatorAttributes)
            {
                validatorAttribute.GetValidator().ValidateProperty(property);
            }

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            bool enabled = PropertyUtility.IsEnabled(property);

            using (new EditorGUI.DisabledScope(disabled: !enabled))
            {
                OnGUI_Internal(rect, property, PropertyUtility.GetLabel(property));
            }

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {
                PropertyUtility.CallOnValueChangedCallbacks(property);
            }
        }

        public float GetPropertyHeight(SerializedProperty property)
        {
            return GetPropertyHeight_Internal(property);
        }

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
        protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
    }

    public static class SpecialCaseDrawerAttributeExtensions
    {
        private static Dictionary<Type, SpecialCasePropertyDrawerBase> _drawersByAttributeType;

        static SpecialCaseDrawerAttributeExtensions()
        {
            _drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>();
            _drawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;
        }

        public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr)
        {
            SpecialCasePropertyDrawerBase drawer;
            if (_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}
