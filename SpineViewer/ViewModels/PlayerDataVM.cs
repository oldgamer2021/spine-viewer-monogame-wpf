using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using SpineViewer.Common.Player;
using System.Collections.ObjectModel;

namespace SpineViewer.ViewModels
{
    public class PlayerDataVM : INotifyPropertyChanged
    {
        private string _version;
        public string Version { get { return _version; } set { if (_version != value) { _version = value; NotifyChanged("Version"); } } }

        private string _orgSize;
        public string OriginSize { get { return _orgSize; } set { if (_orgSize != value) { _orgSize = value; NotifyChanged("OriginSize"); } } }

        private bool _premultipedAlpha;
        public bool PremulipledAlpha { get { return _premultipedAlpha; } set { if (_premultipedAlpha != value) { _premultipedAlpha = value; NotifyChanged("PremulipledAlpha"); } } }

        private bool _useAlpha;
        public bool UseAlpha { get { return _useAlpha; } set { if (_useAlpha != value) { _useAlpha = value; NotifyChanged("UseAlpha"); } } }
        private bool _flipX;
        public bool FlipX { get { return _flipX; } set { if (_flipX != value) { _flipX = value; NotifyChanged("FlipX"); } } }
        private bool _flipY;
        public bool FlipY { get { return _flipY; } set { if (_flipY != value) { _flipY = value; NotifyChanged("FlipY"); } } }

        public WpfObservableRangeCollection<string> SkinNames { get; set; } = new WpfObservableRangeCollection<string>();
        public WpfObservableRangeCollection<string> AnimNames { get; set; } = new WpfObservableRangeCollection<string>();

        private float _scale;
        public float Scale { get { return _scale; } set { if (_scale != value) { _scale = value; NotifyChanged("Scale"); } } }
        private float _x;
        public float X { get { return _x; } set { if (_x != value) { _x = value; NotifyChanged("X"); } } }
        private float _y;
        public float Y { get { return _y; } set { if (_y != value) { _y = value; NotifyChanged("Y"); } } }

        private bool _isLoop;
        public bool IsLoop { get { return _isLoop; } set { if (_isLoop != value) { _isLoop = value; NotifyChanged("IsLoop"); } } }
        private double _playSpeed = 1;
        public double PlaySpeed { get { return _playSpeed; } set { if (_playSpeed != value) { _playSpeed = value; NotifyChanged("PlaySpeed"); } } }

        private string _selSkin;
        public string SelSkin { get { return _selSkin; } set { if (_selSkin != value) { _selSkin = value; NotifyChanged("SelSkin"); } } }
        private string _selAnim;
        public string SelAnim { get { return _selAnim; } set { if (_selAnim != value) { _selAnim = value; NotifyChanged("SelAnim"); } } }

        public void FromPlayer(Player p)
        {
            Version = p.Info.Version;
            OriginSize = string.Format("{0} x {1}", p.Info.OrgWidth, p.Info.OrgHeight);
            PremulipledAlpha = p.Info.PremultipliedAlpha;

            UseAlpha = p.Props.UseAlpha;
            FlipX = p.Props.FlipX;
            FlipY = p.Props.FlipY;

            Scale = p.Props.Scale;

            SkinNames.Clear();
            SkinNames.AddRange(p.Info.SkinNames);
            AnimNames.Clear();
            AnimNames.AddRange(p.Info.AnimNames);
            SelSkin = p.Props.Skin;
            SelAnim = p.Props.Anim;

            IsLoop = p.Props.IsLoop;
            PlaySpeed = p.Props.PlaySpeed;
        }

        public void ToPlayer(Player p)
        {
            p.Props.Scale = Scale;
            p.Props.X = X; p.Props.Y = Y;

            p.Props.UseAlpha = UseAlpha;
            p.Props.FlipX = FlipX; p.Props.FlipY = FlipY;

            p.Props.Skin = SelSkin;
            p.Props.Anim = SelAnim;

            p.Props.IsLoop = IsLoop;
            p.Props.PlaySpeed = PlaySpeed;
        }

        #region INotifyPropertyChanged
        private readonly Dictionary<string, PropertyChangedEventArgs> _argsCache = new Dictionary<string, PropertyChangedEventArgs>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyChanged(string propertyName)
        {
            if (_argsCache != null)
            {
                if (!_argsCache.ContainsKey(propertyName))
                    _argsCache[propertyName] = new PropertyChangedEventArgs(propertyName);

                NotifyChanged(_argsCache[propertyName]);
            }
        }
        private void NotifyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        #endregion
    }
}
