using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;

namespace SpineViewer.Common
{
    public class Camera2D
    {
        private float _x = 0, _y = 0;
        private float _w, _h;

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Camera2D()
        {
            CreateView(0, 0);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void CreateView(float x, float y)
        {
            _x = x; _y = y;
            View = Matrix.CreateLookAt(new Vector3(x, y, 1.0f), new Vector3(x, y, 0), Vector3.Up);
        }

        public void CreateProjection(float w, float h)
        {
            _w = w; _h = h;
            Projection = Matrix.CreateOrthographicOffCenter(0, w, h, 0, 1, 0);
        }

        public void Move(float x, float y)
        {
            CreateView(_x + x, _y + y);
        }

        public bool UpdateProjectionFlag = false;
    }
}
