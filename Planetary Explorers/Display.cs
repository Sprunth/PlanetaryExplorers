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

        private List<DrawableObject> drawables;
        /// <summary>
        /// SFML objects to draw
        /// Each contains a z-level for drawing
        /// Do not directly add to this list. Use AddItemToDraw
        /// </summary>
        public List<Tuple<Drawable, uint>> toDraw;

        public Color BackgroundColor { get; set; }

        public Display(Vector2u displaySize)
        {
            toUpdate = new List<IUpdateable>();
            drawables = new List<DrawableObject>();
            toDraw = new List<Tuple<Drawable, uint>>();

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
            target.Clear(new Color(0,0,0,0));
            foreach (var tuple in toDraw)
            {
                target.Draw(tuple.Item1);
            }
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
            drawables.Add(new DrawableObject(this));
        }

        /// <summary>
        /// Called right before display is closed
        /// </summary>
        protected virtual void OnPause()
        {
            
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (OnKeyPress != null)
            {
                OnKeyPress(sender, e);
            }
        }

        private void MouseMoved(object sender, MouseMoveEventArgs e)
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

    class ZlevelDrawableCompare : IComparer<Tuple<Drawable, uint>>
    {
        /// <summary>
        /// Compares by zlevels, sorts 0 to +infinity.
        /// </summary>
        int IComparer<Tuple<Drawable, uint>>.Compare(Tuple<Drawable, uint> first, Tuple<Drawable, uint> second)
        {
            if (first.Item2 > second.Item2)
                return 1;
            if (first.Item2 < second.Item2)
                return -1;
            return 0;
        }

        public static IComparer<Tuple<Drawable, uint>> Comparer = new ZlevelDrawableCompare();
    }
}
