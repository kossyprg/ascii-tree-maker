using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AsciiTreeMaker.UI.Helpers
{
    /// <summary>
    /// TextBox ヘルパー
    /// </summary>
    /// <seealso cref="https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-add-a-watermark-to-a-textbox">
    /// 公式ドキュメント
    /// </seealso>
    public static class TextBoxHelper
    {
        public static string GetPlaceholder(DependencyObject obj) =>
            (string)obj.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(DependencyObject obj, string value) =>
            obj.SetValue(PlaceholderProperty, value);

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnPlaceholderChanged)
                );

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBoxControl)
            {
                if (!textBoxControl.IsLoaded)
                {
                    // Ensure that the events are not added multiple times
                    textBoxControl.Loaded -= TextBoxControl_Loaded;
                    textBoxControl.Loaded += TextBoxControl_Loaded;
                }

                textBoxControl.TextChanged -= TextBoxControl_TextChanged;
                textBoxControl.TextChanged += TextBoxControl_TextChanged;

                // If the adorner exists, invalidate it to draw the current text
                if (GetOrCreateAdorner(textBoxControl, out PlaceholderAdorner? adorner))
                {
                    adorner.InvalidateVisual();
                }
            }
        }

        private static void TextBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBoxControl)
            {
                textBoxControl.Loaded -= TextBoxControl_Loaded;
                //GetOrCreateAdorner(textBoxControl, out _);

                // 初期状態でテキストが入っている可能性があるため adorner の Visibility を更新する
                if (GetOrCreateAdorner(textBoxControl, out var adorner))
                {
                    UpdateAdornerVisibility(textBoxControl, adorner);
                }
            }
        }

        private static void TextBoxControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBoxControl
                && GetOrCreateAdorner(textBoxControl, out PlaceholderAdorner? adorner))
            {
                UpdateAdornerVisibility(textBoxControl, adorner);
            }
        }

        /// <summary>
        /// TextBox の AdornerLayer から PlaceholderAdorner を取得する。
        /// 存在しない場合は新規作成して追加する。
        /// </summary>
        /// <param name="textBoxControl">対象の TextBox</param>
        /// <param name="adorner">
        /// 取得または生成された Adorner。
        /// メソッドの戻り値が true の場合、[NotNullWhen(true)] により null ではないことが保証される。
        /// </param>
        /// <returns>AdornerLayer が取得でき、Adorner を取得または生成できた場合は true</returns>
        private static bool GetOrCreateAdorner(
            TextBox textBoxControl,
            [NotNullWhen(true)] out PlaceholderAdorner? adorner)
        {
            // Get the adorner layer
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(textBoxControl);

            // If null, it doesn't exist or the control's template isn't loaded
            if (layer == null)
            {
                adorner = null;
                return false;
            }

            // Layer exists, try to find the adorner
            adorner = layer.GetAdorners(textBoxControl)?.OfType<PlaceholderAdorner>().FirstOrDefault();

            // Adorner never added to control, so add it
            if (adorner == null)
            {
                adorner = new PlaceholderAdorner(textBoxControl);
                layer.Add(adorner);
            }

            return true;
        }

        private static void UpdateAdornerVisibility(TextBox textBoxControl, PlaceholderAdorner adorner)
        {
            if (textBoxControl.Text.Length > 0)
            {
                // Control has text. Hide the adorner.
                adorner.Visibility = Visibility.Hidden;
            }
            else
            {
                // Control has no text. Show the adorner.
                adorner.Visibility = Visibility.Visible;
            }
        }

        public class PlaceholderAdorner : Adorner
        {
            public PlaceholderAdorner(TextBox textBox) : base(textBox) { }

            protected override void OnRender(DrawingContext drawingContext)
            {
                TextBox textBoxControl = (TextBox)AdornedElement;

                string placeholderValue = TextBoxHelper.GetPlaceholder(textBoxControl);

                if (string.IsNullOrEmpty(placeholderValue))
                    return;

                // Create the formatted text object
                FormattedText text = new FormattedText(
                                            placeholderValue,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                            textBoxControl.FlowDirection,
                                            new Typeface(textBoxControl.FontFamily,
                                                         textBoxControl.FontStyle,
                                                         textBoxControl.FontWeight,
                                                         textBoxControl.FontStretch),
                                            textBoxControl.FontSize,
                                            SystemColors.InactiveCaptionBrush,
                                            VisualTreeHelper.GetDpi(textBoxControl).PixelsPerDip);

                text.MaxTextWidth = System.Math.Max(textBoxControl.ActualWidth - textBoxControl.Padding.Left - textBoxControl.Padding.Right, 10);
                text.MaxTextHeight = System.Math.Max(textBoxControl.ActualHeight, 10);

                // Render based on padding of the control, to try and match where the textbox places text
                Point renderingOffset = new Point(textBoxControl.Padding.Left, textBoxControl.Padding.Top);

                // Template contains the content part; adjust sizes to try and align the text
                if (textBoxControl.Template.FindName("PART_ContentHost", textBoxControl) is FrameworkElement part)
                {
                    Point partPosition = part.TransformToAncestor(textBoxControl).Transform(new Point(0, 0));
                    renderingOffset.X += partPosition.X;
                    renderingOffset.Y += partPosition.Y;

                    text.MaxTextWidth = System.Math.Max(part.ActualWidth - renderingOffset.X, 10);
                    text.MaxTextHeight = System.Math.Max(part.ActualHeight, 10);
                }

                // Draw the text
                drawingContext.DrawText(text, renderingOffset);
            }
        }
    }
}
