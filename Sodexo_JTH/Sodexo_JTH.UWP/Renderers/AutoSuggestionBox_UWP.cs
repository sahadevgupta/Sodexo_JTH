using Sodexo_JTH.Controls;
using Sodexo_JTH.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AutoSuggestionBox), typeof(AutoSuggestionBox_UWP))]
namespace Sodexo_JTH.UWP.Renderers
{
    public class AutoSuggestionBox_UWP : ViewRenderer<AutoSuggestionBox,AutoSuggestBox>
    {
        AutoSuggestionBox _autoSuggestionBox;
        AutoSuggestBox _autoSuggestBox;

        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestionBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement !=null)
            {
                _autoSuggestBox.TextChanged -= AutoSuggestBoxTextChanged;
            }
            if (e.NewElement == null) return;

            if (Control == null)
            {
                _autoSuggestBox = new AutoSuggestBox
                {
                    Style = (Windows.UI.Xaml.Style)App.Current.Resources["DefaultAutoSuggestBox"]
                };
                SetNativeControl(_autoSuggestBox);
            }
            _autoSuggestBox.VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

            _autoSuggestionBox = e.NewElement as AutoSuggestionBox;
            _autoSuggestBox.ItemsSource = _autoSuggestionBox.ItemsSource;
           // _autoSuggestBox.DisplayMemberPath = _autoSuggestionBox.DisplayName;
            _autoSuggestBox.AutoMaximizeSuggestionArea = true;
            _autoSuggestBox.LightDismissOverlayMode = LightDismissOverlayMode.Auto;
           // _autoSuggestBox.TextMemberPath = _autoSuggestionBox.DisplayName;
           
            _autoSuggestBox.TextChanged += AutoSuggestBoxTextChanged;

            _autoSuggestBox.QuerySubmitted += _autoSuggestBox_QuerySubmitted;

            _autoSuggestBox.SuggestionChosen += _autoSuggestBox_SuggestionChosen1;

        }

        private void _autoSuggestBox_SuggestionChosen1(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem.ToString() == "No Results")
            {
                sender.Text = string.Empty;

            }
        }

        private void _autoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion.ToString().Contains("No Results"))
            {
                return;
            }
            _autoSuggestionBox.ItemSelected(args.ChosenSuggestion);
        }

        private void _autoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
           
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AutoSuggestionBox.ItemsSourceProperty.PropertyName)
            {
                _autoSuggestBox.ItemsSource = ((AutoSuggestionBox)sender).ItemsSource;
            }
            if (e.PropertyName == AutoSuggestionBox.TextProperty.PropertyName)
            {
                if (_autoSuggestionBox.Text != _autoSuggestBox.Text)
                    _autoSuggestBox.Text = ((AutoSuggestionBox)sender).Text;
            }
        }

        private void AutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                _autoSuggestionBox.ControlTextChanged(sender.Text);
           
        }
    }
}
