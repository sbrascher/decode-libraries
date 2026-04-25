using FluentValidation.Results;
using System.Net;

namespace Decode.Notifications.FluentValidation;

/// <summary>
/// Provides extension methods for adding FluentValidation <see cref="ValidationResult"/> errors to a <see cref="DomainNotificationContext"/>.
/// </summary>
public static class NotificationContextExtensions
{
    /// <summary>
    /// Adds all errors from the specified <see cref="ValidationResult"/> to the <see cref="DomainNotificationContext"/>.
    /// </summary>
    /// <param name="context">The notification context to add errors to.</param>
    /// <param name="validationResult">The validation result containing errors.</param>
    /// <param name="statusCode">The HTTP status code to associate with each error. Defaults to <see cref="HttpStatusCode.BadRequest"/>.</param>
    public static void AddValidationResult(
        this DomainNotificationContext context,
        ValidationResult validationResult,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        if (validationResult.IsValid)
        {
            return;
        }

        foreach (ValidationFailure? error in validationResult.Errors)
        {
            context.Add(error.ErrorMessage, statusCode, error.PropertyName);
        }
    }
}
