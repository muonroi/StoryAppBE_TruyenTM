using BaseConfig.EntityObject.Entity;
namespace BaseConfig.MethodResult
{
    public static class MethodResultHelpers
    {
        public static string GetErrorMessage(string errorCode)
        {
            return Helpers.GetErrorMessage(errorCode);
        }
        public static void AddApiErrorMessage(this VoidMethodResult errorResult, string errorCode, string[] errorValues)
        {
            errorResult.AddErrorMessage(errorCode, GetErrorMessage(errorCode), errorValues);
        }
    }
}
