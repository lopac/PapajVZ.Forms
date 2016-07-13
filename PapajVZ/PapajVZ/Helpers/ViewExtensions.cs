using System;
using System.Reflection;
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

        public static IVisualElementRenderer GetRenderer(this BindableObject bindableObject)
        {
            var value = bindableObject.GetValue(RendererProperty);
            return (IVisualElementRenderer) bindableObject.GetValue(RendererProperty);
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