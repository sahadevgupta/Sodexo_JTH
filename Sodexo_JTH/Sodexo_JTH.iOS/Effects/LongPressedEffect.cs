using System.Diagnostics;
using System.Linq;
using Sodexo_JTH.Effects;
using Sodexo_JTH.iOS.Events;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(ItemholdingEffect.EffectGroupName)]
[assembly: ExportEffect(typeof(LongPressedEffect), ItemholdingEffect.EffectName)]
namespace Sodexo_JTH.iOS.Events
{
    public class LongPressedEffect : PlatformEffect
    {
        private bool _attached;
        private ItemholdingEffect _longPressedEffect;
        private readonly UILongPressGestureRecognizer _longPressRecognizer;
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Yukon.Application.iOSComponents.Effects.iOSLongPressedEffect"/> class.
        /// </summary>
        public LongPressedEffect()
        {
            _longPressRecognizer = new UILongPressGestureRecognizer(HandleLongClick);
            _longPressRecognizer.MinimumPressDuration = 0.5;
        }

        /// <summary>
        /// Apply the handler
        /// </summary>
        protected override void OnAttached()
        {
            _longPressedEffect = Element.Effects.OfType<ItemholdingEffect>().FirstOrDefault();
            //because an effect can be detached immediately after attached (happens in listview), only attach the handler one time
            if (!_attached)
            {

                Container.AddGestureRecognizer(_longPressRecognizer);
                _attached = true;
            }
        }

        /// <summary>
        /// Invoke the command if there is one
        /// </summary>
        private void HandleLongClick()
        {
            if (Element == null || _longPressedEffect == null)
            {
                Debug.WriteLine("Null");
                return;
            }

            if (_longPressRecognizer.State == UIGestureRecognizerState.Began)
            {
                _longPressedEffect.ControlLongPressed(Element);
                var command = ItemholdingEffect.GetCommand(Element);
                command?.Execute(ItemholdingEffect.GetCommandParameter(Element));
            }


        }

        /// <summary>
        /// Clean the event handler on detach
        /// </summary>
        protected override void OnDetached()
        {
            if (_attached)
            {
                Container.RemoveGestureRecognizer(_longPressRecognizer);
                _attached = false;
            }
        }

    }
}