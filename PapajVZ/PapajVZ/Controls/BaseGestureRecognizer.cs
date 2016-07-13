using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using TwinTechs.Gestures;

[assembly: 
	InternalsVisibleTo ("TwinTechsLib.iOS"),
	InternalsVisibleTo ("TwinTechsLib.Droid")]
namespace PapajVZ.Controls
{
	public enum GestureRecognizerState
	{
		Possible,
		Began,
		Changed,
		Ended,
		Cancelled,
		Failed,
		Recognized = 3
	}


	//TODO I would love to make this generic!
	/// <summary>
	/// Base gesture recognizer.
	/// </summary>
	public class BaseGestureRecognizer : BindableObject, IGestureRecognizer
	{
		/// <summary>
		/// Delegate callback to check if this recognizer should proceed to begin state.
		/// If false is returned, then the gesture will cancel.
		/// If delayed touches is true, and false is returned, then the gesture will replay it's delayed touches
		/// (implementaiton is slightly different on android/iOS; but the result should be more or less equal).
		/// </summary>
		/// <value>The view.</value>
		public delegate bool GestureShouldBeginDelegate (BaseGestureRecognizer gestureRecognizer);

		public GestureShouldBeginDelegate OnGestureShouldBeginDelegate;

		public View View { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is consuming touches in parallel.
		/// if true, then this gesture will register with the main touch dispatcher, and intercept touches as they occur at the system level
		/// </summary>
		/// <value><c>true</c> if this instance is consuming touches in parallel; otherwise, <c>false</c>.</value>
		public bool IsConsumingTouchesInParallel { get; set; }

		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>The command.</value>
		public ICommand Command {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the command parameter.
		/// </summary>
		/// <value>The command parameter.</value>
		public object CommandParameter {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the OnAction callback. Made available in case your views need access to the gesture responses
		/// </summary>
		/// <value>The tapped callback.</value>
		public event Action<BaseGestureRecognizer, GestureRecognizerState> OnAction;


	    private bool _delaysTouches;

		public bool DelaysTouches {
			get{ return _delaysTouches; }
			set {
				_delaysTouches = value;
			    NativeGestureRecognizer?.UpdateDelaysTouches (_delaysTouches);
			}
		}

	    private bool _cancelsTouchesInView;

		public bool CancelsTouchesInView {
			get{ return _cancelsTouchesInView; }
			set {
				_cancelsTouchesInView = value;
			    NativeGestureRecognizer?.UpdateCancelsTouchesInView (_cancelsTouchesInView);
			}
		}

		public GestureRecognizerState State => NativeGestureRecognizer?.State ?? GestureRecognizerState.Failed;

	    public int NumberOfTouches => NativeGestureRecognizer?.NumberOfTouches ?? 0;


	    public Point LocationInView (VisualElement view)
		{
			return NativeGestureRecognizer.LocationInView (view);
		}

		public Point LocationOfTouch (int touchIndex, VisualElement view)
		{
			return NativeGestureRecognizer.LocationOfTouch (touchIndex, view);
		}

		#region internal impl

		internal void SendAction ()
		{
		    Command?.Execute (CommandParameter);
		    OnAction?.Invoke (this, State);
		}

		/// <summary>
		/// Sets the underlying gesture recognzier - used by the factory for adding/removal
		/// </summary>
		/// <value>The native gesture recognizer.</value>
		internal INativeGestureRecognizer NativeGestureRecognizer { get; set; }

		/// <summary>
		/// Gets or sets the native gesture coordinator. - ONLY USED BY ANDROID
		/// </summary>
		/// <value>The native gesture coordinator.</value>
		internal INativeGestureRecognizerCoordinator NativeGestureCoordinator { get; set; }

		#endregion

		public override string ToString ()
		{
			return $"[BaseGestureRecognizer: View={View}, State={State}]";
		}

	}
}

