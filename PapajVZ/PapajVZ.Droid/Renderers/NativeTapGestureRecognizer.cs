﻿using PapajVZ.Controls;
using System;
using Android.Views;
using Xamarin.Forms;
using TapGestureRecognizer = PapajVZ.Controls.TapGestureRecognizer;

namespace PapajVZ.Droid.Renderers
{
    public class NativeTapGestureRecognizer : BaseNativeGestureRecognizer
    {
      
        private System.Timers.Timer _multiTapTimer;

        private int _currentTapCount;
        private DateTime _startTime;

        private TapGestureRecognizer TapGestureRecognizer => Recognizer as TapGestureRecognizer;

        #region implemented abstract members of BaseNativeGestureRecognizer

        internal override void ProcessMotionEvent(GestureMotionEvent e)
        {
            _startTime = DateTime.Now;
            if (e.Action == MotionEventActions.Down && PointerId == -1)
            {
                OnDown(e);
                if (State == GestureRecognizerState.Began)
                {
                    e.IsCancelled = true;
                }
            }
            else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended ||
                     State == GestureRecognizerState.Failed)
            {
                return;
            }
            else if (e.ActionMasked == MotionEventActions.Cancel)
            {
                State = GestureRecognizerState.Cancelled;
                Console.WriteLine("GESTURE CANCELLED");
            }
            else if (e.ActionMasked == MotionEventActions.Up)
            {
                OnUp(e);
                if (PointerId == e.GetPointerId(0))
                {
                }
            }
        }

        #endregion

        private void OnDown(GestureMotionEvent e)
        {
            State = (e.PointerCount == TapGestureRecognizer.NumberOfTouchesRequired)
                ? GestureRecognizerState.Began
                : GestureRecognizerState.Failed;
            _currentTapCount = 0;

            PointerId = e.GetPointerId(0);
            FirstTouchPoint = new Xamarin.Forms.Point(e.GetX(0), e.GetY(0));
            if (TapGestureRecognizer.NumberOfTapsRequired > 1 && State == GestureRecognizerState.Began)
            {
                ResetMultiTapTimer(true);
            }
        }


        private void OnUp(GestureMotionEvent e)
        {
            NumberOfTouches = e.PointerCount;
            var tooLongBetweenTouches = (DateTime.Now - _startTime).Milliseconds > 400;
            var wrongNumberOfTouches = NumberOfTouches <
                                       (this.Recognizer as TapGestureRecognizer).NumberOfTouchesRequired;
            if (tooLongBetweenTouches || wrongNumberOfTouches)
            {
                State = GestureRecognizerState.Failed;
                return;
            }
            _currentTapCount++;
            Console.WriteLine("Tapped current tap count " + _currentTapCount);

            var requiredTaps = (this.Recognizer as TapGestureRecognizer).NumberOfTapsRequired;
            if (requiredTaps == 1)
            {
                State = GestureRecognizerState.Recognized;
            }
            else
            {
                if (_currentTapCount == requiredTaps)
                {
                    Console.WriteLine("did multi tap, required " + requiredTaps);
                    NumberOfTouches = 1;
                    State = GestureRecognizerState.Recognized;
                    ResetMultiTapTimer(false);
                    _currentTapCount = 0;
                }
                else
                {
                    Console.WriteLine("incomplete multi tap, " + _currentTapCount + "/" + requiredTaps);
                }
            }
        }

        private void _multiTapTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("didn't finish multi tap gesture");
            _currentTapCount = 0;
            ResetMultiTapTimer(false);
        }

        private void ResetMultiTapTimer(bool isActive)
        {
            if (_multiTapTimer != null)
            {
                _multiTapTimer.Elapsed -= _multiTapTimer_Elapsed;
                _multiTapTimer.AutoReset = true;
                _multiTapTimer.Stop();
                _multiTapTimer.Close();
            }
            if (isActive)
            {
                State = GestureRecognizerState.Possible;
                _multiTapTimer = new System.Timers.Timer();
                _multiTapTimer.Interval = 300*(TapGestureRecognizer.NumberOfTapsRequired - 1);
                _multiTapTimer.AutoReset = false;
                _multiTapTimer.Elapsed += _multiTapTimer_Elapsed;
                _multiTapTimer.Start();
            }
            else
            {
                _currentTapCount = 0;
                if (State == GestureRecognizerState.Possible)
                {
                    State = GestureRecognizerState.Failed;
                }
            }
        }
    }
}