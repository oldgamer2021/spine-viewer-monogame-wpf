using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpineViewer.Common.UI
{
    public class TextureManager
    {
        private int _curId = 1;
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        public int LoadTexture4Content(ContentManager content, string name)
        {
            Texture2D tex = content.Load<Texture2D>(name);
            _textures.Add(++_curId, tex);

            return _curId;
        }

        public void FreeTexture(int id)
        {
            if (_textures.ContainsKey(id))
            {
                Texture2D tex = _textures[id];
                tex.Dispose();
            }
        }
    }
}
