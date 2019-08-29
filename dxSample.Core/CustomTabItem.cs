using DevExpress.Xpf.Core;
using System.ComponentModel;

namespace dxSample.Core
{
    public class CustomTabItem : DXTabItem
    {
        public CustomTabItem()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Header = "TabItem";
            }
        }
    }
}
