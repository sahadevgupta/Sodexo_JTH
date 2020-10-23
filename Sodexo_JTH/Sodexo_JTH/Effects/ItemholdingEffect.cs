using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sodexo_JTH.Effects
{
    public class ItemholdingEffect : RoutingEffect
    {
        public const string EffectGroupName = "MyApp_BIH";
        public const string EffectName = "LongPressedGesture";
        public ItemholdingEffect() : base($"{EffectGroupName}.{EffectName}")
        {

        }
        public event EventHandler<EventArgs> ItemLongPressed;
        public event EventHandler<EventArgs> ItemTapped;


        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(ItemholdingEffect), (object)null);
        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public void ControlLongPressed(object data) => ItemLongPressed?.Invoke(data, EventArgs.Empty);

        public void ControlItemTapped(object data) => ItemTapped?.Invoke(data, EventArgs.Empty);

        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(ItemholdingEffect), (object)null);
        public static object GetCommandParameter(BindableObject view)
        {
            return view.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject view, object value)
        {
            view.SetValue(CommandParameterProperty, value);
        }
    }
}
