using OverdoseTheGame;
using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
    public class Card
    {
		private Pill _pill;

		public Pill Pill
		{
			get
			{
				return _pill;
			}
			set
			{
				_pill = value;
                IsInitialized = false;
			}
		}

        public bool IsInitialized { get; set; } = false;
    }
}
