using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatLaser
{
    class XBOXONE
    {
        Controller controller;
        Gamepad gamepad;
        public bool connected = false;
        public int deadband = 2000;
        public float leftThumbY, leftThumbX, rightThumbY, rightThumbX;
        public float leftTrigger, rightTrigger;

        public XBOXONE()
        {
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;
        }

        // Call this method to update all class values
        public void Update()
        {
            if (!connected)
                return;

            gamepad = controller.GetState().Gamepad;

            leftThumbX= (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
            leftThumbY = (Math.Abs((float)gamepad.LeftThumbY) < (deadband+2000)) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
            rightThumbX = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
            rightThumbY = (Math.Abs((float)gamepad.RightThumbY) < deadband + 2000) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;
            
            leftTrigger = gamepad.LeftTrigger;
            rightTrigger = gamepad.RightTrigger;
        }
    }
}
