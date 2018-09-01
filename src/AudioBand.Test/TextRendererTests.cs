using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AudioBand;
using System.Drawing;
using AudioBand.ViewModels;

namespace AudioBand.Text
{
    [TestClass]
    public class TextRendererTests
    {
        [TestMethod]
        public void ParseNormal()
        {
            var format = "hello";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("hello", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }

        [TestMethod]
        public void ParseEmpty()
        {
            var format = "";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left);

            Assert.AreEqual(0, r.Chunks.Count);
        }

        [TestMethod]
        public void ParseSinglePlaceholder()
        {
            var format = "{artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Artist));
        }

        [TestMethod]
        public void ParsePlaceholderWithNormal()
        {
            var format = "{artist} song";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.Chunks.Count);

            Assert.AreEqual(artist, r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Artist));

            Assert.AreEqual(" song", r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }

        [TestMethod]
        public void ParseSingleNormalWithPlaceholder()
        {
            var format = "by {artist}";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(2, r.Chunks.Count);

            Assert.AreEqual("by ", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));

            Assert.AreEqual(artist, r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextRenderer.TextFormat.Artist));
        }

        [TestMethod]
        public void ParseUnclosed()
        {
            var format = "{artist";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("{artist", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }

        [TestMethod]
        public void ParseOnlyClosing()
        {
            var format = "}";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);
            Assert.AreEqual("}", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }

        [TestMethod]
        public void ParseComplex()
        {
            var format = "this is {artist} and ";
            var artist = "123";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left)
            {
                Artist = artist
            };

            Assert.AreEqual(3, r.Chunks.Count);

            Assert.AreEqual("this is ", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));

            Assert.AreEqual(artist, r.Chunks[1].Text);
            Assert.IsTrue(r.Chunks[1].Type.HasFlag(FormattedTextRenderer.TextFormat.Artist));

            Assert.AreEqual(" and ", r.Chunks[2].Text);
            Assert.IsTrue(r.Chunks[2].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }

        [TestMethod]
        public void ParseInvalidFormat()
        {
            var format = "{something}";
            var r = new FormattedTextRenderer(format, Color.Black, 8.5f, "Arial", TextAlignment.Left);

            Assert.AreEqual(1, r.Chunks.Count);

            Assert.AreEqual("! invalid format !", r.Chunks[0].Text);
            Assert.IsTrue(r.Chunks[0].Type.HasFlag(FormattedTextRenderer.TextFormat.Normal));
        }
    }
}
