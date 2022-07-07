using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Models;

namespace WpfPersonBook.Services.Data
{
    public interface IUserRepository
    {
        /// <summary>
        /// Метод отправляет в Api service введённые пользователем данные для авторизации
        /// и возвращает JWT-Token в случае успешного входа
        /// </summary>
        /// <param name="user">Данные пользователя для авторизации в Api service</param>
        /// <returns>JWT-Token or BadRequest status code</returns>
        Task<HttpResponseMessage> LoginUser(ApplicationUser user);
        Task<HttpResponseMessage> RegisterUser(ApplicationUser user);
    }
}
