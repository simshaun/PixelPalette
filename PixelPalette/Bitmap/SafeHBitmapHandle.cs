using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace PixelPalette.Bitmap;

public partial class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [LibraryImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DeleteObject(nint hObject);
    
    [SecurityCritical]
    public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
    {
        SetHandle(preexistingHandle);
    }

    protected override bool ReleaseHandle()
    {
        return DeleteObject(handle);
    }
}