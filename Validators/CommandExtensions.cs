using System.Windows.Input;

namespace HanumanInstitute.Validators;

/// <summary>
/// Provides extension methods for ICommand. 
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Executes a command.1
    /// This overloads passes null parameter.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public static void Execute(this ICommand command) => 
        command.CheckNotNull(nameof(command)).Execute();
    
    /// <summary>
    /// Returns whether the command can execute in its current state.
    /// This overloads passes null parameter.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public static bool CanExecute(this ICommand command) => 
        command.CheckNotNull(nameof(command)).CanExecute(null);
    
    /// <summary>
    /// Executes the command if CanExecute if true.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="param">The command parameter.</param>
    public static void ExecuteIfCan(this ICommand command, object? param = null)
    {
        command.CheckNotNull(nameof(command));
        if (command.CanExecute(param))
        {
            command.Execute(param);
        }
    }
}
