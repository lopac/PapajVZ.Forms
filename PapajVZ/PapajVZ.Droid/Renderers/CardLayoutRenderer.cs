using Android.Graphics.Drawables;
using PapajVZ.Droid.Renderers;
using PapajVZ.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CardLayout), typeof(CardLayoutRenderer))]

namespace PapajVZ.Droid.Renderers
{
    public class CardLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            this.SetBackgroundResource(Resource.Drawable.shadow);
        }
    }
}