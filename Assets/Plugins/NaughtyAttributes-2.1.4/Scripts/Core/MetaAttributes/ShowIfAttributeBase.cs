﻿using System;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.Utility;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Core.MetaAttributes
{
    public class ShowIfAttributeBase : MetaAttribute
    {
        public string[] Conditions { get; private set; }
        public EConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum EnumValue { get; private set; }

        public ShowIfAttributeBase(string condition)
        {
            ConditionOperator = EConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public ShowIfAttributeBase(string enumName, Enum enumValue)
            : this(enumName)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");
            }

            EnumValue = enumValue;
        }
    }
}
