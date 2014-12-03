using SFML.Graphics;

namespace Planetary_Explorers.Spaceships
{
    class SpaceshipImageGenerator
    {
        //http://web.archive.org/web/20080529020512/http://www.davebollinger.com/works/pixelspaceships/applet/PixelSpaceships.pde.txt

        //https://github.com/zfedoran/pixel-sprite-generator/blob/master/pixel-sprite-generator.js

        public SpaceshipImageGenerator()
        {
            
        }

        public Texture GenerateShipTexture()
        {
            return new Texture("Spaceships/ship.png");
        }

    }
}
