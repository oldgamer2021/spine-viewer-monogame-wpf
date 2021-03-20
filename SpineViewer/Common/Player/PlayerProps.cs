using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineViewer.Common.Player
{
    [Flags]
    public enum PlayerPropChanged { None = 0, World = 1, Skel = 2, Anim = 4 }

    public class PlayerProps
    {
        // World Maxtrix
        public float X { get; set; }
        public float Y { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; } = 1f;
        
        public bool UseAlpha { get; set; }

        // Skeleton
        public bool FlipX { get; set; } = false;
        public bool FlipY { get; set; } = false;
        public bool IsLoop { get; set; }
        public string Skin { get; set; }
        // State
        public string Anim { get; set; }

        private double _playSpeed = 1;
        public double PlaySpeed
        {
            get { return _playSpeed; }
            set { _playSpeed = value > 0.1f ? value : 0.1f; }
        }
    }
}
