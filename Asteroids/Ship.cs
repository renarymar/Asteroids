using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для создания объекта типа корабль
    /// </summary>
    class Ship : BaseObject
    {
        private int _energy = 100;
        Image newImage = Image.FromFile(@"..\..\ship.png");


        public int Energy => _energy;

        public void EnergyLow(int n)
        {          
            _energy -= n;
        }

        public void EnergyHigh(int n)
        {
            if (_energy + n <= 100) _energy += n;
            _energy = 100;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(newImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }

        public static event Message MessageDie;
    }
}
