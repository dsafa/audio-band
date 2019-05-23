using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base view model for buttons.
    /// </summary>
    /// <typeparam name="TButton">The button model.</typeparam>
    public class ButtonViewModelBase<TButton> : ViewModelBase<TButton>
        where TButton : ButtonModelBase, new()
    {
        private readonly List<ButtonContentViewModel> _contentViewModels = new List<ButtonContentViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonViewModelBase{TButton}"/> class.
        /// </summary>
        /// <param name="model">The button model.</param>
        /// <param name="dialogService">The dialog service to use.</param>
        public ButtonViewModelBase(TButton model, IDialogService dialogService)
            : base(model)
        {
            DialogService = dialogService;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.Width))]
        public double Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.Height))]
        public double Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.XPosition))]
        public double XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.YPosition))]
        public double YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.BackgroundColor))]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(nameof(Model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.HoveredBackgroundColor))]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(nameof(Model.HoveredBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonModelBase.ClickedBackgroundColor))]
        public Color ClickedBackgroundColor
        {
            get => Model.ClickedBackgroundColor;
            set => SetProperty(nameof(Model.ClickedBackgroundColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Track the button content view models edit state.
        /// </summary>
        /// <param name="viewModel">The button content view model.</param>
        protected void TrackContentViewModel(ButtonContentViewModel viewModel)
        {
            Debug.Assert(!_contentViewModels.Contains(viewModel), "Already tracked this view model");
            _contentViewModels.Add(viewModel);
            viewModel.PropertyChanged += ButtonContentViewModelOnPropertyChanged;
        }

        /// <inheritdoc />
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            foreach (var buttonContentViewModel in _contentViewModels)
            {
                buttonContentViewModel.CancelEdit();
            }
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            foreach (var buttonContentViewModel in _contentViewModels)
            {
                buttonContentViewModel.EndEdit();
            }
        }

        /// <inheritdoc />
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            foreach (var buttonContentViewModel in _contentViewModels)
            {
                buttonContentViewModel.BeginEdit();
            }
        }

        /// <inheritdoc />
        protected override void OnReset()
        {
            base.OnReset();
            foreach (var buttonContentViewModel in _contentViewModels)
            {
                buttonContentViewModel.Reset();
            }
        }

        private void ButtonContentViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsEditing))
            {
                return;
            }

            var vm = (ButtonContentViewModel)sender;
            if (vm.IsEditing)
            {
                BeginEdit();
            }
        }
    }
}
