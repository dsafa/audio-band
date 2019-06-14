using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using AudioBand.Messages;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Base view model for buttons.
    /// </summary>
    /// <typeparam name="TButton">The button model.</typeparam>
    public class ButtonViewModelBase<TButton> : LayoutViewModelBase<TButton>
        where TButton : ButtonModelBase, new()
    {
        private readonly List<ButtonContentViewModel> _contentViewModels = new List<ButtonContentViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonViewModelBase{TButton}"/> class.
        /// </summary>
        /// <param name="source">The button model with the initial data.</param>
        /// <param name="dialogService">The dialog service to use.</param>
        /// <param name="messageBus">The message bus to use.</param>
        public ButtonViewModelBase(TButton source, IDialogService dialogService, IMessageBus messageBus)
            : base(messageBus, source)
        {
            DialogService = dialogService;
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [TrackState]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(Model, nameof(Model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when hovered.
        /// </summary>
        [TrackState]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(Model, nameof(Model.HoveredBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
        [TrackState]
        public Color ClickedBackgroundColor
        {
            get => Model.ClickedBackgroundColor;
            set => SetProperty(Model, nameof(Model.ClickedBackgroundColor), value);
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
