using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для создания, отрисовки и перемещения астероидов
    /// </summary>
    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        public int Power { get; set; } = 3;
        Image newImage = Image.FromFile(@"..\..\asteroid.png");

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(
                new Point(Pos.X, Pos.Y),
                new Point(Dir.X, Dir.Y),
                new Size(Size.Width, Size.Height))
                {Power = Power};

            return asteroid;

        }

        /// <summary>
        /// Рисуем астероид из asteroid.png
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(newImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Апдейт координат астероида с учетом вектора движения или перерисовка с новыми координатами, если достигнут предел экрана
        /// </summary>
        public override void Update()
        {
            if (Pos.X < 0)
            {
                Pos.X = Game.Width;
                Pos.Y = Game.random.Next(0, Game.Height);
            }
            Pos.X = Pos.X + Dir.X;
        }

        /// <summary>
        /// Перерисовка в случае коллизии
        /// </summary>
        public override void Update(bool collision)
        {
            Pos.X = Game.Width;
            Pos.Y = Game.random.Next(0, Game.Height);
        }

        int IComparable<Asteroid>.CompareTo(Asteroid otherAsteroid)
        {
            if (Power > otherAsteroid.Power)
                return 1;
            if (Power < otherAsteroid.Power)
                return -1;
            return 0;
        }
    }
}
