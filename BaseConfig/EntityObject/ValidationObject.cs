using BaseConfig.EntityObject.EntityObject;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BaseConfig.EntityObject.Entity
{
    public class ValidationObject
    {
        protected List<ErrorResult> _errorMessages = new();
        [JsonIgnore]
        public IReadOnlyCollection<ErrorResult> ErrorMessages => _errorMessages;

        protected void AddValidationError(string errorCode, string propertyName, object propertyValue)
        {
            AddValidationError(errorCode, new List<string> { Helpers.GenerateErrorResult(propertyName, propertyValue) });
        }

        protected void AddValidationError(string errorCode, List<string> errorValues)
        {
            _errorMessages.Add(new ErrorResult
            {
                ErrorCode = errorCode,
                ErrorMessage = Helpers.GetErrorMessage(errorCode),
                ErrorValues = errorValues
            });
        }

        protected void AddValidationErrors(IEnumerable<ErrorResult> errorMessages)
        {
            _errorMessages.AddRange(errorMessages);
        }

        public virtual bool IsValid()
        {
            ValidationContext validationContext = new(this, null, null);
            List<ValidationResult> list = new();
            if (!Validator.TryValidateObject(this, validationContext, list, validateAllProperties: true))
            {
                foreach (ValidationResult item in list)
                {
                    ErrorResult errorResult = new()
                    {
                        ErrorCode = item.ErrorMessage,
                        ErrorMessage = Helpers.GetErrorMessage(item.ErrorMessage ?? string.Empty)
                    };
                    foreach (string memberName in item.MemberNames)
                    {
                        PropertyInfo property = validationContext.ObjectType.GetProperty(memberName);
                        object value = property.GetValue(validationContext.ObjectInstance, null);
                        errorResult.ErrorValues.Add(Helpers.GenerateErrorResult(memberName, value));
                    }

                    _errorMessages.Add(errorResult);
                }
            }

            return _errorMessages.Count == 0;
        }
    }
}