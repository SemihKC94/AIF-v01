using System.Collections;
using System.Collections.Generic;
using SKC.AIF.Storage;
using UnityEngine;

namespace SKC.AIF.Customers
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Customers) + "/" + nameof(CustomerBase))]
    public class CustomerBase : ScriptableObject
    {
        public CustomerType customerType;
        public OrderInformation orderInformation;
        public int customerMoney => (int)customerType * 10;
    }

    [System.Serializable]
    public class OrderInformation
    {
        public ObjectDefinition _orderDefinition;
        public int Amount;
    }
}
