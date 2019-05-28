﻿using AudioBand.Models;

namespace AudioBand.ViewModels
{
    public class LayoutViewModelBase<TModel> : ViewModelBase<TModel>
        where TModel : LayoutModelBase, new()
    {
        public LayoutViewModelBase(TModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(LayoutModelBase.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [PropertyChangeBinding(nameof(LayoutModelBase.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(LayoutModelBase.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(LayoutModelBase.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(LayoutModelBase.YPosition))]
        public double YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(LayoutModelBase.Anchor))]
        public PositionAnchor Anchor
        {
            get => Model.Anchor;
            set => SetProperty(nameof(Model.Anchor), value);
        }
    }
}
