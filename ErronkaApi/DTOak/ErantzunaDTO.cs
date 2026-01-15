namespace ErronkaApi.DTOak
{
    public class ErantzunaDTO<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<T>? Datuak { get; set; }
    }
}
