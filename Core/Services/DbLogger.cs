using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Core.Providers;
using Core.Models.Systems;

namespace Core.Services
{
    /// <summary>  
    /// Creates a new instance of <see cref="FileLogger" />.  
    /// </summary>  
    /// <param name="fileLoggerProvider">Instance of <see cref="FileLoggerProvider" />.</param>  
    /// <param name="dbLoggerProvider">  
    /// Instance of <see cref="DbLoggerProvider" />.  
    /// </param>  
    public class DbLogger([NotNull] DbLoggerProvider dbLoggerProvider) : ILogger
    {
        IDisposable? ILogger.BeginScope<TState>(TState state) => null;

        /// <summary>  
        /// Whether to log the entry.  
        /// </summary>  
        /// <param name="logLevel"></param>  
        /// <returns></returns>  
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }


        /// <summary>  
        /// Used to log the entry.  
        /// </summary>  
        /// <typeparam name="TState"></typeparam>  
        /// <param name="logLevel">An instance of <see cref="LogLevel"/>.</param>  
        /// <param name="eventId">The event's ID. An instance of <see cref="EventId"/>.</param>  
        /// <param name="state">The event's state.</param>  
        /// <param name="exception">The event's exception. An instance of <see cref="Exception" /></param>  
        /// <param name="formatter">A delegate that formats </param>  
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                // Don't log the entry if it's not enabled.  
                return;
            }

            var threadId = Environment.CurrentManagedThreadId; // Get the current thread ID to use in the log file.   

            // Store record.  
            using (var connection = new SqlConnection(dbLoggerProvider.Options.ConnectionString))
            {
                connection.Open();

                // Add to database.  

                // LogLevel  
                // ThreadId  
                // EventId  
                // Exception Message (use formatter)  
                // Exception Stack Trace  
                // Exception Source  

                var values = new JObject();

                if (dbLoggerProvider?.Options?.LogFields?.Any() ?? false)
                {
                    foreach (var logField in dbLoggerProvider.Options.LogFields)
                    {
                        switch (logField)
                        {
                            //case "LogLevel":
                            //    if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            //    {
                            //        values["LogLevel"] = logLevel.ToString();
                            //    }
                            //    break;
                            //case "ThreadId":
                            //    values["ThreadId"] = threadId;
                            //    break;
                            //case "EventId":
                            //    values["EventId"] = eventId.Id;
                            //    break;
                            //case "EventName":
                            //    if (!string.IsNullOrWhiteSpace(eventId.Name))
                            //    {
                            //        values["EventName"] = eventId.Name;
                            //    }
                            //    break;
                            //case "Message":
                            //    if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                            //    {
                            //        values["Message"] = formatter(state, exception);
                            //    }
                            //    break;
                            case "ExceptionMessage":
                                if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                                {
                                    values["ExceptionMessage"] = exception?.Message;
                                }
                                break;
                            case "ExceptionStackTrace":
                                if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                                {
                                    values["ExceptionStackTrace"] = exception?.StackTrace;
                                }
                                break;
                            case "ExceptionSource":
                                if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                                {
                                    values["ExceptionSource"] = exception?.Source;
                                }
                                break;
                        }
                    }
                }


                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $@"INSERT INTO {dbLoggerProvider.Options.LogTable} 
                                                ([{nameof(Log.LogLevel)}],[{nameof(Log.ThreadId)}],[{nameof(Log.EventId)}],[{nameof(Log.EventName)}],[{nameof(Log.Message)}],[{nameof(Log.Values)}], [{nameof(Log.Created)}]) 
                                            VALUES (@{nameof(Log.LogLevel)},@{nameof(Log.ThreadId)},@{nameof(Log.EventId)},@{nameof(Log.EventName)},@{nameof(Log.Message)},@{nameof(Log.Values)}, @{nameof(Log.Created)})";

                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.LogLevel)}", logLevel.ToString()));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.ThreadId)}", threadId));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.EventId)}", eventId.Id));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.EventName)}", eventId.Name ?? ""));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.Message)}", formatter(state, exception) ?? string.Empty));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.Values)}", JsonConvert.SerializeObject(values, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        Formatting = Formatting.None
                    }).ToString()));
                    command.Parameters.Add(new SqlParameter($"@{nameof(Log.Created)}", DateTimeOffset.Now));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
