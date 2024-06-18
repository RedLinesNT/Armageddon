using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Armageddon {

    /// <summary>
    /// Log types used by the <see cref="Logger"/>.
    /// </summary>
    internal enum ELogType : byte {
        TRACE = 0x0,
        WARNING,
        ERROR,
        EXCEPTION,
    }

    /// <summary>
    /// <see cref="Logger"/> is a wrapper for stylizing the <see cref="UnityEngine.Debug"/> log calls and allow to save them in runtime.
    /// </summary>
    /// <remarks>
    /// The following preprocessor definitions must be active such as "<i>LOGGER_ENABLE</i>" to authorize log calls,
    /// and "<i>LOGGER_ENABLE_FILE</i>" to authorize runtime logging into a file.
    /// </remarks>
    public static class Logger {
        
        #region Settings

        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.Log(object)"/> messages.
        /// </summary>
        private const string TRACE_COLOR = nameof(Color.white);
        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.LogWarning(object)"/> messages.
        /// </summary>
        private const string WARNING_COLOR = nameof(Color.yellow);
        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.LogError(object)"/> messages.
        /// </summary>
        private const string ERROR_COLOR = nameof(Color.red);
        
        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.Log(object)"/> messages inside the console.
        /// </summary>
        private const ConsoleColor TRACE_COLOR_CONSOLE = ConsoleColor.White;
        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.LogWarning(object)"/> messages inside the console.
        /// </summary>
        private const ConsoleColor WARNING_COLOR_CONSOLE = ConsoleColor.Yellow;
        /// <summary>
        /// The color of the <see cref="UnityEngine.Debug.LogError(object)"/> messages inside the console.
        /// </summary>
        private const ConsoleColor ERROR_COLOR_CONSOLE = ConsoleColor.Red;

        #endregion
        
        #region Attributes

        /// <summary>
        /// The <see cref="StreamWriter"/> for the output log file.
        /// </summary>
        private static StreamWriter logWriter = null;
        /// <summary>
        /// The <see cref="FileStream"/> for the output log file.
        /// </summary>
        private static FileStream logStream = null;

        #endregion
        
        #region Properties

        /// <summary>
        /// Indicate if the <see cref="Logger"/> is able and will write log calls into an output file.
        /// </summary>
        public static bool CanWriteLogFile { get; private set; } = false;

        #endregion
        
        #region Logger's Initialization Method

        /// <summary>
        /// Initialize the <see cref="Logger"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="RuntimeInitializeLoadType.SubsystemRegistration"/>
        /// </remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void Initialize() {
#if !UNITY_EDITOR && LOGGER_ENABLE_FILE
            SetupOutputLogFile();
            
            Application.quitting += Dispose;
            
            Console.Title = $"Armageddon ({Application.version})"; //Set the title of the Console application
            Console.Clear(); //Clear the Console's previously written content
            Console.InputEncoding = Encoding.Unicode; //Set the correct input encoding for special characters (Japanese, Chinese, Korean, ...)
            Console.OutputEncoding = Encoding.Unicode; //Set the correct output encoding for special characters (Japanese, Chinese, Korean, ...)
#endif
        }

        #endregion
        
        #region Logger's Internal Methods

        /// <summary>
        /// Called when the application is about to shutdown, when it's time to dispose the <see cref="Logger"/>'s content.
        /// </summary>
        private static void Dispose() {
            logWriter?.Close();
        }

        /// <summary>
        /// Format a log message with the correct colors inside the editor and on the dedicated server's console.
        /// </summary>
        /// <param name="_color">The <see cref="UnityEngine.Color"/> of the text (<i>only used inside the Editor</i>).</param>
        /// <param name="_message">The log message.</param>
        /// <param name="_consoleColor">Use this parameter to customize the color of the log call inside the console (<i>only visible when using the console</i>).</param>
        private static string FormatMessage(string _color, object _message, ConsoleColor _consoleColor = ConsoleColor.White) {
#if UNITY_EDITOR //In editor, format it with colors for the Console Window
            return $"<color={_color}>{_message}</color>";
#else
            Console.ForegroundColor = _consoleColor;
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes($"[{GetTimestamp(DateTime.Now)}] {_message}"));
#endif
        }
        
        /// <summary>
        /// Format a log message and its category with the correct colors inside the editor and on the dedicated server's console.
        /// </summary>
        /// <param name="_color">The <see cref="UnityEngine.Color"/> of the text (<i>only used inside the Editor</i>).</param>
        /// <param name="_category">The category of the log message.</param>
        /// <param name="_message">The log message.</param>
        /// <param name="_consoleColor">Use this parameter to customize the color of the log call inside the console (<i>only visible when using the console</i>).</param>
        private static string FormatMessageWithCategory(string _color, string _category, object _message, ConsoleColor _consoleColor = ConsoleColor.White) {
#if UNITY_EDITOR //In editor, format it with colors for the Console Window
            return $"<color={_color}><b>[{_category}]</b> {_message}</color>";
#else
            Console.ForegroundColor = _consoleColor;
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes($"[{GetTimestamp(DateTime.Now)}] [{_category}] {_message}"));
#endif
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> object to a formatted timestamp string.
        /// </summary>
        /// <param name="_time">The time to format.</param>
        private static string GetTimestamp(DateTime _time) {
            return _time.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Setup the output log file if possible.
        /// </summary>
        [Conditional("LOGGER_ENABLE_FILE")] private static void SetupOutputLogFile() {
            string _logFilePath = $"{Application.dataPath}/Logs/Log_{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt"; //The log's path
            FileInfo _logFileInfo = new FileInfo(_logFilePath);
            DirectoryInfo _directoryInfo = null;

            if(_logFileInfo.DirectoryName != null) {
                _directoryInfo = new DirectoryInfo(_logFileInfo.DirectoryName);
            } else {
                TraceError("Internal Logger", "Unable to write logs into an output file!");
                return;
            }
            
            if(!_directoryInfo.Exists) _directoryInfo.Create(); //If the directory doesn't exists, create it.
            
            if(!_logFileInfo.Exists) { //If the file doesn't exists
                logStream = _logFileInfo.Create();
            } else {
                logStream = _logFileInfo.Create();
            }
            
            CanWriteLogFile = true;
            logWriter = new StreamWriter(logStream);
            
            //Write a basic log setup
            logWriter.WriteLine($"{Application.productName} - {Application.companyName}  ({Application.version} - {Application.platform.ToString()})");
            logWriter.WriteLine($"Output log file ({DateTime.Now})");
            logWriter.WriteLine($"-------------------------------------------------------------------------------------------------------------------------");
        }
        
        [Conditional("LOGGER_ENABLE_FILE")] private static void WriteOnFile(ELogType _logType, string _message, string _category = "") {
            if(!CanWriteLogFile) return;

            if(string.IsNullOrEmpty(_category)) { //I should be using a ? expression, but the line is gonna be too long
                logWriter.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{_logType}] {_message}");
            } else {
                logWriter.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{_logType}] [{_category}] {_message}");
            }
        }

        #endregion

        #region Logger's External Logging Methods

        [Conditional("LOGGER_ENABLE")] public static void Trace(object _message) {
            Debug.Log(FormatMessage(TRACE_COLOR, _message, TRACE_COLOR_CONSOLE));
            WriteOnFile(ELogType.TRACE, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")] public static void Trace(string _category, object _message) {
            Debug.Log(FormatMessageWithCategory(TRACE_COLOR, _category, _message, TRACE_COLOR_CONSOLE));
            WriteOnFile(ELogType.TRACE, _message.ToString(), _category);
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceFormat(string _format, params object[] _args) {
            Debug.Log(FormatMessage(TRACE_COLOR, string.Format(_format, _args), TRACE_COLOR_CONSOLE));
            WriteOnFile(ELogType.TRACE, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceFormat(string _category, string _format, params object[] _args) {
            Debug.Log(FormatMessageWithCategory(TRACE_COLOR, _category, string.Format(_format, _args), TRACE_COLOR_CONSOLE));
            WriteOnFile(ELogType.TRACE, string.Format(_format, _args), _category);
        }
        
        [Conditional("LOGGER_ENABLE")] public static void TraceWarning(object _message) {
            Debug.LogWarning(FormatMessage(WARNING_COLOR, _message, WARNING_COLOR_CONSOLE));
            WriteOnFile(ELogType.WARNING, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceWarning(string _category, object _message) {
            Debug.LogWarning(FormatMessageWithCategory(WARNING_COLOR, _category, _message, WARNING_COLOR_CONSOLE));
            WriteOnFile(ELogType.WARNING, _message.ToString(), _category);
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceWarningFormat(string _format, params object[] _args) {
            Debug.LogWarningFormat(FormatMessage(WARNING_COLOR, string.Format(_format, _args), WARNING_COLOR_CONSOLE));
            WriteOnFile(ELogType.WARNING, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceWarningFormat(string _category, string _format, params object[] _args) {
            Debug.LogWarningFormat(FormatMessageWithCategory(WARNING_COLOR, _category, string.Format(_format, _args), WARNING_COLOR_CONSOLE));
            WriteOnFile(ELogType.WARNING, string.Format(_format, _args), _category);
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceError(object _message) {
            Debug.LogError(FormatMessage(ERROR_COLOR, _message, ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.ERROR, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceError(string _category, object _message) {
            Debug.LogError(FormatMessageWithCategory(ERROR_COLOR, _category, _message, ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.ERROR, _message.ToString(), _category);
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceErrorFormat(string _format, params object[] _args) {
            Debug.LogErrorFormat(FormatMessage(ERROR_COLOR, string.Format(_format, _args), ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.ERROR, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceErrorFormat(string _category, string _format, params object[] _args) {
            Debug.LogErrorFormat(FormatMessageWithCategory(ERROR_COLOR, _category, string.Format(_format, _args), ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.ERROR, string.Format(_format, _args), _category);
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceException(Exception _exception) {
            Debug.LogError(FormatMessage(ERROR_COLOR, _exception.Message, ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.EXCEPTION, _exception.ToString());
        }

        [Conditional("LOGGER_ENABLE")] public static void TraceException(string _category, Exception _exception) {
            Debug.LogError(FormatMessageWithCategory(ERROR_COLOR, _category, _exception.Message, ERROR_COLOR_CONSOLE));
            WriteOnFile(ELogType.EXCEPTION, _exception.ToString(), _category);
        }

        #endregion
        
    }
    
}
