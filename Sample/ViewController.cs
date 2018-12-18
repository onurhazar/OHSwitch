using System;
using UIKit;
using OHSwitchLib;
using CoreGraphics;
using System.Diagnostics;

namespace Sample
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var ohSwitch = new OHSwitch(new CGRect((UIScreen.MainScreen.Bounds.Width - 50) / 2, (UIScreen.MainScreen.Bounds.Height - 26) / 2, 50, 26));
            View.AddSubview(ohSwitch);

            //ohSwitch.SetOn(true, true);

            ohSwitch.AddTarget(OnSwitchChanged, UIControlEvent.ValueChanged);

            if (ohSwitch.On)
            {
                Debug.WriteLine("Switch is on, do something here");
            }
        }

        private void OnSwitchChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Switch value is changed");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
