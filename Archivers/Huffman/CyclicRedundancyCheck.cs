namespace Archivers.Huffman
{
	public class CyclicRedundancyCheck
	{
		static uint poly = 0x82608edb;
		static uint[] table = new uint[256];
		uint crc;

		static CyclicRedundancyCheck() {
			for (uint i = 0; i < 256; i++) {
                uint cs = i;
                for (uint j = 0; j < 8; j++)
	                cs = (cs & 1) > 0 ? (cs >> 1) ^ poly : cs >> 1;
                table[i] = cs;
            }
		}		
		
		public uint Get() => crc;
				
		public CyclicRedundancyCheck () {
			crc = 0xffffffff;
		}
		
		public uint UpdateByte(byte b) {
			crc = table[(crc ^ b) & 0xff] ^ (crc >> 8);
            crc ^= 0xffffffff;
			return crc;
		}
	}
}
