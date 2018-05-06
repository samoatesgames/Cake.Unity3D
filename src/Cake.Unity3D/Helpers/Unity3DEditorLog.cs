
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        /// A map between Regex patterns and the type of message they represent.
        /// This allows us to do some basic pattern matching on log lines to improve
        /// the readability.
        /// </summary>
        private static readonly Dictionary<Regex, MessageType> LogPatterns = new Dictionary<Regex, MessageType>();

        /// <summary>
        /// Static constructor used for setting up message type patterns.
        /// </summary>
        static Unity3DEditorLog()
        {
            LogPatterns[new Regex(@"build\sfailed", RegexOptions.IgnoreCase)] = MessageType.Error;
            LogPatterns[new Regex(@"error\s\w+\d+\:", RegexOptions.IgnoreCase)] = MessageType.Error;

            LogPatterns[new Regex(@"^warning\s", RegexOptions.IgnoreCase)] = MessageType.Warning;
            LogPatterns[new Regex(@"\swarning\s\w+\d+\:", RegexOptions.IgnoreCase)] = MessageType.Warning;

            LogPatterns[new Regex(@"\[Cake\.Unity3D\]", RegexOptions.IgnoreCase)] = MessageType.Info;
            LogPatterns[new Regex(@"^-\s", RegexOptions.IgnoreCase)] = MessageType.Info;
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
            foreach (var pattern in LogPatterns)
            {
                if (pattern.Key.IsMatch(line))
                {
                    return pattern.Value;
                }
            }

            return MessageType.Debug;
        }
    }
}
