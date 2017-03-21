using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCustomControls
{
    public class CustomTextBoxWaterMarkAdorner : Adorner
    {
        private readonly VisualCollection _collection;

        private readonly CustomTextBox _textBox;

        internal readonly TextBlock _textBlock;
        public CustomTextBoxWaterMarkAdorner(CustomTextBox adornedElement) : base(adornedElement)
        {
            _collection = new VisualCollection(this);
            _textBlock = new TextBlock()
            {
                FontSize = adornedElement.FontSize,
                FontFamily = adornedElement.FontFamily,
                FontStyle = adornedElement.FontStyle,
                FontWeight = adornedElement.FontWeight,
                VerticalAlignment = adornedElement.VerticalContentAlignment,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(3,0,0,0),
                Cursor = System.Windows.Input.Cursors.IBeam
            };
            _textBlock.SetBinding(TextBlock.TextProperty, new Binding("WaterMark") { Source = adornedElement });
            _textBox = adornedElement;
            _collection.Add(_textBlock);
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _collection.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _collection[index];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBlock.Arrange(new Rect(0, 0, _textBox.DesiredSize.Width, _textBox.DesiredSize.Height));
            return finalSize;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            _textBox.Focus();
        }
    }

    public class CustomTextBoxBorderAdorner : Adorner
    {
        private readonly CustomTextBox _textBox;
        public CustomTextBoxBorderAdorner(CustomTextBox adornedElement) : base(adornedElement)
        {
            _textBox = adornedElement;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(null,new Pen(_textBox.MouseOverBorderBrush,1),new Rect(0,0,_textBox.DesiredSize.Width,_textBox.DesiredSize.Height));
        }
    }
}
