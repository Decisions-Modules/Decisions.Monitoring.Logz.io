namespace Decisions.Monitoring.Logz.io.Data
{
    internal class LogzErrorResponse
    {
        public int? emptyLogLines;
        public int? malformedLines;
        public int? oversizedLines;
        public int? successfulLines;
    }
}