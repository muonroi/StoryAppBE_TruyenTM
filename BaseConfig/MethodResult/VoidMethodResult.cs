using BaseConfig.EntityObject.EntityObject;
using Microsoft.AspNetCore.Mvc;

namespace BaseConfig.MethodResult
{
    public class VoidMethodResult
    {
        private readonly List<ErrorResult> _errorMessages = new();

        public IReadOnlyCollection<ErrorResult> ErrorMessages => _errorMessages;

        public bool IsOK => _errorMessages.Count == 0;

        public int? StatusCode { get; set; }

        public void AddErrorMessage(ErrorResult errorResult)
        {
            _errorMessages.Add(errorResult);
        }

        public void AddErrorMessage(string errorCode, string errorMessage, string[] errorValues)
        {
            ErrorResult errorResult = new()
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
            if (errorValues != null && errorValues.Length != 0)
            {
                foreach (string item in errorValues)
                {
                    errorResult?.ErrorValues?.Add(item);
                }
            }

            AddErrorMessage(errorResult);
        }

        public void AddErrorMessage(string exceptionErrorMessage, string exceptionStackTrace = "")
        {
            AddErrorMessage("ERR_COM_API_SERVER_ERROR", "API_ERROR", Array.Empty<string>(), exceptionErrorMessage, exceptionStackTrace);
        }

        private void AddErrorMessage(string errorCode, string errorMessage, string[] errorValues, string exceptionErrorMessage, string exceptionStackTrace)
        {
            _errorMessages.Add(new ErrorResult
            {
                ErrorCode = errorCode,
                ErrorMessage = "Error: " + errorMessage + ", Exception Message: " + exceptionErrorMessage + ", Stack Trace: " + exceptionStackTrace,
                ErrorValues = new List<string>(errorValues)
            });
        }

        public virtual IActionResult GetActionResult()
        {
            ObjectResult objectResult = new(this);
            if (!StatusCode.HasValue)
            {
                objectResult.StatusCode = 500;
                return objectResult;
            }

            objectResult.StatusCode = StatusCode;
            return objectResult;
        }
    }
}
