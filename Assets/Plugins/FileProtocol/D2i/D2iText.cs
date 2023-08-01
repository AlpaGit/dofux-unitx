namespace DofusCoube.FileProtocol.D2i
{
    public sealed class D2IText<TKey>
    {
        public D2IText(TKey id, string text) =>
            (Id, Text, NotDiacriticalText) = (id, text, string.Empty);

        public D2IText(TKey id, string text, string notDiacriticalText) =>
            (Id, Text, UseNotDiacriticalText, NotDiacriticalText) = (id, text, true, notDiacriticalText);

        public TKey Id { get; set; }

        public string Text { get; set; }

        public bool UseNotDiacriticalText { get; set; }

        public string NotDiacriticalText { get; set; }
    }
}