using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.TextFormatting;
using Moq;

namespace AudioBand.Test
{
    [TestClass]
    public class CustomTextParsingTests
    {
        [TestMethod]
        public void ParseNormal()
        {
            var format = "hello";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual("hello", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseEmpty()
        {
            var format = "";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(0, segments.Count);
        }

        [TestMethod]
        public void ParseSinglePlaceholder()
        {
            var format = "{artist}";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual(artist, segments[0].Text);
        }

        [TestMethod]
        public void ParsePlaceholderWithNormal()
        {
            var format = "{artist} song";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();
            Assert.AreEqual(2, segments.Count);

            Assert.AreEqual(artist, segments[0].Text);

            Assert.AreEqual(" song", segments[1].Text);
            Assert.IsTrue(segments[1].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseSingleNormalWithPlaceholder()
        {
            var format = "by {artist}";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.AreEqual(2, segments.Count);

            Assert.AreEqual("by ", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, segments[1].Text);
        }

        [TestMethod]
        public void ParseUnclosed()
        {
            var format = "{artist";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual("{artist", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseOnlyClosing()
        {
            var format = "}";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual("}", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseComplex()
        {
            var format = "this is {artist} and ";
            var artist = "123";
            var session = new Mock<IAudioSession>();
            session.SetupGet(m => m.SongArtist).Returns(artist);
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, session.Object).ToList();

            Assert.AreEqual(3, segments.Count);

            Assert.AreEqual("this is ", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, segments[1].Text);

            Assert.AreEqual(" and ", segments[2].Text);
            Assert.IsTrue(segments[2].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseInvalidFormat()
        {
            var format = "{something}";
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);

            Assert.AreEqual("!Invalid format!", segments[0].Text);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseArtist()
        {
            var format = "{artist}";
            var artist = "123";
            var artist2 = "next";
            var mock = new Mock<IAudioSession>();
            mock.SetupSequence(m => m.SongArtist).Returns(artist).Returns(artist2);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual(artist, segments[0].Text);

            mock.Raise(m => m.PropertyChanged += null, null, new PropertyChangedEventArgs(nameof(IAudioSession.SongArtist)));
            Assert.AreEqual(artist2, segments[0].Text);
        }

        [TestMethod]
        public void ParseSong()
        {
            var format = "{song}";
            var song = "the song";
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongName).Returns(song);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual(song, segments[0].Text);
        }

        [TestMethod]
        public void ParseAlbum()
        {
            var format = "{album}";
            var album = "the album";
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.AlbumName).Returns(album);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual(album, segments[0].Text);
        }

        [TestMethod]
        public void ParseTime()
        {
            var format = "{time}";
            var time = TimeSpan.FromSeconds(40);
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongProgress).Returns(time);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual("0:40", segments[0].Text);
        }

        [TestMethod]
        public void ParseLength()
        {
            var format = "{length}";
            var time = TimeSpan.FromSeconds(80);
            var mock = new Mock<IAudioSession>();
            mock.SetupGet(m => m.SongLength).Returns(time);
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.AreEqual("1:20", segments[0].Text);
        }

        [TestMethod]
        public void ParseStyleBold()
        {
            var format = "{*artist}";
            var mock = new Mock<IAudioSession>();
            var segments = FormattedTextParser.ParseFormattedString(format, Colors.Black, mock.Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Bold));
        }

        [TestMethod]
        public void ParseStyleItalic()
        {
            var format = "{&artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Italic));
        }

        [TestMethod]
        public void ParseStyleUnderline()
        {
            var format = "{_artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
        }

        [TestMethod]
        public void ParseStyleWithColor()
        {
            var format = "{_artist:#ff00ff}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
            Assert.AreEqual(Color.FromRgb(255, 0, 255), segments[0].Color);
        }

        [TestMethod]
        public void ParseMultipleStyles()
        {
            var format = "{*_artist}";
            var segments = FormattedTextParser
                .ParseFormattedString(format, Colors.Black, new Mock<IAudioSession>().Object).ToList();

            Assert.AreEqual(1, segments.Count);
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Underline));
            Assert.IsTrue(segments[0].Flags.HasFlag(FormattedTextFlags.Bold));
        }
    }
}
