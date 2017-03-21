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
        private CustomTextBoxAdorner _adorner = null;
        public CustomTextBox()
        {
            
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

        #region internal method

        private void SetWaterMark()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            if (string.IsNullOrWhiteSpace(this.Text) && !string.IsNullOrWhiteSpace(this.WaterMark))
            {
                //添加装饰器
                if (_adorner == null)
                {
                    _adorner = new CustomTextBoxAdorner(this);
                }
                
                bool alreadyAdded = false;
                Adorner[] adorners = layer.GetAdorners(this);
                if (adorners != null)
                {
                    foreach (var adorner in adorners)
                    {
                        if (adorner is CustomTextBoxAdorner)
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
                        if (adorner is CustomTextBoxAdorner)
                        {
                            layer.Remove(adorner);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
