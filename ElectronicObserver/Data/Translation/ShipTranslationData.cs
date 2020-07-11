﻿using DynaJson;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronicObserver.Data.Translation
{
	public class ShipTranslationData : TranslationBase
	{
		private string FilePath = TranslationManager.WorkingFolder + @"\ship.json";

		private Dictionary<string, string> ShipList;
		private Dictionary<string, string> TypeList;
		private Dictionary<string, string> SuffixList;

		public bool IsTranslated(string rawData)
			=> Configuration.Config.UI.JapaneseShipName == false && ShipList.ContainsKey(rawData);
		public bool IsTypeTranslated(string rawData)
			=> Configuration.Config.UI.JapaneseShipType == false && TypeList.ContainsKey(rawData);
		public string Name(string rawData)
		{
			if (ShipList == null || SuffixList == null) return rawData;

			foreach (var s in ShipList.OrderByDescending(s => s.Key.Length))
			{
				if (rawData.Equals(s.Key)) return s.Value;

				if (rawData.StartsWith(s.Key))
				{
					var pos = rawData.IndexOf(s.Key);
					rawData = rawData.Remove(pos, s.Key.Length).Insert(pos, s.Value);
				}
			}
			foreach (var sf in SuffixList.OrderByDescending(sf => sf.Key.Length))
			{
				if (rawData.Contains(sf.Key))
				{
					var pos = rawData.IndexOf(sf.Key);
					rawData = rawData.Remove(pos, sf.Key.Length).Insert(pos, " ").Insert(pos + 1, sf.Value);
				}
			}
			return rawData;
			//return IsTranslated(rawData) ? ShipList[rawData] : rawData;
		}

		public string TypeName(string rawData) => IsTypeTranslated(rawData) ? TypeList[rawData] : rawData;
		public ShipTranslationData()
		{
			Initialize();
		}
		public override void Initialize()
		{
			ShipList = new Dictionary<string, string>();
			TypeList = new Dictionary<string, string>();
			SuffixList = new Dictionary<string, string>();
			LoadDictionary(FilePath);
		}

		public void LoadDictionary(string path)
		{
			var json = Load(path);
			if (json == null) return;
			
			foreach (KeyValuePair<string, object> category in json)
			{
				if (category.Key == "version") continue;

				var entries = JsonObject.Parse(category.Value.ToString());
				foreach (KeyValuePair<string, dynamic> entry in entries)
				{
					if (category.Key == "ship")
					{
						ShipList.Add(entry.Key, entries[entry.Key]);
					}
					if (category.Key == "stype")
					{
						TypeList.Add(entry.Key, entries[entry.Key]);
					}
					if (category.Key == "suffix")
					{
						SuffixList.Add(entry.Key, entries[entry.Key]);
					}
				}
			}
		}
	}
}