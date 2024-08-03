namespace CollegeApp.MyLogging
{
    public class LogToFile : IMyLoger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Logtofile");
        }
    }
}
