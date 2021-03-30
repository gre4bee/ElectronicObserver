﻿using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.WinformsWrappers;

namespace ElectronicObserver.Window.Wpf.WinformsHost
{
	/// <summary>
	/// Interaction logic for WinformsHostView.xaml
	/// </summary>
	public partial class WinformsHostView : UserControl
	{
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
			"ViewModel", typeof(FormBrowserHostViewModel), typeof(WinformsHostView), new PropertyMetadata(default(FormBrowserHostViewModel)));

		public FormBrowserHostViewModel ViewModel
		{
			get => (FormBrowserHostViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		public WinformsHostView()
		{
			InitializeComponent();
		}
	}
}
