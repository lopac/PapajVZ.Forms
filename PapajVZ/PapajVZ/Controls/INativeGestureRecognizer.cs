﻿using Xamarin.Forms;

namespace PapajVZ.Controls
{
	public interface INativeGestureRecognizer
	{
		int NumberOfTouches { get; }

		Point LocationInView (VisualElement view);

		Point LocationOfTouch (int touchIndex, VisualElement view);

		GestureRecognizerState State { get; }

		void UpdateCancelsTouchesInView (bool _cancelsTouchesInView);

		void UpdateDelaysTouches (bool _delaysTouches);
	}
}

