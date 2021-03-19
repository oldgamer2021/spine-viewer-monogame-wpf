using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpineViewer.Common.UI
{
    public class Canvas
    {
        private ContentManager _content;

        public TextureManager TexMan { get; set; }

        public Canvas(ContentManager content)
        {
            TexMan = new TextureManager();
            _content = content;
        }
    }
}
