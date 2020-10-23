using Sodexo_JTH.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;


namespace Sodexo_JTH.Controls
{
    public class BindableRadioGroup : StackLayout
    {

        public List<RadioButton> rads;

        public BindableRadioGroup()
        {
            rads = new List<RadioButton>();

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: "ItemsSource",
            returnType: typeof(IEnumerable),
            declaringType: typeof(BindableRadioGroup),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
            propertyName: "SelectedIndex",
            returnType: typeof(int),
            declaringType: typeof(BindableRadioGroup),
            defaultValue: -1,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnSelectedIndexChanged
            );


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public event EventHandler<int> CheckedChanged;
        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var radButtons = bindable as BindableRadioGroup;

            radButtons.rads.Clear();
            radButtons.Children.Clear();
            if (newValue != null)
            {

                int radIndex = 0;
                foreach (var item in newValue as IEnumerable)
                {
                    var rad = new RadioButton();
                    rad.Text = item.ToString();
                    rad.Id = radIndex;

                    rad.CheckedChanged += radButtons.OnCheckedChanged;

                    radButtons.rads.Add(rad);

                    radButtons.Children.Add(rad);
                    radIndex++;
                }
            }

            OnSelectedIndexChanged(bindable, 0, 0);
        }

        private void OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (e.Value == false) return;

            var selectedRad = sender as RadioButton;

            foreach (var rad in rads)
            {
                if (!selectedRad.Id.Equals(rad.Id))
                {
                    rad.Checked = false;
                }
                else
                {
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke(sender, rad.Id);

                }

            }

        }

        private static void OnSelectedIndexChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if ((int)newvalue == -1) return;
            var bindableRadioGroup = bindable as BindableRadioGroup;

            foreach (var rad in bindableRadioGroup.rads)
            {
                if (rad.Id == bindableRadioGroup.SelectedIndex)
                {
                    rad.Checked = true;
                }

            }
        }

    }
}
