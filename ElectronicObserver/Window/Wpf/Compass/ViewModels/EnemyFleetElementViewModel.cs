﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels
{
	public class EnemyFleetElementViewModel : ObservableObject
	{
		private KCDatabase Db { get; }
		public EnemyFleetRecord.EnemyFleetElement EnemyFleetCandidate { get; set; } = default!;

		public IEnumerable<MasterShipViewModel> FleetMember => EnemyFleetCandidate.FleetMember
			.Select(i => i switch
			{
				< 1 => null,
				_ => Db.MasterShips[i]
			})
			.Select(s => new MasterShipViewModel {Ship = s})
			.Take(6);

		public string Formation => Constants.GetFormationShort(EnemyFleetCandidate.Formation);

		public ImageSource? AirIcon { get; } = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedFighter);
		public string Air => Calculator.GetAirSuperiority(EnemyFleetCandidate.FleetMember).ToString();
		public string? AirToolTip =>
			GetAirSuperiorityString(Calculator.GetAirSuperiority(EnemyFleetCandidate.FleetMember));

		public EnemyFleetElementViewModel()
		{
			Db = KCDatabase.Instance;
		}

		private static string? GetAirSuperiorityString(int air)
		{
			if (air > 0)
			{
				return string.Format("AS+: {0}\r\nAS: {1}\r\nAP: {2}\r\nAI: {3}\r\n",
					(int)(air * 3.0),
					(int)Math.Ceiling(air * 1.5),
					(int)(air / 1.5 + 1),
					(int)(air / 3.0 + 1));
			}
			return null;
		}
	}
}