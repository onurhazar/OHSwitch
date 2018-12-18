using System;
using CoreAnimation;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Linq;

namespace OHSwitchLib
{
    public class OHSwitch : UIControl
    {
        #region private props

        CALayer backLayer;
        CALayer thumbLayer;
        float thumbDelta = 6;

        UIColor _trackOffBorderColor = UIColor.FromRGB(177, 187, 195);
        UIColor _trackOffPushBorderColor = UIColor.FromRGB(224, 228, 233);
        UIColor _trackOffFillColor = UIColor.Clear;
        UIColor _thumbOffBorderColor = UIColor.FromRGB(177, 187, 195);
        UIColor _thumbOffPushBorderColor = UIColor.FromRGB(177, 187, 195);
        UIColor _thumbOffFillColor = UIColor.White;
        UIColor _trackOnFillColor = UIColor.Clear;
        UIColor _trackOnBorderColor = UIColor.FromRGB(255, 184, 49);
        UIColor _thumbOnBorderColor = UIColor.FromRGB(240, 170, 38);
        UIColor _thumbOnFillColor = UIColor.White;
        UIColor _thumbShadowColor = UIColor.FromRGBA(145, 156, 166, 0.26f);
        float _thumbDiameter = 14;
        float _cornerRadius = 13;
        float _thumbCornerRadius = 7;
        bool _shouldFillOnPush = true;
        float _trackInset;
        float _shadowStrength = 1;
        bool _on;

        #endregion

        #region public props

        /// <summary>
        ///   Track border color when state is Off
        /// </summary>
        public UIColor TrackOffBorderColor
        {
            get
            {
                return _trackOffBorderColor;
            }
            set
            {
                _trackOffBorderColor = value;
                backLayer.BorderColor = _trackOffBorderColor.CGColor;
            }
        }

        /// <summary>
        ///   Track border color when switch is pressed (touch began, but not ended). Border width is animated and fills inside of the track completely if `ShouldFillOnPush` is true
        /// </summary>
        public UIColor TrackOffPushBorderColor
        {
            get
            {
                return _trackOffPushBorderColor;
            }
            set
            {
                _trackOffPushBorderColor = value;
            }
        }

        /// <summary>
        ///   Track fill color when state is Off
        /// </summary>
        public UIColor TrackOffFillColor
        {
            get
            {
                return _trackOffFillColor;
            }
            set
            {
                _trackOffFillColor = value;
                backLayer.BackgroundColor = _trackOffFillColor.CGColor;
            }
        }

        /// <summary>
        ///   Thumb border color when state is Off
        /// </summary>
        public UIColor ThumbOffBorderColor
        {
            get
            {
                return _thumbOffBorderColor;
            }
            set
            {
                _thumbOffBorderColor = value;
                this.thumbLayer.BorderColor = _thumbOffBorderColor.CGColor;
            }
        }

        /// <summary>
        ///   Thumb border color when switch is pressed (touch began, but not ended). Set to the same value as `ThumbOffBorderColor` if border color animation is not desireable
        /// </summary>
        public UIColor ThumbOffPushBorderColor
        {
            get
            {
                return _thumbOffPushBorderColor;
            }
            set
            {
                _thumbOffPushBorderColor = value;
            }
        }

        /// <summary>
        ///   Thumb fill color when state is Off
        /// </summary>
        public UIColor ThumbOffFillColor
        {
            get
            {
                return _thumbOffFillColor;
            }
            set
            {
                _thumbOffFillColor = value;
                thumbLayer.BackgroundColor = _thumbOffFillColor.CGColor;
            }
        }

        /// <summary>
        ///   Track fill color when state is On
        /// </summary>
        public UIColor TrackOnFillColor
        {
            get
            {
                return _trackOnFillColor;
            }
            set
            {
                _trackOnFillColor = value;
            }
        }

        /// <summary>
        ///   Track border color when state is On. If `ShouldFillOnPush` is true then border completely fills track
        /// </summary>
        public UIColor TrackOnBorderColor
        {
            get
            {
                return _trackOnBorderColor;
            }
            set
            {
                _trackOnBorderColor = value;
            }
        }

