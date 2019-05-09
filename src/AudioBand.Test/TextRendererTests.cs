using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using AudioBand.TextFormatting;

namespace AudioBand.Test
{
    [TestClass]
    public class TextRendererTests
    {
        [TestMethod]
        public void ParseNormal()
        {
            var format = "hello";
            var r = new FormattedTextRenderer(format, Colors.Black);

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual("hello", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseEmpty()
        {
            var format = "";
            var r = new FormattedTextRenderer(format, Colors.Black);

            Assert.AreEqual(0, r.TextSegments.Count);
        }

        [TestMethod]
        public void ParseSinglePlaceholder()
        {
            var format = "{artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParsePlaceholderWithNormal()
        {
            var format = "{artist} song";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.TextSegments.Count);

            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));

            Assert.AreEqual(" song", r.TextSegments[1].Text);
            Assert.IsTrue(r.TextSegments[1].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseSingleNormalWithPlaceholder()
        {
            var format = "by {artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.TextSegments.Count);

            Assert.AreEqual("by ", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, r.TextSegments[1].Text);
            Assert.IsTrue(r.TextSegments[1].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParseUnclosed()
        {
            var format = "{artist";
            var r = new FormattedTextRenderer(format, Colors.Black);

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual("{artist", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseOnlyClosing()
        {
            var format = "}";
            var r = new FormattedTextRenderer(format, Colors.Black);

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual("}", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseComplex()
        {
            var format = "this is {artist} and ";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(3, r.TextSegments.Count);

            Assert.AreEqual("this is ", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, r.TextSegments[1].Text);
            Assert.IsTrue(r.TextSegments[1].Type.HasFlag(FormattedTextFlags.Artist));

            Assert.AreEqual(" and ", r.TextSegments[2].Text);
            Assert.IsTrue(r.TextSegments[2].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseInvalidFormat()
        {
            var format = "{something}";
            var r = new FormattedTextRenderer(format, Colors.Black);

            Assert.AreEqual(1, r.TextSegments.Count);

            Assert.AreEqual("! invalid format !", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseArtist()
        {
            var format = "{artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParseSong()
        {
            var format = "{song}";
            var song = "the song";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                SongName = song
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(song, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Song));
        }

        [TestMethod]
        public void ParseAlbum()
        {
            var format = "{album}";
            var album = "the album";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                AlbumName = album
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(album, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Album));
        }

        [TestMethod]
        public void ParseTime()
        {
            var format = "{time}";
            var time = TimeSpan.FromSeconds(40);
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                SongProgress = time,
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual("0:40", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.CurrentTime));
        }

        [TestMethod]
        public void ParseLength()
        {
            var format = "{length}";
            var time = TimeSpan.FromSeconds(80);
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                SongLength = time,
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual("1:20", r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.SongLength));
        }

        [TestMethod]
        public void ParseStyleBold()
        {
            var format = "{*artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Bold));
        }

        [TestMethod]
        public void ParseStyleItalic()
        {
            var format = "{&artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Italic));
        }

        [TestMethod]
        public void ParseStyleUnderline()
        {
            var format = "{_artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Underline));
        }

        [TestMethod]
        public void ParseStyleWithColor()
        {
            var format = "{_artist:#ff00ff}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Underline));
            Assert.AreEqual(Color.FromRgb(255, 0, 255), r.TextSegments[0].Color);
        }

        [TestMethod]
        public void ParseMultipleStyles()
        {
            var format = "{*_artist}";
            var artist = "test";
            var r = new FormattedTextRenderer(format, Colors.Black)
            {
                Artist = artist,
            };
            Assert.AreEqual(1, r.TextSegments.Count);
            Assert.AreEqual(artist, r.TextSegments[0].Text);
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Underline));
            Assert.IsTrue(r.TextSegments[0].Type.HasFlag(FormattedTextFlags.Bold));
        }
    }
}
