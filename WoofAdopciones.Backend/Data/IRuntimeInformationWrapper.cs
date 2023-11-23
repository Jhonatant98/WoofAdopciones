using System.Runtime.InteropServices;

namespace WoofAdopciones.Backend.Data
{
    public interface IRuntimeInformationWrapper
    {
        bool IsOSPlatform(OSPlatform osPlatform);
    }
}