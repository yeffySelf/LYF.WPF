using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCustomControls
{
    public class CustomTextBox:TextBox
    {
        private CustomTextBoxWaterMarkAdorner _adorner = null;
        private CustomTextBoxBorderAdorner _borderAdorner = null;
        private Brush normalBrush;
        public CustomTextBox()
        {
            normalBrush = this.BorderBrush;
        }

        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(CustomTextBox));

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(CustomTextBox),new PropertyMetadata(""));

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetWaterMark();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            SetWaterMark();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            SetBorder();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            ClearBorder();
        }

        #region internal method

        private void SetWaterMark()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            if (string.IsNullOrWhiteSpace(this.Text) && !string.IsNullOrWhiteSpace(this.WaterMark))
            {
                //添加装饰器
                if (_adorner == null)
                {
                    _adorner = new CustomTextBoxWaterMarkAdorner(this);
                    _adorner._textBlock.MouseEnter += _textBlock_MouseEnter;
                }
                
                bool alreadyAdded = false;
                Adorner[] adorners = layer.GetAdorners(this);
                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        if (adorner is CustomTextBoxWaterMarkAdorner)
                        {
                            alreadyAdded = true;
                        }
                    }
                }
                if (!alreadyAdded)
                    layer.Add(_adorner);
            }
            else
            {
                //移除已添加的装饰器
                Adorner[] adorners = layer.GetAdorners(this);
                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        if (adorner is CustomTextBoxWaterMarkAdorner)
                        {
                            layer.Remove(adorner);
                        }
                    }
                }
            }
        }

        private void _textBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            SetBorder();
        }

        private void SetBorder()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            if (_borderAdorner == null)
                _borderAdorner = new CustomTextBoxBorderAdorner(this);
            bool alreadyAdded = false;
            Adorner[] adorners = layer.GetAdorners(this);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    if (adorner is CustomTextBoxBorderAdorner)
                    {
                        alreadyAdded = true;
                    }
                }
            }
            if (!alreadyAdded)
                layer.Add(_borderAdorner);
        }

        private void ClearBorder()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            Adorner[] adorners = layer.GetAdorners(this);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    if (adorner is CustomTextBoxBorderAdorner)
                    {
                        layer.Remove(adorner);
                    }
                }
            }
        }

        #endregion
    }
}
