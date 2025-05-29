using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes_SpecialCase;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Worker
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Worker) + "/" + nameof(WorkerManager))]
    public class WorkerManager : ScriptableObject
    {
        [SerializeField] WorkerMovement workerMoverPrefab;

        [NonSerialized] readonly List<WorkerMovement> _activeEmployees = new List<WorkerMovement>();

        public void AssignEmployee(Vector3 loadPoint, Vector3 unloadPoint, float loadTime, float unloadTime, Vector3 instantiatingPoint)
        {
            var employee = Instantiate(workerMoverPrefab, instantiatingPoint, Quaternion.identity);
            employee.Initialize(loadPoint, unloadPoint, loadTime, unloadTime);
            _activeEmployees.Add(employee);
        }

        [Button]
        public void PauseAll()
        {
            foreach (WorkerMovement activeEmployee in _activeEmployees)
            {
                activeEmployee.Pause();
            }
        }

        [Button]
        public void ResumeAll()
        {
            foreach (WorkerMovement activeEmployee in _activeEmployees)
            {
                activeEmployee.Resume();
            }
        }
    }
}
