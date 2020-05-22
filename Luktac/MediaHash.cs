using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Luktac {
	public enum MediaProtection {
		Unknow,
		Modified,  // class for the known messages  
		Protected    
	}

	public class MediaHash {

		/**
		 * Method to compute the hash value 
		 * */
		public static byte[] ComputeHash(BinaryReader reader, ulong dataSize) {
			// embadding of the SHA256 algorithm
			SHA256Managed sha = new SHA256Managed();
			int offset = 0;
			byte[] output = new byte[512]; // buffer size
			byte[] input;

			// with the buffer, we are possible to process large file with small memory usage.
			// increasing of the buffer size will speed up the process.

			// read and transform a block at a time according to the buffer size
			while ((ulong)output.Length - (ulong)offset < dataSize) {
				input = reader.ReadBytes(output.Length);
				offset += sha.TransformBlock(input, 0, output.Length, output, 0);
			}

			// transform final block
			input = reader.ReadBytes((int)(dataSize - (ulong)offset));
			sha.TransformFinalBlock(input, 0, input.Length);

			return sha.Hash;
		}

		/**
		 * Method to validate the "signed" file
		 * */
		public static MediaProtection Validate(FileInfo file) {
			Dictionary<string, byte[]> table = null;
			Dictionary<string, byte[]> hashs = new Dictionary<string, byte[]>();

			using (BinaryReader reader = new BinaryReader(file.OpenRead())) {
				BoxHeader header = new BoxHeader();
				while (reader.PeekChar() > -1) {
					header.TotalSize = ReadUInt32big(reader);
					header.BoxType = reader.ReadBytes(4);

					string type = Encoding.ASCII.GetString(header.BoxType);
					ulong dataSize = 0;
					if (header.TotalSize == 0) {
						// read to end
						dataSize = (ulong)(reader.BaseStream.Length - reader.BaseStream.Position);
					} else if (header.TotalSize == 1) {
						// 64bit
						header.ExtendedSize = ReadUInt64Big(reader);
						dataSize = header.ExtendedSize;
					} else {
						dataSize = header.TotalSize;
					}

					if (!type.Equals("HASH")) {
						reader.BaseStream.Position -= 8; // step back header size
						hashs[type] = ComputeHash(reader, dataSize);
					} else {
						// custom box that we use to sign the file
						table = new Dictionary<string, byte[]>();
						int offset = 8;
						while (offset < (int)dataSize) {
							string k = Encoding.ASCII.GetString(reader.ReadBytes(4));
							table[k] = reader.ReadBytes(32);
							offset += 36;
						}
					}
				}

				reader.Close();
			}

			if (table != null && (table.Count == hashs.Count)) {
				// compare the hash for all values
				foreach (KeyValuePair<string, byte[]> kv in table) {
					if (!hashs.ContainsKey(kv.Key)) return MediaProtection.Modified;

					byte[] data = hashs[kv.Key];
					for (int i = 0; i < kv.Value.Length; i++) {
						if (data[i] != kv.Value[i]) return MediaProtection.Modified;
					}
				}
			} else { // no table, invalid file
				return MediaProtection.Unknow;
			}

			return MediaProtection.Protected;
		}

		/**
		 * Method to "sign" the file(mp4 only)
		 * */
		public static bool Protect(FileInfo file) {
			Dictionary<string, byte[]> hashs = new Dictionary<string, byte[]>();

			// create a temporary variable
			FileInfo ofile = new FileInfo(file.FullName + ".tmp");

			try {
				using (BinaryReader reader = new BinaryReader(file.OpenRead())) {
					using (BinaryWriter writer = new BinaryWriter(ofile.Open(FileMode.Create, FileAccess.Write, FileShare.None))) {

						BoxHeader header = new BoxHeader();
						while (reader.PeekChar() > -1) {
							header.TotalSize = ReadUInt32big(reader);
							header.BoxType = reader.ReadBytes(4);

							string type = Encoding.ASCII.GetString(header.BoxType);
							ulong dataSize = 0;
							if (header.TotalSize == 0) {
								// read to end
								dataSize = (ulong)(reader.BaseStream.Length - reader.BaseStream.Position);
							} else if (header.TotalSize == 1) {
								// 64bit
								header.ExtendedSize = ReadUInt64Big(reader);
								dataSize = header.ExtendedSize;
							} else {
								dataSize = header.TotalSize;
							}
							reader.BaseStream.Position -= 8; // step back header size

							if (!type.Equals("HASH")) {
								hashs[type] = ComputeHash(reader, dataSize);

								reader.BaseStream.Position -= (int)dataSize; // travel back
								writer.Write(reader.ReadBytes((int)dataSize));
							} else {
								// skip, we will create a new HASH box, and append to end of stream
								reader.BaseStream.Position += (int)dataSize;
							}
						}

						#region write HASH box
						writer.Write((uint)0);
						writer.Write(Encoding.ASCII.GetBytes("HASH"));
						int size = 8;
						foreach (KeyValuePair<string, byte[]> kv in hashs) {
							byte[] kb = Encoding.ASCII.GetBytes(kv.Key);
							writer.Write(kb);
							writer.Write(kv.Value);
							size += kb.Length;
							size += kv.Value.Length;

						}
						writer.BaseStream.Position -= size;
						writer.Write(SwapUInt32((uint)size));
						#endregion

						writer.Close();
					}

					reader.Close();
				}

				//file.Delete();
				//ofile.MoveTo(file.FullName);

				ofile.CopyTo(file.FullName, true);
				ofile.Delete();

				return true;
			} catch {
				ofile.Delete();
				return false;
			}
		}

		private static string BytesToStr(byte[] bytes) {
			StringBuilder str = new StringBuilder();

			for (int i = 0; i < bytes.Length; i++)
				str.AppendFormat("{0:X2}", bytes[i]);

			return str.ToString();
		}


		#region Big Endian
		private static ulong ReadUInt64Big(BinaryReader reader) {
			byte[] buffer = reader.ReadBytes(8);
			uint num = (uint)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
			uint num2 = (uint)(((buffer[7] | (buffer[6] << 8)) | (buffer[5] << 0x10)) | (buffer[4] << 0x18));
			return ((num2 << 0x20) | num);
		}

		private static UInt32 ReadUInt32big(BinaryReader reader) {
			byte[] buffer = reader.ReadBytes(4);
			return (uint)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
		}

		private static UInt32 SwapUInt32(UInt32 value) {
			byte[] buffer = BitConverter.GetBytes(value);
			return (uint)(((buffer[3] | (buffer[2] << 8)) | (buffer[1] << 0x10)) | (buffer[0] << 0x18));
		}
		#endregion

	} // eoc
}
