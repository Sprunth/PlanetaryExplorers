using System;
using System.Collections.Generic;
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
                //_planetCircle.Texture.Repeated = true;
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
                if (args.Button == Mouse.Button.Middle)
                {
                    _planetCircle.Texture = Planet.GeneratePlanetTexture(new Vector2u(32, 32));
                    _planetCircle.TextureRect = new IntRect(0, 0, 16, 16);
                }
            };
        }
        public override void Update()
        {
           _planetCircle.TextureRect = new IntRect(_planetCircle.TextureRect.Left + 1,
               0, _planetCircle.TextureRect.Width, _planetCircle.TextureRect.Height);
            
            base.Update();
        }
    }
}
