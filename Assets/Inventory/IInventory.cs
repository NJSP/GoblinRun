using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Inventory
{
    internal interface IInventory
    {
        void AddItem(ItemSO item);
        ItemSO DropLowestValueItem();
        List<ItemSO> GetItems();
    }
}
