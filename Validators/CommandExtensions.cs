using System.Windows.Input;

namespace HanumanInstitute.Validators;

/// <summary>
/// Provides extension methods for ICommand. 
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Executes a command.
    /// This overloads passes null parameter.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public static void Execute(this ICommand command) =>
        Check.NotNull(command).Execute(null);
    
    /// <summary>
    /// Returns whether the command can execute in its current state.
    /// This overloads passes null parameter.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public static bool CanExecute(this ICommand command) =>
        Check.NotNull(command).CanExecute(null);
    
    /// <summary>
    /// Executes the command if CanExecute is true.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="param">The command parameter.</param>
    public static void ExecuteIfCan(this ICommand command, object? param = null)
    {
        Check.NotNull(command);
        if (command.CanExecute(param))
        {
            command.Execute(param);
        }
    }
}
