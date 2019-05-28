using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AudioBand.Models;

namespace AudioBand.Views.Settings
{
    /// <summary>
    /// Use control to select control anchoring.
    /// </summary>
    public partial class AnchorSelector : UserControl
    {
        public static readonly DependencyProperty AnchorProperty
            = DependencyProperty.Register(nameof(Anchor), typeof(PositionAnchor), typeof(AnchorSelector), new FrameworkPropertyMetadata(default(PositionAnchor), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Initializes a new instance of the <see cref="AnchorSelector"/> class.
        /// </summary>
        public AnchorSelector()
        {
            InitializeComponent();
            Anchor = PositionAnchor.TopLeft;
        }

        public PositionAnchor Anchor
        {
            get => (PositionAnchor)GetValue(AnchorProperty);
            set => SetValue(AnchorProperty, value);
        }
    }
}
