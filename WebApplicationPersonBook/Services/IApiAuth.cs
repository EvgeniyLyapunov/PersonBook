using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationPersonBook.AuthData;

namespace WebApplicationPersonBook.Services
{
    public interface IApiAuth
    {
        /// <summary>
        /// Метод реализует регистрацию или вход пользователя в Api service
        /// </summary>
        /// <param name="endOfPath">Route "login" or Route "register"</param>
        /// <param name="apiUser">Данные пользователя, осуществляющего вход</param>
        public Task<HttpResponseMessage> ApiUserValidation(string endOfPath, ApiUser apiUser);

        /// <summary>
        /// Метод реализует регистрацию и сохранение пользователя в Api service через Admin аккаунт
        /// </summary>
        /// <param name="endOfPath">Route "login" or Route "register"</param>
        /// <param name="apiUser">Данные пользователя, осуществляющего вход</param>
        public Task<HttpResponseMessage> ApiUserCreation(string endOfPath, ApiUser apiUser, string token);

        /// <summary>
        /// Метод реализует удаление пользователя из Api service через Admin аккаунт
        /// </summary>
        /// <param name="endOfPath">Route "login" or Route "register"</param>
        /// <param name="apiUser">Данные пользователя, осуществляющего вход</param>
        public Task<HttpResponseMessage> ApiUserDelete(string endOfPath, string userName, string token);

    }
}
