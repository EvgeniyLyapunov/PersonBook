# PersonBook
Удобный способ записывать, хранить и использовать свои контакты<br><br>

## 📖 О проекте<br> 
<br>
💡 Этот проект реализован в рамках технического задания (ТЗ), которое представлено в Web Application данного проекта.<br><br>
🧩 Проект состоит из трёх независимых завершённых частей - API сервиса, WEB приложения и Desktop приложения.<br><br>
🎯 Цель проекта — получить практические навыки и закрепить теорию, в рамках изучения фрэймворка ASP.NET Core.<br><br>
🛠 Стек - C#, ASP.NET Core, WPF, Entity Framework Core<br><br>
⚙️ Функционал - регистрация, авторизация и CRUD - операции с данными, которые представленны в объекте Контакт. Данные хранятся и предоставляются API сервисом посредством HTTP запросов. Пара - логин пароль валидны как на сайте, так и в десктоп клиенте.<br><br>
🎭 На сайте есть администратор - он может управлять созданием и удалением Юзеров и Администраторов. Юзер может видеть детали всех контактов, но редактировать и удалять только свои. Не зарегистрированный гость может только видеть список имён и аватарок контактов. Данные регистрации на сайте можно использовать для авторизации в десктоп клиенте, и наоборот.<br><br><br> 

![titlescreen](https://user-images.githubusercontent.com/77357671/178049289-e7f09bbc-ca33-4352-b0a5-f992a5f84471.png)


---
# 🚀 Начало работы
### Для пользователей Visual Studio:
1. Скачать и открыть решение в Visual Studio.
2. В проектах ApiPersonBook, WebApplicationPersonBook в файле appsettings.json в строке подключения к базе данных пропишите имя вашего сервера;
3. В проекте DataBaseLibrary запустить миграции для создания базы данных;
4. В проекте ApiPersonBook в файле launchSettings.json скопируйте значение свойства "applicationUrl"
и вставьте его в проектах WebApplicationPersonBook, WpfPersonBook в файле AppConst.cs как значение в переменную ApiPath;
5. В свойствах решения назначьте несколько запускаемых проектов: ApiPersonBook, WebApplicationPersonBook, WpfPersonBook;
6. Запустите решение;
---




## 📺 Демонстрация работы

<video src="https://user-images.githubusercontent.com/77357671/178057265-4589fecd-da66-4539-852f-ec0bed3847b4.mp4" ></video>

<video src="https://user-images.githubusercontent.com/77357671/178059785-9007446a-ef32-4977-943f-6390ee0b5a6f.mp4" ></video>






---
## 🛠️ Инструменты и технологии
<div>
<a>
   <img src="https://user-images.githubusercontent.com/61462657/171970442-3c60c757-6df1-4d2f-8d20-200e1f2d4448.svg"  title="C#" alt="С#" width="40" height="40"/>&nbsp;
</a>

<a>
   <img src="https://user-images.githubusercontent.com/77357671/178060769-b5ad4d71-041c-448d-b33a-94997559aa0d.png" title="WPF" alt="WPF" width="40" height="40"/>&nbsp;
</a>

<a>
   <img src="https://user-images.githubusercontent.com/77357671/178063226-c8ee96c0-44c5-4b2d-a1d1-356b509563ee.png" title="ASP.NET Core" alt="ASP.NET Core" width="60" height="40"/>&nbsp;
</a>   

<a>
   <img src="https://user-images.githubusercontent.com/77357671/178065354-e1b3ef76-755d-4fc6-8cb4-9cdb97e662f8.png" title="Entity Framework Core" alt="Entity Framework Core" height="40" />&nbsp;
</a>   

<a>
   <img src="https://user-images.githubusercontent.com/77357671/178066126-75ff0003-6023-4dc6-88a5-ec98de838b9b.png" title="VS" alt="VS" height="40" />&nbsp;
</a>   

<a>
   <img src="https://user-images.githubusercontent.com/61462657/171980547-0b97aec8-7e04-49e1-b6b5-8905651249b3.png" title="AutoMapper" alt="AutoMapper" width="40" height="40"/>&nbsp;
</a>   

</div>


