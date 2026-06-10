

public partial class AuthWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;

    private int _errorsCounter;

    public async Task Authenticate(Window window)
    {
        await using var db = new MyDbContext();
        
        var user = await db.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Login == _login);

        if (user == null)
        {
            _errorsCounter++;

            if (_errorsCounter >= 3)
                window.Close();
            
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Логин или пароль неверн")
                .ShowAsync();
            
            return;
        }

        if (!user.Isactive)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Бан")
                .ShowAsync();
            
            return; 
        }

        if (user.Password != _password)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Бан")
                .ShowAsync();

            user.CountErrors ??= 0;
            
            user.CountErrors ++;
            if (user.CountErrors >= 3)
            {
                user.Isactive = false;
                await MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Бан")
                    .ShowAsync();
            }
            
            db.Update(user);
            await db.SaveChangesAsync();
            
            return;
        }
        
        await MessageBoxManager.GetMessageBoxStandard("Успех", "Успешная авторизация").ShowAsync();

        if (user.Role.Name == "Admin")
        {
            var adminWindow = new AdminWindow();
            adminWindow.Show();
            window.Close();
        }
    }
}


