using NLog;

namespace RestApiAutomationTraining.Logging;

/// <summary>
/// Utility functions for creating the test results directory and test logs directory
/// </summary>
public class TestResults
{

    public static string WorkspaceDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

    public static string ResultFolder = "TestResults";

    /// <summary>
    /// If the _logger is not initialized throws an exception.
    /// </summary>
    public static Logger Log => _logger ?? throw new NullReferenceException("_logger is null. SetLogger() first.");

    private static object _setLoggerLock = new object();

    [ThreadStatic]
    public static DirectoryInfo CurrentTestDirectory;

    //[ThreadStatic]
    private static Logger? _logger;

    /// <summary>
    /// Creates the test results folder for every test case
    /// </summary>
    public static void SetLogger(TestContext testContext)
    {
        lock (_setLoggerLock)
        {
            var testResultsDir = WorkspaceDirectory + ResultFolder;
            var testName = testContext.Test.Name;
            var fullPath = $"{testResultsDir}/{testName}";

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, recursive: true);
            }

            CurrentTestDirectory = Directory.CreateDirectory(fullPath);
            _logger = LogManager.GetCurrentClassLogger();
        }

        var config = new NLog.Config.LoggingConfiguration();

        // Targets where to log to: File and Console
        var logfile = new NLog.Targets.FileTarget("logfile") 
        { 
            FileName = CurrentTestDirectory.FullName + "/logfile.txt" 
        };
        var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

        // Rules for mapping loggers to targets            
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

        // Apply config           
        LogManager.Configuration = config;
    }
}
