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
            AddItemToDraw(_ship, 3);
        }
    }
}
