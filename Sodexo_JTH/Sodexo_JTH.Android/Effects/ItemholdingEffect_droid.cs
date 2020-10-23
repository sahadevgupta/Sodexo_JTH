using Android.Views;
using Sodexo_JTH.Droid.Effects;
using Sodexo_JTH.Effects;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ResolutionGroupName(ItemholdingEffect.EffectGroupName)]
[assembly: ExportEffect(typeof(ItemholdingEffect_droid), ItemholdingEffect.EffectName)]
namespace Sodexo_JTH.Droid.Effects
{
    public class ItemholdingEffect_droid : PlatformEffect
    {

        private ItemholdingEffect _itemholdingEffect;
        private bool _attached;
        

        protected override void OnAttached()
        {
            _itemholdingEffect = Element.Effects.OfType<ItemholdingEffect>().FirstOrDefault();

            if (!_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += Control_LongClick;
                    Control.Click += Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += Control_LongClick;
                    Container.Click += Control_Click;
                }
                _attached = true;
            }

        }

        private void Control_Click(object sender, EventArgs e)
        {
            var patientInfo = Element.BindingContext;
            _itemholdingEffect?.ControlItemTapped(patientInfo);
        }

        private void Control_LongClick(object sender, LongClickEventArgs e)
        {
            var patientInfo = Element.BindingContext;
            _itemholdingEffect?.ControlLongPressed(patientInfo);
        }

       

        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= Control_LongClick;
                    Control.Click -= Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= Control_LongClick;
                    Container.Click -= Control_Click;
                }
                _attached = false;
            }
        }
    }
}