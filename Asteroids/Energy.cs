using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для создания, отрисовки и апдейта аптечек
    /// </summary>
    class Energy : BaseObject
    {
        public int Power { get; set; } = 3;
        Image newImage = Image.FromFile(@"..\..\energy.png");

        public Energy(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }


        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(newImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            if (Pos.X < 0)
            {
                Pos.X = Game.Width;
                Pos.Y = Game.random.Next(0, Game.Height);
            }
            Pos.X = Pos.X + Dir.X;
        }

    }
}
