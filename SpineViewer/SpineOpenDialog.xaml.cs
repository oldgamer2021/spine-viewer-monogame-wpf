using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;
using Newtonsoft.Json;

using SpineViewer.Common;

namespace SpineViewer
{
    /// <summary>
    /// Interaction logic for SpineOpenDialog.xaml
    /// </summary>
    public partial class SpineOpenDialog : Window
    {
        // TODO: binding?
        private OpenFileDialog _ofd = new OpenFileDialog();

        public string AtlasFile { get; set; }
        public string SpineFile { get; set; }
        public SwVersion LoaderVersion { get; set; }
        public bool PremultipledAlpha { get; set; }

        private List<SwVersion> Versions { get; set; } = new List<SwVersion>()
        {
            new SwVersion(3, 5, 9999),
            new SwVersion(3, 6, 9999),
            new SwVersion(3, 7, 94),
            new SwVersion(3, 8, 95)
        };
        private SwVersion _curVersion = new SwVersion();

        public SpineOpenDialog()
        {
            InitializeComponent();
            cboVersion.ItemsSource = Versions;
        }

        private void btBrowseAtlas_Click(object sender, RoutedEventArgs e)
        {
            _ofd.Filter = "Atlas file (*.atlas)|*.atlas";
            if (_ofd.ShowDialog() == true)
            {
                txtAtlasFile.Text = _ofd.FileName;
                if (string.IsNullOrEmpty(txtSpineFile.Text))
                {
                    string atlasFile = Path.ChangeExtension(_ofd.FileName, ".json");
                    if (!SetSkelPath(atlasFile))
                    {
                        atlasFile = Path.ChangeExtension(_ofd.FileName, ".skel");
                        SetSkelPath(atlasFile);
                    }
                }
            }
        }

        private void btBrowseSpine_Click(object sender, RoutedEventArgs e)
        {
            _ofd.Filter = "Spine file (*.skel;*.json)|*.skel;*.json|Spine json file(*.json)|*.json|Spine binary file (*.skel)|*.skel";
            if (_ofd.ShowDialog() == true)
            {
                SetSkelPath(_ofd.FileName);
                if (string.IsNullOrEmpty(txtAtlasFile.Text))
                {
                    string atlasFile = Path.ChangeExtension(_ofd.FileName, ".atlas");
                    if (File.Exists(atlasFile)) txtAtlasFile.Text = atlasFile;
                }
            }
        }

        private bool SetSkelPath(string filename)
        {
            if (File.Exists(filename)) {
                txtSpineFile.Text = filename;
                DetectVertion(filename);
                return true;
            }
            return false;
        }

        private void DetectVertion(string spineFile)
        {
            string ext = Path.GetExtension(spineFile);
            if (ext == ".skel")
            {
                int slen = 0;
                byte[] buf;
                using (BinaryReader reader = new BinaryReader(File.Open(spineFile, FileMode.Open)))
                {
                    // Hash
                    slen = reader.ReadByte();
                    reader.BaseStream.Seek(slen, SeekOrigin.Current);
                    // Version
                    slen = reader.ReadByte();
                    buf = reader.ReadBytes(slen);
                }
                _curVersion.Parse(ASCIIEncoding.ASCII.GetString(buf));
            }
            else if (ext == ".json")
            {
                SpineJsonHeader sp = null;

                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(spineFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    sp = (SpineJsonHeader)serializer.Deserialize(file, typeof(SpineJsonHeader));
                }

                if (sp != null) _curVersion.Parse(sp.Skeleton.Version);
            }
            else
            {
                MessageBox.Show(string.Format("Not supported file: {0}", ext));
                return;
            }

            txtFileVersion.Text = _curVersion.ToString();
            foreach (SwVersion v in Versions)
            {
                if (_curVersion.CompareTo(v) <= 0)
                {
                    cboVersion.SelectedItem = v;
                    return;
                }
            }
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            AtlasFile = txtAtlasFile.Text;
            SpineFile = txtSpineFile.Text;
            LoaderVersion = cboVersion.SelectedItem as SwVersion;
            PremultipledAlpha = chkPremultiAlpha.IsChecked == true;
            DialogResult = true;
        }
    }

    public class SpineJsonHeader
    {
        public SpineJsonHeaderSkeleton Skeleton { get; set; }
    }

    public class SpineJsonHeaderSkeleton
    {
        public string Hash { get; set; }
        [JsonProperty("spine")]
        public string Version { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Images { get; set; }
        public string Audio { get; set; }
    }
}
