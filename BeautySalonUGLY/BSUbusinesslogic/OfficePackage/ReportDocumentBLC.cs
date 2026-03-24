using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.DataModels;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.OfficePackage;
using BSUmodels.Exceptions;

namespace BSUbusinesslogic.OfficePackage;

/// Реализация сервиса для формирования документов отчётов
/// Формирует Word документы используя WordReportBuilder
public class ReportDocumentBLC : IReportDocumentBLC
{
    private readonly IWordBuilder _wordBuilder;

    public ReportDocumentBLC(IWordBuilder wordBuilder)
    {
        _wordBuilder = wordBuilder ?? throw new ArgumentNullException(nameof(wordBuilder));
    }

    /// Сформировать Word документ с отчётом по посещениям мастера
    public Stream GenerateMasterVisitsWordReport(List<MasterVisitsDM> masterVisits)
    {
        if (masterVisits == null || masterVisits.Count == 0)
            throw new NullListException();

        ClearBuilder();

        // Основной заголовок документа
        _wordBuilder.AddHeader("ОТЧЁТ ПО ПОСЕЩЕНИЯМ МАСТЕРА");

        // Проходимся по каждому мастеру (обычно один)
        foreach (var master in masterVisits)
        {
            _wordBuilder.AddHeader($"Мастер: {master.MasterFullName}");
            _wordBuilder.AddParagraph($"Всего посещений: {master.Visits.Count}");
            _wordBuilder.AddParagraph(string.Empty); // пустая строка для разделения

            // Добавляем список всех посещений
            if (master.Visits.Count > 0)
            {
                _wordBuilder.AddParagraph("Список посещений:");

                // Преобразуем список посещений в таблицу
                var tableData = ConvertVisitsToTableData(master.Visits);
                var columnWidths = new int[] { 25, 35, 25, 15 }; // Дата, Клиент, Услуги, Сумма (в %)

                _wordBuilder.AddTable(columnWidths, tableData);
            }

            _wordBuilder.AddParagraph(string.Empty);
            _wordBuilder.AddParagraph($"Дата создания отчёта: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        }

        return _wordBuilder.Build();
    }

    /// Сформировать Word документ с расширенной информацией по посещениям
    public Stream GenerateMasterVisitsWordReportWithDates(List<MasterVisitsDM> masterVisits, DateTime dateFrom, DateTime dateTo)
    {
        if (masterVisits == null || masterVisits.Count == 0)
            throw new NullListException();

        ClearBuilder();

        // Основной заголовок документа
        _wordBuilder.AddHeader("ОТЧЁТ ПО ПОСЕЩЕНИЯМ МАСТЕРА");

        // Информация о периоде
        _wordBuilder.AddParagraph($"Период отчёта: с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy}");
        _wordBuilder.AddParagraph(string.Empty);

        // Проходимся по каждому мастеру
        foreach (var master in masterVisits)
        {
            _wordBuilder.AddHeader($"Мастер: {master.MasterFullName}");
            _wordBuilder.AddParagraph($"Всего посещений: {master.Visits.Count}");

            // Считаем общую сумму
            decimal totalSum = 0;
            try
            {
                totalSum = master.Visits
                    .Where(v => v.Contains("руб."))
                    .Select(v =>
                    {
                        var parts = v.Split('|');
                        if (parts.Length >= 4 && decimal.TryParse(
                            parts[3].Trim().Replace(" руб.", "").Replace(",", "."),
                            System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out decimal sum))
                        {
                            return sum;
                        }
                        return 0;
                    })
                    .Sum();
            }
            catch
            {
                // Если не удалось рассчитать сумму, продолжаем
                totalSum = 0;
            }

            _wordBuilder.AddParagraph($"Общая сумма: {totalSum:F2} руб.");
            _wordBuilder.AddParagraph(string.Empty);

            // Добавляем список всех посещений
            if (master.Visits.Count > 0)
            {
                _wordBuilder.AddParagraph("Подробный список посещений:");

                // Преобразуем список посещений в таблицу
                var tableData = ConvertVisitsToTableData(master.Visits);
                var columnWidths = new int[] { 25, 35, 25, 15 }; // Дата, Клиент, Услуги, Сумма

                _wordBuilder.AddTable(columnWidths, tableData);
            }

            _wordBuilder.AddParagraph(string.Empty);
        }

        _wordBuilder.AddParagraph($"Дата создания отчёта: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");

        return _wordBuilder.Build();
    }

    /// Преобразовать список посещений в формат таблицы
    /// Строки формата: "дата | клиент | услуги | сумма"
    private List<string[]> ConvertVisitsToTableData(List<string> visits)
    {
        var tableData = new List<string[]>();

        // Добавляем заголовок таблицы
        tableData.Add(new[] { "Дата и время", "Клиент", "Услуги", "Сумма" });

        // Добавляем данные посещений
        foreach (var visit in visits)
        {
            var parts = visit.Split('|');
            if (parts.Length >= 4)
            {
                tableData.Add(new[]
                {
                    parts[0].Trim(),        // Дата и время
                    parts[1].Trim(),        // Клиент
                    parts[2].Trim(),        // Услуги
                    parts[3].Trim()         // Сумма
                });
            }
        }

        return tableData;
    }

    /// Очистить построитель (вспомогательный метод)
    private void ClearBuilder()
    {
        if (_wordBuilder is WordReportBuilder wb)
        {
            wb.Clear();
        }
    }
}
