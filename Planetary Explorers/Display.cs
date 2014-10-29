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
        public static Display ActiveDisplay { get; set; }

        protected RenderTexture target;

        private List<IUpdateable> toUpdate; 
        
        protected Sprite spr;
        public Vector2f Position { get { return spr.Position; } set { spr.Position = value; } }

        public delegate void KeyPressHandler(object sender, KeyEventArgs e);
        public event KeyPressHandler OnKeyPress;

        public Display(Vector2u displaySize, bool physicsEnabled = false)
        {
            toUpdate = new List<IUpdateable>();

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

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            if (OnKeyPress != null)
            {
                OnKeyPress(sender, e);
            }
        }
    }
}
