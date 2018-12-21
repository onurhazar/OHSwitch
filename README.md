# OHSwitch
Fully Customizable UISwitch written in C#

![Demo](https://github.com/onurhazar/OHSwitch/blob/master/OHSwitch.gif)

## Requirements
iOS 8.3+
Xcode 8.0+

## How to Use
```C#
var frame = new CGRect(0, 0, 50, 26);
ohSwitch = new OHSwitch(frame)
{
    ThumbOffFillColor = UIColor.Gray,
    ThumbOnFillColor = UIColor.White,
    TrackOffFillColor = UIColor.Red,
    TrackOnFillColor = UIColor.Green
};
View.AddSubview(ohSwitch);

//Add event handler
ohSwitch.AddTarget(OnSwitchChanged, UIControlEvent.ValueChanged);

//Event handler
private void OnSwitchChanged(object sender, EventArgs e)
{
    Debug.WriteLine("Switch value is changed");
}

//Check switch status
if (ohSwitch.On)
{
    Debug.WriteLine("Switch is on, do something here");
}

//Set switch status On/Off, w/ or w/o Animation
ohSwitch.SetOn(true, true);
```
