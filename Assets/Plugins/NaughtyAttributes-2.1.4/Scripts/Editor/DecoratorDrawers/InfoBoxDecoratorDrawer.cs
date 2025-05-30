﻿using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes;
using SKC.AIF.NaughtyAttributes_2._1._4.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Editor.DecoratorDrawers
{
    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    public class InfoBoxDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return GetHelpBoxHeight();
        }

        public override void OnGUI(Rect rect)
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;

            float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
            Rect infoBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
        }

        private float GetHelpBoxHeight()
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, string infoText, EInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case EInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case EInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case EInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            NaughtyEditorGUI.HelpBox(rect, infoText, messageType);
        }
    }
}
