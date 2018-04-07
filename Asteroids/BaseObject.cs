using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        public delegate void Message();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        /// <summary>
        /// Абстрактный метод для вывода объектов на экран
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Абстрактный метод для перерисовки объектов 
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Виртуальный метод для перерисовки конкретных объектов, после столкновения
        /// </summary>
        public virtual void Update(bool collision)
        {

        }

        /// <summary>
        /// Bool метод для определения коллизии двух объектов
        /// </summary>
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);
    }

}
