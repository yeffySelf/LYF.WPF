using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCustomControls
{
    public class CustomTreeViewItem : HeaderedItemsControl
    {
        static CustomTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTreeViewItem), new FrameworkPropertyMetadata(typeof(CustomTreeViewItem)));
        }


        #region Properties

        #region IsExpandedProperty::是否展开

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(
                    "IsExpanded",
                    typeof(bool),
                    typeof(CustomTreeViewItem),
                    new FrameworkPropertyMetadata(false, OnExpandedChanged));

        private static void OnExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomTreeViewItem treeViewItem = d as CustomTreeViewItem;
            if ((bool)e.NewValue)
            {
                RoutedEventArgs args = new RoutedEventArgs(ExpandedEvent, treeViewItem);
                treeViewItem.RaiseEvent(args);
            }
            else
            {
                RoutedEventArgs args = new RoutedEventArgs(CollapsedEvent, treeViewItem);
                treeViewItem.RaiseEvent(args);
            }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        #endregion

        #region IsSelectedProperty::是否选中

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                    "IsSelected",
                    typeof(bool),
                    typeof(CustomTreeViewItem),
                    new FrameworkPropertyMetadata(false));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        #region ExpandedEvent::展开事件

        public static readonly RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));

        public event RoutedEventHandler Expanded
        {
            add
            {
                AddHandler(ExpandedEvent, value);
            }

            remove
            {
                RemoveHandler(ExpandedEvent, value);
            }
        }

        #endregion

        #region CollapsedEvent::合起事件

        public static readonly RoutedEvent CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));

        public event RoutedEventHandler Collapsed
        {
            add
            {
                AddHandler(CollapsedEvent, value);
            }

            remove
            {
                RemoveHandler(CollapsedEvent, value);
            }
        }

        #endregion

        #region SelectedEvent::选中后事件

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));

        public event RoutedEventHandler Selected
        {
            add
            {
                AddHandler(SelectedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedEvent, value);
            }
        }

        #endregion

        #region 取消选中后事件

        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));

        public event RoutedEventHandler Unselected
        {
            add
            {
                AddHandler(UnselectedEvent, value);
            }

            remove
            {
                RemoveHandler(UnselectedEvent, value);
            }
        }

        #endregion


        #endregion

        #region Public Methods

        public void ExpandSubNode()
        {
            
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// 获取当前CustomTreeViewItem所属的CustomTreeView对象
        /// </summary>
        /// <returns></returns>
        internal CustomTreeView GetParentTreeView()
        {
            ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(this);
            while (parent != null)
            {
                CustomTreeView tv = parent as CustomTreeView;
                if (tv != null)
                {
                    return tv;
                }

                parent = ItemsControl.ItemsControlFromItemContainer(parent);
            }

            return null;
        }

        internal void SetIsSelect(CustomTreeViewItem treeViewItem,bool isSelected)
        {
            if (treeViewItem == null) return;
            CustomTreeView tree = GetParentTreeView();
            ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(treeViewItem);
            if (tree!=null && parent!=null)
            {
                object data = parent.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                tree.ChangeSelection(data, treeViewItem, isSelected);

                if (IsSelected)
                {
                    RoutedEventArgs args = new RoutedEventArgs(SelectedEvent, treeViewItem);
                    treeViewItem.RaiseEvent(args);
                }
                else
                {
                    RoutedEventArgs args = new RoutedEventArgs(UnselectedEvent, treeViewItem);
                    treeViewItem.RaiseEvent(args);
                }
            }
            if (tree.SelectMode != SelectionMode.Single)
            {
                treeViewItem.IsExpanded = true;
                treeViewItem.UpdateLayout();
                for (int i = 0; i < treeViewItem.Items.Count; i++)
                {
                    CustomTreeViewItem subItem = (CustomTreeViewItem)treeViewItem.ItemContainerGenerator.ContainerFromIndex(i);
                    SetIsSelect(subItem, isSelected);
                }
            }
        }

        #endregion

        #region ItemControl Implements
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled && IsEnabled)
            {
                if (Focus())
                {
                    CustomTreeView treeview = GetParentTreeView();
                    if (treeview.SelectMode != SelectionMode.Single)
                    {
                        SetIsSelect(this,!IsSelected);
                    }
                    else
                    {
                        if (!IsSelected)
                        {
                            SetIsSelect(this,true);
                        }
                    }

                    e.Handled = true;
                }

                if ((e.ClickCount % 2) == 0)
                {
                    IsExpanded = !IsExpanded;
                    e.Handled = true;
                }
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CustomTreeViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                    break;
                default:
                    throw new NotSupportedException("CustomTreeViewItem.OnItemsChanged Not Supported Action:" + e.Action.ToString());
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
        }



        #endregion
    }
}
