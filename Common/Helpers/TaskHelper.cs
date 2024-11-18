namespace Common.Helpers
{
    public static class TaskHelper
    {
        public static int Stage1 { get; set; } = 800;
        public static int Stage2 { get; set; } = 1100;
        public static int Stage3 { get; set; } = 1500;
        public static int Stage4 { get; set; } = 2000;

        public static async Task ShortDelay()
        {
            Random random = new Random();
            await Task.Delay(random.Next(Stage1, Stage2));
        }

        public static async Task MediumDelay()
        {
            Random random = new Random();
            await Task.Delay(random.Next(Stage2, Stage3));
        }

        public static async Task LongDelay()
        {
            Random random = new Random();
            await Task.Delay(random.Next(Stage3, Stage4));
        }
        public static async Task DynamicDelay(int min, int max)
        {
            Random random = new Random();
            await Task.Delay(random.Next(min, max));
        }
    }
}
