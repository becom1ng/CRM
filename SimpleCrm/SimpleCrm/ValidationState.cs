namespace SimpleCrm
{
    internal class ValidationState
    {
        public List<ValidationError> Errors { get; set; }
        public ValidationState()
        {
            Errors = new List<ValidationError>();
        }
    }
}
