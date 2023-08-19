using BaseConfig.EntityObject.EntityObject;
using Microsoft.AspNetCore.Mvc;

namespace BaseConfig.MethodResult
{
    public class MethodResult<T> : VoidMethodResult
    {
        public T? Result { get; set; }

        public void AddResultFromErrorList(IEnumerable<ErrorResult> errorMessages)
        {
            foreach (ErrorResult errorMessage in errorMessages)
            {
                AddErrorMessage(errorMessage);
            }
        }

        public override IActionResult GetActionResult()
        {
            ObjectResult objectResult = new(this);
            if (!base.StatusCode.HasValue)
            {
                if (base.IsOK)
                {
                    objectResult.StatusCode = ((Result != null) ? 200 : 204);
                }
                else
                {
                    objectResult.StatusCode = 500;
                }

                return objectResult;
            }

            objectResult.StatusCode = base.StatusCode;
            return objectResult;
        }
    }
}
