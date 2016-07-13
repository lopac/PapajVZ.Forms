using System;
using Android.Views;
using PapajVZ.Controls;
using Xamarin.Forms;

namespace PapajVZ.Droid.Renderers
{
    public class NativeSwipeGestureRecognizer : BaseNativeGestureRecognizer
    {
        //private const int MinimumSwipeDistance = 5;
        private const int MaxSwipeDuration = 1000;
        private DateTime _startTime;


        private SwipeGestureRecognizer SwipeGestureRecognizer => Recognizer as SwipeGestureRecognizer;

        #region implemented abstract members of BaseNativeGestureRecognizer

        internal override void ProcessMotionEvent(GestureMotionEvent e)
        {
            if (e.Action == MotionEventActions.Down && PointerId == -1)
            {
                OnDown(e);

                if (State == GestureRecognizerState.Began)
                {
                    PointerId = e.GetPointerId(0);
                    e.IsCancelled = Recognizer.CancelsTouchesInView;
                }
            }
            else if (State == GestureRecognizerState.Cancelled || State == GestureRecognizerState.Ended ||
                     State == GestureRecognizerState.Failed)
            {
            }
            else if (e.ActionMasked == MotionEventActions.Cancel)
            {
                State = GestureRecognizerState.Cancelled;
                Console.WriteLine("GESTURE CANCELLED");
            }
            else if (e.ActionMasked == MotionEventActions.Up)
            {
                OnUp(e);
            }
        }

        #endregion

        private void OnDown(GestureMotionEvent e)
        {
            State = GestureRecognizerState.Began;

            FirstTouchPoint = new Point(e.GetX(0), e.GetY(0));
            _startTime = DateTime.Now;
        }


        private void OnUp(GestureMotionEvent e)
        {
            NumberOfTouches = e.PointerCount;
            var tookTooLong = (DateTime.Now - _startTime).Milliseconds > MaxSwipeDuration;
            var wrongAmountOfTouches = NumberOfTouches < SwipeGestureRecognizer.NumberOfTouchesRequired;
            if (tookTooLong || wrongAmountOfTouches)
            {
                State = GestureRecognizerState.Failed;
                return;
            }
            var endTouchPoint = new Point(e.GetX(0), e.GetY(0));
            var velocityX = endTouchPoint.X - FirstTouchPoint.X;
            var velocityY = endTouchPoint.Y - FirstTouchPoint.Y;
            var direction = GetSwipeDirection(velocityX, velocityY);
            var expectedDirection = ((SwipeGestureRecognizer) Recognizer).Direction;
            if (direction == expectedDirection)
            {
                State = GestureRecognizerState.Recognized;
            }
            else
            {
                State = GestureRecognizerState.Failed;
                Console.WriteLine($"failed gesture was expecting {expectedDirection} got {direction}");
            }
        }

        private SwipeGestureRecognizerDirection GetSwipeDirection(double velocityX, double velocityY)
        {
            var isHorizontalSwipe = Math.Abs(velocityX) > Math.Abs(velocityY);
            if (isHorizontalSwipe)
            {
                return velocityX > 0 ? SwipeGestureRecognizerDirection.Right : SwipeGestureRecognizerDirection.Left;
            }
            return velocityY > 0 ? SwipeGestureRecognizerDirection.Down : SwipeGestureRecognizerDirection.Up;
        }
    }
}