namespace CollegeApp.MyLogging
{
    public class LogToDB : IMyLoger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogtoDB");
        }
    }
}
