using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SpineViewer.Common.Spine;
using SpineViewer.Common.Spine_3_6;

namespace SpineViewer.Common.Player
{
    public class Player_3_6 : Player
    {
        private Atlas _atlas;
        private Skeleton _skeleton;
        private AnimationState _state;
        private SkeletonRenderer _skeletonRenderer;

        //private SkeletonData skeletonData;

        public Player_3_6(PlayerInfo info) { Info = info; Props = new PlayerProps(); }
        public Player_3_6(PlayerInfo info, PlayerProps stat) { Info = info; Props = stat; }

        public override void Initialize(GraphicsDevice gd)
        {
            _skeletonRenderer = new SkeletonRenderer(gd);
            _skeletonRenderer.PremultipliedAlpha = Props.UseAlpha;
            _effect = _skeletonRenderer.Effect as BasicEffect;

            _atlas = new Atlas(Info.AtlasFile, new XnaTextureLoader(gd, Info.PremultipliedAlpha));

            SkeletonData skeletonData;
            if (System.IO.Path.GetExtension(Info.SpineFile) == ".skel")
            {
                SkeletonBinary binary = new SkeletonBinary(_atlas);
                binary.Scale = Info.Scale;
                skeletonData = binary.ReadSkeletonData(Info.SpineFile);
            }
            else
            {
                SkeletonJson json = new SkeletonJson(_atlas);
                json.Scale = Info.Scale;
                skeletonData = json.ReadSkeletonData(Info.SpineFile);
            }
            _skeleton = new Skeleton(skeletonData);
            _skeleton.X = 0; _skeleton.Y = 0;
            _skeleton.FlipX = Props.FlipX;
            _skeleton.FlipY = Props.FlipY;

            AnimationStateData stateData = new AnimationStateData(_skeleton.Data);
            _state = new AnimationState(stateData);

            string curSkin = _skeleton.Data.Skins.Items[0].Name;
            string curAnim = _skeleton.Data.Animations.Items[0].Name;
            _state.SetAnimation(0, curAnim, Props.IsLoop);

            // Return spine info
            Props.Skin = curSkin;
            Props.Anim = curAnim;
            Info.Version = skeletonData.Version;
            Info.OrgWidth = skeletonData.Width;
            Info.OrgHeight = skeletonData.Height;
            foreach (Animation ani in _state.Data.skeletonData.Animations)
            {
                Info.AnimNames.Add(ani.Name);
            }
            foreach (Skin sk in _state.Data.skeletonData.Skins)
            {
                Info.SkinNames.Add(sk.Name);
            }

            IsLoaded = true;
        }

        protected override void UpdateStat()
        {
            base.UpdateStat();

            _skeletonRenderer.PremultipliedAlpha = Props.UseAlpha;

            _skeleton.FlipX = Props.FlipX;
            _skeleton.FlipY = Props.FlipY;

            _state.ClearTracks();
            _skeleton.SetToSetupPose();
            _state.SetAnimation(0, Props.Anim, Props.IsLoop);

            _skeleton.SetSkin(Props.Skin);
            _skeleton.SetSlotsToSetupPose();
        }

        public override void Draw(GameTime gameTime, Camera2D cam)
        {
            if (!IsLoaded) return;

            _state.Update((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Props.PlaySpeed / 1000));
            _state.Apply(_skeleton);
            _skeleton.UpdateWorldTransform();

            _effect.View = cam.View;
            _effect.Projection = cam.Projection;

            _skeletonRenderer.Begin();
            _skeletonRenderer.Draw(_skeleton);
            _skeletonRenderer.End();
        }

        #region IDisposable
        // To detect redundant calls
        private bool _disposed = false;

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
                if (_effect != null) _effect.Dispose();
                if (_atlas != null) _atlas.Dispose();
                _effect = null;
                _atlas = null;
                _state = null;
                //skeletonData = null;
                _skeleton = null;
                _skeletonRenderer = null;
            }

            _disposed = true;

            // Call base class implementation.
            base.Dispose(disposing);
        }
        #endregion
    }
}