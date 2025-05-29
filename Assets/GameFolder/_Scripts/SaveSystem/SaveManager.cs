using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SKC.AIF.OdinSerializer;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes_SpecialCase;
using SKC.AIF.Helpers;
using UnityEngine;

namespace SKC.AIF.Save
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Save) + "/" + nameof(SaveManager))]
	public class SaveManager : ScriptableObject
	{
		const int SAVE_VERSION = 0;
		const DataFormat DATA_FORMAT = DataFormat.JSON;

		[SerializeField] List<SaveVariable> _saveables;

		SaveData _saveData = new SaveData();
		SaveUpgrader _saveUpgrader = new SaveUpgrader();
		string _savePath;

		public event Action RestoreCompleted;

		void OnEnable()
		{
			_savePath = Path.Combine(Application.persistentDataPath, "SKC-AIF-Save.json");
		}

		[Button("Find Duplicate Save Ids")]
		void OnValidate()
		{
			List<SaveVariable> duplicates = SaveManagerHelper.FindDuplicates(_saveables);
			if (duplicates.Count > 0)
			{
				string log = "<color=#00FF00>Duplicate Save Ids found in the Save Manager: </color>";
				foreach (SaveVariable saveable in duplicates)
				{
					log += "\n save id: " + saveable.SaveId + " in this object: " + saveable.name;
				}
				Debug.LogError(log);
			}
		}

		public void RestoreAll()
		{
			if (File.Exists(_savePath))
			{
				byte[] bytes = File.ReadAllBytes(_savePath);
				SaveData saveData = SerializationUtility.DeserializeValue<SaveData>(bytes, DATA_FORMAT);

				foreach (SaveVariable saveableSO in _saveables)
				{
					if (saveData.Saves.TryGetValue(saveableSO.SaveId, out object save))
					{
						if (save != null)
						{
							saveableSO.RestoreState(save);
						}
					}
				}

				_saveUpgrader.CheckAndUpgrade(saveData, SAVE_VERSION);
			}
			else
			{
				foreach (SaveVariable saveableSO in _saveables)
				{
					saveableSO.RestoreState(saveableSO.GetDefaultValue);
				}
			}

			RestoreCompleted?.Invoke();
		}

		public void SaveAll()
		{
			foreach (SaveVariable saveableSO in _saveables)
			{
				if (_saveData.Saves.ContainsKey(saveableSO.SaveId))
				{
					_saveData.Saves[saveableSO.SaveId] = saveableSO.CaptureState();
				}
				else
				{
					_saveData.Saves.Add(saveableSO.SaveId, saveableSO.CaptureState());
				}
			}
			_saveData.Version = SAVE_VERSION;
			byte[] convertedSaveData = SerializationUtility.SerializeValue(_saveData, DATA_FORMAT);
			File.WriteAllBytes(_savePath, convertedSaveData);
		}
	}
}
