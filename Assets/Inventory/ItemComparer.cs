using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Inventory
{
    internal class ItemComparer : IComparer<ItemSO>
    {
        public int Compare(ItemSO x, ItemSO y)
        {
       
            if (x.value == y.value)
            {
                return x.itemName.CompareTo(y.itemName);
            }
            return x.value.CompareTo(y.value);
        }
    }
}
