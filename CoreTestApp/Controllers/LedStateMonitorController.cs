using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace CoreTestApp.Controllers
{
    public class LedStateMonitorController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var pinStates = getPinState();

            ViewData["Pin8Value"] = pinStates.First(x => x.pinNo == 8).pinValue;
            ViewData["Pin22Value"] = pinStates.First(x => x.pinNo == 22).pinValue;

            return View();
        }

        /// <summary>
        /// LEDを指定状態に変更します
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangeLed(int pinNo, bool isOn)
        {
            // make pin operator from 'bcm' pin no
            var pin = Pi.Gpio[pinNo];

            // set pinmode 'output'
            pin.PinMode = GpioPinDriveMode.Output;

            // change value
            pin.Write(isOn);

            // get PinMode
            var gpioValue = pin.ReadValue();
            Console.WriteLine($"pinNo:{pinNo} / value:{gpioValue}");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// アクティブなピンの値を全て取得します
        /// </summary>
        /// <returns></returns>
        private IEnumerable<(int pinNo, GpioPinValue pinValue)> getPinState()
        {
            return Pi.Gpio.Select(x => (x.PinNumber, x.ReadValue()));
        }
    }
}