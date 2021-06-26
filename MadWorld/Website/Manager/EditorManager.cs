using System;
using System.Collections.Generic;

namespace Website.Manager
{
    public static class EditorManager
    {
        public static string ResultToPrint { get; private set; } = string.Empty;

        public static void Reset()
        {
            ResultToPrint = string.Empty;
        }

        public static void WriteLine(string showText)
        {
            ResultToPrint += showText + Environment.NewLine;
        }
    }
}
