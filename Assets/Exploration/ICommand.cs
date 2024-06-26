namespace Exploration
{
    public interface ICommand
    {
        /// <summary>
        /// Execute the command and return whenever or not it support undo
        /// </summary>
        /// <returns></returns>
        public bool Execute();
    }
}