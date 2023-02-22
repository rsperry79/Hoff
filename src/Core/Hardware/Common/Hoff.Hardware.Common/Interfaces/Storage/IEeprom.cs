
using System.Collections;

namespace Hoff.Hardware.Common.Interfaces.Storage
{
    public interface IEeprom
    {
        int GetPageCount();
        int GetPageSize();
        int GetSize();
        ArrayList ReadArrayList(byte address);
        string ReadString(byte address);
        bool Write(byte address, byte[] message);
        bool WriteArrayList(byte address, ArrayList list);
        bool WriteByte(byte address, byte message);
        bool WriteString(byte address, string message);
    }

}
