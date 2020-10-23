using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Sodexo_JTH.Controls;
using Sodexo_JTH.Droid.Renderer;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace Sodexo_JTH.Droid.Renderer
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;
        public ImageEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 25;
            Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
           var mDrawable = System.IO.Path.GetFileNameWithoutExtension(imageEntryImage);
            var resourceId = (int)typeof(Resource.Drawable).GetField(mDrawable).GetValue(null);
           // int resID = this.Context.Resources.GetIdentifier("Ulogin.png", "drawable", "com.companyname.appname");
            var drawable = ContextCompat.GetDrawable(this.Context, resourceId);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }

    }
}