using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFCustomControls
{
    public class CustomTextBox:TextBox
    {
        public CustomTextBox()
        {
            
        }

        public static readonly DependencyProperty AllowWaterMarkProperty = DependencyProperty.Register("AllowWaterMark", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false, OnAllowWaterMarkChanged));

        private static void OnAllowWaterMarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomTextBox ctb = d as CustomTextBox;
            ctb.SetWaterMark((bool)e.NewValue, ctb.WaterMark ?? "");
        }

        public bool AllowWaterMark
        {
            get { return (bool)GetValue(AllowWaterMarkProperty); }
            set { SetValue(AllowWaterMarkProperty,value); }
        }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(CustomTextBox),new PropertyMetadata("",OnWaterMarkChanged));

        private static void OnWaterMarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomTextBox ctb = d as CustomTextBox;
            ctb.SetWaterMark(ctb.AllowWaterMark, e.NewValue ?? "");
        }

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        #region internal method

        private void SetWaterMark(bool allowWaterMark,object waterMark)
        {

        }

        #endregion
    }
}
