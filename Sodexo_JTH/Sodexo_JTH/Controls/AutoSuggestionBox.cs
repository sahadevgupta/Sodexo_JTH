using Sodexo_JTH.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sodexo_JTH.Controls
{
    public class AutoSuggestionBox : View
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(ICollection<object>),
        typeof(AutoSuggestionBox),
        null,
        BindingMode.TwoWay);
        public static readonly BindableProperty DisplayNameProperty = BindableProperty.Create(
           nameof(DisplayName),
           typeof(string),
           typeof(AutoSuggestionBox),
           null,
           BindingMode.OneWay);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
          nameof(Text),
          typeof(string),
          typeof(AutoSuggestionBox),
          null,
          BindingMode.TwoWay,
          propertyChanged: TextChange);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
          nameof(SelectedItem),
          typeof(object),
          typeof(AutoSuggestionBox),
          null,
          BindingMode.TwoWay);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
           nameof(ItemTemplate),
           typeof(DataTemplate),
           typeof(AutoSuggestionBox),
           default(DataTemplate));

        public static BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(AutoSuggestionBox),
        defaultValue: string.Empty);

        public static BindableProperty IsBorderErrorVisibleProperty = BindableProperty.Create(
            nameof(IsBorderErrorVisible),
            typeof(bool),
            typeof(AutoSuggestionBox),
            false,
            BindingMode.TwoWay);
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                var control = (AutoSuggestionBox)bindable;
                control.Text = (string)newValue;
            }
        }

        public bool IsBorderErrorVisible
        {
            get { return (bool)GetValue(IsBorderErrorVisibleProperty); }
            set
            {
                SetValue(IsBorderErrorVisibleProperty, value);
            }

        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public ICollection<object> ItemsSource
        {
            get => (ICollection<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public event EventHandler<EventArgs<string>> TextChanged;
        public event EventHandler<EventArgs<object>> Item;
        public event EventHandler<EventArgs<object>> QuerySubmitted;
        public event EventHandler<EventArgs<object>> GotFocus;
        public event EventHandler<EventArgs<object>> LostFocus;
        public event EventHandler<EventArgs> SetFocus;

        public void ControlTextChanged(object text)
        {
            //Text = text.ToString();
            TextChanged?.Invoke(this, text.ToString());
        }
        public void ItemSelected(object item)
        {
            SelectedItem = item;
            Item?.Invoke(this, item);
        }
        public void QuerySubmit(object Query)
        {
            QuerySubmitted?.Invoke(this, Query);
        }
        public void ControlGotFocus(object item)
        {
            GotFocus?.Invoke(this, item);
        }
        public void ControlLostFocus(object item)
        {
            LostFocus?.Invoke(this, item);
        }

        public void Setfocus()
        {
            SetFocus?.Invoke(this, null);
        }
    }
}
