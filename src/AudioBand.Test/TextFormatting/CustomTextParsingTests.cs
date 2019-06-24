using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.TextFormatting;
using Moq;
using Xunit;

namespace AudioBand.Test
{
    public class CustomTextParsingTests
    {
        [Fact]
        public void ParseNormal()
        {
            var format = "hello";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.Equal("hello", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseEmpty()
        {
            var format = "";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Empty(segments);
        }

        [Fact]
        public void ParseSinglePlaceholder()
        {
            var format = "{artist}";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.Single(segments);
            Assert.Equal(artist, segments[0].Text);
        }

        [Fact]
        public void ParsePlaceholderWithNormal()
        {
            var format = "{artist} song";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();
            Assert.Equal(2, segments.Count);

            Assert.Equal(artist, segments[0].Text);

            Assert.Equal(" song", segments[1].Text);
            Assert.True(segments[1].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseSingleNormalWithPlaceholder()
        {
            var format = "by {artist}";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.Equal(2, segments.Count);

            Assert.Equal("by ", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));

            Assert.Equal(artist, segments[1].Text);
        }

        [Fact]
        public void ParseUnclosed()
        {
            var format = "{artist";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.Equal("{artist", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseOnlyClosing()
        {
            var format = "}";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.Equal("}", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseComplex()
        {
            var format = "this is {artist} and ";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.Equal(3, segments.Count);

            Assert.Equal("this is ", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));

            Assert.Equal(artist, segments[1].Text);

            Assert.Equal(" and ", segments[2].Text);
            Assert.True(segments[2].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseInvalidFormat()
        {
            var format = "{something}";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);

            Assert.Equal("!Invalid format!", segments[0].Text);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [Fact]
        public void ParseArtist()
        {
            var format = "{artist}";
            var artist = "123";
            var artist2 = "next";
            var mock = new Mock<IAudioSession>();
            mock.SetupSequence(m => m.SongArtist).Returns(artist).Returns(artist2);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.Equal(artist, segments[0].Text);

            mock.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.SongArtist)));
            Assert.Equal(artist2, segments[0].Text);
        }

        [Fact]
        public void ParseSong()
        {
            var format = "{song}";
            var song = "the song";
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongName).Returns(song);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.Equal(song, segments[0].Text);
        }

        [Fact]
        public void ParseAlbum()
        {
            var format = "{album}";
            var album = "the album";
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.AlbumName).Returns(album);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.Equal(album, segments[0].Text);
        }

        [Fact]
        public void ParseTime()
        {
            var format = "{time}";
            var time = TimeSpan.FromSeconds(40);
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongProgress).Returns(time);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.Equal("0:40", segments[0].Text);
        }

        [Fact]
        public void ParseLength()
        {
            var format = "{length}";
            var time = TimeSpan.FromSeconds(80);
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongLength).Returns(time);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.Equal("1:20", segments[0].Text);
        }

        [Fact]
        public void ParseStyleBold()
        {
            var format = "{*artist}";
            var mock = new Mock<IAudioSession>();
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.Single(segments);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Bold));
        }

        [Fact]
        public void ParseStyleItalic()
        {
            var format = "{&artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Italic));
        }

        [Fact]
        public void ParseStyleUnderline()
        {
            var format = "{_artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
        }

        [Fact]
        public void ParseStyleWithColor()
        {
            var format = "{_artist:#ff00ff}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
            Assert.Equal(Color.FromRgb(255, 0, 255), segments[0].Color);
        }

        [Fact]
        public void ParseMultipleStyles()
        {
            var format = "{*_artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.Single(segments);
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
            Assert.True(segments[0].Flags.HasFlag(FormattedTextFlags.Bold));
        }
    }
}
