using SKC.AIF.Save;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace SKC.AIF.Core
{
	public class GameBoot : MonoBehaviour
	{
		[SerializeField] SaveManager _saveManager;
		[SerializeField] Image _progressBarImage;

		public string LoadingLevelName;

		bool _saveFileRestored;

		void OnEnable()
		{
			_saveManager.RestoreCompleted += SaveManager_RestoreCompleted;
		}

		void OnDisable()
		{
			_saveManager.RestoreCompleted -= SaveManager_RestoreCompleted;
		}

		void SaveManager_RestoreCompleted()
		{
			_saveFileRestored = true;
		}

		IEnumerator Start()
		{
			yield return null;

			while (!_saveFileRestored)
			{
				yield return null;
			}

			// Load scene async after save file restored
			AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingLevelName);
			operation.allowSceneActivation = false;

			while (!operation.isDone)
			{
				_progressBarImage.fillAmount = operation.progress;

				if (operation.progress >= 0.9f)
				{
					operation.allowSceneActivation = true;
				}

				yield return null;
			}
		}
	}
}
