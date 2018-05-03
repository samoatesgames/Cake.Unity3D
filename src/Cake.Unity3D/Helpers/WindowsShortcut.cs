using System;
using System.IO;

namespace Cake.Unity3D.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class WindowsShortcut
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetShortcutTarget(string file)
        {
            try
            {
                if (Path.GetExtension(file)?.ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                var fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (var fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    var flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                                           // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        var offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    var fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                                                                // structure begins
                    var totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    var fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                                                              // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                                                                                        // base pathname (target)
                    var pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                                                                                                       // the base pathname. I don't need the 2 terminating nulls.
                    var linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    var begin = link.IndexOf("\0\0", StringComparison.Ordinal);
                    if (begin > -1)
                    {
                        var end = link.IndexOf("\\\\", begin + 2, StringComparison.Ordinal) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        var firstPart = link.Substring(0, begin);
                        var secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }

                    return link;
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
