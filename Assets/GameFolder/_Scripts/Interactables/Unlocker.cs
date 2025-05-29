using SKC.AIF.NaughtyAttributes_2._1._4.Core.MetaAttributes;
using DG.Tweening;
using SKC.AIF.Helpers;
using SKC.AIF.Storage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SKC.AIF.Interactables
{
    public class Unlocker : MonoBehaviour
    {
		const int VISUAL_FEEDBACK_SPAWN_RATE_MAX = 40;

		[SerializeField, Tooltip("This will be called when enough resource spent.")]
		UnityEvent _onUnlocked;

		[SerializeField, Tooltip("Text which shows how much resource do we have need to unlock."), BoxGroup("UI")]
		TextMeshProUGUI _resourceCountText;

		[SerializeField, Tooltip("Image which shows how much resource do we have need to unlock."), BoxGroup("UI")]
		Image _progressBar;

		[SerializeField, Tooltip("Resource to spend when unlocking."), BoxGroup("Spending")]
		ObjectDefinition _neededResource;

		[SerializeField, BoxGroup("Spending")] Ease _spendingSpeedCurve;
		[SerializeField, Range(0f, 100f), BoxGroup("Spending")] float _spendingSpeed;

		[SerializeField, Tooltip("Amount of resource needed for unlocking."), Min(0), BoxGroup("Spending")]
		int _requiredResource;

		[SerializeField, Tooltip("If true, then it will be locked again when it's unlocked so it can be unlocked multiple times."), BoxGroup("Spending")]
		bool _workMultipleTimes;

		[SerializeField, Tooltip("controls how frequent resource object will be shown"), Range(1, VISUAL_FEEDBACK_SPAWN_RATE_MAX)]
		int _visualFeedbackSpawnRate;

		[SerializeField, Range(0f, 10f)] float _jumpHeight;
		[SerializeField, Range(0f, 3f)] float _jumpDuration;

		Storage.Storage _inventory;
		Tween _spendingTween;
		Coroutine _cor;
		WaitForSeconds _waitForSeconds;
		int _previousResourceSpentAmount;
		int _collectedResource;
		int _spawnCount;

		void Awake()
		{
			_waitForSeconds = new WaitForSeconds(0.1f);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Storage.Storage inventory))
			{
				_inventory = inventory;
				_cor = StartCoroutine(CheckInventory(inventory));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Storage.Storage _))
			{
				StopSpending();
				_inventory = null;
			}
		}

		void OnValidate()
		{
			_resourceCountText.text = _requiredResource.ToString();
		}

		public void SetRequiredResource(int requiredResource)
		{
			_collectedResource = 0;
			_previousResourceSpentAmount = 0;
			_progressBar.fillAmount = 0;
			_requiredResource = requiredResource;
			_resourceCountText.text = _requiredResource.ToString();
		}

		IEnumerator CheckInventory(Storage.Storage inventory)
		{
			while (true)
			{
				if (!inventory.Interactable)
				{
					yield return _waitForSeconds;
					continue;
				}

				int resourceAmount = _neededResource.Variable.RuntimeValue;
				TweenHelper.SpendResource(_requiredResource, _collectedResource, resourceAmount, out _spendingTween, _spendingSpeed, _spendingSpeedCurve, SpendMoney);
				yield break;
			}
		}

		void SpendMoney(int x)
		{
			int decreasingAmountDelta = x - _previousResourceSpentAmount;


			_neededResource.Variable.RuntimeValue -= decreasingAmountDelta;
			if (decreasingAmountDelta != 0)
			{
				_spawnCount++;
				if (_spawnCount >= VISUAL_FEEDBACK_SPAWN_RATE_MAX + 1 - _visualFeedbackSpawnRate)
				{
					ObjectItem item = _neededResource.Pool.TakeFromPool();
					Transform trans = item.transform;
					trans.position = _inventory.transform.position;
					TweenHelper.Jump(trans, transform.position, _jumpHeight, 1, _jumpDuration, item.ReleaseToPool);
					_spawnCount = 0;
				}
			}

			//			_resourceSpender.Spend(_neededResource, decreasingAmountDelta, transform);
			_collectedResource += decreasingAmountDelta;
			_resourceCountText.text = (_requiredResource - _collectedResource).ToString();
			_previousResourceSpentAmount = x;
			_progressBar.fillAmount = (float)_collectedResource / _requiredResource;


			if (_collectedResource == _requiredResource)
			{
				_onUnlocked?.Invoke();
				StopSpending();

				if (_workMultipleTimes)
				{
					_cor = StartCoroutine(CheckInventory(_inventory));
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}

		void StopSpending()
		{
			_spendingTween?.Kill();
			if (_cor != null)
			{
				StopCoroutine(_cor);
			}
		}
	}
}
