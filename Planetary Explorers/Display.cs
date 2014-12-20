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
    class Display : IUpdateable, IDrawable, Drawable
    {
        private RenderTexture _target;
        protected View DisplayView { get { return _target.GetView(); } set { _target.SetView(value);} }
       
        private readonly Sprite _targetSpr;
        public Vector2f Position { get { return _targetSpr.Position; } set { _targetSpr.Position = border.Position = value; } }
        public Vector2f Scale { get { return _targetSpr.Scale; } set { _targetSpr.Scale = value; } }

        public RenderTarget Target { get { return _target; } }

        public delegate void LostFocusHandler(object sender, EventArgs e);
        public event LostFocusHandler OnLostFocus;

        public delegate void KeyPressHandler(object sender, KeyEventArgs e);
        public event KeyPressHandler OnKeyPress;

        public delegate void MouseMoveHandler(object sender, MouseMoveEventArgs e, Vector2f displayCoords);
        public event MouseMoveHandler OnMouseMove;

        public delegate void MousePressHandler(object sender, MouseButtonEventArgs e, Vector2f displayCoords);
        public event MousePressHandler OnMousePress;

        public delegate void MouseReleaseHandler(object sender, MouseButtonEventArgs e, Vector2f displayCoords);
        public event MouseReleaseHandler OnMouseRelease;
        
        private readonly List<IUpdateable> toUpdate;
        /// <summary>
        /// SFML objects to draw
        /// Each contains a z-level for drawing
        /// Do not directly add to this list. Use AddItemToDraw
        /// </summary>
        private readonly List<Tuple<Drawable, uint>> toDraw;

        protected Color BackgroundColor { get; set; }
        
        private readonly RectangleShape border;
        public bool ShowBorder
        {
            get { return border.OutlineColor.A == 255; }
            set
            {
                border.OutlineColor = value ? 
                    border.OutlineColor = new Color(border.OutlineColor.R, border.OutlineColor.G, border.OutlineColor.B, 255) :
                    border.OutlineColor = new Color(border.OutlineColor.R, border.OutlineColor.G, border.OutlineColor.B, 0);
            }
        }

        public Display(Vector2u displaySize)
        {
            toUpdate = new List<IUpdateable>();
            toDraw = new List<Tuple<Drawable, uint>>();

            _target = new RenderTexture(displaySize.X, displaySize.Y)
            {
                Smooth = true
            };

            BackgroundColor = new Color(0, 0, 0, 0);

            _targetSpr = new Sprite()
            {
                Scale = new Vector2f(0.5f,0.5f)
            };

            border = new RectangleShape(new Vector2f(displaySize.X * _targetSpr.Scale.X, displaySize.Y * _targetSpr.Scale.Y))
            {
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                FillColor = Color.Transparent,
            };
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
            Draw((RenderTarget)sourceTexture);
        }

        public virtual void Draw(RenderTarget sourceTarget)
        {
            _target.Clear(BackgroundColor);
            foreach (var tuple in toDraw)
            {
                _target.Draw(tuple.Item1);
            }
            _target.Display();
            _targetSpr.Texture = _target.Texture;
            sourceTarget.Draw(_targetSpr);//, new RenderStates(aa));
            sourceTarget.Draw(border);
        }

        void Drawable.Draw(RenderTarget target, RenderStates states)
        {
            Draw(target);
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
                window.LostFocus += LostFocus;
                window.KeyPressed += KeyPressed;
                window.MouseMoved += MouseMoved;
                window.MouseButtonPressed += MousePressed;
                window.MouseButtonReleased += MouseReleased;
            }
            else
            {
                OnPause();
                window.LostFocus -= LostFocus;
                window.KeyPressed -= KeyPressed;
                window.MouseMoved -= MouseMoved;
                window.MouseButtonPressed -= MousePressed;
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

        public void AddItemToDraw(Drawable drawable, uint zlevel)
        {
            if (toDraw.Contains(new Tuple<Drawable, uint>(drawable, zlevel)))
            {
                return;
            }
            var tup = new Tuple<Drawable, uint>(drawable, zlevel);
            var index = toDraw.BinarySearch(tup, ZlevelDrawableCompare.Comparer);
            if (index < 0)
                toDraw.Insert(~index, tup);
            else
                toDraw.Insert(index, tup);
        }

        public void RemoveItemToDraw(Drawable drawable, uint zlevel)
        {
            // Could be sped up with binary search, or keep an index.
            toDraw.Remove(new Tuple<Drawable, uint>(drawable, zlevel));
        }

        protected void ResizeDisplay(Vector2u size)
        {
            var viewTemp = _target.GetView();
            _target = new RenderTexture(size.X, size.Y)
            {
                Smooth = true
            };
            _target.SetView(viewTemp);
        }

        private void LostFocus(object sender, EventArgs e)
        {
            if (OnLostFocus != null)
            {
                OnLostFocus(sender, e);
            }
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
            if (OnMouseMove != null && DisplayContainsMouseMove(e))
            {
                OnMouseMove(sender, e, MouseCoordToDisplayCoord(e));
            }
        }

        private void MousePressed(object sender, MouseButtonEventArgs e)
        {
            if (OnMousePress != null)
            {
                OnMousePress(sender, e, MouseCoordToDisplayCoord(e));
            }
        }

        private void MouseReleased(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseRelease != null)
            {
                OnMouseRelease(sender, e, MouseCoordToDisplayCoord(e));
            }
        }

        private Vector2f MouseCoordToDisplayCoord(MouseMoveEventArgs e)
        { return MouseCoordToDisplayCoord(new Vector2i(e.X, e.Y)); }
        private Vector2f MouseCoordToDisplayCoord(MouseButtonEventArgs e)
        { return MouseCoordToDisplayCoord(new Vector2i(e.X, e.Y)); }
        private Vector2f MouseCoordToDisplayCoord(Vector2i e)
        {
            //e.X *= 2;
            //e.Y *= 2;
            var rawDisplayCoord = e - new Vector2i((int) Math.Round(Position.X), (int) Math.Round(Position.Y));
            rawDisplayCoord.X = (int)Math.Round(rawDisplayCoord.X * 1 / _targetSpr.Scale.X);
            rawDisplayCoord.Y = (int)Math.Round(rawDisplayCoord.Y * 1 / _targetSpr.Scale.Y);
            return _target.MapPixelToCoords(rawDisplayCoord);
        }

        private bool DisplayContainsMouseMove(MouseMoveEventArgs e)
        {
            return (
                (Position.X <= e.X) &&
                (Position.X + _target.Size.X >= e.X) &&
                (Position.Y <= e.Y) &&
                (Position.Y + _target.Size.Y >= e.Y)
                );
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

        public static readonly IComparer<Tuple<Drawable, uint>> Comparer = new ZlevelDrawableCompare();
    }
}
