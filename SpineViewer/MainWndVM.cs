using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpineViewer.MonoGameControls;
using SpineViewer.Common;
using SpineViewer.Common.Player;
using System.Windows;
using Microsoft.Xna.Framework.Input;
using SpineViewer.ViewModels;

namespace SpineViewer
{
    public class MainWndVM : MonoGameViewModel
    {
        private SpriteBatch _spriteBatch;
        public Camera2D Camera { get; set; } = new Camera2D();
        private Player _player = null;

        public bool IsPausing { get; set; } = false;
        public Vector2 ViewSize { get; set; }

        public PlayerDataVM PlayerData { get; set; } = new PlayerDataVM();

        public MainWndVM()
        {
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void AddSpine(string atlasFile, string spineFile, SwVersion loaderVer, bool premultipledAlpha)
        {
            PlayerInfo playerInfo = new PlayerInfo()
            {
                AtlasFile = atlasFile, SpineFile = spineFile, 
                PremultipliedAlpha = premultipledAlpha, 
            };
            PlayerProps playerStat = new PlayerProps()
            {
                IsLoop = true, UseAlpha = true
            };

            try
            {
                if (loaderVer.IsEqual(3, 5)) _player = new Player_3_5(playerInfo, playerStat);
                else if (loaderVer.IsEqual(3, 6)) _player = new Player_3_6(playerInfo, playerStat);
                else if (loaderVer.IsEqual(3, 7)) _player = new Player_3_7(playerInfo, playerStat);
                else if (loaderVer.IsEqual(3, 8)) _player = new Player_3_8(playerInfo, playerStat);

                _player.Initialize(GraphicsDevice);
                _player.ZoomAll(ViewSize.X, ViewSize.Y);

                Camera.CreateView(-ViewSize.X / 2, -ViewSize.Y);
                
                PlayerData.FromPlayer(_player);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error Add Spine2D");
            }
        }
        public void RemoveSpine()
        {
            if (_player != null) _player.RemoveFlag = true;
        }
        public void ApplySpine()
        {
            if (_player != null)
            {
                PlayerData.ToPlayer(_player);
                _player.ChangeStatFlag = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_player != null) {
                if (_player.RemoveFlag) _player = null;
            }

            if (Camera != null)
            {
                if (Camera.UpdateProjectionFlag)
                {
                    Camera.CreateProjection(ViewSize.X, ViewSize.Y);
                    Camera.UpdateProjectionFlag = false;
                }
            }

            if (IsPausing) return;
            _player?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsPausing) return;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (_player != null) _player.Draw(gameTime, Camera);
        }

        public override void SizeChanged(object sender, SizeChangedEventArgs args)
        {
            base.SizeChanged(sender, args);

            ViewSize = new Vector2((float)args.NewSize.Width, (float)args.NewSize.Height);
            Camera.UpdateProjectionFlag = true;
        }
    }
}