using System.Collections.Generic;
using dxSample.Core;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Policies;

namespace dxSample.Core.Design
{
    [UsesItemPolicy(typeof(TabItemSelectionPolicy))]
    public class CustomTabItemAdornerProvider : AdornerProvider
    {
        protected override void Activate(ModelItem item)
        {
            base.Activate(item);
            var tabItem = item.GetCurrentValue() as CustomTabItem;
            if (tabItem != null)
                tabItem.IsSelected = true;
        }
    }
    
    public class TabItemSelectionPolicy : SelectionPolicy
    {
        protected override IEnumerable<ModelItem> GetPolicyItems(Selection selection)
        {
            for (var item = selection.PrimarySelection; item != null; item = item.Parent)
            {
                yield return item;
                if (item.IsItemOfType(typeof(CustomTabItem)))
                    yield break;
            }
        }
    }
}
