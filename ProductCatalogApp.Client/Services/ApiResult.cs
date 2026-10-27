using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductCatalogApp.Client.Services
{
    public class ApiResult
    {
        public bool Succeeded { get; private init; }

        public bool NotFound { get; private init; }

        public IReadOnlyDictionary<string, string[]> Errors { get; private init; } =
            new Dictionary<string, string[]>();

        public static ApiResult Success() => new() { Succeeded = true };

        public static ApiResult NotFoundResult() => new() { NotFound = true };

        public static ApiResult Invalid(IDictionary<string, string[]> errors) =>
            new() { Errors = new Dictionary<string, string[]>(errors) };

        public void CopyErrorsTo(ModelStateDictionary modelState)
        {
            if (NotFound)
            {
                modelState.AddModelError(string.Empty, "Запись не найдена: возможно, её уже удалили.");
                return;
            }

            if (Errors.Count == 0)
            {
                modelState.AddModelError(string.Empty, "Не удалось выполнить операцию.");
                return;
            }

            foreach (var (field, messages) in Errors)
            {
                foreach (var message in messages)
                {
                    modelState.AddModelError(field, message);
                }
            }
        }
    }
}
