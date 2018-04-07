using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Класс для создания, отрисовки и перемещения пули
    /// </summary>
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        /// <summary>
        /// Рисуем пулю как Rect (нужно для определения коллизии)
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Задаем постоянную скорость
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + 15;
        }

    }
}
