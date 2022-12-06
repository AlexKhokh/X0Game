namespace App.Entities
{
    public class CustomException : Exception
    {
        public CustomException(string msg) : base(msg)
        { }
    }
}