        /// <summary>
        ///   Thumb border color when state is On
        /// </summary>
        public UIColor ThumbOnBorderColor
        {
            get
            {
                return _thumbOnBorderColor;
            }
            set
            {
                _thumbOnBorderColor = value;
            }
        }

        /// <summary>
        ///   Thumb border color when state is On
        /// </summary>
        public UIColor ThumbOnFillColor
        {
            get
            {
                return _thumbOnFillColor;
            }
            set
            {
                _thumbOnFillColor = value;
            }
        }

        /// <summary>
        ///   Thumb shadow color. Alpha value can be used to change shadow opacity
        /// </summary>
        public UIColor ThumbShadowColor
        {
            get
            {
                return _thumbShadowColor;
            }
            set
            {
                _thumbShadowColor = value;
                thumbLayer.ShadowColor = _thumbShadowColor.CGColor;
            }
        }

        /// <summary>
        ///   Diameter of thumb in pixels
        /// </summary>
        public float ThumbDiameter
        {
            get
            {
                return _thumbDiameter;
            }
            set
            {
                _thumbDiameter = value;

                this.thumbLayer.Frame = GetThumbOffRect();
                thumbLayer.CornerRadius = _thumbDiameter / 2;
            }
        }

        /// <summary>
        ///   Track corner radius
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = value;
                backLayer.CornerRadius = _cornerRadius;
            }
        }

        /// <summary>
        ///   Thumb corner radius
        /// </summary>
        public float ThumbCornerRadius
        {
            get
            {
                return _thumbCornerRadius;
            }
            set
            {
                _thumbCornerRadius = value;
                thumbLayer.CornerRadius = _thumbCornerRadius;
            }
        }

        /// <summary>
        ///   Track border width is animated and fills inside of the track completely when switch is pressed if true
        /// </summary>
        public bool ShouldFillOnPush
        {
            get
            {
                return _shouldFillOnPush;
            }
            set
            {
                _shouldFillOnPush = value;
            }
        }

        /// <summary>
        ///   Track inset from the outer control frame. Usable if thumb is bigger than track
        /// </summary>
        public float TrackInset
        {
            get
            {
                return _trackInset;
            }
            set
            {
                _trackInset = value;
            }
        }

        /// <summary>
        ///   Overall strength of thumb shadow
        /// </summary>
        public float ShadowStrength
        {
            get
            {
                return _shadowStrength;
            }
            set
            {
                _shadowStrength = value;
                thumbLayer.ShadowOffset = new CGSize(0, 1.5 * _shadowStrength);
                thumbLayer.ShadowRadius = (nfloat)0.6 * (_shadowStrength * 2);
            }
        }

        /// <summary>
        ///   Current switch status
        /// </summary>
        public bool On
        {
            get
            {
                return _on;
            }
        }

        #endregion

        #region Ctor

        public OHSwitch(CGRect frame)
        {
            Frame = frame;

            ClipsToBounds = false;

            backLayer = new CALayer();
            backLayer.Frame = new CGRect(0, 0, 50, 26);
            backLayer.CornerRadius = _cornerRadius;
            backLayer.BorderWidth = 1;
            backLayer.BorderColor = _trackOffBorderColor.CGColor;
            backLayer.BackgroundColor = _trackOffFillColor.CGColor;

            Layer.AddSublayer(backLayer);


            thumbLayer = new CALayer();
            thumbLayer.Frame = GetThumbOffRect();
            thumbLayer.CornerRadius = _thumbCornerRadius;
            thumbLayer.BorderWidth = 1;
            thumbLayer.BorderColor = _thumbOffBorderColor.CGColor;
            thumbLayer.BackgroundColor = _thumbOffFillColor.CGColor;
            thumbLayer.ShadowOffset = new CGSize(0, 1.5 * _shadowStrength);
            thumbLayer.ShadowRadius = (nfloat)0.6 * (_shadowStrength * 2);
            thumbLayer.ShadowColor = _thumbShadowColor.CGColor;
            thumbLayer.ShadowOpacity = 1;

            Layer.AddSublayer(thumbLayer);
        }

        #endregion

        #region Override Methods

        public override CGSize IntrinsicContentSize
        {
            get
            {
                return new CGSize(50, 26);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            backLayer.Frame = new CGRect(0 + TrackInset, 0 + TrackInset, Frame.Width - TrackInset * 2, Frame.Height - TrackInset * 2);

            if (_on)
            {
                thumbLayer.Frame = GetThumbOnRect();

                if (_shouldFillOnPush)
                {
                    backLayer.BorderWidth = Frame.Height / 2;
                }
            }
            else
            {
                thumbLayer.Frame = GetThumbOffRect();
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if(_on)
            {
                var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
                thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffRect()));
                thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffPushRect()));
                thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
                thumbBoundsAnimation.Duration = 0.25;
                thumbBoundsAnimation.RemovedOnCompletion = false;

                var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
                thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOnPos()));
                thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOnPushPos()));
                thumbPosAnimation.FillMode = CAFillMode.Forwards;
                thumbPosAnimation.Duration = 0.25;
                thumbPosAnimation.RemovedOnCompletion = false;

                var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                thumbBorderColorAnimation.SetFrom(_thumbOnBorderColor.CGColor);
                thumbBorderColorAnimation.SetTo(_thumbOnBorderColor.CGColor);
                thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
                thumbBorderColorAnimation.Duration = 0.25;
                thumbBorderColorAnimation.RemovedOnCompletion = false;

                var thumbFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
                thumbFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                thumbFillColorAnimation.SetFrom(_thumbOnFillColor.CGColor);
                thumbFillColorAnimation.SetTo(_thumbOnFillColor.CGColor);
                thumbFillColorAnimation.FillMode = CAFillMode.Forwards;
                thumbFillColorAnimation.Duration = 0.25;
                thumbFillColorAnimation.RemovedOnCompletion = false;

                var animThumbGroup = new CAAnimationGroup();
                animThumbGroup.Duration = 0.25;
                animThumbGroup.FillMode = CAFillMode.Forwards;
                animThumbGroup.RemovedOnCompletion = false;
                animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation, thumbFillColorAnimation };

                thumbLayer.RemoveAllAnimations();
                thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
            }
            else
            {
                var bgBorderAnimation = CABasicAnimation.FromKeyPath("borderWidth");
                bgBorderAnimation.TimingFunction = new CAMediaTimingFunction(0.55f, 0.055f, 0.675f, 0.19f);
                bgBorderAnimation.SetFrom(NSNumber.FromFloat(1));
                bgBorderAnimation.SetTo(NSNumber.FromNFloat(Frame.Height / 2));
                bgBorderAnimation.FillMode = CAFillMode.Forwards;
                bgBorderAnimation.Duration = 0.25;
                bgBorderAnimation.RemovedOnCompletion = false;

                var bgBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                bgBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.55f, 0.055f, 0.675f, 0.19f);
                bgBorderColorAnimation.SetFrom(_trackOffBorderColor.CGColor);
                bgBorderColorAnimation.SetTo(_trackOffPushBorderColor.CGColor);
                bgBorderColorAnimation.FillMode = CAFillMode.Forwards;
                bgBorderColorAnimation.Duration = 0.25;
                bgBorderColorAnimation.RemovedOnCompletion = false;

                var animGroup = new CAAnimationGroup();
                animGroup.Duration = 0.25;
                animGroup.FillMode = CAFillMode.Forwards;
                animGroup.RemovedOnCompletion = false;
                animGroup.Animations = new CAAnimation[] { bgBorderColorAnimation };

                if(_shouldFillOnPush)
                {
                    animGroup.Animations.Concat(new CAAnimation[] { bgBorderAnimation });
                }

                backLayer.AddAnimation(animGroup, "bgAnimation");

                var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
                thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffRect()));
                thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffPushRect()));
                thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
                thumbBoundsAnimation.Duration = 0.25;
                thumbBoundsAnimation.RemovedOnCompletion = false;

                var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
                thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOffPos()));
                thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOffPushPos()));
                thumbPosAnimation.FillMode = CAFillMode.Forwards;
                thumbPosAnimation.Duration = 0.25;
                thumbPosAnimation.RemovedOnCompletion = false;

                var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.55f, 0.055f, 0.675f, 0.19f);
                thumbBorderColorAnimation.SetFrom(_thumbOffBorderColor.CGColor);
                thumbBorderColorAnimation.SetTo(_thumbOffPushBorderColor.CGColor);
                thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
                thumbBorderColorAnimation.Duration = 0.25;
                thumbBorderColorAnimation.RemovedOnCompletion = false;

                var animThumbGroup = new CAAnimationGroup();
                animThumbGroup.Duration = 0.25;
                animThumbGroup.FillMode = CAFillMode.Forwards;
                animThumbGroup.RemovedOnCompletion = false;
                animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation };

                thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            var touch = touches.AnyObject as UITouch;
            if(touch != null)
            {
                var touchPoint = touch.LocationInView(this);
                if(Bounds.Contains(touchPoint))
                {
                    if(_on)
                    {
                        OnToOffAnim();
                    }
                    else
                    {
                        OffToOnAnim();
                    }

                    _on = !_on;

                    SendActionForControlEvents(UIControlEvent.ValueChanged);
                }
                else
                {
                    if (_on)
                    {
                        var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
                        thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                        thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffPushRect()));
                        thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffRect()));
                        thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
                        thumbBoundsAnimation.Duration = 0.25;
                        thumbBoundsAnimation.RemovedOnCompletion = false;

                        var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
                        thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.175f, 0.885f, 0.32f, 1.275f);
                        thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOnPushPos()));
                        thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOnPos()));
                        thumbPosAnimation.FillMode = CAFillMode.Forwards;
                        thumbPosAnimation.Duration = 0.25;
                        thumbPosAnimation.RemovedOnCompletion = false;

                        var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                        thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                        thumbBorderColorAnimation.SetFrom(_thumbOnBorderColor.CGColor);
                        thumbBorderColorAnimation.SetTo(_thumbOnBorderColor.CGColor);
                        thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
                        thumbBorderColorAnimation.Duration = 0.25;
                        thumbBorderColorAnimation.RemovedOnCompletion = false;

                        var thumbFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
                        thumbFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                        thumbFillColorAnimation.SetFrom(_thumbOnFillColor.CGColor);
                        thumbFillColorAnimation.SetTo(_thumbOnFillColor.CGColor);
                        thumbFillColorAnimation.FillMode = CAFillMode.Forwards;
                        thumbFillColorAnimation.Duration = 0.25;
                        thumbFillColorAnimation.RemovedOnCompletion = false;

                        var animThumbGroup = new CAAnimationGroup();
                        animThumbGroup.Duration = 0.25;
                        animThumbGroup.FillMode = CAFillMode.Forwards;
                        animThumbGroup.RemovedOnCompletion = false;
                        animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation, thumbFillColorAnimation };

                        thumbLayer.RemoveAllAnimations();
                        thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
                    }
                    else
                    {
                        var bgBorderAnimation = CABasicAnimation.FromKeyPath("borderWidth");
                        bgBorderAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                        bgBorderAnimation.SetFrom(NSNumber.FromNFloat(Frame.Height / 2));
                        bgBorderAnimation.SetTo(NSNumber.FromFloat(1));
                        bgBorderAnimation.FillMode = CAFillMode.Forwards;
                        bgBorderAnimation.Duration = 0.25;
                        bgBorderAnimation.RemovedOnCompletion = false;

                        var bgBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                        bgBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
                        bgBorderColorAnimation.SetFrom(_trackOffPushBorderColor.CGColor);
                        bgBorderColorAnimation.SetTo(_trackOffBorderColor.CGColor);
                        bgBorderColorAnimation.FillMode = CAFillMode.Forwards;
                        bgBorderColorAnimation.Duration = 0.25;
                        bgBorderColorAnimation.RemovedOnCompletion = false;

                        var animGroup = new CAAnimationGroup();
                        animGroup.Duration = 0.25;
                        animGroup.FillMode = CAFillMode.Forwards;
                        animGroup.RemovedOnCompletion = false;
                        animGroup.Animations = new CAAnimation[] { bgBorderColorAnimation };

                        if (_shouldFillOnPush)
                        {
                            animGroup.Animations.Concat(new CAAnimation[] { bgBorderAnimation });
                        }

                        backLayer.RemoveAllAnimations();
                        backLayer.AddAnimation(animGroup, "bgAnimation");

                        var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
                        thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
                        thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffPushRect()));
                        thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffRect()));
                        thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
                        thumbBoundsAnimation.Duration = 0.25;
                        thumbBoundsAnimation.RemovedOnCompletion = false;

                        var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
                        thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
                        thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOffPushPos()));
                        thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOffPos()));
                        thumbPosAnimation.FillMode = CAFillMode.Forwards;
                        thumbPosAnimation.Duration = 0.25;
                        thumbPosAnimation.RemovedOnCompletion = false;

                        var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
                        thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.55f, 0.055f, 0.675f, 0.19f);
                        thumbBorderColorAnimation.SetFrom(_thumbOffPushBorderColor.CGColor);
                        thumbBorderColorAnimation.SetTo(_thumbOffBorderColor.CGColor);
                        thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
                        thumbBorderColorAnimation.Duration = 0.25;
                        thumbBorderColorAnimation.RemovedOnCompletion = false;

                        var animThumbGroup = new CAAnimationGroup();
                        animThumbGroup.Duration = 0.25;
                        animThumbGroup.FillMode = CAFillMode.Forwards;
                        animThumbGroup.RemovedOnCompletion = false;
                        animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation };

                        thumbLayer.RemoveAllAnimations();
                        thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
                    }
                }
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
        }

        #endregion

        #region Private Methods

        CGRect GetThumbOffRect()
        {
            return new CGRect((Frame.Height - ThumbDiameter) / 2, (Frame.Height - ThumbDiameter) / 2, ThumbDiameter, ThumbDiameter);
        }

        CGRect GetThumbOffPushRect()
        {
            return new CGRect((Frame.Height - ThumbDiameter) / 2, (Frame.Height - ThumbDiameter) / 2, ThumbDiameter + thumbDelta, ThumbDiameter);
        }

        CGRect GetThumbOnRect()
        {
            return new CGRect((Frame.Width - ThumbDiameter) - ((Frame.Height - ThumbDiameter) / 2), (Frame.Height - ThumbDiameter) / 2, ThumbDiameter, ThumbDiameter);
        }

        CGPoint GetThumbOffPos()
        {
            return new CGPoint(Frame.Height / 2, Frame.Height / 2);
        }

        CGPoint GetThumbOffPushPos()
        {
            return new CGPoint(Frame.Height / 2 + thumbDelta - 3, Frame.Height / 2);
        }

        CGPoint GetThumbOnPos()
        {
            return new CGPoint(Frame.Width - Frame.Height / 2, Frame.Height / 2);
        }

        CGPoint GetThumbOnPushPos()
        {
            return new CGPoint(Frame.Width - Frame.Height / 2 - thumbDelta + 3, Frame.Height / 2);
        }

        void OnToOffAnim()
        {
            var bgBorderAnimation = CABasicAnimation.FromKeyPath("borderWidth");
            bgBorderAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            bgBorderAnimation.SetFrom(NSNumber.FromNFloat(Frame.Height / 2));
            bgBorderAnimation.SetTo(NSNumber.FromFloat(1));
            bgBorderAnimation.FillMode = CAFillMode.Forwards;
            bgBorderAnimation.Duration = 0.25;
            bgBorderAnimation.RemovedOnCompletion = false;

            var bgBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
            bgBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            bgBorderColorAnimation.SetFrom(_trackOnBorderColor.CGColor);
            bgBorderColorAnimation.SetTo(_trackOffBorderColor.CGColor);
            bgBorderColorAnimation.FillMode = CAFillMode.Forwards;
            bgBorderColorAnimation.Duration = 0.25;
            bgBorderColorAnimation.RemovedOnCompletion = false;

            var bgFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
            bgFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            bgFillColorAnimation.SetFrom(_trackOnFillColor.CGColor);
            bgFillColorAnimation.SetTo(_trackOffFillColor.CGColor);
            bgFillColorAnimation.FillMode = CAFillMode.Forwards;
            bgFillColorAnimation.Duration = 0.25;
            bgFillColorAnimation.RemovedOnCompletion = false;

            var animGroup = new CAAnimationGroup();
            animGroup.Duration = 0.25;
            animGroup.FillMode = CAFillMode.Forwards;
            animGroup.RemovedOnCompletion = false;
            animGroup.Animations = new CAAnimation[] { bgBorderColorAnimation, bgFillColorAnimation };

            if (_shouldFillOnPush)
            {
                animGroup.Animations.Concat(new CAAnimation[] { bgBorderAnimation });
            }

            backLayer.RemoveAllAnimations();
            backLayer.AddAnimation(animGroup, "bgAnimation");

            var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
            thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
            thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffPushRect()));
            thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffRect()));
            thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
            thumbBoundsAnimation.Duration = 0.25;
            thumbBoundsAnimation.RemovedOnCompletion = false;

            var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
            thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
            thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOnPushPos()));
            thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOffPos()));
            thumbPosAnimation.FillMode = CAFillMode.Forwards;
            thumbPosAnimation.Duration = 0.25;
            thumbPosAnimation.RemovedOnCompletion = false;

            var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
            thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            thumbBorderColorAnimation.SetFrom(_thumbOnBorderColor.CGColor);
            thumbBorderColorAnimation.SetTo(_thumbOffBorderColor.CGColor);
            thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
            thumbBorderColorAnimation.Duration = 0.25;
            thumbBorderColorAnimation.RemovedOnCompletion = false;

            var thumbFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
            thumbFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            thumbFillColorAnimation.SetFrom(_thumbOnFillColor.CGColor);
            thumbFillColorAnimation.SetTo(_thumbOffFillColor.CGColor);
            thumbFillColorAnimation.FillMode = CAFillMode.Forwards;
            thumbFillColorAnimation.Duration = 0.25;
            thumbFillColorAnimation.RemovedOnCompletion = false;

            var animThumbGroup = new CAAnimationGroup();
            animThumbGroup.Duration = 0.25;
            animThumbGroup.FillMode = CAFillMode.Forwards;
            animThumbGroup.RemovedOnCompletion = false;
            animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation, thumbFillColorAnimation };

            thumbLayer.RemoveAllAnimations();
            thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
        }

        void OffToOnAnim()
        {
            var bgBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
            bgBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            bgBorderColorAnimation.SetFrom(_trackOffPushBorderColor.CGColor);
            bgBorderColorAnimation.SetTo(_trackOnBorderColor.CGColor);
            bgBorderColorAnimation.FillMode = CAFillMode.Forwards;
            bgBorderColorAnimation.Duration = 0.25;
            bgBorderColorAnimation.RemovedOnCompletion = false;

            var bgFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
            bgFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            bgFillColorAnimation.SetFrom(_trackOffFillColor.CGColor);
            bgFillColorAnimation.SetTo(_trackOnFillColor.CGColor);
            bgFillColorAnimation.FillMode = CAFillMode.Forwards;
            bgFillColorAnimation.Duration = 0.25;
            bgFillColorAnimation.RemovedOnCompletion = false;

            var animGroup = new CAAnimationGroup();
            animGroup.Duration = 0.25;
            animGroup.FillMode = CAFillMode.Forwards;
            animGroup.RemovedOnCompletion = false;
            animGroup.Animations = new CAAnimation[] { bgBorderColorAnimation, bgFillColorAnimation };

            backLayer.AddAnimation(animGroup, "bgOffToOnAnimation");

            var thumbBoundsAnimation = CABasicAnimation.FromKeyPath("bounds");
            thumbBoundsAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
            thumbBoundsAnimation.SetFrom(NSValue.FromCGRect(GetThumbOffPushRect()));
            thumbBoundsAnimation.SetTo(NSValue.FromCGRect(GetThumbOffRect()));
            thumbBoundsAnimation.FillMode = CAFillMode.Forwards;
            thumbBoundsAnimation.Duration = 0.25;
            thumbBoundsAnimation.RemovedOnCompletion = false;

            var thumbPosAnimation = CABasicAnimation.FromKeyPath("position");
            thumbPosAnimation.TimingFunction = new CAMediaTimingFunction(0.77f, 0f, 0.175f, 1f);
            thumbPosAnimation.SetFrom(NSValue.FromCGPoint(GetThumbOffPushPos()));
            thumbPosAnimation.SetTo(NSValue.FromCGPoint(GetThumbOnPos()));
            thumbPosAnimation.FillMode = CAFillMode.Forwards;
            thumbPosAnimation.Duration = 0.25;
            thumbPosAnimation.RemovedOnCompletion = false;

            var thumbBorderColorAnimation = CABasicAnimation.FromKeyPath("borderColor");
            thumbBorderColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            thumbBorderColorAnimation.SetFrom(_thumbOffPushBorderColor.CGColor);
            thumbBorderColorAnimation.SetTo(_thumbOnBorderColor.CGColor);
            thumbBorderColorAnimation.FillMode = CAFillMode.Forwards;
            thumbBorderColorAnimation.Duration = 0.25;
            thumbBorderColorAnimation.RemovedOnCompletion = false;

            var thumbFillColorAnimation = CABasicAnimation.FromKeyPath("backgroundColor");
            thumbFillColorAnimation.TimingFunction = new CAMediaTimingFunction(0.165f, 0.84f, 0.44f, 1f);
            thumbFillColorAnimation.SetFrom(_thumbOffFillColor.CGColor);
            thumbFillColorAnimation.SetTo(_thumbOnFillColor.CGColor);
            thumbFillColorAnimation.FillMode = CAFillMode.Forwards;
            thumbFillColorAnimation.Duration = 0.25;
            thumbFillColorAnimation.RemovedOnCompletion = false;

            var animThumbGroup = new CAAnimationGroup();
            animThumbGroup.Duration = 0.25;
            animThumbGroup.FillMode = CAFillMode.Forwards;
            animThumbGroup.RemovedOnCompletion = false;
            animThumbGroup.Animations = new CAAnimation[] { thumbBoundsAnimation, thumbPosAnimation, thumbBorderColorAnimation, thumbFillColorAnimation };

            thumbLayer.RemoveAllAnimations();
            thumbLayer.AddAnimation(animThumbGroup, "thumbAnimation");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the switch On/Off.
        /// </summary>
        /// <param name="value">Status value</param>
        /// <param name="animated">Status change animation</param>
        public void SetOn(bool value, bool animated)
        {
            _on = value;

            if(animated)
            {
                if(_on)
                {
                    var bgBorderAnimation = CABasicAnimation.FromKeyPath("borderWidth");
                    bgBorderAnimation.TimingFunction = new CAMediaTimingFunction(0.55f, 0.055f, 0.675f, 0.19f);
                    bgBorderAnimation.SetFrom(NSNumber.FromFloat(1));
                    bgBorderAnimation.SetTo(NSNumber.FromNFloat(Frame.Height / 2));
                    bgBorderAnimation.FillMode = CAFillMode.Forwards;
                    bgBorderAnimation.Duration = 0.25;
                    bgBorderAnimation.RemovedOnCompletion = false;

                    backLayer.AddAnimation(bgBorderAnimation, "bgAnimation");

                    OffToOnAnim();
                }
                else
                {
                    OnToOffAnim();
                }
            }
            else
            {
                if(_on)
                {
                    if(_shouldFillOnPush)
                    {
                        backLayer.BorderWidth = Frame.Height / 2;
                    }

                    backLayer.BorderColor = _trackOnBorderColor.CGColor;
                    backLayer.BackgroundColor = _trackOnFillColor.CGColor;

                    thumbLayer.Position = GetThumbOnPos();
                    thumbLayer.BorderColor = _thumbOnBorderColor.CGColor;
                    thumbLayer.BackgroundColor = _thumbOnFillColor.CGColor;
                }
                else
                {
                    if(_shouldFillOnPush)
                    {
                        backLayer.BorderWidth = 1;
                    }

                    backLayer.BorderColor = _trackOffBorderColor.CGColor;
                    backLayer.BackgroundColor = _trackOffFillColor.CGColor;

                    thumbLayer.Position = GetThumbOffPos();
                    thumbLayer.BorderColor = _thumbOffBorderColor.CGColor;
                    thumbLayer.BackgroundColor = _thumbOffFillColor.CGColor;
                }
            }
        }

        #endregion
    }
}
