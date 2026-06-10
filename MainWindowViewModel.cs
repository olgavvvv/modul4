public partial class MainWindowViewModel : ViewModelBase
{
    private string _fio = string.Empty;
    private string _result = string.Empty;

    public string FIO
    {
        get => _fio;
        set => SetProperty(ref _fio, value);
    }
    
    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    public async Task GetFio()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://89.125.39.39:8080/TransferSimulator/fullName");
        var content = await response.Content.ReadFromJsonAsync<ResultFio>();

        FIO = content.Value;

        if (Regex.IsMatch(FIO, @"^[А-Яа-яЁ-ё\s]+$"))
        {
            Result = "ФИО не содержит запрещенные символы";
        }
        else
        {
            Result = "ФИО содержит запрещенные символы";
        }
    }
    
    public void Validation()
    {
        const string path = "ТестКейс.docx";

        using var doc = WordprocessingDocument.Open(path, true);
        var textElements = doc.MainDocumentPart.Document.Descendants<Text>();

        foreach (var textElement in textElements)
        {
            if (textElement.Text.Contains("FIO"))
            {
                textElement.Text = textElement.Text.Replace("FIO", FIO);
            }
            
            if (textElement.Text.Contains("Res"))
            {
                textElement.Text = textElement.Text.Replace("Res", Result);
            }
        }
        
        doc.MainDocumentPart.Document.Save();
    }
}

public class ResultFio
{
    public string Value { get; set; }
}




public async Task GetFio()
{
    try
    {
        var client = new HttpClient();
        // Пример: IP - 192.168.1.100, порт - 5256
        string serverIp = "192.168.1.100";  // IP сервера
        int port = 5256;                    // Порт сервера
        
        string url = $"http://{serverIp}:{port}";
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadFromJsonAsync<ResultFio>();

        FIO = content.Value;

        if (Regex.IsMatch(FIO, @"^[А-Яа-яЁ-ё\s]+$"))
        {
            Result = "ФИО не содержит запрещенные символы";
        }
        else
        {
            Result = "ФИО содержит запрещенные символы";
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
