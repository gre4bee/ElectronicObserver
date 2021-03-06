﻿using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers
{
	public class FormInformationViewModel : WinformsHostViewModel
	{
		public FormInformationViewModel() : base("Info", "FormInformation",
			ImageSourceIcons.GetIcon(ResourceManager.IconContent.FormInformation))
		{
			// todo remove parameter cause it's never used
			WinformsControl = new FormInformation(null!) { TopLevel = false };

			WindowsFormsHost.Child = WinformsControl;
		}
	}
}