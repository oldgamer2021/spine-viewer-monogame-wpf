using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineViewer.Common
{
    public class SwVersion : IComparable<SwVersion>
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; } = -1;
        public int Revision { get; set; } = -1;

        public SwVersion() { }
        public SwVersion(int major, int minor, int build = -1, int revision = -1)
        {
            Major = major; Minor = minor; Build = build; Revision = revision;
        }
        public SwVersion(string vstr)
        {
            Parse(vstr);
        }

        public override string ToString()
        {
            if (Build >= 0)
            {
                string bn = Build >= 9999 ? "xx" : Build.ToString("00");
                if (Revision >= 0) return string.Format("{0}.{1}.{2}.{3}", Major, Minor, bn, Revision);
                return string.Format("{0}.{1}.{2}", Major, Minor, bn);
            }
            return string.Format("{0}.{1}", Major, Minor);
        }

        public void Parse(string ver)
        {
            string[] ss = ver.Split('.');
            int vn = 0;
            if (ss.Length > 0 && int.TryParse(ss[0], out vn))
            {
                Major = vn;
                if (ss.Length > 1 && int.TryParse(ss[1], out vn))
                {
                    Minor = vn;
                    if (ss.Length > 2 && int.TryParse(ss[2], out vn))
                    {
                        Build = vn;
                        if (ss.Length > 3 && int.TryParse(ss[3], out vn)) Revision = vn;
                    }
                }
            }
        }

        public int CompareTo(SwVersion other)
        {
            if (Major < other.Major) return -1;
            else if (Major > other.Major) return 1;
            else
            {
                if (Minor < other.Minor) return -1;
                else if (Minor > other.Minor) return 1;
                else
                {
                    if (Build < other.Build) return -1;
                    else if (Build > other.Build) return 1;
                }
            }
            return Revision.CompareTo(other.Revision);
        }

        public bool IsEqual(int major, int minor = -1, int build = -1, int revision = -1)
        {
            if (Major != major) return false;
            if (minor >= 0) {
                if (minor != Minor) return false;
                if (build >= 0)
                {
                    if (Build != build) return false;
                    if (revision >= 0) return Revision == revision;
                }
            }
            return true;
        }
    }

}
