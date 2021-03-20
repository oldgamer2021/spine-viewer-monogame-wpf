using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpineViewer.Common.Player
{
    public abstract class Player: IDisposable
    {
        public PlayerInfo Info { get; set; }
        public PlayerProps Props { get; set; }

        protected BasicEffect _effect;

        public bool IsLoaded { get; protected set; }
        public bool RemoveFlag { get; set; }
        public bool ChangeStatFlag { get; set; }

        public abstract void Initialize(GraphicsDevice gd);
        public virtual void Update(GameTime gameTime)
        {
            if (ChangeStatFlag)
            {
                UpdateStat();
                ChangeStatFlag = false;
            }
        }
        protected virtual void UpdateStat()
        {
            SetWorldMatrix(Props.X, Props.Y, Props.Scale);
        }
        public abstract void Draw(GameTime gameTime, Camera2D cam);

        public void ZoomAll(float viewWidth, float viewHeight, float spineW, float spineH)
        {
            if (spineW > 0 && spineH > 0)
            {
                float sw = viewWidth / spineW;
                float sh = viewHeight / spineH;
                sw = sw < sh ? sw : sh;

                SetWorldMatrix(viewWidth / 2, viewHeight, sw);
            }
        }

        public void ZoomAll(float viewWidth, float viewHeight)
        {
            double sw = Info.OrgWidth;
            double sh = Info.OrgHeight;
            if (sw > 0 && sh > 0)
            {
                sw = viewWidth / sw;
                sh = viewHeight / sh;
                sw = Math.Round(sw < sh ? sw : sh, 2);

                SetWorldMatrix(0, 0, (float)sw);
            }
        }

        public virtual void SetWorldMatrix(float x, float y, float scale)
        {
            // Use WorldMatrix instead of Skeleton
            Props.X = x; Props.Y = y; Props.Scale = scale;
            if (_effect != null)
                _effect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(x, y, 0);
        }

        #region IDisposable
        // To detect redundant calls
        private bool _disposed = false;

        ~Player() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }
        #endregion
    }
}
