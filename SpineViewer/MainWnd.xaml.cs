using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpineViewer
{
    /// <summary>
    /// Interaction logic for MainWIndow.xaml
    /// </summary>
    public partial class MainWnd : Window
    {
        private MainWndVM _vm = new MainWndVM();

        public MainWnd()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void mniFileOpen_Click(object sender, RoutedEventArgs e)
        {
            SpineOpenDialog dlg = new SpineOpenDialog() { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                _vm.AddSpine(dlg.AtlasFile, dlg.SpineFile, dlg.LoaderVersion, dlg.PremultipledAlpha);
            }
        }

        private void btUnloadSpine_Click(object sender, RoutedEventArgs e)
        {
            _vm.RemoveSpine();
        }

        private void btApplySpine_Click(object sender, RoutedEventArgs e)
        {
            _vm.ApplySpine();
        }

        private void btPlay_Click(object sender, RoutedEventArgs e)
        {
            _vm.IsPausing = !_vm.IsPausing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region Save app size (Just for fun)
        private void mniHelpSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.WindowLocation = string.Format("Max,{0},{1},{2},{3}",
                    RestoreBounds.Left, RestoreBounds.Top, RestoreBounds.Width, RestoreBounds.Height);
            }
            else
            {
                Properties.Settings.Default.WindowLocation = string.Format("Normal,{0},{1},{2},{3}",
                    (int)this.Left, (int)this.Top, (int)this.Width, (int)this.Height);
            }

            Properties.Settings.Default.Save();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            string[] wndLoc = Properties.Settings.Default.WindowLocation.Split(',');
            if (wndLoc.Length > 4 && int.TryParse(wndLoc[1], out int x) && int.TryParse(wndLoc[2], out int y) &&
                int.TryParse(wndLoc[3], out int w) && int.TryParse(wndLoc[4], out int h))
            {
                this.Top = y;
                this.Left = x;
                this.Height = h;
                this.Width = w;
                if (wndLoc[0] == "Max") WindowState = WindowState.Maximized;
            }
        }
        #endregion

        #region MonoGameControl Mouse Handler
        bool monoControl_Mouse = false;
        Point monoControl_MousePos;
        private void monoGameControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            monoControl_Mouse = monoGameControl.CaptureMouse();
            monoControl_MousePos = e.GetPosition(monoGameControl);
        }

        private void monoGameControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (monoControl_Mouse)
            {
                Point mp = e.GetPosition(monoGameControl);
                _vm.Camera.Move((float)(monoControl_MousePos.X - mp.X), (float)(monoControl_MousePos.Y - mp.Y));
                monoControl_MousePos = mp;
            }
        }

        private void monoGameControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (monoControl_Mouse)
            {
                monoGameControl.ReleaseMouseCapture();
                monoControl_Mouse = false;
            }
        }
        #endregion
    }
}
