using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace BSUbusinesslogic.OfficePackage;

/// Построитель отчётов в формате Word (.docx)
/// Генерирует документы отчётов по посещениям мастеров с использованием OpenXml

public class WordReportBuilder : IWordBuilder
{
    private readonly List<string> _headers = new();
    private readonly List<string> _paragraphs = new();
    private readonly List<(int[], List<string[]>)> _tables = new();

    /// Добавить заголовок в отчёт
    public override IWordBuilder AddHeader(string header)
    {
        if (!string.IsNullOrWhiteSpace(header))
        {
            _headers.Add(header);
        }
        return this;
    }

    /// Добавить параграф текста в отчёт
    public override IWordBuilder AddParagraph(string text)
    {
        // Добавляем даже пустые параграфы для создания визуального разделения
        _paragraphs.Add(text ?? string.Empty);
        return this;
    }


    /// Добавить таблицу в отчёт
    /// <param name="widths">Массив ширин столбцов в процентах</param>
    /// <param name="data">Список массивов строк (каждый массив - одна строка таблицы)</param>
    public override IWordBuilder AddTable(int[] widths, List<string[]> data)
    {
        if (widths != null && widths.Length > 0 && data != null && data.Count > 0)
        {
            _tables.Add((widths, data));
        }
        return this;
    }

    /// > Построить документ и вернуть поток байтов
    /// Использует DocumentFormat.OpenXml для генерации .docx файла
    public override Stream Build()
    {
        var stream = new MemoryStream();

        try
        {
            using (var wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, autoSave: true))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                var body = mainPart.Document.Body;

                // Добавляем заголовки
                foreach (var header in _headers)
                {
                    body.AppendChild(CreateHeaderParagraph(header));
                }

                // Добавляем параграфы
                foreach (var paragraph in _paragraphs)
                {
                    body.AppendChild(CreateRegularParagraph(paragraph));
                }

                // Добавляем таблицы
                foreach (var (widths, tableData) in _tables)
                {
                    body.AppendChild(CreateTable(widths, tableData));
                }
            }

            stream.Position = 0;
            return stream;
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    /// Очистить построитель для создания нового отчёта
    public void Clear()
    {
        _headers.Clear();
        _paragraphs.Clear();
        _tables.Clear();
    }

    /// Создать параграф с форматированием заголовка
    private static Paragraph CreateHeaderParagraph(string text)
    {
        var paragraph = new Paragraph();
        var run = new Run();

        var runProperties = new RunProperties();
        runProperties.AppendChild(new Bold());
        runProperties.AppendChild(new FontSize { Val = "28" }); // 14pt

        run.AppendChild(runProperties);
        run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });

        paragraph.AppendChild(run);

        var paragraphProperties = new ParagraphProperties();
        paragraphProperties.AppendChild(new SpacingBetweenLines { After = "200" });
        paragraphProperties.AppendChild(new Justification { Val = JustificationValues.Center });

        paragraph.InsertBefore(paragraphProperties, paragraph.FirstChild);

        return paragraph;
    }

    /// Создать обычный параграф текста
    private static Paragraph CreateRegularParagraph(string text)
    {
        var paragraph = new Paragraph();

        var paragraphProperties = new ParagraphProperties();
        paragraphProperties.AppendChild(new SpacingBetweenLines { After = "100" });
        paragraph.AppendChild(paragraphProperties);

        // Если текст не пуст, добавляем его
        if (!string.IsNullOrEmpty(text))
        {
            var run = new Run();
            var runProperties = new RunProperties();
            runProperties.AppendChild(new FontSize { Val = "22" }); // 11pt

            run.AppendChild(runProperties);
            run.AppendChild(new Text(text) { Space = SpaceProcessingModeValues.Preserve });

            paragraph.AppendChild(run);
        }

        return paragraph;
    }

    /// Создать таблицу с указанными данными и шириной столбцов
    /// <param name="widths">Массив ширин в процентах (сумма должна быть <= 100)</param>
    /// <param name="data">Данные таблицы (строки массивов)</param>
    private static Table CreateTable(int[] widths, List<string[]> data)
    {
        var table = new Table();

        var tblPr = new TableProperties();
        var tblW = new TableWidth { Width = "5000", Type = TableWidthUnitValues.Auto };
        var tblBorders = GetTableBorders();

        tblPr.AppendChild(tblW);
        tblPr.AppendChild(tblBorders);
        table.AppendChild(tblPr);

        // Добавляем строки с данными
        foreach (var rowData in data)
        {
            var tableRow = new TableRow();

            for (int i = 0; i < rowData.Length && i < widths.Length; i++)
            {
                var cell = new TableCell();

                var cellProperties = new TableCellProperties();
                var cellWidth = new TableCellWidth
                {
                    Type = TableWidthUnitValues.Dxa,
                    Width = (widths[i] * 50).ToString() // Конвертируем проценты в твип-ы
                };
                cellProperties.AppendChild(cellWidth);
                cell.AppendChild(cellProperties);

                var paragraph = new Paragraph();
                var run = new Run();
                var runProps = new RunProperties();
                runProps.AppendChild(new FontSize { Val = "22" }); // 11pt
                run.AppendChild(runProps);
                run.AppendChild(new Text(rowData[i]) { Space = SpaceProcessingModeValues.Preserve });
                paragraph.AppendChild(run);

                cell.AppendChild(paragraph);
                tableRow.AppendChild(cell);
            }

            table.AppendChild(tableRow);
        }

        return table;
    }

    /// Получить свойства границ таблицы
    private static TableBorders GetTableBorders()
    {
        var borders = new TableBorders();

        borders.TopBorder = new TopBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };
        borders.BottomBorder = new BottomBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };
        borders.LeftBorder = new LeftBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };
        borders.RightBorder = new RightBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };
        borders.InsideHorizontalBorder = new InsideHorizontalBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };
        borders.InsideVerticalBorder = new InsideVerticalBorder { Val = BorderValues.Single, Size = 12, Space = 0, Color = "000000" };

        return borders;
    }
}
