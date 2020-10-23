using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sodexo_JTH.Controls;
using Sodexo_JTH.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AutoSuggestionBox), typeof(AutoSuggestionBox_Droid))]
namespace Sodexo_JTH.Droid.Renderer
{
    public class AutoSuggestionBox_Droid : ViewRenderer<AutoSuggestionBox, AutoCompleteTextView>
    {
        AutoCompleteTextView _autoCompletetextView;
        AutoSuggestionBox _autoSuggestionBox;
        ICollection<object> itemSource;
        public AutoSuggestionBox_Droid(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestionBox> e)
        {
            if (e.OldElement != null)
            {
                _autoCompletetextView.ItemClick -= _autoCompletetextView_ItemClick;
                _autoCompletetextView.TextChanged -= _autoCompletetextView_TextChanged;
                //_autoCompletetextView.FocusChange -= _autoCompletetextView_FocusChange;
            }
            if (e.NewElement == null) return;
            if (Control == null)
            {
                _autoCompletetextView = new AutoCompleteTextView(this.Context);
                SetNativeControl(_autoCompletetextView);
            }
            _autoSuggestionBox = e.NewElement as AutoSuggestionBox;

            _autoCompletetextView.Hint = _autoSuggestionBox.Placeholder;
            _autoCompletetextView.Text = _autoSuggestionBox.Text;
            itemSource = _autoSuggestionBox.ItemsSource;
            _autoCompletetextView.DropDownHeight = 300;

            _autoCompletetextView.TextChanged += _autoCompletetextView_TextChanged;
            _autoCompletetextView.ItemClick += _autoCompletetextView_ItemClick;

            _autoCompletetextView.ItemSelected += _autoCompletetextView_ItemSelected;

           // _autoCompletetextView.FocusChange += _autoCompletetextView_FocusChange;

            _autoCompletetextView.Threshold = 0;
        }

        private void _autoCompletetextView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            
        }

        private void _autoCompletetextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text.ToString()))
                _autoSuggestionBox.ControlTextChanged(e.Text.ToString());
        }

        

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == AutoSuggestionBox.ItemsSourceProperty.PropertyName)
            {
                itemSource = ((AutoSuggestionBox)sender).ItemsSource;
                UpdateAdapter();
            }
            if (e.PropertyName == AutoSuggestionBox.TextProperty.PropertyName)
            {
                if (_autoSuggestionBox.Text != _autoCompletetextView.Text)
                    _autoCompletetextView.Text = _autoSuggestionBox.Text;
               
            }
        }

        private void _autoCompletetextView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedItem = itemSource.ElementAt(0);
            _autoSuggestionBox.ItemSelected(selectedItem);
           

        }
        private void UpdateAdapter()
        {
            try
            {
                var DataList = new List<object>();
                foreach (var item in itemSource)
                    DataList.Add((item as object));
                ArrayAdapter _arrayAdapter = new ArrayAdapter<object>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, DataList);

                _autoCompletetextView.Adapter = _arrayAdapter;
                _autoCompletetextView.ShowDropDown();

            }
            catch (Exception)
            {
                throw new Exception("Autosuggest box item source update error");
            }
        }
    }
}