using DevExpress.Xpf.Core;
using System.Windows;

namespace dxSample.Core
{
    public class CustomTabControl : DXTabControl {

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTabItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CustomTabItem;
        }
    }
}
