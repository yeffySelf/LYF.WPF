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
    public class CustomTreeView : ItemsControl
    {
        private List<CustomTreeViewItem> _selectedContainers = new List<CustomTreeViewItem>();
        static CustomTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTreeView), new FrameworkPropertyMetadata(typeof(CustomTreeView)));
        }

        #region Properties

        #region SelectModeProperty::选择模式，单选还是多选

        public static readonly DependencyProperty SelectModeProperty = DependencyProperty.Register("SelectMode", typeof(SelectionMode), typeof(CustomTreeView), new PropertyMetadata(SelectionMode.Single));

        public SelectionMode SelectMode
        {
            get { return (SelectionMode)GetValue(SelectModeProperty); }
            set { SetValue(SelectModeProperty, value); }
        }

        #endregion

        #region SelectedItemsProperty::选中项列表(只读属性)
        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(List<object>), typeof(CustomTreeView),new PropertyMetadata(new List<object>()));

        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;
        public List<object> SelectedItems
        {
            get { return (List<object>)GetValue(SelectedItemsProperty); }
        }

        #endregion

        #region CheckgBoxStyleProperty::复选框样式

        public static readonly DependencyProperty CheckgBoxStyleProperty = DependencyProperty.Register("CheckgBoxStyle",typeof(Style),typeof(CustomTreeView));
        public Style CheckgBoxStyle
        {
            get { return (Style)GetValue(CheckgBoxStyleProperty); }
            set { SetValue(CheckgBoxStyleProperty, value); }
        }

        #endregion

        #region ExpanderButtonStyleProperty::收缩/展开样式

        public static readonly DependencyProperty ExpanderButtonStyleProperty = DependencyProperty.Register("ExpanderButtonStyle", typeof(Style), typeof(CustomTreeView));
        public Style ExpanderButtonStyle
        {
            get { return (Style)GetValue(ExpanderButtonStyleProperty); }
            set { SetValue(ExpanderButtonStyleProperty, value); }
        }

        #endregion

        #region TreeLineStyleProperty::连接线样式

        public static readonly DependencyProperty TreeLineStyleProperty = DependencyProperty.Register("TreeLineStyle", typeof(Style), typeof(CustomTreeView));
        public Style TreeLineStyle
        {
            get { return (Style)GetValue(TreeLineStyleProperty); }
            set { SetValue(TreeLineStyleProperty, value); }
        }

        #endregion

        #region 是否显示连接线

        public static readonly DependencyProperty ShowTreeLineProperty = DependencyProperty.Register("ShowTreeLine", typeof(bool), typeof(CustomTreeView));
        public bool ShowTreeLine
        {
            get { return (bool)GetValue(ShowTreeLineProperty); }
            set { SetValue(ShowTreeLineProperty, value); }
        }

        #endregion

        #endregion

        #region Internal Methods

        internal void ChangeSelection(object data, CustomTreeViewItem container, bool? selected)
        {
            if (SelectMode == SelectionMode.Single)
            {
                if (selected.Value)
                {//单选时选中
                    _selectedContainers.ForEach(r =>
                    {
                        r.IsSelected = false;
                    });
                    _selectedContainers.Clear();
                    container.IsSelected = selected.Value;
                    _selectedContainers.Add(container);
                    SetValue(SelectedItemsPropertyKey, new List<object>() { data });
                }
            }
            else if (SelectMode != SelectionMode.Single)
            {
                container.IsSelected = selected;
                if (selected.HasValue)
                {
                    if (selected.Value)
                    {//多选选中
                        if (_selectedContainers.Contains(container) == false)
                        {
                            _selectedContainers.Add(container);
                        }
                        var selItems = SelectedItems;
                        if (selItems.Contains(data) == false)
                        {
                            selItems.Add(data);
                            SetValue(SelectedItemsPropertyKey, selItems);
                        }
                    }
                    else
                    {
                        if (_selectedContainers.Contains(container))
                        {
                            _selectedContainers.Remove(container);
                        }
                        var selItems = SelectedItems;
                        if (selItems.Contains(data))
                        {
                            selItems.Remove(data);
                            SetValue(SelectedItemsPropertyKey, selItems);
                        }
                    }
                }
            }
            RoutedEventArgs args = new RoutedEventArgs(SelectedItemsChangedEvent, this);
            RaiseEvent(args);
        }

        #endregion

        #region Public Methods

        public void ExpandAllNodes()
        {

        }

        public void ExpandRootNode()
        {

        }

        #endregion

        #region Events

        public static readonly RoutedEvent SelectedItemsChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemsChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeView));

        public event RoutedEventHandler SelectedItemsChanged
        {
            add
            {
                AddHandler(SelectedItemsChangedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedItemsChangedEvent, value);
            }
        }

        #endregion

        #region ItemControl Implements

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
                    throw new NotSupportedException("CustomTreeView.OnItemsChanged Not Supported Action:" + e.Action.ToString());
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
