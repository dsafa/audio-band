using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Custom tooltip class
    /// </summary>
    public abstract class AudioBandTooltip : ToolTip, IBindableComponent
    {
        private readonly MethodInfo _setToolMethod = typeof(ToolTip).GetMethod("SetTool", BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandTooltip"/> class.
        /// </summary>
        /// <param name="name">The name of the tooltip.</param>
        protected AudioBandTooltip(string name)
        {
            _name = name;
            OwnerDraw = true;
            ShowAlways = true;
            Popup += (o, e) => OnPopup(e);
            Draw += (o, e) => OnDraw(e);
            DataBindings = new ControlBindingsCollection(this);
        }

        /// <summary>
        /// Flags for the positioning of the tooltip
        /// </summary>
        [Flags]
        protected enum PositionType
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,

            /// <summary>
            /// Auto
            /// </summary>
            Auto = 1,

            /// <summary>
            /// Absolute
            /// </summary>
            Absolute = 1 << 1,

            /// <summary>
            /// Semi absolute
            /// </summary>
            SemiAbsolute = 1 << 2,
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public BindingContext BindingContext { get; set; } = new BindingContext();

        /// <inheritdoc/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings { get; }

        /// <summary>
        /// Called when <see cref="ToolTip.Popup"/> occurs.
        /// </summary>
        /// <param name="drawToolTipEventArgs">Event args.</param>
        protected abstract void OnDraw(DrawToolTipEventArgs drawToolTipEventArgs);

        /// <summary>
        /// Called when <see cref="ToolTip.Popup"/> occurs. Use to set tooltip size.
        /// </summary>
        /// <param name="popupEventArgs">Event args.</param>
        protected abstract void OnPopup(PopupEventArgs popupEventArgs);

        /// <summary>
        /// Shows the tooltip without focus being required
        /// </summary>
        /// <param name="parent">The parent control to show the tooltip over.</param>
        /// <param name="positionType">The type of positioning used to interpret <paramref name="position"/>.</param>
        /// <param name="position">The position of the tooltip</param>
        protected void Show(Control parent, PositionType positionType, Point position)
        {
            _setToolMethod.Invoke(this, new object[] { parent, _name, positionType, position });
        }
    }
}
