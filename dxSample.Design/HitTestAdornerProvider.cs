using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Bars;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Policies;
using Microsoft.Windows.Design.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace dxSample.Core.Design
{
    [UsesItemPolicy(typeof(HitTestAdornerSelectionPolicy))]
    public class HitTestAdornerProvider : AdornerProvider
    {
        DesignerView View { get; set; }
        ModelItem CurrentItem { get; set; }

        protected override void Activate(ModelItem item)
        {
            base.Activate(item);
            CurrentItem = item;
            View = DesignerView.FromContext(Context);
            View.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (View.RootView == null)
                return;
            var hitTestObject = GetHitTestObject(e.GetPosition(View));
            if (hitTestObject != null)
            {
                var service = Context.Services.GetRequiredService<ModelService>();
                var items = service.Find(CurrentItem.Root, typeof(BarItem));
                var modelItem = items.FirstOrDefault(x => x.GetCurrentValue() == hitTestObject);
                if (modelItem == null)
                    return;
                SelectionOperations.SelectOnly(Context, modelItem);
                e.Handled = true;
            }
        }
        object GetHitTestObject(Point mousePosition)
        {
            Point hitTestPoint = View.RootView.TransformFromVisual(View).Transform(mousePosition);
            return GetItemForSelect(HitTest(View.RootView, hitTestPoint));
        }
        object GetItemForSelect(IEnumerable<ViewItem> children)
        {
            var viewItem = children.Reverse().FirstOrDefault(x => x.ItemType == typeof(BarItemLinkInfo));
            if (viewItem == null)
                return null;
            return ((BarItemLinkInfo)viewItem.PlatformObject).Item;
        }
        IEnumerable<ViewItem> HitTest(ViewItem viewItem, Point hitTestPoint)
        {
            var hitTestResult = viewItem.HitTest(null, null, new PointHitTestParameters(hitTestPoint)).ViewHit;
            return GetSelfAndChildren(hitTestResult)
                .Where(x => x.RenderSizeBounds.Contains(viewItem.TransformToView(x).Transform(hitTestPoint)));
        }
        IEnumerable<ViewItem> GetSelfAndChildren(ViewItem item)
        {
            return new[] { item }.Concat(item.VisualChildren.SelectMany(GetSelfAndChildren));
        }
        protected override void Deactivate()
        {
            View.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            CurrentItem = null;
            base.Deactivate();
        }
        T FindVisualElement<T>(DependencyObject element) where T : FrameworkElement
        {
            var item = element;
            while (item != null)
            {
                if (item is T)
                    return (T)item;
                item = VisualTreeHelper.GetParent(item);
            }
            return null;
        }
    }

    public class HitTestAdornerSelectionPolicy : SelectionPolicy
    {
        protected override IEnumerable<ModelItem> GetPolicyItems(Selection selection)
        {
            for (var item = selection.PrimarySelection; item != null; item = item.Parent)
            {
                yield return item;
                if (item.IsItemOfType(typeof(DependencyObject)))
                    yield break;
            }
        }
    }
}
