using System;
using System.Collections.Generic;
using PapajVZ.Controls;

namespace PapajVZ.Droid.Renderers
{
    public class NativeGestureRecognizerFactory : INativeGestureRecognizerFactory
    {
        private readonly Dictionary<Type, Type> _typeDictionary = new Dictionary<Type, Type>
        {
            {typeof(SwipeGestureRecognizer), typeof(NativeSwipeGestureRecognizer)},
            {typeof(TapGestureRecognizer), typeof(NativeTapGestureRecognizer)}
        };

        #region INativeGestureRecognizerFactory implementation

        public void AddNativeGestureRecognizerToRecgonizer<T>(T recognizer) where T : BaseGestureRecognizer
        {
            if (!_typeDictionary.ContainsKey(recognizer.GetType()))
            {
                throw new ArgumentException("no native gesture recognizer for this forms recognizer " +
                                            recognizer.GetType());
            }

            var targetType = _typeDictionary[recognizer.GetType()];
            var nativeRecongizer = (BaseNativeGestureRecognizer) Activator.CreateInstance(targetType);
            nativeRecongizer.Recognizer = recognizer;
            recognizer.NativeGestureRecognizer = nativeRecongizer;

            if (recognizer.NativeGestureCoordinator == null)
            {
                recognizer.NativeGestureCoordinator = new NativeGestureCoordinator(recognizer.View);
            }

            var coordinator = recognizer.NativeGestureCoordinator as NativeGestureCoordinator;
            if (coordinator == null)
            {
                throw new InvalidOperationException(
                    "the recognizer's native gesture coordinator is null, or an invalid type");
            }
            coordinator.AddRecognizer(nativeRecongizer);
        }

        public void RemoveRecognizer(BaseGestureRecognizer recognizer)
        {
            if (recognizer.NativeGestureRecognizer != null)
            {
                var coordinator = recognizer.NativeGestureCoordinator as NativeGestureCoordinator;
                if (coordinator == null)
                {
                    throw new InvalidOperationException(
                        "the recognizer's native gesture coordinator is null, or an invalid type");
                }
                coordinator.RemoveRecognizer((BaseNativeGestureRecognizer) recognizer.NativeGestureRecognizer);
                if (!coordinator.HasRecognizers)
                {
                    coordinator.Destroy();
                    recognizer.NativeGestureCoordinator = null;
                }
            }
        }

        #endregion
    }
}