using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace CoreTestApp.Controllers
{
    public class AlarmController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("alermTest");

            // TODO 
            return View();
        }

        /// <summary>
        /// 指定した音階のビープ音を再生します
        /// </summary>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public IActionResult Beep(Pitch pitch)
        {
            var pin = Pi.Gpio[3];

            Console.WriteLine("Sound On");

            switch (pitch)
            {
                case Pitch.Do:
                    pin.SoftToneFrequency = 523;
                    break;

                case Pitch.Re:
                    pin.SoftToneFrequency = 587;
                    break;

                case Pitch.Mi:
                    pin.SoftToneFrequency = 659;
                    break;

                case Pitch.Fa:
                    pin.SoftToneFrequency = 698;
                    break;

                case Pitch.So:
                    pin.SoftToneFrequency = 783;
                    break;

                case Pitch.Ra:
                    pin.SoftToneFrequency = 880;
                    break;

                case Pitch.Si:
                    pin.SoftToneFrequency = 987;
                    break;
            }

            System.Threading.Thread.Sleep(1000);

            pin.SoftToneFrequency = 0;

            Console.WriteLine("Sound Off");

            return RedirectToAction("Index");
        }
    }

    /// <summary>
    /// 音階
    /// </summary>
    public enum Pitch
    {
        Do,Re,Mi,Fa,So,Ra,Si
    }
}