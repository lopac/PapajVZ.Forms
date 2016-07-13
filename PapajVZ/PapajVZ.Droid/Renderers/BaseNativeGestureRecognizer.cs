using Java.Lang;
using PapajVZ.Controls;
using PapajVZ.Helpers;
using Xamarin.Forms;
using View = Android.Views.View;

namespace PapajVZ.Droid.Renderers
{
    public abstract class BaseNativeGestureRecognizer : Object, INativeGestureRecognizer
    {
        /// <summary>
        ///     The pointer identifier. 
        /// </summary>
        protected int PointerId = -1;

        internal BaseGestureRecognizer Recognizer { get; set; }

        internal View NativeView { get; set; }

        /// <summary>
        ///     Gets or sets the first touch point - for convenience
        /// </summary>
        /// <value>The first touch point.</value>
        protected Point FirstTouchPoint { get; set; }


        /// <summary>
        ///     Gets or sets the second touch point - for convenience
        /// </summary>
        /// <value>The second touch point.</value>
        protected Point SecondTouchPoint { get; set; }


        /// <summary>
        ///     Gets or sets a value indicating whether this gesture is continuous or a one off.
        ///     If it's not continuous, then only recognized events get dispatched
        ///     override if your gesture does things like pinch/pan/zoom/rotate, which have discrete active phases.
        /// </summary>
        /// <value><c>true</c> if this instance is a continuous gesture; otherwise, <c>false</c>.</value>
        protected virtual bool IsGestureCotinuous => false;

        /// <summary>
        ///     Processes the motion event.
        ///     Overriding methods should updat the GestureMotionEvent state properties, marking for cancels, delays or consumption
        /// </summary>
        /// <param name="e">E.</param>
        internal abstract void ProcessMotionEvent(GestureMotionEvent e);

        #region handling gestures at a group level


        public void ProcessGestureMotionEvent(GestureMotionEvent gestureEvent)
        {
            var ev = gestureEvent.MotionEvent;
            var nativeViewScreenLocation = Recognizer.View.GetNativeScreenPosition();

            var offset = Point.Zero;
            var touchPoint = new Point(ev.GetX(), ev.GetY());
            var mainPointerId = ev.GetPointerId(0);
            var isInsideOfView = touchPoint.X >= nativeViewScreenLocation.X &&
                                 touchPoint.Y >= nativeViewScreenLocation.Y &&
                                 touchPoint.X <= NativeView.Width + nativeViewScreenLocation.X &&
                                 touchPoint.Y <= NativeView.Height + nativeViewScreenLocation.Y;

            if (isInsideOfView || PointerId == mainPointerId)
            {
                offset.X = -nativeViewScreenLocation.X;
                offset.Y = -nativeViewScreenLocation.Y;
                ev.OffsetLocation((float) offset.X, (float) offset.Y);
              

                ProcessMotionEvent(gestureEvent);

                ev.OffsetLocation((float) -offset.X, (float) -offset.Y);
            }
        }

        #endregion

        #region INativeGestureRecognizer impl

        public void UpdateCancelsTouchesInView(bool _cancelsTouchesInView)
        {
            //does nothign on android
        }

        public void UpdateDelaysTouches(bool _delaysTouches)
        {
            //does nothign on android
        }


        public virtual Point LocationInView(VisualElement view)
        {
            return GetLocationInAncestorView(FirstTouchPoint, view);
        }

        public virtual Point LocationOfTouch(int touchIndex, VisualElement view)
        {
            return GetLocationInAncestorView(FirstTouchPoint, view);
        }

        public int NumberOfTouches { get; protected set; }

        private bool _gestureDidBegin;

        private GestureRecognizerState _state;

        public GestureRecognizerState State
        {
            get { return _state; }
            protected set
            {
                var oldState = _state;
                _state = value;
                if (oldState == GestureRecognizerState.Possible && value == GestureRecognizerState.Began)
                {
                    if (Recognizer.OnGestureShouldBeginDelegate != null &&
                        !Recognizer.OnGestureShouldBeginDelegate(Recognizer))
                    {
                        _state = GestureRecognizerState.Failed;
                    }
                }

                if (_state == GestureRecognizerState.Cancelled || _state == GestureRecognizerState.Ended ||
                    _state == GestureRecognizerState.Failed)
                {
                    PointerId = -1;
                }

                if (_state == GestureRecognizerState.Began)
                {
                    _gestureDidBegin = true;
                }

                if (_state == GestureRecognizerState.Recognized || (IsGestureCotinuous && _gestureDidBegin))
                {
                    SendGestureEvent();
                }

                if (GetIsFinishedState(_state))
                {
                    _gestureDidBegin = false;
                }
            }
        }

        private bool GetIsFinishedState(GestureRecognizerState state)
        {
            return state == GestureRecognizerState.Ended || state == GestureRecognizerState.Cancelled ||
                   state == GestureRecognizerState.Recognized ||
                   state == GestureRecognizerState.Failed;
        }

        private void SendGestureEvent()
        {
            if (Recognizer != null)
            {
                Device.BeginInvokeOnMainThread(() => { Recognizer.SendAction(); });
            }
        }

        protected Point GetLocationInAncestorView(Point location, VisualElement view)
        {
            var nativeViewLocation = new int[2];
            NativeView.GetLocationOnScreen(nativeViewLocation);
            var nativeViewLocationOnScreen = new Point(nativeViewLocation[0], nativeViewLocation[1]);

            var offsetLocation = new Point(location.X + nativeViewLocationOnScreen.X,
                location.Y + nativeViewLocationOnScreen.Y);

            var targetViewRenderer = view.GetRenderer();
            var targetView = targetViewRenderer.ViewGroup;
            var targetViewLocation = new int[2];
            targetView.GetLocationOnScreen(targetViewLocation);
            var nativeViewScreenLocation = new Point(targetViewLocation[0], targetViewLocation[1]);

            var returnPoint = offsetLocation;
            returnPoint.X -= nativeViewScreenLocation.X;
            returnPoint.Y -= nativeViewScreenLocation.Y;

            return returnPoint;
        }

        #endregion
    }
}