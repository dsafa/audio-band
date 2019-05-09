using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand;
using System.Drawing;
using AudioBand.Models;
using AudioBand.ViewModels;
using AudioBand.Views.Winforms;
using AudioBand.Views.Winforms.TextFormatting;

namespace AudioBand.Test
{
    [TestClass]
    public class TextRendererTests
    {
        [TestMethod]
        public void ParseNormal()
        {
            var format = "hello";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("hello", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseEmpty()
        {
            var format = "";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left);

            Assert.AreEqual(0, r.Chunks.Count);
        }

        [TestMethod]
        public void ParseSinglePlaceholder()
        {
            var format = "{artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParsePlaceholderWithNormal()
        {
            var format = "{artist} song";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.Chunks.Count);

            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));

            Assert.AreEqual(" song", r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseSingleNormalWithPlaceholder()
        {
            var format = "by {artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.Chunks.Count);

            Assert.AreEqual("by ", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParseUnclosed()
        {
            var format = "{artist";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("{artist", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseOnlyClosing()
        {
            var format = "}";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("}", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseComplex()
        {
            var format = "this is {artist} and ";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(3, r.Chunks.Count);

            Assert.AreEqual("this is ", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));

            Assert.AreEqual(artist, r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextFlags.Artist));

            Assert.AreEqual(" and ", r.Chunks[2].Text);
            Assert.IsTrue(r.Chunks[2].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseInvalidFormat()
        {
            var format = "{something}";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);

            Assert.AreEqual("! invalid format !", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Normal));
        }

        [TestMethod]
        public void ParseArtist()
        {
            var format = "{artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
        }

        [TestMethod]
        public void ParseSong()
        {
            var format = "{song}";
            var song = "the song";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                SongName = song
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(song, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Song));
        }

        [TestMethod]
        public void ParseAlbum()
        {
            var format = "{album}";
            var album = "the album";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                AlbumName = album
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(album, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Album));
        }

        [TestMethod]
        public void ParseTime()
        {
            var format = "{time}";
            var time = TimeSpan.FromSeconds(40);
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                SongProgress = time,
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("0:40", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.CurrentTime));
        }

        [TestMethod]
        public void ParseLength()
        {
            var format = "{length}";
            var time = TimeSpan.FromSeconds(80);
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                SongLength = time,
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("1:20", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.SongLength));
        }

        [TestMethod]
        public void ParseStyleBold()
        {
            var format = "{*artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Bold));
        }

        [TestMethod]
        public void ParseStyleItalic()
        {
            var format = "{&artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Italic));
        }

        [TestMethod]
        public void ParseStyleUnderline()
        {
            var format = "{_artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Underline));
        }

        [TestMethod]
        public void ParseStyleWithColor()
        {
            var format = "{_artist:#ff00ff}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", CustomLabel.TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Artist));
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextFlags.Underline));
            Assert.AreEqual(ColorTranslator.FromHtml("#ff00ff"), r.Chunks[0].Color);
        }
    }
}
