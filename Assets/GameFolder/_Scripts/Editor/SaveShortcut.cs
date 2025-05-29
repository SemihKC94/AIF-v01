using UnityEngine;
using UnityEditor;
using System.IO;

namespace SKC.AIF.Editor
{   
    public class SaveShortcut : EditorWindow
    {
        [MenuItem("AIF/Delete Save")]
        public static void DeleteSaveFunction()
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
            foreach (string filePath in filePaths) File.Delete(filePath);
        }
    }

}
