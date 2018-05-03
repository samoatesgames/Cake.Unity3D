
namespace Cake.Unity3D.Helpers
{
    /// <summary>
    /// Helper class for processing the Unity3D editor log
    /// </summary>
    public static class Unity3DEditorLog
    {
        /// <summary>
        /// The type of message we deem a line of log text to be.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// Useful for diagnostics
            /// </summary>
            Debug,

            /// <summary>
            /// Important information
            /// </summary>
            Info,

            /// <summary>
            /// A warning.
            /// </summary>
            Warning,

            /// <summary>
            /// An error.
            /// </summary>
            Error
        }

        /// <summary>
        /// Given a line of text from the editor log, calculate
        /// the type of message the line is.
        /// This is very privative and needs vast improvements.
        /// </summary>
        /// <param name="line">The line of text to process.</param>
        /// <returns>The type of message the line represents.</returns>
        public static MessageType ProcessLogLine(string line)
        {
            if (line.Contains("Build Failed"))
            {
                return MessageType.Error;
            }

            if (line.StartsWith("WARNING", System.StringComparison.OrdinalIgnoreCase))
            {
                return MessageType.Warning;
            }

            if (line.StartsWith("[Cake.Unity3D]", System.StringComparison.OrdinalIgnoreCase))
            {
                return MessageType.Info;
            }

            return MessageType.Debug;
        }
    }
}
