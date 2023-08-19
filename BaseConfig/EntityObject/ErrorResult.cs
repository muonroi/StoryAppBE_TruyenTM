using System.ComponentModel.DataAnnotations.Schema;

namespace BaseConfig.EntityObject.EntityObject
{
    [NotMapped]
    public class ErrorResult
    {
        private List<string>? errorValues;

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public List<string>? ErrorValues { get => errorValues; set => errorValues = value; }

        public ErrorResult() => ErrorValues = new List<string>();

        public override string ToString()
        {
            if (ErrorValues != null && ErrorValues.Count > 0)
            {
                return "[" + ErrorCode + ": " + ErrorMessage + " (" + string.Join(',', ErrorValues) + ")]";
            }

            return "[" + ErrorCode + ": " + ErrorMessage + "]";
        }
    }
}
