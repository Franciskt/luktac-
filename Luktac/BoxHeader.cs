using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luktac {
	public struct BoxHeader {
		public UInt32 TotalSize;
		public byte[] BoxType;
		public UInt64 ExtendedSize;
	}
}
