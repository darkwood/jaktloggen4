﻿using System;
using System.Globalization;
using System.Windows.Input;
using Jaktloggen.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Cells
{
    public class DateTimeCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(DateTimeCell), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text), 
                typeof(string), 
                typeof(DateTimeCell), 
                null,
                propertyChanged: (bindable, oldValue, newValue) => {
                ((DateTimeCell)bindable).TextLabel.Text = newValue as string;
                }
            );
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Label TextLabel { get; private set; }
        public Label ValueLabel { get; private set; }

        /***************************************************************************/

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(DateTimeCell),
                DateTime.Now,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((DateTimeCell)bindable).ValueLabel.Text = ((DateTime)newValue).ToString("g", new CultureInfo("nb-no"));
                }
            );

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }


        public DateTimeCell()
        {
            View = CreateLayout();
        }

        private StackLayout CreateLayout()
        {
            CreateTextLabel();
            CreateValueLabel();

            var viewLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10
            };

            viewLayout.Children.Add(TextLabel);
            viewLayout.Children.Add(ValueLabel);
            viewLayout.GestureRecognizers.Add(CreateTapGestureRecognizer());

            return viewLayout;
        }

        private TapGestureRecognizer CreateTapGestureRecognizer()
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (s, e) =>
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(Date);
                } 
            };
            return gestureRecognizer;
        }

        private void CreateTextLabel()
        {
            TextLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.StartAndExpand
            };
        }

        private void CreateValueLabel()
        {
            ValueLabel = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };
        }
    }
}
