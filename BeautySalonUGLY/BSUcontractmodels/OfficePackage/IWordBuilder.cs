public abstract class IWordBuilder
{
    public abstract IWordBuilder AddHeader(string header);

    public abstract IWordBuilder AddParagraph(string text);

    public abstract IWordBuilder AddTable(int[] widths, List<string[]> data);

    public abstract Stream Build();
}