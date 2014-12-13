using System;
using SFML.Graphics;
using SFML.Window;

namespace Planetary_Explorers.Spaceships
{
    class Spaceship : DrawableObject
    {
        private static readonly SpaceshipImageGenerator _imgGenerator = new SpaceshipImageGenerator();

        private Sprite _ship;

        public Spaceship(Display parentDisplay) : base(parentDisplay)
        {
            _ship = new Sprite(_imgGenerator.GenerateShipTexture())
            {
                Position = new Vector2f(40,80)
            };
            _ship.Origin = new Vector2f(_ship.Texture.Size.X/2f, _ship.Texture.Size.Y/2f);
            AddItemToDraw(_ship, 3);

            parentDisplay.OnMousePress += parentDisplay_OnMousePress;
        }

        void parentDisplay_OnMousePress(object sender, MouseButtonEventArgs e)
        {
            if (ContainsVector(e.X, e.Y))
            {
                Console.WriteLine(e);
            }
        }

        public override bool ContainsVector(double x, double y)
        {
            var mouseCoords = parentDisplay.Target.MapPixelToCoords(
                new Vector2i((int)Math.Round(x), (int)Math.Round(y)));
            
            return (
                (Math.Abs(_ship.Position.X - mouseCoords.X) <= _ship.Texture.Size.X/2.0) &&
                (Math.Abs(_ship.Position.Y - mouseCoords.Y) <= _ship.Texture.Size.Y/2.0)
                );
        }
    }
}
