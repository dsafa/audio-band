using System.Linq;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.UI;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class CustomLabelViewModelTests
    {
        private Mock<IDialogService> _dialogMock;
        private Mock<IMessageBus> _messageBusMock;
        private Mock<IAudioSession> _sessionMock;

        public CustomLabelViewModelTests()
        {
            _dialogMock = new Mock<IDialogService>();
            _messageBusMock = new Mock<IMessageBus>();
            _sessionMock = new Mock<IAudioSession>();
        }

        [Fact]
        void CustomLabel_TextFormatStringChanged_TextSegmentsMatch()
        {
            var songname = "song";
            var artist = "artist";
            var model = new CustomLabel { FormatString = "first format" };
            _sessionMock.SetupGet(m => m.SongName).Returns(songname);
            _sessionMock.SetupGet(m => m.SongArtist).Returns(artist);
            var vm = new CustomLabelViewModel(model, _dialogMock.Object, _sessionMock.Object, _messageBusMock.Object);

            var firstSegment = "second format with";
            var secondSegment = "{artist}";
            var thirdSegment = "and";
            var forthSegment = "{song}";
            vm.FormatString = firstSegment + secondSegment + thirdSegment + forthSegment;

            var segments = vm.TextSegments.ToList();
            Assert.Equal(4, segments.Count);
            Assert.Equal(firstSegment, segments[0].Text);
            Assert.Equal(artist, segments[1].Text);
            Assert.Equal(thirdSegment, segments[2].Text);
            Assert.Equal(songname, segments[3].Text);
        }

        [Fact]
        void CustomLabel_TextFormatStringChangedThenCanceled_CorrectTextSegments()
        {
            var songname = "song";
            var model = new CustomLabel {FormatString = "text format {song}"};
            _sessionMock.SetupGet(m => m.SongName).Returns(songname);
            var vm = new CustomLabelViewModel(model, _dialogMock.Object, _sessionMock.Object, _messageBusMock.Object);

            Assert.Equal(2, vm.TextSegments.Count());
            vm.FormatString = "new format string";
            vm.CancelEdit();

            Assert.Equal(2, vm.TextSegments.Count());
            Assert.Equal(songname, vm.TextSegments.ToList()[1].Text);
        }

        [Fact]
        void CustomLabel_ColorChangedThenCanceled_TextSegmentsHaveCorrectColor()
        {
            var model = new CustomLabel { FormatString = "text format {song}", Color = Colors.Blue };
            var vm = new CustomLabelViewModel(model, _dialogMock.Object, _sessionMock.Object, _messageBusMock.Object);

            vm.Color = Colors.Black;
            vm.CancelEdit();

            Assert.All(vm.TextSegments, segment => Assert.Equal(Colors.Blue, segment.Color));
        }

        [Fact]
        void CustomLabel_CancelEdit_ModelHasNoChanges()
        {
            var formatString = "test";
            var model = new CustomLabel {FormatString = formatString};
            var vm = new CustomLabelViewModel(model, _dialogMock.Object, _sessionMock.Object, _messageBusMock.Object);

            vm.FormatString = "new";
            vm.CancelEdit();

            Assert.Equal(formatString, vm.FormatString);
            Assert.Equal(formatString, model.FormatString);
        }

        [Fact]
        void CustomLabel_SaveEdit_ModelHasNewChanges()
        {
            var formatString = "test";
            var newFormatstring = "format";
            var model = new CustomLabel { FormatString = formatString };
            var vm = new CustomLabelViewModel(model, _dialogMock.Object, _sessionMock.Object, _messageBusMock.Object);

            vm.FormatString = newFormatstring;
            vm.EndEdit();

            Assert.Equal(newFormatstring, vm.FormatString);
            Assert.Equal(newFormatstring, model.FormatString);
        }
    }
}
