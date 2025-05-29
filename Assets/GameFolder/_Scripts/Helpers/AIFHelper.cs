using System;
using UnityEngine;
using SKC.AIF.Interactables;
using SKC.AIF.Storage;
using SKC.AIF.Save;
using ObjectItem = SKC.AIF.Storage.ObjectItem;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace SKC.AIF.Helpers
{
	public static class AIFHelper
	{
		/// <summary>
		/// Feed it with index and layout and it returns point in the layout. 
		/// </summary>
		/// <param name="currentIndex"></param>
		/// <param name="rowColumnHeight"></param>
		/// <returns></returns>
		public static Vector3 GetPoint(int currentIndex, RowColumnHeight rowColumnHeight)
		{
			float maxRowWidth = (rowColumnHeight.RowCount - 1) * rowColumnHeight.RowOffset;
			float maxColumnWidth = (rowColumnHeight.ColumnCount - 1) * rowColumnHeight.ColumnOffset;
			int columnIndex = currentIndex % rowColumnHeight.ColumnCount;
			int rowIndex = currentIndex / rowColumnHeight.ColumnCount % rowColumnHeight.RowCount;
			int heightIndex = currentIndex / (rowColumnHeight.RowCount * rowColumnHeight.ColumnCount);
			Vector3 up = Vector3.up * (rowColumnHeight.HeightOffset * heightIndex);
			Vector3 right = Vector3.right * (columnIndex * rowColumnHeight.ColumnOffset - maxColumnWidth / 2f);
			Vector3 forward = Vector3.forward * (rowIndex * rowColumnHeight.RowOffset - maxRowWidth / 2f);
			Vector3 targetPos = up + right + forward;
			return targetPos;
		}

        public static void MoveUI(Transform trans, Vector3 target, float duration, TweenCallback onComplete)
        {
            Sequence sequence = DOTween.Sequence().SetRecyclable();
            sequence.Append(trans.DOMove(target, duration * 0.3f));
            sequence.Join(trans.DOScale(Vector3.one, duration * 0.3f));
            sequence.Append(trans.DOScale(Vector3.one / 2f, duration).SetEase(Ease.InBack, 5f));
            sequence.Join(trans.DOLocalMove(Vector3.zero, duration * 0.7f).SetEase(Ease.InQuart));
            sequence.AppendCallback(onComplete);
        }
        
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }

    public static class TweenHelper
    {
        public static void JumpOrganized(ObjectItem item, Transform pivotPoint, RowColumnHeight rowColumnHeight, float jumpHeight, float jumpDuration, int index, TweenCallback onComplete)
        {
            Vector3 point = AIFHelper.GetPoint(index, rowColumnHeight);
            Vector3 adjustedPos = pivotPoint.TransformPoint(point);
            Jump(item.transform, adjustedPos, jumpHeight, 1, jumpDuration, onComplete);
        }

        public static void JumpOrganized(ObjectItem item, Transform pivotPoint, RowColumnHeight rowColumnHeight, float jumpHeight, float jumpDuration, int index)
        {
            Vector3 point = AIFHelper.GetPoint(index, rowColumnHeight);
            Vector3 adjustedPos = pivotPoint.TransformPoint(point);
            JumpAndRotate(item.transform, adjustedPos, Vector3.zero, jumpHeight, jumpDuration);
        }

        public static void JumpAndRotate(Transform item, Vector3 targetPosition, Vector3 targetRotation, float jumpPower, float duration)
        {
            item.DOJump(targetPosition, jumpPower, 1, duration).SetRecyclable().SetAutoKill();
            item.DOLocalRotate(targetRotation, duration).SetRecyclable().SetAutoKill();
        }

        public static void LocalJumpAndRotate(Transform item, Vector3 targetPosition, Vector3 targetRotation, float jumpPower, float duration)
        {
            item.DOLocalJump(targetPosition, jumpPower, 1, duration).SetRecyclable().SetAutoKill();
            item.DOLocalRotate(targetRotation, duration).SetRecyclable().SetAutoKill();
        }

        public static void LocalJumpAndRotate(Transform item, Vector3 targetPosition, Vector3 targetRotation, float jumpPower, float duration, TweenCallback onComplete)
        {
            item.DOLocalJump(targetPosition, jumpPower, 1, duration).SetRecyclable().SetAutoKill().OnComplete(onComplete);
            item.DOLocalRotate(targetRotation, duration).SetRecyclable().SetAutoKill();
        }

        public static void SpendResource(int requiredResource, int collectedResource, int myResource, out Tween resourceSpendingTween, float spendingSpeed,
                                         Ease resourceSpendEase, TweenCallback<int> onTweenUpdate)
        {
            int remainedMoney = requiredResource - collectedResource;
            int to = myResource >= remainedMoney ? requiredResource : collectedResource + myResource;
            resourceSpendingTween = DOVirtual.Int(collectedResource, to, (float)to / requiredResource * spendingSpeed, onTweenUpdate)
                .SetEase(resourceSpendEase).SetAutoKill();
        }

        public static void SetParentAndJump(this Transform transform, Transform to, Action onJumped)
        {
            transform.SetParent(to);
            transform.DOLocalJump(Vector3.zero, 1f, 1, 0.4f).SetRecyclable().SetAutoKill().OnComplete(() => onJumped?.Invoke());
            transform.DOScale(Vector3.kEpsilon, 0.4f).SetEase(Ease.InBack, 5f).SetRecyclable().SetAutoKill().OnComplete(() => transform.gameObject.SetActive(false));
        }

        public static Tweener LocalMove(Transform transform, Vector3 targetPoint, float duration)
        {
            return transform.DOLocalMove(targetPoint, duration).SetRecyclable();
        }

        public static Sequence Jump(Transform transform, Transform targetPoint)
        {
            return transform.DOJump(targetPoint.position, 1f, 1, 1f).SetRecyclable().SetAutoKill();
        }

        public static Sequence Jump(Transform transform, Vector3 targetPoint, float jumpPower, int numJumps, float duration)
        {
            return transform.DOJump(targetPoint, jumpPower, numJumps, duration).SetEase(Ease.Linear).SetRecyclable().SetAutoKill();
        }

        public static void Jump(Transform transform, Vector3 targetPoint, float jumpPower, int numJumps, float duration, TweenCallback onComplete)
        {
            transform.DOJump(targetPoint, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill().OnComplete(onComplete);
        }

        public static void ShowSlowly(Transform transform, Vector3 targetScale, float duration, TweenCallback onComplete)
        {
            transform.gameObject.SetActive(true);
            transform.DOScale(targetScale, duration).SetEase(Ease.OutBack, 2f).OnComplete(onComplete);
        }

        public static void DisappearSlowly(Transform transform)
        {
            transform.DOScale(Vector3.one * Mathf.Epsilon, 0.2f).SetAutoKill().SetRecyclable().OnComplete(() => { transform.gameObject.SetActive(false); });
        }

        public static Tween DisappearSlowly(Transform transform, float duration, TweenCallback onCompleted)
        {
            return transform.DOScale(Vector3.one * Mathf.Epsilon, duration).SetEase(Ease.InBack, 2f).SetAutoKill().SetRecyclable().OnComplete(onCompleted);
        }

        public static void CompleteAll(Transform transform)
        {
            transform.DOComplete();
        }

        public static void KillAllTweens(Transform transform)
        {
            transform.DOKill();
        }

        public static void DelayedCall(float delay, TweenCallback callback)
        {
            DOVirtual.DelayedCall(delay, callback);
        }

        public static void ShakeScale(Transform trans, Vector3 targetScale, float duration)
        {
            trans.DOShakeScale(duration, targetScale).SetRecyclable();
        }

        public static void PunchScale(Transform trans, Vector3 targetScale, float duration)
        {
            trans.DOPunchScale(targetScale, duration, 2).SetRecyclable();
        }
    }
    public abstract class TweenFeedback : ScriptableObject
    {
        [SerializeField, Range(0.02f, 3f)] protected float Duration = 0.5f;

        public void Play(Transform trans)
        {
            TweenHelper.CompleteAll(trans);
            OnTweening(trans);
        }

        protected abstract void OnTweening(Transform trans);
    }

    public static class SaveManagerHelper
    {
        public static List<SaveVariable> FindDuplicates(List<SaveVariable> inputList)
        {
            // Create a dictionary to store the count of each saveId
            Dictionary<string, int> countMap = new Dictionary<string, int>();

            // Iterate through the input list and count occurrences of each saveId
            foreach (SaveVariable saveable in inputList)
            {
                if (countMap.ContainsKey(saveable.SaveId))
                {
                    // If saveId already exists in dictionary, increment count
                    countMap[saveable.SaveId]++;
                }
                else
                {
                    // If saveId is encountered for the first time, add it to dictionary with count 1
                    countMap[saveable.SaveId] = 1;
                }
            }

            // Filter dictionary to include only Saveables with count > 1 (i.e., duplicates)
            List<SaveVariable> duplicates = inputList.Where(saveable => countMap[saveable.SaveId] > 1).ToList();

            return duplicates;
        }
    }

    public static class ItemUtil
    {
        public static void JumpToDisappearIntoPool(ObjectItem item, Vector3 targetPoint, float jumpPower, int numJumps, float duration)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(TweenHelper.Jump(item.transform, targetPoint, jumpPower, numJumps, duration));
            sequence.Append(TweenHelper.DisappearSlowly(item.transform, 0.2f, item.ReleaseToPool));
            sequence.SetAutoKill().SetRecyclable();
            sequence.Play();
        }
    }
}
