using System;
using Android.Content;
using Android.Text.Method;
using Android.Views;
using Sodexo_JTH.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace Sodexo_JTH.Droid.Renderer
{
    public class CustomEditorRenderer : EditorRenderer
    {

        public CustomEditorRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetBackgroundResource(Resource.Drawable.backtext);
               //While scrolling inside Editor stop scrolling parent view.
               nativeEditText.OverScrollMode = OverScrollMode.Always;
                nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                nativeEditText.SetOnTouchListener(new DroidTouchListener());
               
                //For Scrolling in Editor innner area
                Control.VerticalScrollBarEnabled = true;
                Control.MovementMethod = ScrollingMovementMethod.Instance;
                Control.ScrollBarStyle = Android.Views.ScrollbarStyles.InsideInset;
                //Force scrollbars to be displayed
                Android.Content.Res.TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
                InitializeScrollbars(a);
                a.Recycle();
            }
        }

        private class DroidTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            

            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(true);
                if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
                {
                    v.Parent?.RequestDisallowInterceptTouchEvent(false);
                }
                return false;
            }
        }
    }
}