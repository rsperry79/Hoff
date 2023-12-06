using System;

namespace Hoff.Core.Common.Interfaces
{
    public interface IChangeNotifcation
    {
        event EventHandler<bool> DataChanged;
    }
}