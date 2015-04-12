using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	public struct HitState
	{
		#region define

		const uint leftBit = 0x0001;
		const uint rightBit = 0x0002;
		const uint topBit = 0x0004;
		const uint bottomBit = 0x0008;

		#endregion

		#region variable

		uint _flag;

		#endregion

		#region property

		public bool Enabled { get { return this._flag != 0; } }

		public bool Left
		{
			get { return Get(leftBit); }
			set { Set(leftBit, value); }
		}
		public bool Right
		{
			get { return Get(rightBit); }
			set { Set(rightBit, value); }
		}
		public bool Top
		{
			get { return Get(topBit); }
			set { Set(topBit, value); }
		}
		public bool Bottom
		{
			get { return Get(bottomBit); }
			set { Set(bottomBit, value); }
		}

		#endregion

		#region function
		
		private bool Get(uint bit)
		{
			return (this._flag & bit) == bit;
		}

		private void Set(uint bit, bool value)
		{
			if(value) {
				this._flag |= bit;
			} else {
				this._flag &= ~(this._flag & bit);
			}
		}

		#endregion
	}
}
