using System;
using System.Diagnostics;


namespace Asteroids
{
    class Journal
    {
        /// <summary>
        /// Класс Паттерн “наблюдатель”
        /// </summary>
        public event Action<string> EventPost;
        public string Eventname { get; set; }

        public Journal() { }

        public void PublicMessage(string Message)
        {
            EventPost?.Invoke(Message);
        }

        public void PostEvent(string Msg)
        {
            Game.sw.WriteLine($"{DateTime.Now}: {Msg}");
            Debug.WriteLine($"{DateTime.Now}: {Msg}");
        }

    }
}
