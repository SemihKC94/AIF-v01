using System;
using UnityEngine;

namespace SKC.AIF.Pool
{
	public class PoolObject : MonoBehaviour
	{
		public event Action Destroyed;

		void OnDestroy()
		{
			Destroyed?.Invoke();
		}
	}
}
