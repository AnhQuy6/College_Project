namespace CollegeApp.MyLogging
{
    public class LogToServerMemory : IMyLoger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToServerMemory");
        }
    }
}
