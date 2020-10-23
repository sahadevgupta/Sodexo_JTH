using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Sodexo_JTH.Behaviors
{
    public class SpecialCharValidationBehaviorEditor : Behavior<Editor>
    {
        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
            //bindable.Unfocused += Bindable_Unfocused;
        }
        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
            //bindable.Unfocused -= Bindable_Unfocused;
        }
        void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((Editor)sender == null)
                return;

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ((Editor)sender).Text = string.Empty;
                return;
            }

            if (GetCount(e.NewTextValue, '.') > 1)
            {
                ((Editor)sender).Text = e.OldTextValue;
                return;
            }
            if (e.NewTextValue.StartsWith("-") && e.NewTextValue.Length == 1)
            {
                return;
            }
            if (e.NewTextValue.EndsWith("."))
            {
                return;
            }
            string Reg = $"[!@#$%^&*(),.?\":|<>]";
            bool isValid = Regex.IsMatch(e.NewTextValue, Reg);

            if (isValid)
            {
                ((Editor)sender).Text = e.OldTextValue;
            }
            else
            {
                ((Editor)sender).Text = e.NewTextValue;
            }
        }

        int GetCount(string source, char ch)
        {
            int count = 0;
            foreach (char c in source)
                if (c == ch) count++;

            return count;
        }
    }
}
