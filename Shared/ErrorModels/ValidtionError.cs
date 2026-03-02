namespace Shared.ErrorModels
{
    public class ValidtionError
    {

        public string Field { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}