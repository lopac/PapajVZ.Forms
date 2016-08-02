using PapajVZ.Droid.Renderers;
using PapajVZ.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CardImage), typeof(CardImageRenderer))]

namespace PapajVZ.Droid.Renderers
{
    public class CardImageRenderer : VisualElementRenderer<StackLayout>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            this.SetBackgroundResource(Resource.Drawable.menu_pattern);
        }
    }
}