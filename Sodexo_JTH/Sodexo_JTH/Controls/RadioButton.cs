using Sodexo_JTH.Events;
using System;
using Xamarin.Forms;


namespace Sodexo_JTH.Controls
{
    public class RadioButton : View
    {

        public static readonly BindableProperty CheckedProperty = BindableProperty.Create(
            propertyName: "Checked",
            returnType: typeof(Boolean),
            declaringType: typeof(RadioButton),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay
            );

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(RadioButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay
            );


        /// <summary>
        /// The checked changed event.
        /// </summary>
        public EventHandler<EventArgs<bool>> CheckedChanged;



        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: "TextColor",
            returnType: typeof(Color),
            declaringType: typeof(RadioButton),
            defaultValue: Color.Default,
            defaultBindingMode: BindingMode.TwoWay
            );
        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public Boolean Checked
        {
            get
            {

                return (Boolean)GetValue(CheckedProperty);
            }

            set
            {
                this.SetValue(CheckedProperty, value);
                var eventHandler = this.CheckedChanged;
                if (eventHandler != null)
                {

                    eventHandler.Invoke<bool>(this, value);
                }
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        public Xamarin.Forms.Color TextColor
        {
            get
            {
                return (Xamarin.Forms.Color)GetValue(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        public int Id { get; set; }



    }
}

