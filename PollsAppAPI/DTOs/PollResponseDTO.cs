namespace PollsApp.DTOs
{
    // Опрос, возвращаемый на фронт (с реальным числом голосов)
    public class PollResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Список опций с количеством голосов
        public List<OptionResponseDTO> Options { get; set; } = new();

        // Если нужно показать, за какую опцию проголосовал пользователь (необязательно)
        public OptionResponseDTO? UserVote { get; set; }
    }

    public class OptionResponseDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Votes { get; set; }
    }
}
