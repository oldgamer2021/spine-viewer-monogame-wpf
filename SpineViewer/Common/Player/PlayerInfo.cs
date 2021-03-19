using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineViewer.Common.Player
{
    public class PlayerInfo
    {
        public string AtlasFile { get; set; }
        public string SpineFile { get; set; }
        public bool PremultipliedAlpha { get; set; }
        public float Scale { get; set; } = 1f;

        public string Version { get; set; }
        public List<string> SkinNames { get; set; } = new List<string>();
        public List<string> AnimNames { get; set; } = new List<string>();
        public float OrgWidth { get; set; }
        public float OrgHeight { get; set; }
    }
}
