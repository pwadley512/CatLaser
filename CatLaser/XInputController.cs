using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.IO;
using System.Drawing;
using SharpDX.XInput;

namespace CatLaser
{
    class XInputController
    {
        Controller controller;
        Gamepad gamepad;
        public bool connected = false;
        public int deadband = 2500;
        public float leftThumbX = 0.0f;
        public float leftThumbY = 0.0f;
        public float rightThumbX = 0.0f;
        public float rightThumbY = 0.0f;
        public float leftTrigger, rightTrigger;

        public XInputController()
        {
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;
        }

        // Call this method to update all class values
        public Boolean Update()
        {
            if (!connected)
                return false;

            gamepad = controller.GetState().Gamepad;

            var leftThumbXPrev = leftThumbX;
            var leftThumbYPrev = leftThumbY;
            var rightThumbXPrev = rightThumbX;
            var rightThumbYPrev = rightThumbY;

            leftThumbX = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
            leftThumbY = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
            rightThumbX = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
            rightThumbY = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

            //leftTrigger  = gamepad.LeftTrigger;
            //rightTrigger =  gamepad.RightTrigger;
            return true;
            //if (leftThumbXPrev != leftThumbX || leftThumbYPrev != leftThumbY || rightThumbXPrev != rightThumbX || rightThumbYPrev != rightThumbY)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
