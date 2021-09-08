using AudioBand.TextFormatting;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AudioBand.UI
{
    /// <summary>
    /// Attached property for text segments.
    /// </summary>
    public static class TextBlockTextSegments
    {
        /// <summary>
        /// Dependency property for text segments.
        /// </summary>
        public static readonly DependencyProperty TextSegmentsProperty
            = DependencyProperty.RegisterAttached("TextSegments", typeof(IEnumerable<TextSegment>), typeof(TextBlockTextSegments), new PropertyMetadata(TextSegmentsPropertyChangedCallback));

        private static readonly ColorToBrushConverter ColorToBrushConverter = new ColorToBrushConverter();

        /// <summary>
        /// Gets the text segments property.
        /// </summary>
        /// <param name="textBlock">The text block.</param>
        /// <returns>The text segments.</returns>
        public static IEnumerable<TextSegment> GetTextSegments(TextBlock textBlock)
        {
            return (IEnumerable<TextSegment>)textBlock.GetValue(TextSegmentsProperty);
        }

        /// <summary>
        /// Sets the text segments property.
        /// </summary>
        /// <param name="textBlock">The text block.</param>
        /// <param name="textSegments">The text segments.</param>
        public static void SetTextSegments(TextBlock textBlock, IEnumerable<TextSegment> textSegments)
        {
            textBlock.SetValue(TextSegmentsProperty, textSegments);
        }

        private static IEnumerable<Inline> CreateInlines(IEnumerable<TextSegment> segments)
        {
            var inlines = new List<Inline>();

            foreach (var textSegment in segments)
            {
                var textBinding = new Binding(nameof(TextSegment.Text)) { Source = textSegment };
                var colorBinding = new Binding(nameof(TextSegment.Color)) { Source = textSegment, Converter = Converters.ColorToBrush };
                var run = new Run();
                run.SetBinding(Run.TextProperty, textBinding);
                run.SetBinding(TextElement.ForegroundProperty, colorBinding);
                if (textSegment.Flags.HasFlag(FormattedTextFlags.Bold))
                {
                    run.FontWeight = FontWeights.Bold;
                }

                if (textSegment.Flags.HasFlag(FormattedTextFlags.Italic))
                {
                    run.FontStyle = FontStyles.Italic;
                }

                if (textSegment.Flags.HasFlag(FormattedTextFlags.Underline))
                {
                    run.TextDecorations.Add(TextDecorations.Underline);
                }

                inlines.Add(run);
            }

            return inlines;
        }

        private static void TextSegmentsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = (TextBlock)d;
            textBlock.Inlines.Clear();

            if (e.NewValue != null)
            {
                var segments = (IEnumerable<TextSegment>)e.NewValue;
                textBlock.Inlines.AddRange(CreateInlines(segments));
            }
        }
    }
}
