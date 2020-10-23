using Sodexo_JTH.Effects;
using Sodexo_JTH.UWP.Effects;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName(ItemholdingEffect.EffectGroupName)]
[assembly: ExportEffect(typeof(ItemholdingEffect_UWP), ItemholdingEffect.EffectName)]
namespace Sodexo_JTH.UWP.Effects
{
    public class ItemholdingEffect_UWP : PlatformEffect
    {

        private FrameworkElement _nativeView;
        private ItemholdingEffect _itemholdingEffect;


        public ItemholdingEffect_UWP()
        {
        }

        protected override void OnAttached()
        {
            _itemholdingEffect = Element.Effects.OfType<ItemholdingEffect>().FirstOrDefault();
            _nativeView = Container;

            _nativeView.PointerReleased += _nativeView_PointerReleased;
        }

        private void _nativeView_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            var block = (e.OriginalSource as TextBlock);
            if (block != null)
            {
                var patientInfo = (block.DataContext as ViewCell).BindingContext;
                _itemholdingEffect?.ControlLongPressed(patientInfo);
            }
        }

       

        protected override void OnDetached()
        {
            _nativeView.PointerReleased -= _nativeView_PointerReleased;
        }

        
    }
}
