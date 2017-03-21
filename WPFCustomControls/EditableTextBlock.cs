using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Data;

namespace WPFCustomControls
{
    public class EditableTextBlock : TextBlock
    {
        private EditableTextBlockBorderAdorner _borderAdorner;
        public bool IsInEditMode
        {
            get
            {
                return (bool)GetValue(IsInEditModeProperty);
            }
            set
            {
                SetValue(IsInEditModeProperty, value);
            }
        }

        private EditableTextBlockAdorner _adorner;

        // Using a DependencyProperty as the backing store for IsInEditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableTextBlock), new UIPropertyMetadata(false, IsInEditModeUpdate));

        /// <summary>
        /// Determines whether [is in edit mode update] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsInEditModeUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            EditableTextBlock textBlock = obj as EditableTextBlock;
            if (null != textBlock)
            {
                //Get the adorner layer of the uielement (here TextBlock)
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(textBlock);

                //If the IsInEditMode set to true means the user has enabled the edit mode then
                //add the adorner to the adorner layer of the TextBlock.
                if (textBlock.IsInEditMode)
                {
                    if (null == textBlock._adorner)
                    {
                        textBlock._adorner = new EditableTextBlockAdorner(textBlock);

                        //Events wired to exit edit mode when the user presses Enter key or leaves the control.
                        textBlock._adorner.TextBoxKeyUp += textBlock.TextBoxKeyUp;
                        textBlock._adorner.TextBoxLostFocus += textBlock.TextBoxLostFocus;
                    }
                    layer.Add(textBlock._adorner);
                }
                else
                {
                    //Remove the adorner from the adorner layer.
                    Adorner[] adorners = layer.GetAdorners(textBlock);
                    if (adorners != null)
                    {
                        foreach (Adorner adorner in adorners)
                        {
                            if (adorner is EditableTextBlockAdorner)
                            {
                                layer.Remove(adorner);
                            }
                        }
                    }

                    //Update the textblock's text binding.
                    BindingExpression expression = textBlock.GetBindingExpression(TextProperty);
                    if (null != expression)
                    {
                        expression.UpdateTarget();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of the max.
        /// </summary>
        /// <value>The length of the max.</value>
        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(EditableTextBlock), new UIPropertyMetadata(0));

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            IsInEditMode = false;
        }

        /// <summary>
        /// release the edit mode when user presses enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsInEditMode = false;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            IsInEditMode = true;
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

        private void SetBorder()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            if (_borderAdorner == null)
                _borderAdorner = new EditableTextBlockBorderAdorner(this);
            bool alreadyAdded = false;
            Adorner[] adorners = layer.GetAdorners(this);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    if (adorner is EditableTextBlockBorderAdorner)
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
                    if (adorner is EditableTextBlockBorderAdorner)
                    {
                        layer.Remove(adorner);
                    }
                }
            }
        }
    }
}