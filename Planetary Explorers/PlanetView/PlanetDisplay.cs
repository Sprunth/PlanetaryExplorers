using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planetary_Explorers.SpaceMap;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.PlanetView
{
    class PlanetDisplay : Display
    {
        private readonly CircleShape _planetCircle;

        private Planet _focusedPlanet;
        public Planet FocusedPlanet
        {
            get { return _focusedPlanet; }
            set
            {
                _focusedPlanet = value;
                _planetCircle.Texture = _focusedPlanet.SurfaceTexture;
                _planetCircle.TextureRect = new IntRect(0, 0, (int)Math.Round(_planetCircle.Texture.Size.X/2f), (int)Math.Round(_planetCircle.Texture.Size.Y/2f));
            }
        }

        public PlanetDisplay(Vector2u displaySize) : base(displaySize)
        {
            _planetCircle = new CircleShape()
            {
                Radius = 64,
                Origin = new Vector2f(32,32),
                OutlineColor = Color.Black,
                OutlineThickness = 3,
                Position = new Vector2f(96,96),
            };

            AddItemToDraw(_planetCircle, 1);

            OnMousePress += delegate(object sender, MouseButtonEventArgs args, Vector2f coords)
            {

            };
        }
        public override void Update()
        {
            //TODO: Precalculate and store the width/height. It should not change
            _planetCircle.TextureRect = new IntRect(_planetCircle.TextureRect.Left + 1,
                _planetCircle.TextureRect.Top+1, (int)Math.Round(_planetCircle.Texture.Size.X / 2f), (int)Math.Round(_planetCircle.Texture.Size.Y / 2f));

            if (_planetCircle.TextureRect.Left > _planetCircle.Texture.Size.X / 2f)
            {
                _planetCircle.TextureRect = new IntRect(0, _planetCircle.TextureRect.Top, (int)Math.Round(_planetCircle.Texture.Size.X / 2f), (int)Math.Round(_planetCircle.Texture.Size.Y / 2f));
            }
            if (_planetCircle.TextureRect.Top > _planetCircle.Texture.Size.Y / 2f)
            {
                _planetCircle.TextureRect = new IntRect(_planetCircle.TextureRect.Left, 0, (int)Math.Round(_planetCircle.Texture.Size.X / 2f), (int)Math.Round(_planetCircle.Texture.Size.Y / 2f));
            }
            base.Update();
        }
    }
}
