using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers
{
    class Display : IUpdateable, IDrawable
    {
        protected RenderTexture target;

        private List<IUpdateable> toUpdate; 
        
        protected Sprite spr;
        public Vector2f Position { get { return spr.Position; } set { spr.Position = value; } }

        public delegate void KeyPressHandler(object sender, KeyEventArgs e);
        public event KeyPressHandler OnKeyPress;

        public delegate void MouseMoveHandler(object sender, MouseMoveEventArgs e);
        public event MouseMoveHandler OnMouseMove;

        public delegate void MouseReleaseHandler(object sender, MouseButtonEventArgs e);
        public event MouseReleaseHandler OnMouseRelease;

        private List<DrawableObject> toDraw; 

        public Display(Vector2u displaySize)
        {
            toUpdate = new List<IUpdateable>();
            toDraw = new List<DrawableObject>();

            target = new RenderTexture(displaySize.X, displaySize.Y)
            {
                Smooth = true
            };

            spr = new Sprite();
        }
    
        public virtual void Update()
        {
            foreach (var updateable in toUpdate)
            {
                updateable.Update();
            }
        }

        public virtual void Draw(RenderTexture sourceTexture)
        {
            target.Clear(new Color(10, 10, 10));

            target.Display();
            spr.Texture = target.Texture;
            sourceTexture.Draw(spr);
        }

        /// <summary>
        /// Subscribes and unsubscribes to window events
        /// </summary>
        /// <param name="on"></param>
        /// <param name="window"></param>
        public void EventSubscribe(bool on, RenderWindow window)
        {
            if (on)
            {
                OnResume();
                window.KeyPressed += KeyPressed;
                window.MouseMoved += MouseMoved;
                window.MouseButtonReleased += MouseReleased;
            }
            else
            {
                OnPause();
                window.KeyPressed -= KeyPressed;
                window.MouseMoved -= MouseMoved;
                window.MouseButtonReleased -= MouseReleased;
            }
        }

        /// <summary>
        /// Called when display resumes
        /// </summary>
        protected virtual void OnResume()
        {
            
        }

        /// <summary>
        /// Called right before display is closed
        /// </summary>
        protected virtual void OnPause()
        {
            
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            if (OnKeyPress != null)
            {
                OnKeyPress(sender, e);
            }
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (OnMouseMove != null)
            {
                OnMouseMove(sender, e);
            }
        }

        private void MouseReleased(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseRelease != null)
            {
                OnMouseRelease(sender, e);
            }
        }
    }
}
