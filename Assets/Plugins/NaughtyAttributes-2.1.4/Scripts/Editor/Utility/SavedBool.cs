using UnityEditor;

namespace SKC.AIF.NaughtyAttributes_2._1._4.Editor.Utility
{
    internal class SavedBool
    {
        private bool _value;
        private string _name;

        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;
                EditorPrefs.SetBool(_name, value);
            }
        }

        public SavedBool(string name, bool value)
        {
            _name = name;
            _value = EditorPrefs.GetBool(name, value);
        }
    }
}