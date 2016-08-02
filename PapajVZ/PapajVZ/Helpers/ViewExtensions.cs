using System;
using System.Reflection;
using PapajVZ.Interfaces;
using PapajVZ.Renderers;
using PapajVZ.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
#pragma warning disable 618

namespace PapajVZ.Helpers
{
    public static class ViewExtensions
    {
        private static readonly Type PlatformType =
            Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);

        private static BindableProperty _rendererProperty;

        public static BindableProperty RendererProperty
        {
            get
            {
                _rendererProperty =
                    (BindableProperty)
                        PlatformType.GetField("RendererProperty",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                            .GetValue(null);

                return _rendererProperty;
            }
        }

        public static async void ScaleAnimate(this VisualElement e, uint length, double scale = 0.85)
        {
            await e.ScaleTo(scale, length, Easing.BounceOut);
            await e.ScaleTo(1, length, Easing.BounceIn);
        }

        public static async void StepFade(this VisualElement e, uint length = 2, double step = 0.1)
        {
            for (var i = 0.0; i < 1; i += step)
            {
                await e.FadeTo(i, length, Easing.Linear);
            }

        }

        public static void Hide(this VisualElement view)
        {
            view.IsVisible = false;
        }

        public static void Show(this VisualElement view)
        {
            view.IsVisible = true;
        }

      

        public static IVisualElementRenderer GetRenderer(this BindableObject bindableObject)
        {
            var value = bindableObject.GetValue(RendererProperty);
            return (IVisualElementRenderer)bindableObject.GetValue(RendererProperty);
        }

        public static View GetNativeView(this BindableObject bindableObject)
        {
            var renderer = bindableObject.GetRenderer();
            var viewGroup = renderer.ViewGroup;
            return viewGroup;
        }

        public static void SetRenderer(this BindableObject bindableObject, IVisualElementRenderer renderer)
        {
            //			var value = bindableObject.GetValue (RendererProperty);
            bindableObject.SetValue(RendererProperty, renderer);
        }

        public static Point GetNativeScreenPosition(this BindableObject bindableObject)
        {
            var view = bindableObject.GetNativeView();
            var point = Point.Zero;
            if (view != null)
            {
                var location = new int[2];
                view.GetLocationOnScreen(location);
                point = new Point(location[0], location[1]);
            }
            return point;
        }

        /// <summary>
        ///     Gets the or create renderer.
        /// </summary>
        /// <returns>The or create renderer.</returns>
        /// <param name="source">Source.</param>
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement source)
        {
            var renderer = source.GetRenderer();
            if (renderer == null)
            {
                renderer = RendererFactory.GetRenderer(source);
                source.SetRenderer(renderer);
                renderer = source.GetRenderer();
            }
            return renderer;
        }
    }
}