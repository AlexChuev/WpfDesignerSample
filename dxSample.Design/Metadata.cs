using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

[assembly: ProvideMetadata(typeof(dxSample.Core.Design.Metadata))]
namespace dxSample.Core.Design
{
    internal class Metadata : IProvideAttributeTable
    {
        public AttributeTable AttributeTable {
            get {
                var builder = new AttributeTableBuilder();
                builder.AddCustomAttributes(typeof(CustomTabItem),
                       new FeatureAttribute(typeof(CustomTabItemAdornerProvider)));
                builder.AddCustomAttributes(typeof(FrameworkElement),
                       new FeatureAttribute(typeof(HitTestAdornerProvider)));
                return builder.CreateTable();
            }
        }
    }
}
