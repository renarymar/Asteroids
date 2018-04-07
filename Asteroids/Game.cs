using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;



namespace Asteroids
{
    /// <summary>
    /// Класс, где задаем конкретное поведение игры
    /// </summary>
    class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static Timer _timer = new Timer { Interval = 100 };
        private static Star[] _stars;
        private static List<Asteroid> _asteroid;
        private static int _asteroidsInitialAmount;
        public static Energy[] _energy;
        private static Ship _ship;
        private static List<Bullet> _bullets;
        private static int _points = 0;
        public static Random random = new Random();
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static Journal journal;
        public static Journal gameEvent;
        public static StreamWriter sw = new StreamWriter(@"..\..\Log.txt");

        static Game() { }
        
        /// <summary>
        /// Инициализируем константы, вызываем метод Load(), стартуем таймер
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Width = form.Width;
            Height = form.Height;

            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            Observer();

            _timer.Start();
            _timer.Tick += Timer_Tick;

            
        }

        /// <summary>
        /// Создаем наблюдателя, подписываемся на gameEvent
        /// </summary>
        public static void Observer()
        {
            journal = new Journal();
            gameEvent = new Journal();

            gameEvent.EventPost += journal.PostEvent;
        }


        /// <summary>
        ///Метод для вызова Draw() и Update() на каждый тик таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Обработчик нажатия клавиш (перемещаем корабль, генерируем пули)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _bullets.Add(new Bullet(
                    new Point(_ship.Rect.X + 30, _ship.Rect.Y + 30),
                    new Point(4, 0),
                    new Size(4, 1)));
                gameEvent.PublicMessage("Произведен выстрел.");
            }    
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Метод для генерации астероидов
        /// </summary>
        /// <param name="amount"></param>
        public static void LoadAsteroids(int amount)
        {
            _asteroid = new List<Asteroid>();
            for (var i = 0; i < amount; i++)
            {
                int r = random.Next(30, 50);
                _asteroid.Add(new Asteroid(
                        new Point(Game.Width, random.Next(0, Game.Height)),
                        new Point(-r / 5, r),
                        new Size(r, r)));
            }

        }
        /// <summary>
        /// Метод для создания массивов объектов типа Star, Asteroid и Пуля
        /// </summary>
        public static void Load()
        {
            _stars = new Star[150];
            //Изначальное количество астероидов
            _asteroidsInitialAmount = 5;
            //Первоначальная генерация астероидов
            LoadAsteroids(_asteroidsInitialAmount);
            _energy = new Energy[5];
            _bullets = new List<Bullet>();

            _ship = new Ship(
                new Point(10, 400),
                new Point(5, 5),
                new Size(60, 60));

            for (var i = 0; i < _stars.Length; i++)
            {
                int r = random.Next(5, 50);

                _stars[i] = new Star(
                    new Point(random.Next(0, Game.Width), random.Next(0, Game.Height)),
                    new Point(random.Next(1, 3), 0),
                    new Size(2, 2));
            }

            for (var i = 0; i < _energy.Length; i++)
            {
                int r = random.Next(30, 50);
                _energy[i] = new Energy(
                    new Point(Game.Width, random.Next(0, Game.Height)),
                    new Point(-r / 5, r),
                    new Size(r, r));
            }     
        }

        /// <summary>
        /// Метод для отрисовки объектов на экране
        /// </summary>
        public static void Draw()
        {
            Game.Buffer.Graphics.Clear(Color.Black);

            foreach (Star obj in _stars)
                obj.Draw();

            foreach (Asteroid asteroid in _asteroid)
                asteroid?.Draw();

            foreach (Energy energy in _energy)
                energy?.Draw();

            foreach (Bullet b in _bullets) b.Draw();
            
            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Points:" + _points, SystemFonts.DefaultFont, Brushes.White, 0, 20);
                Buffer.Graphics.DrawString("Asteroid Count:" + _asteroid.Count, SystemFonts.DefaultFont, Brushes.White, 0, 40);

            }

            Game.Buffer.Render();

        }

        /// <summary>
        /// Метод для перемещения или регенерации объектов в случае коллизии
        /// </summary>
        public static void Update()
        {
            GC.Collect();

            foreach (Star obj in _stars)
                obj.Update();

            foreach (Bullet b in _bullets)
                b.Update();

            for (var i = 0; i < _asteroid.Count; i++)
            {
                if (!_asteroid.Contains(_asteroid[i])) continue;
                _asteroid[i].Update();

                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (_asteroid.Contains(_asteroid[i]) && _bullets[j].Collision(_asteroid[i]))
                    {
                        gameEvent.PublicMessage("Астероид сбит.");
                        System.Media.SystemSounds.Hand.Play();
                        _points++;
                        _bullets.RemoveAt(j); // при коллизии удаляем пулю из коллекции
                        j--;
                        _asteroid.RemoveAt(i); //а также сам астероид
                        if (_asteroid.Count == 0)
                        {
                            _asteroidsInitialAmount += 1;
                            LoadAsteroids(_asteroidsInitialAmount);
                        }
                        continue;
                    }
                }

                if (_asteroid.Contains(_asteroid[i]) && _ship.Collision(_asteroid[i]))
                {
                    _ship.EnergyLow(random.Next(1, 10)); //уменьшаем энергию в случае коллизии
                    _asteroid.RemoveAt(i); // удаляем астероид из коллекции
                    System.Media.SystemSounds.Asterisk.Play();
                    gameEvent.PublicMessage("Столкновение с аcтероидом.");
                }
                
                if (_ship.Energy <= 0)
                {
                    _ship?.Die();
                    Game.gameEvent.PublicMessage("Корабль разрушен.");
                }
            }

            for (var i = 0; i < _energy.Length; i++)
            {
                if (_energy[i] == null) continue;
                _energy[i].Update();

                if (_energy[i] != null && !_ship.Collision(_energy[i])) continue;

                _ship.EnergyHigh(random.Next(1, 10));
                _energy[i] = null;
                System.Media.SystemSounds.Exclamation.Play();
                gameEvent.PublicMessage("Найдена аптечка.");
            }
        }

        /// <summary>
        /// Закрываем файл, отписываемся от событий, рисуем Game Over
        /// </summary>
        public static void Finish()
        {
            GC.Collect();

            _timer.Stop();
            gameEvent.EventPost -= journal.PostEvent;
            sw.Close();

            Buffer.Graphics.DrawString(
                "Game Over",
                new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline),
                Brushes.DarkRed, Width / 2, Height / 2);

            Buffer.Render();
        }
    }

}


